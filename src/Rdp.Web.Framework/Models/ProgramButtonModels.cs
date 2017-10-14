using Rdp.Web.Framework.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace Rdp.Web.Framework.Models
{
    
    public class ProgramButtonAddModel
    {
        ///<summary>
        /// 程序按钮编号
        ///</summary>
        public int ProgramButtonID { get; set; } // Program_Button_ID (Primary key)


        /// <summary>
        /// 父程序ID
        /// </summary>
        public int ParentProgramID { get; set; }


        ///<summary>
        /// 程序编号
        ///</summary>
        [Required]
        public int ProgramID { get; set; } // Program_ID

        ///<summary>
        /// 按钮编号
        ///</summary>
        [Required]
        public int ButtonID { get; set; } // Button_ID

        /// <summary>
        /// 按钮请求的URL
        /// </summary>
        public string Url { get; set; } // Url


        public ProgramButtonAddModel()
        {
            Url = "0";
        }

    }

}