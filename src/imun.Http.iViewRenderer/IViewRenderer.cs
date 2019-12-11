using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace imun.Http.iViewRenderer {
    public interface IViewRenderer {
        Task<string> RenderAsync<TModel>(Controller controller, string name, TModel model);
    }

}
