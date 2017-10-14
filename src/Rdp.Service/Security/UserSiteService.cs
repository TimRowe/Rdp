using Rdp.Core.Data;
using Rdp.Data;
using Rdp.Data.Entity;
using System;
using System.Data;
using System.Linq;
namespace Rdp.Service
{
    ///<summary>
    ///用户网址关系表
    ///</summary>
    public partial class UserSiteService : IUserSiteService
    {
    
         IRepository<UserSite> _userSiteRepository;
         IRepository<Site> _siteRepository;

        public IRepository<UserSite> UseRepository
        {
            get
            {
                return _userSiteRepository;
            }

        }

        public UserSiteService()
            : this(RepositoryFactory.Create<UserSite>(), RepositoryFactory.Create<Site>())
        {
        }

        public UserSiteService(IRepository<UserSite> userSiteRepository, IRepository<Site> siteRepository)
        {
            _userSiteRepository = userSiteRepository;
            _siteRepository = siteRepository;
        }

		public UserSite  GetModel(String userID, String siteDesc)
        {
           
             var o= (from u in UseRepository.Table
                    join p in _siteRepository.Table
                         on u.SiteID equals p.SiteID
                         where p.SiteDesc == siteDesc
                         && u.UserID==userID
                         select u);
             return o.FirstOrDefault();
        }

        public string GetUserSite(String userID)
        {
            var o = (from u in _userSiteRepository.Table
                     join p in _siteRepository.Table
                          on u.SiteID equals p.SiteID
                     where u.UserID == userID
                     select p);
            return o.FirstOrDefault()!=null?o.FirstOrDefault().SiteDesc: "";
        }
			
		
    }
}
