using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rdp.Service.Dto
{
    public class ProgramButtonSearchResultDto
    {
        ///<summary>
        /// 程序按钮编号
        ///</summary>
        public int ProgramButtonID { get; set; } // Program_Button_ID (Primary key)

        public int ButtonID { get; set; }

        public int ProgramID { get; set; }

        ///<summary>
        /// 程序编号
        ///</summary>
        public string ProgramName { get; set; } // Program_ID

        ///<summary>
        /// 按钮编号
        ///</summary>
        public string ButtonName { get; set; } // Button_ID

        ///<summary>
        /// 按钮编号
        ///</summary>
        public string ButtonText { get; set; } // Button_ID

        public string Url { get; set; } // Button_ID
    }
}
