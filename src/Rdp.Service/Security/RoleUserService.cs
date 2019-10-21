
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Linq;
using Rdp.Core.Data;
using System.Text;
using System.Data.SqlClient;
using Rdp.Data.Entity;
using Rdp.Data;
using System.Collections.Generic;
using Rdp.Service.Extension;
using Rdp.Service.Dto;

namespace Rdp.Service
{
    ///<summary>
    ///角色用户表
    ///</summary>
    public partial class RoleUserService : IRoleUserService
    {
        private IRepository<RoleUser> _roleUserRepository;
        private IRepository<RoleMaster> _roleMasterRepository;
        private IRepository<UserMaster> _userMasterRepository;

        public IRepository<RoleUser> UseRepository
        {
            get
            {
                return _roleUserRepository;
            }

        }

        public RoleUserService() : this(
            RepositoryFactory.Create<RoleUser>(),
            RepositoryFactory.Create<RoleMaster>(),
            RepositoryFactory.Create<UserMaster>()
            )
        {
        }

        public RoleUserService(
                IRepository<RoleUser> roleUserRepository,
                IRepository<RoleMaster> roleMasterRepository,
                IRepository<UserMaster> userMasterRepository
            )
        {
            _roleUserRepository = roleUserRepository;
            _roleMasterRepository = roleMasterRepository;
            _userMasterRepository = userMasterRepository;
        }
        
        
        
        public RoleUser GetModel(string userID)
        {
            var a = from e in UseRepository.Table
                    where e.UserID == userID
                    select e;
            return a.FirstOrDefault();
        }

         public int Add(String userID, String roleID) {

            var strSql = new StringBuilder();
            strSql.Append("INSERT INTO tbLOG_Role_User(");
            strSql.Append("User_ID,Role_ID)");
            strSql.Append(" SELECT @User_ID,B.ID FROM (");
            strSql.Append(" SELECT CONVERT(XML, '<v>' + REPLACE(@Role_ID, ',', '</v><v>')+ '</v>') AS ID) A OUTER APPLY (");
            strSql.Append(" SELECT ID = N.v.value('.', 'VARCHAR(1000)') FROM A.ID.nodes('/v') N ( v )) B ");
            strSql.Append("LEFT JOIN dbo.tbLOG_Role_User RU ON RU.Role_ID = B.ID AND RU.User_ID = @User_ID WHERE  RU.Role_ID IS NULL ");
            strSql.Append("SELECT SCOPE_IDENTITY()");
            SqlParameter[] parameters = {
               new SqlParameter("@User_ID", SqlDbType.VarChar, 50),
               new SqlParameter("@Role_ID", SqlDbType.NVarChar, 1000)};
            parameters[0].Value = userID.ToUpper();
            parameters[1].Value = roleID;
            var obj  = DbHelperSql.GetSingle(DbHelperSql.DefaultUpdateConn, strSql.ToString(), parameters);
            if (obj == null){
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        

        public bool Update(String userID,String roleID)
        {
            var hs = new Hashtable();
            var strInsertSql = new StringBuilder();
            strInsertSql.Append("INSERT INTO tbLOG_Role_User(");
            strInsertSql.Append("User_ID,Role_ID)");
            strInsertSql.Append(" SELECT @User_ID,B.ID FROM (");
            strInsertSql.Append(" SELECT CONVERT(XML, '<v>' + REPLACE(@Role_ID, ',', '</v><v>')+ '</v>') AS ID) A OUTER APPLY (");
            strInsertSql.Append(" SELECT ID = N.v.value('.', 'VARCHAR(1000)') FROM A.ID.nodes('/v') N ( v )) B ");
            strInsertSql.Append("LEFT JOIN dbo.tbLOG_Role_User RU ON RU.Role_ID = B.ID AND RU.User_ID = @User_ID WHERE  RU.Role_ID IS NULL ");
            SqlParameter[] parametersInsert = {
               new SqlParameter("@User_ID", SqlDbType.VarChar, 50),
               new SqlParameter("@Role_ID", SqlDbType.VarChar, 1000)};
            parametersInsert[0].Value = userID.ToUpper();
            parametersInsert[1].Value = roleID;
            hs.Add(strInsertSql, parametersInsert);

            var strDeleteSql = new StringBuilder();
            strDeleteSql.Append("DELETE  FROM dbo.tbLOG_Role_User WHERE   User_ID = @User_ID AND Role_ID NOT IN (");
            strDeleteSql.Append(roleID);
            strDeleteSql.Append(")");
            SqlParameter[] parametersDelete = {
             new SqlParameter("@User_ID", SqlDbType.VarChar, 50)};
            parametersDelete[0].Value = userID.ToUpper();
            hs.Add(strDeleteSql, parametersDelete);
            DbHelperSql.ExecuteSqlTran(DbHelperSql.DefaultUpdateConn, hs);

            return true;
         }
            


         virtual public DataSet GetList(String strWhere, String filedList)
        {
            var strSql = String.Format("SELECT {0} FROM tbLOG_Role_User RU INNER JOIN tbLOG_Role_Master RM ON RM.Role_ID = RU.Role_ID WHERE {1}", 
                String.IsNullOrEmpty(filedList) ? " * " : filedList,strWhere);
            return DbHelperSql.Query(DbHelperSql.DefaultQueryConn, strSql.ToString());
        }

        public List<RoleUserSearchResultDto> Search(RoleUserSearchRequestDto searchRequest, ref GridParams gridParams)
        {
            var query = from roleUser in _roleUserRepository.Table
                        join role in _roleMasterRepository.Table
                        on roleUser.RoleID equals role.RoleID
                        join user in _userMasterRepository.Table
                        on roleUser.UserID equals user.UserID
                        select new RoleUserSearchResultDto(){
                            UserRoleID = roleUser.UserRoleID,
                            RoleID = role.RoleID,
                            RoleDesc = role.RoleDesc,
                            UserID = user.UserID,
                            UserName = user.UserName
                        };

            query = query.Where(b => !(
                             (searchRequest.UserRoleID != 0 && b.UserRoleID != searchRequest.UserRoleID) ||
                             (!string.IsNullOrEmpty(searchRequest.UserID) && b.UserID.Trim() != searchRequest.UserID) ||
                             (searchRequest.RoleID != 0 && b.RoleID != searchRequest.RoleID)
                         ));

            gridParams.TotalCount = query.Count();
            return QueryExtensions.SortAndPage(query, gridParams).ToList();
        }
    }

    

    
}
