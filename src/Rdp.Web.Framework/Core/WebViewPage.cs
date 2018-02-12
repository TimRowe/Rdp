using System.Threading;
using Rdp.Core.Dependency;
using Rdp.Service;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Rdp.Web.Framework.Core {
    /// <summary>
    /// WebViewPage 的摘要说明
    /// </summary>
    public abstract class WebViewPage<TModel> : RazorPage<TModel>
    {
        private IGlobalResourcesService _globalResourcesService;

        public WebViewPage():base()
        {
            _globalResourcesService = IocObjectManager.GetInstance().Resolve<IGlobalResourcesService>();
        }


        public string L(string name)
        {
            return _globalResourcesService.GetValue(name, Thread.CurrentThread.CurrentUICulture.Name);
        }
    }
}