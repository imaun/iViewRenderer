using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace imun.Http.iViewRenderer {
    public interface IViewRenderer {
        Task<string> GetViewAsStringAsync<TModel>(Controller controller, string name, TModel model);
        Task RenderStringAsync(Controller controller, string contents);
        Task RenderViewAsync<TModel>(Controller controller, string name, TModel model);
    }

}
