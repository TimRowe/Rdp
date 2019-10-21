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
using System.Threading;

namespace Rdp.Web.Framework.Controllers
{
    [CustomHandleError]
    public class BaseController : Controller
    {
        /// <summary>
        /// 获取多语言
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string L(string name)
        {
            IGlobalResourcesService globalResourcesService = IocObjectManager.GetInstance().Resolve<IGlobalResourcesService>();
            return globalResourcesService.GetValue(name, Thread.CurrentThread.CurrentUICulture.Name);
        }
        
        /// <summary>
        /// 获取模型参数失败消息
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 获取表格参数
        /// </summary>
        /// <returns></returns>
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

