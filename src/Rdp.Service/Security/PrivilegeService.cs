using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Rdp.Core.Data;
using Rdp.Core.Caching;
using System.Linq;
using Rdp.Core.Util;
using Rdp.Core;
using Rdp.Data.Entity;
using Rdp.Data;
using Rdp.Service.Dto;
using Rdp.Core.Security;
using AutoMapper;
using Rdp.Core.Dependency;
using System.Threading.Tasks;


namespace Rdp.Service
{
    ///<summary>
    ///用户权限表
    ///</summary>
    public partial class PrivilegeService : IPrivilegeService
    {
        IRepository<Privilege> _privilegeRepository;
        IRepository<Program> _programRepository;
        IRepository<ProgramButton> _programButtonRepository;
        IRepository<Button> _buttonRepository;
        IVersionControlService _versionControlService;

        public IRepository<Privilege> UseRepository
        {
            get
            {
                return _privilegeRepository;
            }

        }

        public PrivilegeService()
            : this(RepositoryFactory.Create<Privilege>(), 
                  RepositoryFactory.Create<Program>(), 
                  RepositoryFactory.Create<ProgramButton>(),
                  new VersionControlService(),
                  RepositoryFactory.Create<Button>())
        {
        }

        public PrivilegeService(
            IRepository<Privilege> privilegeRepository, 
            IRepository<Program> programRepository,
            IRepository<ProgramButton> programButtonRepository,
            IVersionControlService versionControlService,
            IRepository<Button> buttonRepository
        )
        {
            _privilegeRepository = privilegeRepository;
            _programRepository = programRepository;
            _programButtonRepository = programButtonRepository;
            _versionControlService = versionControlService;
            _buttonRepository = buttonRepository;
        }

        #region "ExtensionMethod"

        /// <summary>
        /// 是否有权限
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsPrivilege(string userID, int roleID)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" Access_Master = 1 AND Valid_From < GETDATE() AND Valid_Until > DATEADD(DAY, 1, GETDATE()) AND ( ( Privilege_Master = 1 AND Privilege_Value = '");
            strWhere.Append(roleID);
            strWhere.Append("') OR ( Privilege_Master = 2 AND Privilege_Value = '");
            strWhere.Append(userID.ToUpper());
            strWhere.Append("')) AND Operation_ID = 1");
            DataSet ds = this.GetList(strWhere.ToString(), "Access_Value");
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="programID"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool CheckRight(string userID, int programID)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append("Access_Value =");
            strWhere.Append(programID.ToString());
            strWhere.Append(" AND Access_Master = 1 AND Valid_From < GETDATE() AND Valid_Until > DATEADD(DAY, 1, GETDATE()) AND ( ( Privilege_Master = 1 AND Privilege_Value IN (SELECT Role_ID FROM dbo.tbLOG_Role_User WHERE User_ID = N'");
            strWhere.Append(programID);
            strWhere.Append("')) OR ( Privilege_Master = 2 AND Privilege_Value = N'");
            strWhere.Append(userID.ToUpper());
            strWhere.Append("')) AND Operation_ID = 1");
            DataSet ds = this.GetList(strWhere.ToString(), "Access_Value");
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return true;
            }
        }


        public string GetTreeGrid(PageParam pageParam)
        {
            DataSet program = new DataSet();
            DataSet button = new DataSet();
            int total = 0;
            program = this.GetListByPage(pageParam, ref total);
            ProgramButtonService bllProgramButton = new ProgramButtonService();
            StringBuilder strWhere = new StringBuilder();
            bool flag = true;
            for (int i = 0; i <= program.Tables[0].Rows.Count - 1; i++)
            {
                if (flag)
                {
                    flag = false;
                    strWhere.Append(program.Tables[0].Rows[i]["Program_ID"]);
                }
                else
                {
                    strWhere.Append(",");
                    strWhere.Append(program.Tables[0].Rows[i]["Program_ID"]);
                }
            }
            button = bllProgramButton.GetList("Program_ID IN (" + strWhere.ToString() + ")", "Button_ID AS Program_ID,Button_Text AS Program_Name,2 AS Access_Master,Program_ID AS _parentId");
            if (button.Tables[0].Rows.Count > 0)
            {
                List<DataTable> ls = new List<DataTable>();
                ls.Add(program.Tables[0]);
                ls.Add(button.Tables[0]);
                return "{\"total\":" + total.ToString() + ",\"rows\":" + JSONHelper.ToJson(ls).Replace("],[", ",").Replace("[[", "[").Replace("]]", "]") + "}";
            }
            else
            {
                return JSONHelper.ToJson(program.Tables[0], total);
            }
        }

        /// <summary>
        /// 分配权限
        /// </summary>
        /// <param name="privilegeID">已经存在的权限</param>
        /// <param name="strSql">添加SQL:select..union select</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public ResultInfo AssignPrivilege(string privilegeID, string strSql)
        {
            SqlParameter[] parameters = {
	        new SqlParameter("@ErrorNo", SqlDbType.Int, 4),
	        new SqlParameter("@ErrorMsg", SqlDbType.NVarChar, 500),
	        new SqlParameter("@privilegeID", SqlDbType.VarChar, 4000),
	        new SqlParameter("@strSql", SqlDbType.NVarChar, 4000)
            };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Direction = ParameterDirection.Output;
            parameters[2].Value = privilegeID;
            parameters[3].Value = strSql;
            DbHelperSql.RunProcedure(DbHelperSql.DefaultUpdateConn, "SP_LOG_ASSIGN_PRIVILEGE", parameters);
            return new ResultInfo()
            {
                ErrorNo = (int)parameters[0].Value,
                ErrorMsg = (string)parameters[1].Value
            };
        }


        /// <summary>
        /// 分配权限新
        /// </summary>
        /// <param name="privilegeList"></param>
        /// <returns></returns>
        public ResultInfo AssignPrivilege(List<PrivilegeAssignDto> privilegeList)
        {
            DataTable dt = this.ToDataTable(privilegeList);

            SqlParameter[] parameters = {
                new SqlParameter("@ErrorNo", SqlDbType.Int, 4),
                new SqlParameter("@ErrorMsg", SqlDbType.NVarChar, 500),
                new SqlParameter("@tpLOG_Privilege", dt)
            };

            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Direction = ParameterDirection.Output;
            parameters[2].Value = dt;
            DbHelperSql.RunProcedure(DbHelperSql.DefaultUpdateConn, "SP_LOG_ASSIGN_PRIVILEGE", parameters);

            return new ResultInfo()
            {
                ErrorNo = (int)parameters[0].Value,
                ErrorMsg = (string)parameters[1].Value
            };
        }


        public List<PrivilegeDto> GetUrlPermissionItems(string userID, int roleID)
        {
            short[] roleIDs = new short[1];
            roleIDs[0] = (short)roleID;
            return GetUrlPermissionItems(userID, roleIDs);
        }

        public List<PrivilegeDto> GetUrlPermissionItems(string userID, short[] roleIDs)
        {
            var sessionKey = "UrlPermission";
            ICacheManager _sessionManager = IocObjectManager.GetInstance().Resolve<IHttpContextSessionManager>();
            var result = _sessionManager.Get(sessionKey, 20, () =>
            {
                return (from p in UseRepository.Table
                        join u in _programRepository.Table
                        on p.AccessValue equals u.ProgramID
                        where p.AccessMaster == 1 && p.OperationID == 1
                        //&& ((p.PrivilegeMaster == 1 && p.PrivilegeValue == roleID.ToString()) || (p.PrivilegeMaster == 2 && p.PrivilegeValue == userID)) && p.OperationID == 1
                        select new PrivilegeDto { PrivilegeID = p.PrivilegeID, ValidFrom = p.ValidFrom, ValidUntil = p.ValidUntil, PrivilegeMaster = p.PrivilegeMaster, PrivilegeValue = p.PrivilegeValue, AccessMaster = p.AccessMaster, AccessValue = p.AccessValue, BranchMember = p.BranchMember, OperationID = p.OperationID, IsIdentity = p.IsIdentity, Url = u.Url }).Union(
                        from p in UseRepository.Table
                        join u in _programButtonRepository.Table
                        on p.AccessValue equals u.ProgramButtonID
                        where p.AccessMaster == 2 && p.OperationID == 1
                        //&& ((p.PrivilegeMaster == 1 && p.PrivilegeValue == roleID.ToString()) || (p.PrivilegeMaster == 2 && p.PrivilegeValue == userID)) && p.OperationID == 1
                        select new PrivilegeDto { PrivilegeID = p.PrivilegeID, ValidFrom = p.ValidFrom, ValidUntil = p.ValidUntil, PrivilegeMaster = p.PrivilegeMaster, PrivilegeValue = p.PrivilegeValue, AccessMaster = p.AccessMaster, AccessValue = p.AccessValue, BranchMember = p.BranchMember, OperationID = p.OperationID, IsIdentity = p.IsIdentity, Url = u.Url }
                        ).ToList();
            }, () =>
            {
                return _versionControlService.GetVersionFlag(sessionKey);
            });
            var item = result.Where(p => (p.PrivilegeMaster == 1 && roleIDs.Contains(short.Parse(p.PrivilegeValue))) || (p.PrivilegeMaster == 2 && p.PrivilegeValue == userID));
            return item != null ? item.ToList() : null;
        }

        public List<string> GetUrlIdentityItems(int privilegeMaster, string privilegeValue)
        {
            var sessionKey = "Privilege_Button";
            ICacheManager _sessionManager = IocObjectManager.GetInstance().Resolve<IHttpContextSessionManager>();
            var result = _sessionManager.Get(sessionKey, 20, () =>
            {
                return (from privelege in UseRepository.Table
                        join programButton in _programButtonRepository.Table
                        on privelege.AccessValue equals programButton.ProgramButtonID
                        where privelege.AccessMaster == 2
                        //p.IsIdentity == 2 & p.PrivilegeMaster == privilegeMaster & p.PrivilegeValue == privilegeValue
                        select new { privelege, programButton }).ToList();
            }, () =>
            {
                return _versionControlService.GetVersionFlag(sessionKey);
            });

           return result.Where(o => o.privelege.IsIdentity == 2 && o.privelege.PrivilegeMaster == privilegeMaster && o.privelege.PrivilegeValue == privilegeValue).Select(o => o.programButton.Url).ToList();
        }

        public List<Button> GetProgramButton(RoleUser roleUser, int programID)
        {
            var buttonLinq = (from b in _buttonRepository.Table
                    join pb in _programButtonRepository.Table
                    on b.ButtonID equals pb.ButtonID
                    join p in _privilegeRepository.Table
                    on pb.ProgramButtonID equals p.AccessValue
                    where b.StatusFlag == 0
                    && pb.ProgramID == programID
                    && p.AccessMaster == 2
                    && ((p.PrivilegeMaster == 1 && p.PrivilegeValue == roleUser.RoleID.ToString()) || (p.PrivilegeMaster == 2 && p.PrivilegeValue == roleUser.UserID.ToString()))
                    && p.ValidFrom <= DateTime.Now && p.ValidUntil >= DateTime.Now
                    select b).Distinct().OrderBy(b=>b.Priority);

            return buttonLinq.ToList();
        }

        /// <summary>
        /// 异步版本
        /// </summary>
        /// <param name="roleUser"></param>
        /// <param name="programID"></param>
        /// <returns></returns>
        public async Task<List<Button>> GetProgramButtonAsync(RoleUser roleUser, int programID)
        {
            var buttonLinq = (from b in _buttonRepository.Table
                              join pb in _programButtonRepository.Table
                              on b.ButtonID equals pb.ButtonID
                              join p in _privilegeRepository.Table
                              on pb.ProgramButtonID equals p.AccessValue
                              where b.StatusFlag == 0
                              && pb.ProgramID == programID
                              && p.AccessMaster == 2
                              && ((p.PrivilegeMaster == 1 && p.PrivilegeValue == roleUser.RoleID.ToString()) || (p.PrivilegeMaster == 2 && p.PrivilegeValue == roleUser.UserID.ToString()))
                              && p.ValidFrom <= DateTime.Now && p.ValidUntil >= DateTime.Now
                              select b).Distinct().OrderBy(b => b.Priority);

            return await buttonLinq.ToListAsync();
        }


        /// <summary>
        /// GetTreeGrid 获取权限目录树(新)
        /// 2017-04-18  Tim  Add     
        /// </summary>
        /// <param name="privilegeMaster">权限分类1角色，2用户</param>
        /// <param name="privilegeValue">权限值</param>
        /// <param name="gridParams">gird参数</param>
        /// <returns></returns>
        public DataTable GetTreeGrid(string parentId, int privilegeMaster, string privilegeValue, ref GridParams gridParams)
        {
            var pageParam = Mapper.Map<GridParams, PageParam>(gridParams);

            if(string.IsNullOrEmpty(pageParam.PrimaryKey))
            {
                pageParam.PrimaryKey = "Program_ID";
            }

            if (string.IsNullOrEmpty(pageParam.Order))
            {
                pageParam.Order = " Rn, Program_ID ";
            }
            else if (string.IsNullOrEmpty(pageParam.Order.Trim()))
            {
                pageParam.Order = " Rn, Program_ID ";
            }

            pageParam.PreSql = string.Format(@" WITH P AS ( SELECT   Program_ID ,
                        Parent_ID ,
                        Program_Name ,
                        1 AS Access_Master ,
                        PR.Privilege_ID AS Checked ,
                        PR.Branch_Member ,
                        PR.Operation_ID ,
                        PR.Valid_From,
                        PR.Valid_Until,
                        Is_Identity
               FROM     tbLOG_Program P
                        LEFT JOIN dbo.tbLOG_Privilege PR ON PR.Access_Value = P.Program_ID
                                                            AND PR.Access_Master = 1
                                                            AND PR.Privilege_Master = @privilegeMaster
                                                            AND PR.Privilege_Value = @privilegeValue
               WHERE    P.Status_Flag = 0
                        AND P.Is_Visible = 0
                        AND P.Parent_ID <> 0 {0}) ", string.IsNullOrEmpty(parentId)?"": " AND P.Parent_ID = @parentId");

            pageParam.TableName = @"(SELECT  Program_ID ,
                        Program_Name ,
                        1 AS Access_Master ,
                        0 AS _parentId ,
                        Checked ,
                        Branch_Member ,
                        Operation_ID ,
                        CONVERT(VARCHAR(12), Valid_From) Valid_From ,
                        CONVERT(VARCHAR(12), Valid_Until) Valid_Until,
                        Is_Identity,
                        Program_ID as Rn
                FROM    P
                UNION
                SELECT  Program_Button_ID AS Program_ID ,
                        Button_Text AS Program_Name ,
                        2 AS Access_Master ,
                        PB.Program_ID AS _parentId ,
                        P.Privilege_ID AS Checked ,
                        0 AS Branch_Member ,
                        Operation_ID ,
                        CONVERT(VARCHAR(12), Valid_From) Valid_From,
                        CONVERT(VARCHAR(12), Valid_Until) Valid_Until,
                        Is_Identity,
                        PB.Program_ID as Rn
                FROM    dbo.tbLOG_Program_Button PB
                        INNER JOIN dbo.tbLOG_Button B ON B.Button_ID = PB.Button_ID
                        LEFT JOIN dbo.tbLOG_Privilege P ON P.Access_Value = PB.Program_Button_ID
                                                           AND P.Access_Master = 2
                                                           AND P.Privilege_Master = @privilegeMaster
                                                           AND P.Privilege_Value = @privilegeValue
                WHERE   PB.Program_ID IN ( SELECT   Program_ID
                                           FROM     P )) AS TEMP ";

            pageParam.SqlParamList = new List<SqlParameter>() {
                new SqlParameter("@privilegeMaster", privilegeMaster),
                new SqlParameter("@privilegeValue", privilegeValue)
            };

            if (!string.IsNullOrEmpty(parentId))
            {
                pageParam.SqlParamList.Add(new SqlParameter("@parentId", int.Parse(parentId)));
            }

            var dt = DbHelperSql.QueryByPage(ref pageParam);
            gridParams.TotalCount = pageParam.TotalCount;

            return dt;
        }

        #endregion
    }
}


