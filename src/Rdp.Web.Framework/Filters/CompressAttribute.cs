using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Rdp.Web.Framework.Filters
{

    public enum CompressionTypeEnum
    {
        Deflate,
        Gzip
    }

    /// <summary>
    /// CompressAttribute 文本压缩方式
    /// </summary>
    public class CompressAttribute : ActionFilterAttribute
    {
        private CompressionTypeEnum compressionType { get; set; }

        public CompressAttribute()
        {
            compressionType = CompressionTypeEnum.Gzip;
        }

        public CompressAttribute(CompressionTypeEnum type)
        {
            compressionType = type;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var encodingsAccepted = filterContext.HttpContext.Request.Headers["Accept-Encoding"];
            if (string.IsNullOrEmpty(encodingsAccepted)) return;

            encodingsAccepted = encodingsAccepted.ToString().ToLowerInvariant();
            var response = filterContext.HttpContext.Response;

            if (encodingsAccepted.Contains("deflate") && compressionType == CompressionTypeEnum.Deflate)
            {
                response.Headers.Add("Content-encoding", "deflate");
                //todo112
                //response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }
            else if (encodingsAccepted.Contains("gzip") && compressionType == CompressionTypeEnum.Gzip)
            {
                response.Headers.Add("Content-encoding", "gzip");
                //response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
        }
    }
}