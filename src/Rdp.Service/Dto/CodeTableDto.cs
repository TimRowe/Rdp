using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Rdp.Service.Dto
{
    public class CodeTableDto
    {
        public string ValueField { get; set; }
        public string TextField { get; set; }
        public string TableName { get; set; }

        /// <summary>
        /// 参数化列表，让CodeTableDto也支持参数化语句
        /// </summary>
        public List<SqlParameter> SqlParameters { get; set; }

        public string Where { get; set; }
        public bool UseCache { get; set; }
        public string Order { get; set; }

        public CodeTableDto()
        {
            ValueField = "";
            TextField = "";
            TableName = "";
            Where = "";
            UseCache = true;
            Order = "";
        }
    }


    public class CodeTableItemDto
    {
        public string id { get; set; }
        public string text { get; set; }

    }


}
