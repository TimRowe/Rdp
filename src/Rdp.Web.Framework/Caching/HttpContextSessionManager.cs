using Microsoft.AspNetCore.Http;
using Rdp.Core.Caching;
using Rdp.Web.Framework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Rdp.Web.Framework.Caching
{
    public partial class HttpContextSessionManager:IHttpContextSessionManager
    {
        /// <summary>
        /// Cache object
        /// </summary>
        protected ISession Session
        {
            get
            {
                return HttpContextOld.Current.Session;
            }
        }


        public virtual T Get<T>(string key)
        {
            return Session.Get<T>(key);
        }


        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            Session.Set(key, data);
        }


        public virtual bool IsSet(string key)
        {
            byte[] value;
            return Session.TryGetValue(key, out value);
        }

        

        public virtual void Remove(string key)
        {
            Session.Remove(key);
        }

        public virtual void RemoveByPattern(string pattern)
        {

            var enumerator = Session.Keys.GetEnumerator();
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = new List<String>();
            while (enumerator.MoveNext())
            {
                if (regex.IsMatch(enumerator.Current.ToString()))
                {
                    keysToRemove.Add(enumerator.Current.ToString());
                }
            }

            foreach (string key in keysToRemove)
            {
                Session.Remove(key);
            }
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public virtual void Clear()
        {
            var enumerator = Session.Keys.GetEnumerator();
            var keysToRemove = new List<String>();
            while (enumerator.MoveNext())
            {
                keysToRemove.Add(enumerator.Current.ToString());
            }

            foreach (string key in keysToRemove)
            {
                Session.Remove(key);
            }
        }


    }
}
