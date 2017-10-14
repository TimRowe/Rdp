using System.Data.Common;
using System.Data.SqlClient;

namespace Rdp.Core.Data
{
    public class SysDbException : DbException
    {
        private string _execSql;

        public SysDbException(string message, string strExecSql, SqlParameter[] sqlParams) : base(message)
        {
            _execSql = strExecSql;

            if(sqlParams != null)
            {
                foreach (var param in sqlParams)
                {
                    if ((param.ParameterName == null) | (param.Value == null)) continue;
                    _execSql += System.Environment.NewLine + param.ParameterName + "=" + param.Value.ToString();
                }
            }
           
        }

        public override string ToString()
        {
            return base.StackTrace;
        }

        public string SqlInfo
        {
            get { return _execSql; }
        }

    }
}
