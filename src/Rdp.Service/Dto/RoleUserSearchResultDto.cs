using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rdp.Service.Dto
{
    public class RoleUserSearchResultDto
    {
        ///<summary>
        /// 角色用户编号
        ///</summary>
        public int UserRoleID { get; set; } // User_Role_ID (Primary key)

        ///<summary>
        /// 用户编号
        ///</summary>
        public string UserID { get; set; } // User_ID

        public string UserName { get; set; } // User_ID

        ///<summary>
        /// 角色编号
        ///</summary>
        public short RoleID { get; set; } // Role_ID

        public string RoleDesc { get; set; } // Role_ID


        public RoleUserSearchResultDto()
        {
            UserID = "";
            RoleID = 0;
        }
    }
}
