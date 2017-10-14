using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    // tbLOG_User_Master
	[Table("tbLOG_User_Master")]
    public partial class UserMaster: BaseEntity
    {

        ///<summary>
        /// 账号
        ///</summary>
        
		[Column("User_ID",TypeName ="varchar(20)")] [Required] [DatabaseGenerated(DatabaseGeneratedOption.None)]  
		public string UserID { get; set; } // User_ID (Primary key)

        ///<summary>
        /// 用户名
        ///</summary>
        
		[Column("User_Name",TypeName ="nvarchar(100)")] [Required]   
		public string UserName { get; set; } // User_Name

        ///<summary>
        /// 密码
        ///</summary>
        
		[Column("Password",TypeName ="varchar(500)")] [Required]   
		public string Password { get; set; } // Password

        ///<summary>
        /// 部门编号
        ///</summary>
        
		[Column("Branch_Code",TypeName ="int")] [Required]   
		public int BranchCode { get; set; } // Branch_Code

        ///<summary>
        /// IP地址
        ///</summary>
        
		[Column("IP_Address",TypeName ="varchar(200)")] [Required]   
		public string IPAddress { get; set; } // IP_Address

        ///<summary>
        /// 拍卡器类型
        ///</summary>
        
		[Column("Reader_Type",TypeName ="smallint")] [Required]   
		public short ReaderType { get; set; } // Reader_Type

        ///<summary>
        /// 邮箱
        ///</summary>
        
		[Column("Email_Add",TypeName ="nvarchar(200)")] [Required]   
		public string EmailAdd { get; set; } // Email_Add

        ///<summary>
        /// 密码有效日期
        ///</summary>
        
		[Column("Password_Exprity_Date",TypeName ="date")] [Required]   
		public DateTime PasswordExprityDate { get; set; } // Password_Exprity_Date

        ///<summary>
        /// 有效性
        ///</summary>
        
		[Column("Status_Flag",TypeName ="tinyint")] [Required]   
		public byte StatusFlag { get; set; } // Status_Flag

        
        public UserMaster()
        {
            UserName = "0";
            Password = "0";
            BranchCode = 0;
            IPAddress = "0";
            ReaderType = 0;
            EmailAdd = "0";
            StatusFlag = 0;
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
