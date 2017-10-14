using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;

namespace Rdp.Web.Framework.Core
{
    /// <summary>
    /// 自定义的Razor引擎，为了在mvc中使用多级目录
    /// </summary>
    /*public class RdpRazorViewEngine : RazorViewEngine
    {
        public RdpRazorViewEngine()
        {
            ViewLocationFormats = new[]
            {
                "~/Views/{1}/{0}.cshtml",
                 "~/Views/RdpTemplate/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml"
               
            };

            PartialViewLocationFormats = new[]
            {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/RdpTemplate/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml"
            };
        }
    }*/
    
    public class BetterViewEngine : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            return new[]
            {
                 "~/Views/{1}/{0}.cshtml",
                "~/Views/RdpTemplate/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml"
            };
        }
    }
}


