using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using Rdp.Core.Data;
using System.Reflection;
using System.Data.SqlClient;
using System.Linq;

namespace Rdp.Core.Data
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SqlWhereAttribute : Attribute
    {
        /// <summary>
        /// 比较的字段，例如 CouponCode > 100
        /// </summary>
        public string CompareField { get; set; }

        /// <summary>
        /// 比较类型
        /// </summary>
        public SqlCompareType CompareType { get; set; }

        public SqlWhereAttribute()
        {
            CompareType = SqlCompareType.Equal;
        }
    }

    public class SqlWhereResult
    {
        public string whereSql;
        public List<SqlParameter> sqlParamList;
    }


    /// <summary>
    ///拼接字符串
    /// </summary>
    /// <remarks></remarks>
    public class WhereSqlHelper
    {
        /// <summary>
        /// 返回HTTP请求的WHERE条件语句
        /// </summary>
        /// <param name="request">HTTP请求</param>
        /// <param name="dic">条件参数数组</param>
        /// <param name="withWhere">返回字符串是否带‘Where’</param>
        /// <returns></returns>
        /// <remarks></remarks>
        /*public static string HttpGetWhere(NameValueCollection request, Dictionary<string, COU.Common.SqlDataType> dic, bool withWhere = true)
        {
            List<WhereColumn> listWhere = new List<WhereColumn>();
            foreach (var item in dic)
            {
                string str = request[item.Key];
                if (!string.IsNullOrEmpty(request[item.Key]))
                {
                    WhereColumn @where = new WhereColumn();
                    @where.ColumnName = item.Key;
                    @where.ColumnValue = request[item.Key];
                    @where.ColumnDataType = (Rdp.Core.Data.SqlDataType)item.Value;
                    listWhere.Add(@where);
                }
            }
            return GetWhere(listWhere, withWhere);
        }*/

        /// <summary>
        /// 根据模型特性进行where语句凭借
        /// </summary>
        /// <history>
        ///  2017-03-28  Tim  Add     
        /// <history>
        public static SqlWhereResult ModelGetWhere<T>(T model, bool withWhere = true)
        {
            PropertyInfo[] propertys = model.GetType().GetProperties();
            var where = withWhere ? " WHERE 1= 1 " : " 1 = 1 ";
            var sqlParamList = new List<SqlParameter>();

            foreach (var p in propertys)
            {
                var attribute = (SqlWhereAttribute)Attribute.GetCustomAttribute(p, typeof(SqlWhereAttribute));
                if (attribute == null)
                    continue;

                var objValue = p.GetValue(model, null);

                //对于一些不是where里面的条件，参数必须传进去
                if (attribute.CompareType == SqlCompareType.None && objValue == null)
                    objValue = string.Empty;

                //判断是否为null
                if (objValue == null) continue;

                var compareField = attribute.CompareField == null ? p.Name : attribute.CompareField;

                //判断该参数之前是否已经设置值
                if(sqlParamList.Find(m => m.ParameterName == "@" + p.Name) == null && attribute.CompareType != SqlCompareType.In)
                {
                    sqlParamList.Add(new SqlParameter("@" + p.Name, objValue));
                }
                
                switch (attribute.CompareType)
                {
                    case SqlCompareType.Equal:
                        where += " AND " + compareField + " = @" + p.Name;
                        break;
                    case SqlCompareType.EqualGreater:
                        where += " AND " + compareField + " >= @" + p.Name;
                        break;
                    case SqlCompareType.EqualLess:
                        where += " AND " + compareField + " <= @" + p.Name;
                        break;
                    case SqlCompareType.Greater:
                        where += " AND " + compareField + " > @" + p.Name;
                        break;
                    case SqlCompareType.In:
                        {
                            var whereStr = "";
                            var itemList = objValue.ToString().Split(',');

                            for(var i = 0; i <= itemList.Length - 1; ++i)
                            {
                                whereStr += (i == 0?"":",") + "@" +p.Name + i.ToString();

                                //判断该参数之前是否已经设置值
                                if (sqlParamList.Find(m => m.ParameterName == "@" + p.Name + i.ToString()) == null)
                                    sqlParamList.Add(new SqlParameter("@" + p.Name + i.ToString(), itemList[i]));
                            }

                            where += " AND " + compareField + " IN (" + whereStr + ")";
                        }
                       
                        break;
                    case SqlCompareType.Less:
                        where += " AND " + compareField + " < @" + p.Name;
                        break;
                    case SqlCompareType.NotEqual:
                        where += " AND " + compareField + " <> @" + p.Name;
                        break;
                }
            }

            return new SqlWhereResult() { whereSql = where, sqlParamList = sqlParamList };
        }


        public static string HttpGetWhere(NameValueCollection request, Dictionary<string, SqlDataType> dic, bool withWhere = true)
        {
            List<WhereColumn> listWhere = new List<WhereColumn>();
            foreach (var item in dic)
            {
                string str = request[item.Key];
                if (!string.IsNullOrEmpty(request[item.Key]))
                {
                    WhereColumn @where = new WhereColumn();
                    @where.ColumnName = item.Key;
                    @where.ColumnValue = request[item.Key];
                    @where.ColumnDataType = (Rdp.Core.Data.SqlDataType)item.Value;
                    listWhere.Add(@where);
                }
            }
            return GetWhere(listWhere, withWhere);
        }

        /// <summary>
        /// 返回多个条件的SQL语句 
        /// </summary>
        /// <param name="listWhere"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetWhere(List<WhereColumn> listWhere, bool withWhere)
        {
            if (listWhere.Count == 0)
            {
                return "";
            }
            else
            {
                StringBuilder str = new StringBuilder();
                foreach (WhereColumn item in listWhere)
                {
                    if (withWhere)
                    {
                        str.Append(" WHERE ");
                        withWhere = false;
                        str.Append(AppendWhere(item));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(str.ToString()))
                        {
                            str.Append(" AND ");
                        }
                        str.Append(AppendWhere(item));
                    }

                }
                if (withWhere)
                {
                    return " Where +" + str.ToString();
                }
                else
                {
                    return str.ToString();
                }
            }
        }
        /// <summary>
        /// 返回Where类的条件字符串
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static StringBuilder AppendWhere(WhereColumn column)
        {
            StringBuilder reString = new StringBuilder();
            reString.Append(column.ColumnName);
            string @where = null;
            switch (column.ColumnDataType)
            {
                case SqlDataType.IntBetween:
                    string[] str = column.ColumnValue.ToString().Split(',');
                    @where = " BETWEEN" + str[0] + " AND " + str[1];
                    break;
                case SqlDataType.IntEqual:
                    @where = "=" + column.ColumnValue;
                    break;
                case SqlDataType.IntEqualGreater:
                    @where = ">=" + column.ColumnValue;
                    break;
                case SqlDataType.IntEqualLess:
                    @where = "<=" + column.ColumnValue;
                    break;
                case SqlDataType.IntFrom:
                    StringBuilder sb1 = new StringBuilder();
                    sb1.Append(reString.ToString().Substring(0,reString.Length - 5));
                    sb1.Append(">=");
                    sb1.Append(column.ColumnValue);
                    return sb1;
                case SqlDataType.IntGreater:
                    @where = ">" + column.ColumnValue;
                    break;
                case SqlDataType.IntIn:
                    @where = " IN (" + column.ColumnValue + ")";
                    break;
                case SqlDataType.IntLess:
                    @where = "<" + column.ColumnValue;
                    break;
                case SqlDataType.IntNotEqual:
                    @where = "<>" + column.ColumnValue;
                    break;
                case SqlDataType.IntTo:
                        StringBuilder sb2 = new StringBuilder();
                        sb2.Append(reString.ToString().Substring(0, reString.Length - 3));
                        sb2.Append("<=");
                        sb2.Append(column.ColumnValue);
                        return sb2;
                case SqlDataType.NVarcharBetween:
                    string[] str1 = column.ColumnValue.ToString().Split(',');
                    @where = " BETWEEN N'" + str1[0] + "' AND N'" + str1[1] + "'";
                    break;
                case SqlDataType.NVarcharEqual:
                    @where = "= N'" + column.ColumnValue + "'";
                    break;
                case SqlDataType.NVarcharEqualGreater:
                    @where = ">= N'" + column.ColumnValue + "'";
                    break;
                case SqlDataType.NVarcharEqualLess:
                    @where = "<= N'" + column.ColumnValue + "'";
                    break;
                case SqlDataType.NVarcharFrom:
                    StringBuilder sb = new StringBuilder();
                    sb.Append(reString.ToString().Substring(0,reString.Length - 5));
                    sb.Append(">= N'");
                    sb.Append(column.ColumnValue);
                    sb.Append("'");
                    return sb;
                case SqlDataType.NVarcharGreater:
                    @where = "> N'" + column.ColumnValue + "'";
                    break;
                case SqlDataType.NVarcharIn:
                    @where = " IN (N'" + column.ColumnValue.ToString().Replace(",", "',N'") + "')";
                    break;
                case SqlDataType.NVarcharLess:
                    @where = "< N'" + column.ColumnValue + "'";
                    break;
                case SqlDataType.NVarcharLike:
                    @where = " LIKE N'%" + column.ColumnValue + "%'";
                    break;
                case SqlDataType.NVarcharRightLike:
                    @where = " LIKE N'" + column.ColumnValue + "%'";
                    break;
                case SqlDataType.NVarcharUpperLike:
                    @where = " LIKE UPPER(N'%" + column.ColumnValue + "%')";
                    break;
                case SqlDataType.NVarcharNotEqual:
                    @where = "<> N'" + column.ColumnValue + "'";
                    break;
                case SqlDataType.NVarcharTo:
                    StringBuilder sb4 = new StringBuilder();
                    sb4.Append(reString.ToString().Substring(0, reString.Length - 3));
                    sb4.Append("<= N'");
                    sb4.Append(column.ColumnValue);
                    sb4.Append("'");
                    return sb4;
                case SqlDataType.VarcharBetween:
                    string[] str2 = column.ColumnValue.ToString().Split(',');
                    @where = " BETWEEN '" + str2[0] + "' AND '" + str2[1] + "'";
                    break;
                case SqlDataType.VarcharEqual:
                    @where = "= '" + column.ColumnValue + "'";
                    break;
                case SqlDataType.VarcharEqualGreater:
                    @where = ">= '" + column.ColumnValue + "'";
                    break;
                case SqlDataType.VarcharEqualLess:
                    @where = "<= '" + column.ColumnValue + "'";
                    break;
                case SqlDataType.VarcharFrom:
                    StringBuilder sb6 = new StringBuilder();
                    sb6.Append(reString.ToString().Substring(0, reString.Length - 5));
                    sb6.Append(">= '");
                    sb6.Append(column.ColumnValue);
                    sb6.Append("'");
                    return sb6;
                case SqlDataType.VarcharGreater:
                    @where = "> '" + column.ColumnValue + "'";
                    break;
                case SqlDataType.VarcharIn:
                    @where = " IN ('" + column.ColumnValue.ToString().Replace(",", "','") + "')";
                    break;
                case SqlDataType.VarcharLess:
                    @where = "< '" + column.ColumnValue + "'";
                    break;
                case SqlDataType.VarcharLike:
                    @where = " LIKE '%" + column.ColumnValue + "%'";
                    break;
                case SqlDataType.VarcharRightLike:
                    @where = " LIKE '" + column.ColumnValue + "%'";
                    break;
                case SqlDataType.VarcharNotEqual:
                    @where = "<> '" + column.ColumnValue + "'";
                    break;
                case SqlDataType.VarcharTo:
                    StringBuilder sb7 = new StringBuilder();
                    sb7.Append(reString.ToString().Substring(0,reString.Length - 3));
                    try
                    {
                        System.Convert.ToDateTime(column.ColumnValue);
                        sb7.Append("< DATEADD(DAY,1,'");
                        sb7.Append(column.ColumnValue);
                        sb7.Append("')");
                    }
                    catch (Exception)
                    {
                        sb7.Append("<= '");
                        sb7.Append(column.ColumnValue);
                        sb7.Append("'");
                    }
                    return sb7;
            }
            return reString.Append(@where);
        }
    }
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
