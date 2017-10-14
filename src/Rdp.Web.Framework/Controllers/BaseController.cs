using System;
using Microsoft.AspNetCore.Mvc;
using Rdp.Core.Data;
using Rdp.Core.Dependency;
using Rdp.Core.Util;
using Rdp.Data.Entity;
//using Autofac;
using Rdp.Service;
using Rdp.Web.Framework.Core;
using Rdp.Web.Framework.Filters;
using System.Collections.Generic;
using Rdp.Web.Framework.Runtime;
using Microsoft.AspNetCore.Mvc.Filters;
using Rdp.Web.Framework.Extensions;

namespace Rdp.Web.Framework.Controllers
{
    [CustomHandleError]
    public class BaseController : Controller
    {
        /*protected virtual void LogInfo(string userId, string infoMsg)
        {
            var errorInfo = new ErrorInfo();
            errorInfo.ErrorMSG = infoMsg;
            errorInfo.RunningTime = System.DateTime.Now;
            errorInfo.Url = request.Path.ToString();
            _errorInfoService.Add(errorInfo);
        }*/

        /*TODO112
         * protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            //todo完善
            //创建基于http request的scope用于显式的依赖注入,eg RespositoryFactory CouDbContext的构建
            var container = IocContainerManager.GetInstance();
            if (container string.empty)
            {
                var scope = container.BeginLifetimeScope();
                HttpContext.Items["PerRequestScope"] = scope;
            }

            Utils.InitializeCulture();
            return base.BeginExecuteCore(callback, state);
        }*/

        public override void OnActionExecuting(ActionExecutingContext context)
        {
           // Utils.InitializeCulture();
        }


        public String GetModelInvalidMsg()
        {
            string strError = "";

            foreach (var e in ModelState)
            {
                if (e.Value.Errors.Count > 0)
                {
                    strError += e.Key.ToString() + ":" + e.Value.Errors[0].ErrorMessage;
                }
            }

            return strError;
        }


        public GridParams GetGridParams()
        {
            var grid = new GridParams();
            var request = new HttpRequestExtension(HttpContext.Request);

            if (!string.IsNullOrEmpty(request["rows"]))
            {
                grid.PageSize = Int32.Parse(request["rows"]);
            }

            if (grid.PageSize == 0)
            {
                grid.PageSize = 10;
            }
                
            if (!string.IsNullOrEmpty(request["page"]))
            {
                grid.PageIndex = Int32.Parse(request["page"]);

                if (grid.PageIndex == 0)
                {
                    grid.PageIndex = 0;
                }
            }

            if (!string.IsNullOrEmpty(request["sort"]))
            {
                grid.SortField = request["sort"].ToString();
            }

            if (!string.IsNullOrEmpty(request["order"]))
            {
                grid.SortDirection = request["order"].ToString();
            }

            if (!string.IsNullOrEmpty(request["total"]) && request["total"] != "NaN")
            {
                grid.TotalCount = int.Parse(request["total"]);
            }

            if(!string.IsNullOrEmpty(request["gridFields"]))
            {
                grid.Columns = JSONHelper.FromJsonTo<List<GridColumn>>(request["gridFields"]);
            }

            return grid;
        }
    }
}

