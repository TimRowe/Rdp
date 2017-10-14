using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace Rdp.Web.Framework.Models
{

   public class ButtonAddModel
    {
        ///<summary>
        /// 按钮编号
        ///</summary>
        public int ButtonID { get; set; } // Button_ID (Primary key)

        ///<summary>
        /// 按钮名称
        ///</summary>
        public string ButtonName { get; set; } // Button_Name

        ///<summary>
        /// 显示文本
        ///</summary>
        public string ButtonText { get; set; } // Button_Text

        ///<summary>
        /// 按钮样式
        ///</summary>
        public string ButtonClass { get; set; } // Button_Class

        ///<summary>
        /// 按钮图标
        ///</summary>
        public string ButtonIcon { get; set; } // Button_Icon

        ///<summary>
        /// 按钮脚本
        ///</summary>
        public string ButtonScript { get; set; } // Button_Script

        ///<summary>
        /// 按钮数据
        ///</summary>
        public string ButtonDataOptions { get; set; } // Button_Data_Options

        ///<summary>
        /// 按钮链接
        ///</summary>
        public string ButtonHref { get; set; } // Button_Href

        ///<summary>
        /// 按钮排序
        ///</summary>
        public short Priority { get; set; } // Priority

        ///<summary>
        /// 有效性
        ///</summary>
        public byte StatusFlag { get; set; } // Status_Flag

    }

}