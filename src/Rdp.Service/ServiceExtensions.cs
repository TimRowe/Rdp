using Rdp.Core.Caching;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Rdp.Core.Util;
using Rdp.Core.Data;
using Rdp.Core.Dependency;


namespace Rdp.Service
{
    delegate int SwitchUpdateDbExcuteFun();

    public enum DbOperation
    {
        Read,
        Write
    }

    public static class ServiceExtensions
    {
        public static bool SwitchToUpdateDb<T>(this IService<T> service, SwitchDbExcuteFun fun) where T : BaseEntity
        {
            return service.UseRepository.SwitchDb(fun, EDbType.UpdateDb);
        }
        
        public static bool Add<T>(this IService<T> service, T model) where T : BaseEntity
        {
            service.UseRepository.Insert(model);
            return true;
        }

        public static bool Update<T>(this IService<T> service, T model) where T : BaseEntity
        {
            service.UseRepository.Update(model);
            return true;
        }

        public static bool Delete<T>(this IService<T> service, T model) where T : BaseEntity
        {
            service.UseRepository.Delete(model);
            return true;
        }

        public static T GetModel<T>(this IService<T> service, params object[] keyValues) where T : BaseEntity
        {
            return service.UseRepository.Find(keyValues);
        }

        public static T GetModel<T>(this IService<T> service, Expression<Func<T, bool>> match) where T : BaseEntity
        {
            return service.UseRepository.Table.Where(match).FirstOrDefault();
        }

        public static List<T> GetModels<T>(this IService<T> service, Expression<Func<T, bool>>  match) where T : BaseEntity
        {
            return service.UseRepository.Table.Where(match).ToList();
        }

        public static T GetCachedModel<T>(this IService<T> service, params object[] pkeyValues) where T : BaseEntity
        {
            var cacheKey = service.UseRepository.GetTableName();
            foreach (var i in pkeyValues)
            {
                cacheKey = cacheKey + "_" + i.ToString();
            }

            ICacheManager _cacheManager = IocObjectManager.GetInstance().Resolve<IHttpContextCacheManager>();
            T item = _cacheManager.Get(cacheKey, 60, () =>
            {
                return service.GetModel(pkeyValues);
            });

            return item;
        }


        public static T GetCachedModel<T>(this IService<T> service, Predicate<T> match) where T : BaseEntity
        {
            var cacheKey = "GeneralTable_" + service.UseRepository.GetTableName().Replace("dbo.", "");
            var versionControlService = IocObjectManager.GetInstance().Resolve<IVersionControlService>();

            ICacheManager _cacheManager = IocObjectManager.GetInstance().Resolve<IHttpContextCacheManager>();
            List<T> allItems = _cacheManager.Get(cacheKey, 60, () =>
            {
                return service.UseRepository.Table.ToList();
            }, () => { return versionControlService.GetVersionFlag(cacheKey); });

            return allItems.Find(match);
        }

        public static List<T> GetCachedModels<T>(this IService<T> service, Func<T,bool> match) where T : BaseEntity
        {
            var cacheKey = "GeneralTable_" + service.UseRepository.GetTableName().Replace("dbo.", "");
            var versionControlService = IocObjectManager.GetInstance().Resolve<IVersionControlService>();

            ICacheManager _cacheManager = IocObjectManager.GetInstance().Resolve<IHttpContextCacheManager>();
            List<T> allItems = _cacheManager.Get(cacheKey, 60, () =>
            {
                return service.UseRepository.Table.ToList();
            }, () => { return versionControlService.GetVersionFlag(cacheKey); });

            return allItems.Where(match).ToList();
        }

        public static DataSet GetList<T>(this IService<T> service, String strWhere, String filedList)  where T : BaseEntity
        {
            var strSql = String.Format("SELECT {0} FROM {1} WHERE {2}", String.IsNullOrEmpty(filedList) ? " * " : filedList, service.UseRepository.GetTableName(), strWhere);
            return DbHelperSql.Query(DbHelperSql.DefaultQueryConn, strSql.ToString());
        }


        public static DataSet GetList<T>(this IService<T> service, String strWhere, String filedList, String strInner)  where T : BaseEntity
        {
            var strSql = String.Format("SELECT {0} FROM {1} {3} WHERE {2}", String.IsNullOrEmpty(filedList) ? " * " : filedList, service.UseRepository.GetTableName(), strWhere, strInner);
            return DbHelperSql.Query(DbHelperSql.DefaultQueryConn, strSql.ToString());
        }


        /// <summary>
        /// 返回查询Json格式 
        /// </summary>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetJsonByPage<T>(this IService<T> service, PageParam pageParam) where T : BaseEntity
        {
            int nTotalCount = 0;
            var ds = service.GetListByPage(pageParam, ref nTotalCount);
            return JSONHelper.ToJson(ds.Tables[0], nTotalCount);
        }


        /// <summary>
        /// 返回查询Dataset
        /// </summary>
        /// <param name="pageParam"></param>
        /// <param name="total">总行数</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DataSet GetListByPage<T>(this IService<T> service, PageParam pageParam, ref int total) where T : BaseEntity
        {

            SqlParameter[] parameters = {
	            new SqlParameter("@TableName", DbType.AnsiString),
	            new SqlParameter("@FieldList", SqlDbType.NVarChar, 2000),
	            new SqlParameter("@PrimaryKey", SqlDbType.NVarChar, 100),
	            new SqlParameter("@Where", SqlDbType.NVarChar, 2000),
	            new SqlParameter("@Order", SqlDbType.VarChar, 100),
	            new SqlParameter("@PageSize", SqlDbType.Int),
	            new SqlParameter("@PageIndex", SqlDbType.Int),
	            new SqlParameter("@TotalCount", SqlDbType.Int)
            };
            parameters[0].Value = pageParam.TableName;
            parameters[1].Value = pageParam.FieldList;
            parameters[2].Value = pageParam.PrimaryKey;
            parameters[3].Value = pageParam.Where;
            parameters[4].Value = pageParam.Order;
            parameters[5].Value = pageParam.pageSize;
            parameters[6].Value = pageParam.pageIndex;
            parameters[7].Direction = ParameterDirection.Output;
            DataSet ds = DbHelperSql.RunProcedure(DbHelperSql.DefaultQueryConn, "SP_COM_QUERY_BY_PAGE", parameters, "ds");
            total = Convert.ToInt32(parameters[7].Value);
            return ds;
        }

        /// <summary>
        /// 将集合类转换成DataTable
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IService<T> service, IList list) where T : BaseEntity
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {

                        object obj = pi.GetValue(list[i], null) != null && pi.GetValue(list[i], null).GetType() == typeof(DateTime) ?
                            ((DateTime)pi.GetValue(list[i], null)).ToString("yyyy-MM-dd HH:mm:ss") :
                            pi.GetValue(list[i], null);

                        tempList.Add(obj);


                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }




        /// <summary>
        /// 将集合转换成datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        /// <param name="list">要转换的list</param>
        /// <param name="fieldList">指定的列（不在本参数的列不予转换）</param>
        /// <returns></returns>
        /// Tim 2016-09-18 新增
        /// Tim 2016-11-08 列的顺序以fieldList的为准
        public static DataTable ToDataTable<T>(this IService<T> service, IList list, List<GridColumn> fieldList) where T : BaseEntity
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();

                foreach (var field in fieldList)
                {
                    var item = propertys.Where(i=>i.Name == field.Field);
                    if (item != null)
                    {
                        result.Columns.Add(new DataColumn()
                        {
                            ColumnName = field.Field,
                            Caption = field.Title
                        });
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (var field in fieldList)
                    {
                        PropertyInfo item = propertys.Where(o => o.Name == field.Field).FirstOrDefault();
                        if (item != null)
                        {
                            object obj = item.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }


        /// <summary>
        /// 根据查询语句返回DataTable
        /// </summary>
        /// <param name="str">查询语句</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DataTable Query<T>(this IService<T> service, string str) where T : BaseEntity
        {
            return DbHelperSql.Query(DbHelperSql.DefaultQueryConn, str, int.MaxValue).Tables[0];
        }


        /// <summary>
        /// 将pageParam条件得到的Datatable导出Exel
        /// </summary>
        /// <param name="pageParam"></param>
        /// <param name="path">文件路径</param>
        /// <remarks></remarks>
        public static void BatchExport<T>(this IService<T> service, PageParam pageParam, string path) where T : BaseEntity
        {
            DataTable dt = default(DataTable);
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append(pageParam.FieldList);
            sb.Append(" FROM ");
            sb.Append(pageParam.TableName);
            sb.Append(pageParam.Where);
            dt = service.Query(sb.ToString());
            NPOIHelper.ExportEasy(dt, path);
        }


        /// <summary>
        /// 将pageParam条件生成DataTable
        /// </summary>
        /// <param name="pageParam"></param>
        /// <remarks></remarks>
        public static DataTable CreateDataTable<T>(this IService<T> service, PageParam pageParam) where T : BaseEntity
        {
            DataTable dt = default(DataTable);
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append(pageParam.FieldList);
            sb.Append(" FROM ");
            sb.Append(pageParam.TableName);
            sb.Append(" WHERE ");
            sb.Append(pageParam.Where);
            dt = service.Query(sb.ToString());
            return dt;
        }


        public static DataSet RunProcedure<T>(this IService<T> service, 
                        DbOperation opt,
                        String storedProcName,
                        IDataParameter[] parameters,
                        int timeout = 3000) where T : BaseEntity
        {
            return DbHelperSql.RunProcedure(opt == DbOperation.Read ? DbHelperSql.DefaultQueryConn : DbHelperSql.DefaultUpdateConn,
                    storedProcName,
                    parameters, "tb1", timeout);
        }



    }
}
