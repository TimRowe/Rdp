using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rdp.Service.Dto
{

    public class ProgramSearchResultDto
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

        ///<summary>
        /// 父程序名称
        ///</summary>
        public string ParentName { get; set; } // Parent_ID


        ///<summary>
        /// 程序URL
        ///</summary>
        public string Url { get; set; } // Url

        ///<summary>
        /// 图片地址
        ///</summary>
        public string Icon { get; set; } // Icon

        ///<summary>
        /// 可见性
        ///</summary>
        public byte IsVisible { get; set; } // Is_Visible

        ///<summary>
        /// 排序编号
        ///</summary>
        public short Priority { get; set; } // Priority

        ///<summary>
        /// 有效性
        ///</summary>
        public byte StatusFlag { get; set; } // Status_Flag
    }
}
