using Rdp.Core.Data;
using Rdp.Data.Entity;
using Rdp.Service.Dto;
using System.Collections.Generic;
using System.Data;

namespace Rdp.Service
{
  public  interface IUserMasterService : IService<UserMaster>
    {
         DataTable Login(string ip);
        
        /// <summary>
        /// 账号密码登录
        /// </summary>
        /// <param name="userID">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        /// <remarks></remarks>
         UserMaster Login(string userID, string password);
       

        bool AddUserTemplate(
           UserMaster userMaster,
           RoleUser roleUser,
           string userIdTemplate,
           string proFileName,
           string emailAdd,
           string title,
           string context,
           string type);
        List<UserMaster> Search(UserMasterSearchDto model, ref GridParams gridParams);
    }
}
