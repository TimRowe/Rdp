using Rdp.Data.Entity;
using System;

namespace Rdp.Service
{
   public interface IUserSiteService : IService<UserSite>
    {
        UserSite GetModel(String userID, String siteDesc);
        string GetUserSite(String userID);
    }
}
