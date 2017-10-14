namespace Rdp.Core.Data
{
    public class WhereColumn
    {
        private string _columnName;
        private object _columnValue;
        private SqlDataType _columnDataType;
        /// <summary>
        /// 列名
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }
        /// <summary>
        /// 列值
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public object ColumnValue
        {
            get { return _columnValue; }
            set { _columnValue = value; }
        }
        /// <summary>
        /// 列的比较符号
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public SqlDataType ColumnDataType
        {
            get { return _columnDataType; }
            set { _columnDataType = value; }
        }
    }
}
