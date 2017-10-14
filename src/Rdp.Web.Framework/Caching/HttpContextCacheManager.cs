using Microsoft.Extensions.Caching.Memory;
using Rdp.Core.Caching;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Rdp.Web.Framework.Caching
{
    /// <summary>
    /// Represents a manager for caching between HTTP requests (long term caching)
    /// </summary>
    public partial class HttpContextCacheManager : IHttpContextCacheManager
    {

        private IMemoryCache _cache;

        public HttpContextCacheManager(IMemoryCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        public virtual T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        /// <summary>
        /// Adds the specified key and object to the _cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">_cache time</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;
            _cache.Set(key, data, TimeSpan.FromMinutes(cacheTime));
        }

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        public virtual bool IsSet(string key)
        {
            return (this._cache.Get(key) != null);
        }

        /// <summary>
        /// Removes the value with the specified key from the _cache
        /// </summary>
        /// <param name="key">/key</param>
        public virtual void Remove(string key)
        {
            this._cache.Remove(key);
        }

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern">pattern</param>
        public virtual void RemoveByPattern(string pattern)
        {
            throw new NotImplementedException();

        }

        /// <summary>
        /// Clear all _cache data
        /// </summary>
        public virtual void Clear()
        {
            throw new NotImplementedException();
        }
    }
}