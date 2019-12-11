using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;

namespace imun.Http.iViewRenderer {
    public class ViewRenderer: IViewRenderer {

        private readonly IRazorViewEngine viewEngine;

        public ViewRenderer(IRazorViewEngine viewEngine) => this.viewEngine = viewEngine;

        public async Task<string> GetViewAsStringAsync<TModel>(
            Controller controller, 
            string name, TModel model) {

            if (controller == null)
                throw new ArgumentNullException("Controller reference is null.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("View name cannot be null or empty.");

            if (model == null)
                throw new ArgumentNullException("Model you passed is null.");

            string result = string.Empty;

            ViewEngineResult viewEngineResult = viewEngine
                .FindView(controller.ControllerContext, name, false);

            if (!viewEngineResult.Success) {
                throw new InvalidOperationException($"Could not find view: {name}");
            }

            IView view = viewEngineResult.View;
            controller.ViewData.Model = model;

            ViewContext viewContext;

            using (var writer = new StringWriter()) {
                viewContext = new ViewContext(
                    controller.ControllerContext,
                    view,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );
                await view.RenderAsync(viewContext);
                result = writer.ToString();
            }

            return result;
        }

        public async Task RenderStringAsync(Controller controller, string contents) {
            if (string.IsNullOrWhiteSpace(contents))
                throw new ArgumentNullException("contents cannot be null.");

            await controller.Response.WriteAsync(contents);
        }

        public async Task RenderViewAsync<TModel>(Controller controller, string name, TModel model) {

            var viewContents = await GetViewAsStringAsync<TModel>(controller, name, model);
            await controller.Response.WriteAsync(viewContents);
        }

    }
}
