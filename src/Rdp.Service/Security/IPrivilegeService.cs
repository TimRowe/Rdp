using System;
using System.Collections.Generic;
using Rdp.Core;
using Rdp.Core.Data;
using Rdp.Data.Entity;
using Rdp.Service.Dto;
using System.Data;
using System.Threading.Tasks;

namespace Rdp.Service
{
    public interface IPrivilegeService : IService<Privilege>
    {
         bool IsPrivilege(string userID, int roleID);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="programID"></param>
        /// <returns></returns>
        /// <remarks></remarks>
         bool CheckRight(string userID, int programID);
      


         string GetTreeGrid(PageParam pageParam);
       

        /// <summary>
        /// 分配权限
        /// </summary>
        /// <param name="privilegeID">已经存在的权限</param>
        /// <param name="strSql">添加SQL:select..union select</param>
        /// <returns></returns>
        /// <remarks></remarks>
         ResultInfo AssignPrivilege(string privilegeID, string strSql);

         List<PrivilegeDto> GetUrlPermissionItems(string userID, int roleID);

        /// <summary>
        /// 从多个角色组中
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleIDs"></param>
        /// <returns></returns>
         List<PrivilegeDto> GetUrlPermissionItems(string userID, short[] roleIDs);

         List<string> GetUrlIdentityItems(int privilegeMaster, string privilegeValue);

         /// <summary>
         /// 根据用户角色和程序ID获取程序按钮
         /// </summary>
         /// <param name="roleUser"></param>
         /// <param name="programID"></param>
         /// <returns></returns>
         List<Button> GetProgramButton(RoleUser roleUser, int programID);

        /// <summary>
        /// 根据用户角色和程序ID获取程序按钮（异步）
        /// </summary>
        /// <param name="roleUser"></param>
        /// <param name="programID"></param>
        /// <returns></returns>
        Task<List<Button>> GetProgramButtonAsync(RoleUser roleUser, int programID); 

        /// <summary>
        /// GetTreeGrid 获取权限目录树(新)
        /// </summary>
        /// <param name="privilegeMaster"></param>
        /// <param name="privilegeValue"></param>
        /// <param name="gridParams"></param>
        /// <returns></returns>
        DataTable GetTreeGrid(string parentId, int privilegeMaster, string privilegeValue, ref GridParams gridParams);

        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="privilegeList"></param>
        /// <returns></returns>
        ResultInfo AssignPrivilege(List<PrivilegeAssignDto> privilegeList);
    }
}
