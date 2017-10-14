using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rdp.Web.Application
{
    public class AppSettings
    {
        public string javascriptUpdate { get; set; }
        public string cssUpdate { get; set; }
        
        //public string SystemName { get; set; } //系统名称
        //public string CultureCookieName { get; set; } //国际化Cookie名称
        //public string CultureCookieValueFormat { get; set; }//国际化Cookiez值格式

        public string DefaultQueryConn { get; set; }

        public string DefaultUpdateConn { get; set; }

        private static AppSettings _my;

        public static AppSettings GetInstance()
        {
            if (_my == null)
                throw new InvalidOperationException("AppSettings is null");

            return _my;
        }

        public static void SetInstance(AppSettings settings)
        {
            _my = settings;
        }
    }


}
