using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Rdp.Service;
using Rdp.Web.Framework.Core;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Rdp.Web.Framework.Filters
{
    /// <summary>
    /// CustomHandleErrorAttribute 用于处理并记录系统错误
    /// </summary>
    public class CustomHandleErrorAttribute : ExceptionFilterAttribute
    {
        IErrorInfoService _errorInfoService;
        public IErrorInfoService ErrorInfoService
        {
            get{return _errorInfoService;}
            set {_errorInfoService = value;}
        }

        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                string controllerName = (string)filterContext.RouteData.Values["controller"];
                string actionName = (string)filterContext.RouteData.Values["action"];

                if (filterContext.Exception.GetType().Name == "CustomerException")
                {
                    //todo112
                    //HandleErrorInfo info = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
                    filterContext.Result = new ViewResult()
                    {
                        ViewName = "~/Views/Shared/Error.cshtml",
                        //ViewData = new ViewDataDictionary<HandleErrorInfo>(info)
                    };
                    filterContext.ExceptionHandled = true;
                    //filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                }
            }
        }
    }
}
