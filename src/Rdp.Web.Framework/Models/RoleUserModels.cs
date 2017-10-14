using Rdp.Data.Entity;
using Rdp.Web.Framework.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace Rdp.Web.Framework.Models
{
    public class RoleUserAddModel
    {
        ///<summary>
        /// 角色用户编号
        ///</summary>
        public int UserRoleID { get; set; } // User_Role_ID (Primary key)

        ///<summary>
        /// 用户编号
        ///</summary>
        [Required]
        public string UserID { get; set; } // User_ID

        ///<summary>
        /// 角色编号
        ///</summary>
        [Required]
        public short RoleID { get; set; } // Role_ID


        public RoleUserAddModel()
        {
            UserID = "0";
            RoleID = 0;
        }
    }
}