using Rdp.Core.Caching;
using Rdp.Core.Data;
using Rdp.Core.Dependency;
using Rdp.Data;
using Rdp.Data.Entity;
using Rdp.Service;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Rdp.Service
{
    public partial class VersionControlService : IVersionControlService
    {
        private IRepository<VersionControl> _versionRepository;

        public VersionControlService(IRepository<VersionControl> versionRepository)
        {
            _versionRepository = versionRepository;

        }

        public VersionControlService(): this(RepositoryFactory.Create<VersionControl>())
        {
            // TODO: Complete member initialization
        }

      

        public IRepository<VersionControl> UseRepository
        {
            get
            {
                return _versionRepository;
            }
        }

        public static List<VersionControl> versionList;

        public  bool GetVersionFlag(string key)
        {
            ICacheManager cacheManager = IocObjectManager.GetInstance().Resolve<IHttpContextCacheManager>();
            var result = cacheManager.Get("VersionList", 1, () =>
            {
                return UseRepository.Table.ToList();
            });

            if (versionList == null)
            {
                versionList = result;
                return false;
            }

            var cacheItem = result.Find(t => t.Key == key);
            var item = versionList.Find(t => t.Key == key);

            if(cacheItem == null)
            {
                UseRepository.Insert(new VersionControl()
                {
                    Key = key,
                    Version = 1,
                    UpdateDate = System.DateTime.Now
                });
                throw new ArgumentNullException(key + "未插入版本控制表(Version_Control)中");
            }

            if (item == null)
            {
                versionList.Add(cacheItem);
                return true;
            }

            if (item.Version != cacheItem.Version)
            {
                versionList[versionList.IndexOf(item)].Version = cacheItem.Version;
                return true;
            }
           
            return false;
        }

        public int GetVersion(string key)
        {
            ICacheManager cacheManager = IocObjectManager.GetInstance().Resolve<IHttpContextCacheManager>();
            var result = cacheManager.Get("VersionList", 1, () =>
            {
                return UseRepository.Table.ToList();
            });

            var cacheItem = result.Find(t => t.Key == key);

            if(cacheItem == null )
                throw new ArgumentNullException(key + "未插入版本控制表(Version_Control)中");

            return cacheItem.Version;
        }

      
    }
}
