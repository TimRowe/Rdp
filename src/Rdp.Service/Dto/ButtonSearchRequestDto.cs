using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rdp.Service.Dto
{
    public class ButtonSearchRequestDto
    {
        ///<summary>
        /// 程序编号
        ///</summary>
        public int ButtonID { get; set; } // Program_ID (Primary key)

        ///<summary>
        /// 程序名称
        ///</summary>
        public string ButtonName { get; set; } // Program_Name

        ///<summary>
        /// 显示文本
        ///</summary>
        public string ButtonText { get; set; } // Button_Text

        ///<summary>
        /// 有效性
        ///</summary>
        public byte StatusFlag { get; set; } // Status_Flag
    }
}
