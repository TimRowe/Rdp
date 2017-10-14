using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rdp.Web.Framework.Extensions
{
    public class HttpRequestExtension
    {
        private HttpRequest _httpRequest;

        public HttpRequestExtension(HttpRequest httpRequest)
        {
            _httpRequest = httpRequest;
        }

        public string this[string key]
        {
            get
            {
                if (_httpRequest.Form[key].Count > 0)
                    return _httpRequest.Form[key].ToString();

                var queryitem = _httpRequest.Query.FirstOrDefault(m => m.Key == key);
                if (queryitem.Value.ToString() != string.Empty)
                    return queryitem.Value.ToString();

                return null;
            }
        }

    }
}
