using System.Collections.Generic;
using System.Data.SqlClient;

namespace Rdp.Core.Data
{
    public partial class PageParam
    {
        public PageParam()
        {
            _totalCount = 0;
        }

        //预执行Sql，例如一些变量定义，避免在分页部分执行的语句。
        private string _preSql; 
        private string _tableName;
        private string _fieldList = "*";
        private string _primaryKey;
        private string _where = "";
        private string _order;
        private int _pageSize;
        private int _pageIndex;

        private int _totalCount;
        private List<SqlParameter> _sqlParamList;

        /// <summary>
        /// 总共条数
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int TotalCount
        {
            get { return _totalCount; }
            set { _totalCount = value; }
        }


        /// <summary>
        /// 表名或视图
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }
        /// <summary>
        /// 显示列名，如果是全部字段则为* 
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string FieldList
        {
            get { return _fieldList; }
            set { _fieldList = value; }
        }
        /// <summary>
        /// 单一主键或唯一值键 
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string PrimaryKey
        {
            get
            {
                if (string.IsNullOrEmpty(_primaryKey) && !string.IsNullOrEmpty(_fieldList) && _fieldList.IndexOf(",") >=0)
                {
                    return _fieldList.Substring(0, _fieldList.IndexOf(","));
                }
                else
                {
                    return _primaryKey;
                }
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _primaryKey =_fieldList.Substring(0, _fieldList.IndexOf(","));
                }
                else
                {
                    _primaryKey = value;
                }
            }
        }
        /// <summary>
        /// 查询条件 不含'where'字符，如id>10 and len(userid)>9 
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Where
        {
            get { return _where; }
            set { _where = value; }
        }
        /// <summary>
        /// 排序 不含'order by'字符，如id asc,userid desc，必须指定asc或desc --注意当@SortType=3时生效，记住一定要在最后加上主键，否则会让你比较郁闷 
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Order
        {
            get
            {
                if (string.IsNullOrEmpty(_order) && _fieldList.Contains(","))
                {
                    return _fieldList.Substring(0, _fieldList.IndexOf(","));

                }
                else
                {
                    return _order;
                }
            }
            set
            {
                if (string.IsNullOrEmpty(value) && _fieldList.Contains(","))
                {
                    _order = _fieldList.Substring(0, _fieldList.IndexOf(","));
                }
                else
                {
                    _order = value;
                }
            }
        }
        /// <summary>
        /// 每页输出的记录数 
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int pageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }
        /// <summary>
        /// 当前页数
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int pageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }


        /// <summary>
        /// 当前页数
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string PreSql
        {
            get { return _preSql; }
            set { _preSql = value; }
        }

        /// <summary>
        /// Sql查询参数
        /// </summary>
        public List<SqlParameter> SqlParamList
        {
            get { return _sqlParamList; }
            set { _sqlParamList = value; }
        }
    }
}


