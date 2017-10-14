using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Rdp.Core.Data;
using Rdp.Data.Entity;
using Rdp.Data;
using System.Collections.Generic;
using System.Linq;
using Rdp.Service.Extension;
using Rdp.Service.Dto;

namespace Rdp.Service
{
    ///<summary>
    ///程序按钮表
    ///</summary>
    public partial class ProgramButtonService : IProgramButtonService
    {
        IRepository<ProgramButton> _programButtonRepository;
        IRepository<Program> _programRepository;
        IRepository<Button> _buttonRepository;
        public IRepository<ProgramButton> UseRepository
        {
            get
            {
                return _programButtonRepository;
            }
        }

        public ProgramButtonService()
            : this(RepositoryFactory.Create<ProgramButton>(),
                  RepositoryFactory.Create<Program>(),
                  RepositoryFactory.Create<Button>()
                  )
        {
        }

        public ProgramButtonService(IRepository<ProgramButton> programButtonRepository,
            IRepository<Program> programRepository,
            IRepository<Button> buttonRepository
            )
        {
            _programButtonRepository = programButtonRepository;
            _programRepository = programRepository;
            _buttonRepository = buttonRepository;
        }

        #region "ExtensionMethod"


        public int Add(int programID, String buttonID)
        {
            var strSql = new StringBuilder();
            strSql.Append("INSERT INTO tbLOG_Program_Button(");
            strSql.Append("Program_ID,Button_ID)");
            strSql.Append(" SELECT @Program_ID,B.ID FROM (");
            strSql.Append(" SELECT CONVERT(XML, '<v>' + REPLACE(@Button_ID, ',', '</v><v>')+ '</v>') AS ID) A OUTER APPLY (");
            strSql.Append(" SELECT ID = N.v.value('.', 'VARCHAR(1000)') FROM A.ID.nodes('/v') N ( v )) B ");
            strSql.Append("LEFT JOIN dbo.tbLOG_Program_Button PB ON PB.Button_ID = B.ID AND PB.Program_ID = @Program_ID WHERE  PB.Button_ID IS NULL ");
            strSql.Append("SELECT SCOPE_IDENTITY()");
            SqlParameter[] parameters = {
                   new SqlParameter("@Program_ID", SqlDbType.Int, 4),
                   new SqlParameter("@Button_ID", SqlDbType.VarChar, 1000)};
            parameters[0].Value = programID;
            parameters[1].Value = buttonID;
            var obj = DbHelperSql.GetSingle(DbHelperSql.DefaultUpdateConn, strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        public int Add(int programID, String buttonID,String Url)
        {
            var strSql = new StringBuilder();
            strSql.Append("INSERT INTO tbLOG_Program_Button(");
            strSql.Append("Program_ID,Button_ID,Url)");
            strSql.Append(" SELECT @Program_ID,B.ID,@Url FROM (");
            strSql.Append(" SELECT CONVERT(XML, '<v>' + REPLACE(@Button_ID, ',', '</v><v>')+ '</v>') AS ID) A OUTER APPLY (");
            strSql.Append(" SELECT ID = N.v.value('.', 'VARCHAR(1000)') FROM A.ID.nodes('/v') N ( v )) B ");
            strSql.Append("LEFT JOIN dbo.tbLOG_Program_Button PB ON PB.Button_ID = B.ID AND PB.Program_ID = @Program_ID WHERE  PB.Button_ID IS NULL ");
            strSql.Append("SELECT SCOPE_IDENTITY()");
            SqlParameter[] parameters = {
                   new SqlParameter("@Program_ID", SqlDbType.Int, 4),
                   new SqlParameter("@Button_ID", SqlDbType.VarChar, 200),
                   new SqlParameter("@Url", SqlDbType.VarChar, 100)};
            parameters[0].Value = programID;
            parameters[1].Value = buttonID;
            parameters[2].Value = Url;
            var obj = DbHelperSql.GetSingle(DbHelperSql.DefaultUpdateConn, strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }



        public bool Update(int programID, string buttonID)
        {
            var strInsertSql  =  new StringBuilder();
            strInsertSql.Append("INSERT INTO tbLOG_Program_Button(");
            strInsertSql.Append("Program_ID,Button_ID)");
            strInsertSql.Append(" SELECT @Program_ID,B.ID FROM (");
            strInsertSql.Append(" SELECT CONVERT(XML, '<v>' + REPLACE(@Button_ID, ',', '</v><v>')+ '</v>') AS ID) A OUTER APPLY (");
            strInsertSql.Append(" SELECT ID = N.v.value('.', 'VARCHAR(1000)') FROM A.ID.nodes('/v') N ( v )) B ");
            strInsertSql.Append("LEFT JOIN dbo.tbLOG_Program_Button PB ON PB.Button_ID = B.ID AND PB.Program_ID = @Program_ID WHERE  PB.Button_ID IS NULL ");
            SqlParameter[] parametersInsert = {
               new SqlParameter("@Program_ID", SqlDbType.VarChar, 50),
               new SqlParameter("@Button_ID", SqlDbType.VarChar, 1000)};
            parametersInsert[0].Value = programID;
            parametersInsert[1].Value = buttonID;
            var strDeleteSql =  new StringBuilder();
            strDeleteSql.Append("DELETE  FROM dbo.tbLOG_Program_Button WHERE Program_ID = @Program_ID AND Button_ID NOT IN (");
            strDeleteSql.Append(buttonID);
            strDeleteSql.Append(")");
            SqlParameter[] parametersDelete = {
             new SqlParameter("@Program_ID", SqlDbType.VarChar, 50)};
            parametersDelete[0].Value = programID;
            var hs = new Hashtable();
            hs.Add(strInsertSql, parametersInsert);
            hs.Add(strDeleteSql, parametersDelete);
            try
			{
                DbHelperSql.ExecuteSqlTran(DbHelperSql.DefaultUpdateConn, hs);
            }
            catch(Exception ex) 
			{
                return false;
            }

            return true;
        }


        //public bool Update(int programID, string buttonID,string Url)
        //{
        //    var strDeleteSql = new StringBuilder();
        //    strDeleteSql.Append("DELETE  FROM dbo.tbLOG_Program_Button WHERE Program_ID = @Program_ID AND Button_ID  IN (@Button_ID)");
        //    SqlParameter[] parametersDelete = {
        //     new SqlParameter("@Program_ID", SqlDbType.VarChar, 50),
        //     new SqlParameter("@Button_ID", SqlDbType.VarChar, 1000)};
        //    parametersDelete[0].Value = programID;
        //    parametersDelete[1].Value = buttonID;    

        //    var strInsertSql = new StringBuilder();
        //    strInsertSql.Append("INSERT INTO tbLOG_Program_Button(");
        //    strInsertSql.Append("Program_ID,Button_ID,Url)");
        //    strInsertSql.Append(" SELECT @Program_ID,B.ID,@Url FROM (");
        //    strInsertSql.Append(" SELECT CONVERT(XML, '<v>' + REPLACE(@Button_ID, ',', '</v><v>')+ '</v>') AS ID) A OUTER APPLY (");
        //    strInsertSql.Append(" SELECT ID = N.v.value('.', 'VARCHAR(1000)') FROM A.ID.nodes('/v') N ( v )) B ");
        //    strInsertSql.Append("LEFT JOIN dbo.tbLOG_Program_Button PB ON PB.Button_ID = B.ID AND PB.Program_ID = @Program_ID WHERE  PB.Button_ID IS NULL ");
        //    SqlParameter[] parametersInsert = {
        //       new SqlParameter("@Program_ID", SqlDbType.VarChar, 50),
        //       new SqlParameter("@Button_ID", SqlDbType.VarChar, 200),
        //       new SqlParameter("@Url", SqlDbType.VarChar, 100)};
        //    parametersInsert[0].Value = programID;
        //    parametersInsert[1].Value = buttonID;
        //    parametersInsert[2].Value = Url;
          
        //    var hs = new Hashtable();
        //    hs.Add(strDeleteSql, parametersDelete);
        //    hs.Add(strInsertSql, parametersInsert);
          
        //    try
        //    {
        //        DbHelperSql.ExecuteSqlTran(DbHelperSql.DefaultUpdateConn, hs);
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        public bool Update(int programID, string buttonID, string Url)
        {
            var strSql = new StringBuilder();
            strSql.Append("DELETE  FROM dbo.tbLOG_Program_Button WHERE Program_ID = @Program_ID AND Button_ID  IN (@Button_ID);");

            strSql.Append("INSERT INTO tbLOG_Program_Button(");
            strSql.Append("Program_ID,Button_ID,Url)");
            strSql.Append(" SELECT @Program_ID,B.ID,@Url FROM (");
            strSql.Append(" SELECT CONVERT(XML, '<v>' + REPLACE(@Button_ID, ',', '</v><v>')+ '</v>') AS ID) A OUTER APPLY (");
            strSql.Append(" SELECT ID = N.v.value('.', 'VARCHAR(1000)') FROM A.ID.nodes('/v') N ( v )) B ");
            strSql.Append("LEFT JOIN dbo.tbLOG_Program_Button PB ON PB.Button_ID = B.ID AND PB.Program_ID = @Program_ID WHERE  PB.Button_ID IS NULL;");

            SqlParameter[] parameters = {
               new SqlParameter("@Program_ID", SqlDbType.VarChar, 50),
               new SqlParameter("@Button_ID", SqlDbType.VarChar, 200),
               new SqlParameter("@Url", SqlDbType.VarChar, 100)};
            parameters[0].Value = programID;
            parameters[1].Value = buttonID;
            parameters[2].Value = Url;
            var hs = new Hashtable();
            hs.Add(strSql, parameters); 

            try
            {
                DbHelperSql.ExecuteSqlTran(DbHelperSql.DefaultUpdateConn, hs);  
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }


        public bool Update(int programButtonID, int programID, string buttonID, string Url)
        {
            var strSql = new StringBuilder();
            strSql.Append("UPDATE tbLOG_Program_Button SET ");
            strSql.Append(" Program_ID = @Program_ID ,Button_ID = @Button_ID , Url = @Url ");
            strSql.Append("FROM tbLOG_Program_Button LPB ");
            strSql.Append("LEFT JOIN ( ");
            strSql.Append("SELECT  * FROM    ( SELECT    Program_ID ,Button_ID ,ROW_NUMBER() OVER ( PARTITION BY Program_ID,Button_ID ORDER BY Program_Button_ID ASC ) RN FROM      tbLOG_Program_Button  WHERE Program_ID=@Program_ID AND Button_ID=@Button_ID ) AS T WHERE  T.RN > 1 ");
            strSql.Append(" ) TPB ");
            strSql.Append("ON LPB.Program_ID = TPB.Program_ID AND LPB.Button_ID = TPB.Button_ID ");
            strSql.Append("WHERE LPB.Program_ID=@Program_ID AND LPB.Button_ID=@Button_ID AND TPB.Program_ID IS NULL  ");
            SqlParameter[] parameters = {
                new SqlParameter("@Program_Button_ID", SqlDbType.VarChar, 50),
               new SqlParameter("@Program_ID", SqlDbType.VarChar, 50),
               new SqlParameter("@Button_ID", SqlDbType.VarChar, 200),
               new SqlParameter("@Url", SqlDbType.VarChar, 100)};
            parameters[0].Value = programButtonID;
            parameters[1].Value = programID;
            parameters[2].Value = buttonID;
            parameters[3].Value = Url;
            try
            {
                var resultCount = DbHelperSql.ExecuteSql(DbHelperSql.DefaultUpdateConn, strSql.ToString(), parameters);
                if (resultCount == 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 根据程序编号查询按钮
        /// </summary>
        /// <param name="programID">程序编号</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataSet GetList(int programID)
        {
            return this.GetList("Program_ID = " + programID.ToString(), "Button_ID");
        }
        
        public DataSet GetList(string strWhere, string fieldList, int privilegeMaster, string privilegeValue)
        {
            StringBuilder strSql   = new StringBuilder();
            strSql.Append("SELECT ");
            strSql.Append(String.IsNullOrEmpty(fieldList)? "Program_Button_ID , Button_ID , Button_Text , Access_Master , Checked ,  _parentId,Operation_ID,Valid_From,Valid_Until": fieldList);
            strSql.Append(" FROM (SELECT PB.Program_Button_ID , PB.Button_ID , B.Button_Text , 2 AS Access_Master , P.Privilege_ID AS Checked , PB.Program_ID AS _parentId,P.Operation_ID, CONVERT(VARCHAR(12),P.Valid_From) AS Valid_From, CONVERT(VARCHAR(12),P.Valid_Until) AS Valid_Until, ISNULL(P.Is_Identity,0) Is_Identity FROM dbo.tbLOG_Program_Button PB INNER JOIN dbo.tbLOG_Button B ON B.Button_ID = PB.Button_ID LEFT JOIN dbo.tbLOG_Privilege P ON P.Access_Value = PB.Program_Button_ID AND P.Access_Master = 2 AND P.Privilege_Master = " + privilegeMaster.ToString() + " AND P.Privilege_Value = '" + privilegeValue + "') AS tbLOG_Program_Button ");
            if(strWhere != "")
            {
                strSql.Append(" WHERE " + strWhere);
            }
            return  DbHelperSql.Query(DbHelperSql.DefaultQueryConn, strSql.ToString());
        }



        public List<ProgramButtonSearchResultDto> Search(ProgramButtonSearchRequestDto searchRequest, ref GridParams gridParams)
        {
            var query = from programButton in _programButtonRepository.Table
                        join program in _programRepository.Table
                             on programButton.ProgramID equals program.ProgramID
                        join button in _buttonRepository.Table
                             on programButton.ButtonID equals button.ButtonID
                        select new ProgramButtonSearchResultDto{
                            ProgramButtonID = programButton.ProgramButtonID,
                            ProgramID = program.ProgramID,
                            ButtonID = button.ButtonID,
                            ProgramName = program.ProgramName,
                            ButtonName = button.ButtonName,
                            Url = programButton.Url,
                            ButtonText = button.ButtonText
                        };

            query = query.Where(b => !(
                             (searchRequest.ButtonID != 0 && b.ButtonID != searchRequest.ButtonID) ||
                             (searchRequest.ProgramID != 0 && b.ProgramID != searchRequest.ProgramID) ||
                             (searchRequest.ProgramButtonID != 0 && b.ProgramButtonID != searchRequest.ProgramButtonID)
                         ));

            gridParams.TotalCount = query.Count();
            return QueryExtensions.SortAndPage(query, gridParams).ToList();
        }

        #endregion
    }

}