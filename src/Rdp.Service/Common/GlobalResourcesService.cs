using Rdp.Core.Caching;
using Rdp.Core.Data;
using Rdp.Data;
using Rdp.Data.Entity;
using System.Linq;
using System;
using Rdp.Core.Dependency;
using Rdp.Core.Collection;
using System.Collections.Generic;

namespace Rdp.Service
{
    ///<summary>
    ///分店资料表
    ///</summary>
    public partial class GlobalResourcesService : IGlobalResourcesService
    {
        IRepository<GlobalResources> _globalResourcesRepository;
        SortedDictionary<string, string> _resourcesDic;
        ICacheManager _cacheManager;

        public GlobalResourcesService(): this(
            RepositoryFactory.Create<GlobalResources>(), 
            IocObjectManager.GetInstance().Resolve<IHttpContextCacheManager>())
        {
            
        }

        public GlobalResourcesService(IRepository<GlobalResources> globalResourcesRepository, ICacheManager cacheManager)
        {
            _globalResourcesRepository = globalResourcesRepository;
            _cacheManager = cacheManager;

            _resourcesDic = _cacheManager.Get("GlobalResources", 60, () =>
            {
                var list =  _globalResourcesRepository.Table.OrderBy(m => m.Name).ThenBy(m => m.Lanuage).ToList();
                SortedDictionary<string, string> resourcesDic = new SortedDictionary<string, string>();
                foreach (var item in list)
                    resourcesDic.Add(item.Name + "_" + item.Lanuage, item.Value);

                return resourcesDic;
            }, () =>
            {
                return ServiceExtensions.FromIoc<IVersionControlService>().GetVersionFlag("GlobalResources");
            });
        }

        public IRepository<GlobalResources> UseRepository
        {
            get
            {
                return _globalResourcesRepository;
            }
        }

        public string GetValue(string name, string strLan)
        {
            var value = string.Empty;
            _resourcesDic.TryGetValue(name + "_" + strLan, out value);

            if (string.IsNullOrEmpty(value))
                return name;

            return value;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="strLan"></param>
        /// <returns></returns>
        public List<GlobalResources> GetList(string strLan)
        {
            ICacheManager _cacheManager = IocObjectManager.GetInstance().Resolve<IHttpContextCacheManager>();
            var resources = _cacheManager.Get("GlobalResourcesByLan", 60, () =>
            {
                return _globalResourcesRepository.Table.OrderBy(m => m.Lanuage).ToList().ToDictionaryWithMultipleKey(m => { return m.Lanuage; }, true);
            }, () =>
            {
                return ServiceExtensions.FromIoc<IVersionControlService>().GetVersionFlag("GlobalResourcesByLan");
            });

            return resources[strLan];
        }
    }

}