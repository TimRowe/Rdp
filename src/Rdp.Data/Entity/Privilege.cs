using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    // tbLOG_Privilege
	[Table("tbLOG_Privilege")]
    public partial class Privilege: BaseEntity
    {
        
		[Column("Privilege_ID",TypeName ="int")] [Required] [DatabaseGenerated(DatabaseGeneratedOption.None)]  
		public int PrivilegeID { get; set; } // Privilege_ID (Primary key)
        
		[Column("Valid_From",TypeName ="date")] [Required]   
		public DateTime ValidFrom { get; set; } // Valid_From
        
		[Column("Valid_Until",TypeName ="date")] [Required]   
		public DateTime ValidUntil { get; set; } // Valid_Until
        
		[Column("Privilege_Master",TypeName ="tinyint")] [Required]   
		public byte PrivilegeMaster { get; set; } // Privilege_Master
        
		[Column("Privilege_Value",TypeName ="varchar(20)")] [Required]   
		public string PrivilegeValue { get; set; } // Privilege_Value
        
		[Column("Access_Master",TypeName ="tinyint")] [Required]   
		public byte AccessMaster { get; set; } // Access_Master
        
		[Column("Access_Value",TypeName ="int")] [Required]   
		public int AccessValue { get; set; } // Access_Value
        
		[Column("Branch_Member",TypeName ="smallint")] [Required]   
		public short BranchMember { get; set; } // Branch_Member
        
		[Column("Operation_ID",TypeName ="smallint")] [Required]   
		public short OperationID { get; set; } // Operation_ID
        
		[Column("Is_Identity",TypeName ="tinyint")] [Required]   
		public byte IsIdentity { get; set; } // Is_Identity

        
        public Privilege()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
