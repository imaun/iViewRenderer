using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace imun.Http.iViewRenderer {
    public class ViewRenderer: IViewRenderer {
        private readonly IRazorViewEngine viewEngine;

        public ViewRenderer(IRazorViewEngine viewEngine) => this.viewEngine = viewEngine;

        public async Task<string> RenderAsync<TModel>(Controller controller, string name, TModel model) {
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
    }
}
