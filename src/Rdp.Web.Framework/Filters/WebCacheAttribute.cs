using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Rdp.Web.Framework.Filters
{
    /// <summary>
    /// CacheActionFilterAttribute 的摘要说明
    /// </summary>
    public class WebCacheAttribute : ActionFilterAttribute
    {

        public WebCacheAttribute()
        {
        }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Headers.Add("Cache-Control", "public,must-revalidate, proxy-revalidate, max-age=2592000");
        }
    }
}