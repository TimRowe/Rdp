using System.Data;
using Rdp.Core.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Rdp.Core.Security;
using Rdp.Data.Entity;
using System.Transactions;
using Rdp.Data;
using Rdp.Service.Dto;
using System.Collections.Generic;
using System;
using Rdp.Service.Extension;

namespace Rdp.Service
{
    ///<summary>
    ///用户表
    ///</summary>
    public partial class UserMasterService : IUserMasterService
    {
        IRepository<UserMaster> _userMasterRepository;
        IRepository<RoleUser> _roleUserRepository;
        IRepository<UserSite> _userSiteRepository;
        IRepository<Privilege> _privilegeRepository;

        public IRepository<UserMaster> UseRepository
        {
            get
            {
                return _userMasterRepository;
            }

        }

        public UserMasterService()
            : this(RepositoryFactory.Create<UserMaster>(),
                  RepositoryFactory.Create<RoleUser>(),
                  RepositoryFactory.Create<UserSite>(),
                  RepositoryFactory.Create<Privilege>())
        {
        }

        public UserMasterService(
            IRepository<UserMaster> userMasterRepository,
            IRepository<RoleUser> roleUserRepository,
            IRepository<UserSite> userSiteRepository,
            IRepository<Privilege> privilegeRepository)
        {
            _userMasterRepository = userMasterRepository;
            _roleUserRepository = roleUserRepository;
            _userSiteRepository = userSiteRepository;
            _privilegeRepository = privilegeRepository;
        }

        #region "ExtensionMethod"
        /// <summary>
        /// IP登录
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable Login(string ip)
        {
            SqlParameter[] parameters = { new SqlParameter("@IPAddress", ip) };
            //todo
            return this.RunProcedure(DbOperation.Read, "SP_LOG_IP_LOGIN", parameters).Tables[0];

        }
        /// <summary>
        /// 账号密码登录
        /// </summary>
        /// <param name="userID">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public UserMaster Login(string userID, string password)
        {
            var user = this.GetModel(userID);
            return user == null || user.Password != DesEncrypt.Encrypt(password) ? null : user;
        }

        public bool AddUserTemplate(
            UserMaster userMaster,
            RoleUser roleUser,
            string userIdTemplate,
            string proFileName,
            string emailAdd,
            string title,
            string context,
            string type)
        {
           
            var templateUser = this.GetModel(userIdTemplate);
            if (templateUser == null) return false;
            var templateUserSites = (from e in _userSiteRepository.Table
                                     where e.UserID == templateUser.UserID
                                     select e).ToList();

            var templatePrivilege = (from e in _privilegeRepository.Table
                                     where e.PrivilegeMaster == 2 && e.PrivilegeValue == templateUser.UserID
                                     select e).ToList();

            //事务禁止远程访问
            using (TransactionScope transaction = new TransactionScope())
            {
                this.Add(userMaster);
                _roleUserRepository.Insert(roleUser);
                foreach (var e in templateUserSites)
                {
                    e.UserID = userMaster.UserID;
                    _userSiteRepository.Insert(e);
                }

                foreach (var e in templatePrivilege)
                {
                    e.PrivilegeValue = userMaster.UserID;
                    _privilegeRepository.Insert(e);
                }

                var strEmailSendSql = new StringBuilder();
                strEmailSendSql.Append(" EXEC [msdb].dbo.sp_send_dbmail @profile_name = @ProFileName,@recipients = @EmailAdd,");
                strEmailSendSql.Append("@subject = @Title,@body = @Context,@body_format = @Type");
                SqlParameter[] parameters = { new SqlParameter("@ProFileName", proFileName),
                                  new SqlParameter("@EmailAdd", emailAdd),  
                                  new SqlParameter("@Title", title),  
                                  new SqlParameter("@Context", context),  
                                  new SqlParameter("@Type", type)
                                            };
                DbHelperSql.ExecuteSql(DbHelperSql.DefaultUpdateConn, strEmailSendSql.ToString(), parameters);
                //提交事务
                transaction.Complete();

            }
            return true;
        }

        public List<UserMaster> Search(UserMasterSearchDto model, ref GridParams gridParams)
        {
            var query = from u in _userMasterRepository.Table
                        select u;
            if (!String.IsNullOrEmpty(model.UserID))
            {
                query = query.Where(t => t.UserID == model.UserID);
            }
            if (!String.IsNullOrEmpty(model.EmailAdd))
            {
                query = query.Where(t => t.EmailAdd == model.EmailAdd);
            }
            if (model.ReaderType >= 0)
            {
                query = query.Where(t => t.ReaderType == model.ReaderType);
            }
            if (model.BranchCode>=0)
            {
                query = query.Where(t => t.BranchCode == model.BranchCode);
            }
            if (model.PasswordExprityDateFrom != Convert.ToDateTime("1900-01-01"))
            {
                query = query.Where(t => t.PasswordExprityDate >= model.PasswordExprityDateFrom);
            }
            if (model.PasswordExprityDateTo != Convert.ToDateTime("1900-01-01"))
            {
                query = query.Where(t => t.PasswordExprityDate < model.PasswordExprityDateTo);
            }
            gridParams.TotalCount = query.Count();
            return QueryExtensions.SortAndPage(query, gridParams).ToList();
        }
        #endregion

    }
}
