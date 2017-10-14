using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    // tbLOG_Branch_Member
	[Table("tbLOG_Branch_Member")]
    public partial class BranchMember: BaseEntity
    {

        ///<summary>
        /// 分店权限
        ///</summary>
        
		[Column("Branch_Member",TypeName ="smallint")] [Required] [DatabaseGenerated(DatabaseGeneratedOption.None)]  
		public short BranchMember_ { get; set; } // Branch_Member (Primary key)

        ///<summary>
        /// 分店权限
        ///</summary>
        
		[Column("Branch_Member_Desc",TypeName ="nvarchar(100)")] [Required]   
		public string BranchMemberDesc { get; set; } // Branch_Member_Desc

        ///<summary>
        /// 选择SQL
        ///</summary>
        
		[Column("Select_Sql",TypeName ="nvarchar(1000)")] [Required]   
		public string SelectSql { get; set; } // Select_Sql

        ///<summary>
        /// 有效性
        ///</summary>
        
		[Column("Status_Flag",TypeName ="tinyint")] [Required]   
		public byte StatusFlag { get; set; } // Status_Flag

        
        public BranchMember()
        {
            BranchMember_ = 0;
            BranchMemberDesc = "0";
            SelectSql = "0";
            StatusFlag = 0;
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
