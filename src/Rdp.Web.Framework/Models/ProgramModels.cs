using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace Rdp.Web.Framework.Models
{

   public class ProgramAddModel
    {

        ///<summary>
        /// 程序编号
        ///</summary>
        public int ProgramID { get; set; } // Program_ID (Primary key)

        ///<summary>
        /// 程序名称
        ///</summary>
        [Required]
        public string ProgramName { get; set; } // Program_Name

        ///<summary>
        /// 父程序编号
        ///</summary>
        public int ParentID { get; set; } // Parent_ID

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
        [Required]
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