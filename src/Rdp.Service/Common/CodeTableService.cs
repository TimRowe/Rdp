using Rdp.Core.Caching;
using Rdp.Core.Data;
using Rdp.Core.Dependency;
using Rdp.Data;
using Rdp.Data.Entity;
using Rdp.Service.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Rdp.Service
{
    ///<summary>
    ///基础说明表
    ///</summary>
    public partial class CodeTableService : ICodeTableService
    {
        IRepository<CodeTable> _codeTableRepository;
        IVersionControlService _versionControlService;

        public IRepository<CodeTable> UseRepository
        {
            get
            {
                return _codeTableRepository;
            }
        }

        public CodeTableService(): this(
            RepositoryFactory.Create<CodeTable>(),
            new VersionControlService())

        {
        }

        public CodeTableService(
            IRepository<CodeTable> codeTableRepository, 
            IVersionControlService versionControlService)
        {
            _codeTableRepository = codeTableRepository;
            _versionControlService = versionControlService;
        }
      
        #region "ExtensionMethod"
        /// <summary>
        ///返回下拉框数据 
        /// </summary>
        /// <param name="tableName">码表名称</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable GetCombobox(string tableName)
        {
            var sb  = new  StringBuilder();
            sb.Append("DECLARE @SQL NVARCHAR(500)SELECT @SQL = Select_Sql FROM tbCOM_Code_Table WHERE Table_Name = '");
            sb.Append(tableName);
            sb.Append("' EXEC(@SQL)");
            return DbHelperSql.Query(DbHelperSql.DefaultQueryConn, sb.ToString()).Tables[0];
        }

        /// 返回codeTable缓存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<CodeTableItemDto> GetCodeTable(CodeTableDto model)
        {
            var cacheKey = "CodeTable_" + model.TableName;
            var strSql = "";

            List<SqlParameter> parameters = new List<SqlParameter>(){  };

            if(model.SqlParameters != null && model.SqlParameters.Count >= 0)
            {
                model.SqlParameters.ForEach(m => parameters.Add(m));
                strSql = this.GetModel(m => m.TableName == model.TableName).SelectSql;

                if (!string.IsNullOrEmpty(model.Where))
                    strSql = string.Format("SELECT * FROM ({0}) AS temp WHERE  {1} {2}", strSql, model.Where, model.Order);
            }
            else
            {
                strSql = "DECLARE @SQL NVARCHAR(2000) SELECT @SQL = Select_Sql FROM tbCOM_Code_Table WHERE Table_Name = @p1989; ";
                parameters.Add(new SqlParameter("@p1989", model.TableName));

                if (!string.IsNullOrEmpty(model.Where))
                    strSql += string.Format(" SELECT @SQL=N'SELECT * FROM ('+@SQL+N') AS temp WHERE  {0}' " + model.Order, model.Where);

                strSql += " EXEC(@SQL) ";
            }

            Func<List<CodeTableItemDto>> loadFun = () =>
            {
                var data = DbHelperSql.Query(DbHelperSql.CouQuery, strSql, parameters.ToArray());
                return data != null && data.Tables.Count > 0 ? Conversion.ConvertToList<CodeTableItemDto>(data.Tables[0]) : null;
            };

            if (model.UseCache && string.IsNullOrEmpty(model.Where))
            {
                ICacheManager cacheManager = IocObjectManager.GetInstance().Resolve<IHttpContextCacheManager>();
                return cacheManager.Get(cacheKey, 60, loadFun, () => { return _versionControlService.GetVersionFlag(cacheKey); });
            }
            else
            {
                return loadFun();
            }
        }


        public List<CodeTableItemDto> GetGeneralTable(CodeTableDto model)
        {
            string strSql = String.Format("SELECT CONVERT(NVARCHAR(100),{0}) id,{1} text FROM {2} WHERE 1=1 {3} " + model.Order,
               model.ValueField, model.TextField, model.TableName, string.IsNullOrEmpty(model.Where) ? "" : " and " + model.Where);
            return _codeTableRepository.SqlQuery<CodeTableItemDto>(strSql);
        }


        /// 返回Table的所有行
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable GetCodeTable(string tableName)
        {
            return  DbHelperSql.Query(DbHelperSql.DefaultQueryConn, "SELECT * FROM " + tableName).Tables[0];
        }
        /// <summary>
        /// 保存码表，涉及增、删、改操作
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Save(Hashtable hs)
        {
             DbHelperSql.ExecuteSqlTran(DbHelperSql.DefaultQueryConn, hs);
             return true;
        }
        #endregion
    }
}


