/*using System.Threading;
using Autofac;
using Rdp.Core.Dependency;
using System;
using Rdp.Service;

namespace Rdp.Web.Framework.Core {
    /// <summary>
    /// WebViewPage 的摘要说明
    /// </summary>
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
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
}*/