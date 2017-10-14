using System;
using System.Web;
using System.Text;
using Rdp.Web.Framework.Core;
using Microsoft.AspNetCore.Mvc;
using Rdp.Core.Util;
using Rdp.Web.Framework.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Rdp.Web.Framework.Models;
using Microsoft.Extensions.Options;

namespace Rdp.Web.Framework.Controllers
{
    /// <summary>
    /// 资源合成器，替代原来的HttpCombiner.ashx
    /// </summary>
    /*public class HttpCombinerController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private IMemoryCache _cache;
        private AppSettings _appSettings;

        public HttpCombinerController(IHostingEnvironment hostingEnvironment, IMemoryCache cache, IOptions<AppSettings> appSettings)
        {
            _hostingEnvironment = hostingEnvironment;
            _cache = cache;
            _appSettings = appSettings.Value;
        }

        [Compress]
        [ResponseCache(Duration = 3000)]
        public ContentResult Index()
        {
             var reqParams = Request.Query;
             string setName = reqParams["s"];
             string contentType = reqParams["t"];
             string version = reqParams["v"];
             string ex = reqParams["ex"];
             string requestRn = reqParams["Rn"];
             string cacheKey = "HttpCombinerEx." + setName + "." + version + "." + ex;
             TimeSpan CacheDuration = TimeSpan.FromDays(30);

             string result = "";

             //假如浏览器有缓存，服务器缓存没有失效的话则返回304
            if (_cache.TryGetValue(cacheKey, out result) && Request.Headers["If-Modified-Since"].ToString() != string.Empty)
             {
                 HttpContext.Response.StatusCode = 304;
                 return Content(String.Empty);
             }

             if ((result == null) || 0 == result.Length)
             {
                StringBuilder fileContents = new StringBuilder();
                 string setDefinition = Convert.ToString((string.IsNullOrEmpty(ex) ? _appSettings.GetType().GetProperty(setName).GetValue(_appSettings, null).ToString() : ex));
                 string[] fileNames = setDefinition.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                 string[] dependFiles = fileNames; //文件依赖，其中一个文件变更时Cache失效
                 var nIndex = 0;

                 foreach (string fileName in fileNames)
                 {
                     fileContents.AppendLine(System.IO.File.ReadAllText(_hostingEnvironment.WebRootPath + fileName.Trim()));
                     dependFiles[nIndex] = _hostingEnvironment.WebRootPath + fileName.Trim();
                     nIndex = nIndex + 1;
                 }

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(3000));
                _cache.Set(cacheKey, fileContents.ToString());

                result = fileContents.ToString();
             }

            if(!Response.Headers.ContainsKey("last-modified"))
                Response.Headers.Add("last-modified", DateTime.Now.ToString("r"));

            if (!Response.Headers.ContainsKey("expires"))
                Response.Headers.Add("expires", DateTime.Now.Add(CacheDuration).ToString("r"));

            if (!Response.Headers.ContainsKey("cache-control"))
                Response.Headers.Add("cache-control", "public");
 
            return Content(result, contentType, Encoding.UTF8);

        }
    }*/
}


