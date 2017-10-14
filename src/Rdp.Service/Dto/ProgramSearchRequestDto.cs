using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rdp.Service.Dto
{
    public class ProgramSearchRequestDto
    {
        ///<summary>
        /// 程序编号
        ///</summary>
        public int ProgramID { get; set; } // Program_ID (Primary key)

        ///<summary>
        /// 程序名称
        ///</summary>
        public string ProgramName { get; set; } // Program_Name

        ///<summary>
        /// 父程序编号
        ///</summary>
        public int ParentID { get; set; } // Parent_ID
    }
}
