using Rdp.Core.Caching;
using Rdp.Core.Data;
using Rdp.Data;
using Rdp.Data.Entity;
using System.Linq;
using System;
using Rdp.Core.Dependency;

namespace Rdp.Service
{
    ///<summary>
    ///分店资料表
    ///</summary>
    public partial class GlobalResourcesService : IGlobalResourcesService
    {
        IRepository<GlobalResources> _globalResourcesRepository;

        public GlobalResourcesService(): this(RepositoryFactory.Create<GlobalResources>())
        {
          
        }

        public GlobalResourcesService(IRepository<GlobalResources> globalResourcesRepository)
        {
            _globalResourcesRepository = globalResourcesRepository;
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
            ICacheManager _cacheManager = IocObjectManager.GetInstance().Resolve<IHttpContextCacheManager>();
            var resources = _cacheManager.Get("GlobalResources", 60, () =>
            {
                return _globalResourcesRepository.Table.ToList();
            });

            var item = resources.Find(t => t.Name == name & t.Lanuage == strLan);
            return item != null ? item.Value : name + "not found";
        }

    }

}