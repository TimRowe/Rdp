using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Rdp.Core.Dependency;
using Rdp.Web.Framework.Models;
using System;
using System.Text.Encodings.Web;
using System.Web;

namespace Rdp.Web.Framework.Core
{
    /// <summary>
    /// Cookie控制表
    /// </summary>
    [Serializable()]
    public partial class CookieManager
    {
        private const string Language = "Language";
        private const string Version = "v";
        /// <summary>
        /// 添加Language
        /// </summary>
        /// <param name="lan"></param>
        /// <remarks></remarks>
        public static void AddLanguage(string lan)
        {
            HttpContextOld.Current.Response.Cookies.Append(
                 CookieRequestCultureProvider.DefaultCookieName,
                 CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lan)),
                new CookieOptions() { Expires = DateTime.Now.AddDays(30) });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        public static void RemoveLanguage()
        {
            var cookie = HttpContextOld.Current.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            if ((cookie != null))
            {
                HttpContextOld.Current.Response.Cookies.Delete(CookieRequestCultureProvider.DefaultCookieName);
            }
        }


        /// <summary>
        /// 得到Language
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetLanguage()
        {
            var lan = HttpContextOld.Current.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            return lan?.ToString().Split('|')[0].Split('=')[1];
        }
        /// <summary>
        /// 添加Language
        /// </summary>
        /// <param name="ver"></param>
        /// <remarks></remarks>
        public static void AddVersion(string ver)
        {
            var cookie = HttpContextOld.Current.Request.Cookies["Version"];
            if ((cookie != null))
            {
                HttpContextOld.Current.Response.Cookies.Delete(ver);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        public static void RemoveVersion()
        {
           var cookie = HttpContextOld.Current.Request.Cookies["Version"];
            if ((cookie != null))
            {
                HttpContextOld.Current.Response.Cookies.Delete(Version);
            }
        }
        /// <summary>
        /// 得到Language
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetVersion()
        {
           var cookie = HttpContextOld.Current.Request.Cookies["Version"];
            return cookie;
        }
    }
}

