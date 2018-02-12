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

        private int _cacheMinutes;

        public WebCacheAttribute(int cacheMinutes)
        {
            _cacheMinutes = cacheMinutes;
        }

        public WebCacheAttribute()
        {
            _cacheMinutes = 720;
        }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //若是缓存时间未超而又重新请求服务器的话，返回304从本地缓存获取
            var modifiedTime = filterContext.HttpContext.Request.Headers["If-Modified-Since"].ToString();
           
            if (modifiedTime != string.Empty && Convert.ToDateTime(modifiedTime).Add(TimeSpan.FromMinutes(_cacheMinutes)) > DateTime.Now)
            {
                filterContext.Result = new ContentResult() { Content = String.Empty, StatusCode = 304 };
            }
        }


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            
            //设置超时时间
            //context.HttpContext.Response.Headers.Add("Cache-Control", "public,must-revalidate, proxy-revalidate, max-age=2592000");

            //if (!Response.Headers.ContainsKey("last-modified"))
            context.HttpContext.Response.Headers.Add("last-modified", DateTime.Now.ToUniversalTime().ToString("r"));

            /// if (!Response.Headers.ContainsKey("expires"))
            context.HttpContext.Response.Headers.Add("expires", DateTime.Now.Add(TimeSpan.FromMinutes(_cacheMinutes)).ToUniversalTime().ToString("r"));

            //if (!Response.Headers.ContainsKey("cache-control"))
            context.HttpContext.Response.Headers.Add("cache-control", "public");
        }
    }
}