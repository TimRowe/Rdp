using System;
using Rdp.Core.Data;
using Rdp.Core.Security;
using Rdp.Web.Framework.Models;
using System.Text;
/*
namespace Rdp.Web.Framework.Core
{
    /// <summary>
    /// 从旧框架转换而来
    /// todo待优化
    /// </summary>
    public class OverrideIHttpHandler : IHttpHandler, IRequiresSessionState
    {
        public OverrideIHttpHandler()
        {
        }

        public virtual void ProcessRequest(HttpContext context)
        {
        }

        public bool IsReusable
        {
            get { return false; }
        }
        /// <summary>
        /// 檢查操作權限
        /// </summary>
        /// <param name="context">页面查询的参数</param>
        /// <remarks></remarks>
        public void CheckPrivilege(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = Encoding.Unicode;
            if (SessionManager.GetRoleUser() == null && SessionManager.GetLock() == null)
            {
                context.Response.Redirect("/Error/Detail?ErrorNo=" + ErrorTypeEnum.LostSession.ToString());
            }
            else
            {
               Utils.InitializeCulture();
            }
        }


        /// <summary>
        /// 获取当前页面同等权限的分店SQL
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetBranchOfPagePrivilege(HttpRequest request)
        {
            //根据session获取当前的组别及所在分店()
            //获取后判断其能够查看的客诉()
            string roleID = SessionManager.GetRoleUser().RoleID.ToString().Trim();
            string userID = SessionManager.GetUserMaster().UserID.ToString().Trim();
            string branchCode = SessionManager.GetUserMaster().BranchCode.ToString().Trim();
            string branchSql = null;
            string sql = null;
            sql = "SELECT  [Select_Sql] FROM  dbo.[tbLOG_Branch_Member] CBM INNER JOIN [tbLOG_Privilege] CP ON CP.[Branch_Member] = CBM.[Branch_Member] WHERE   [Privilege_Master] = 1 AND [Access_Master] = 1 AND Privilege_Value=" + roleID + " AND Access_Value=" + DesEncrypt.Decrypt(request["PD"].Trim());
            branchSql = Convert.ToString(DbHelperSql.GetSingle(DbHelperSql.DefaultQueryConn, sql));
            if (branchSql == null | string.IsNullOrEmpty(branchSql))
            {
                sql = "SELECT  [Select_Sql] FROM dbo.[tbLOG_Branch_Member] CBM INNER JOIN [tbLOG_Privilege] CP ON CP.[Branch_Member] = CBM.[Branch_Member] WHERE   [Privilege_Master] = 2 AND [Access_Master] = 1 AND Privilege_Value=" + userID + " AND Access_Value=" + DesEncrypt.Decrypt(request["PD"].Trim());
                branchSql = Convert.ToString(DbHelperSql.GetSingle(DbHelperSql.DefaultQueryConn, sql));
            }
            branchSql = branchSql.Replace("{0}", branchCode);

            return branchSql;
        }
        #region "常用的方法"
        #endregion
    }
}
*/