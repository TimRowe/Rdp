using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rdp.Service.Dto
{
   public  class PrivilegeDto
    {

        ///<summary>
        /// 权限编号
        ///</summary>
        public int PrivilegeID { get; set; } // Privilege_ID (Primary key)

        ///<summary>
        /// 开始日期
        ///</summary>
        public DateTime ValidFrom { get; set; } // Valid_From

        ///<summary>
        /// 结束日期
        ///</summary>
        public DateTime ValidUntil { get; set; } // Valid_Until

        ///<summary>
        /// 1为Role2为User
        ///</summary>
        public byte PrivilegeMaster { get; set; } // Privilege_Master

        ///<summary>
        /// 值
        ///</summary>
        public string PrivilegeValue { get; set; } // Privilege_Value

        ///<summary>
        /// 1为Program2为Button3为Branch
        ///</summary>
        public byte AccessMaster { get; set; } // Access_Master

        ///<summary>
        /// 值
        ///</summary>
        public int AccessValue { get; set; } // Access_Value

        ///<summary>
        /// 分店范围
        ///</summary>
        public short BranchMember { get; set; } // Branch_Member

        ///<summary>
        /// 操作权限
        ///</summary>
        public short OperationID { get; set; } // Operation_ID

        ///<summary>
        /// 是否需身份确认，0:不支持 1：支持，未启用身份验证 2：支持，启用身份验证
        ///</summary>
        public byte IsIdentity { get; set; } // Is_Identity

       ///<summary>
        /// 程序URL
        ///</summary>
        public string Url { get; set; } // Url

        public PrivilegeDto()
        { }
    }
}
