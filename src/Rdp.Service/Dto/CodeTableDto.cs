using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rdp.Service.Dto
{
    public class CodeTableDto
    {
        public string ValueField { get; set; }
        public string TextField { get; set; }
        public string TableName { get; set; }
        public string Where { get; set; }
        public bool UseCache { get; set; }
      
        public CodeTableDto()
        {
            ValueField = "";
            TextField = "";
            TableName = "";
            Where = "";
            UseCache = true;
        }
    }


    public class CodeTableItemDto
    {
        public string id { get; set; }
        public string text { get; set; }

    }


}
