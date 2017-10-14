using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rdp.Service.Dto
{
    public class ProgramButtonSearchRequestDto
    {
        ///<summary>
        /// 程序按钮编号
        ///</summary>
        public int ProgramButtonID { get; set; } // Program_Button_ID (Primary key)

        ///<summary>
        /// 程序编号
        ///</summary>
        public int ProgramID { get; set; } // Program_ID

        ///<summary>
        /// 按钮编号
        ///</summary>
        public int ButtonID { get; set; } // Button_ID
    }
}
