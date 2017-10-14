using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    // tbLOG_Role_Master
	[Table("tbLOG_Role_Master")]
    public partial class RoleMaster: BaseEntity
    {
        
		[Column("Role_ID",TypeName ="smallint")] [Required] [DatabaseGenerated(DatabaseGeneratedOption.None)]  
		public short RoleID { get; set; } // Role_ID (Primary key)
        
		[Column("Role_Desc",TypeName ="nvarchar(20)")] [Required]   
		public string RoleDesc { get; set; } // Role_Desc
        
		[Column("Is_Validate",TypeName ="tinyint")] [Required]   
		public byte IsValidate { get; set; } // Is_Validate
        
		[Column("Status_Flag",TypeName ="tinyint")] [Required]   
		public byte StatusFlag { get; set; } // Status_Flag

        
        public RoleMaster()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
