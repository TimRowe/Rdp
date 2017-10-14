using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    // tbLOG_Role_User
	[Table("tbLOG_Role_User")]
    public partial class RoleUser: BaseEntity
    {
        
		[Column("User_Role_ID",TypeName ="int")] [Required] [DatabaseGenerated(DatabaseGeneratedOption.None)]  
		public int UserRoleID { get; set; } // User_Role_ID (Primary key)
        
		[Column("User_ID",TypeName ="varchar(50)")] [Required]   
		public string UserID { get; set; } // User_ID
        
		[Column("Role_ID",TypeName ="smallint")] [Required]   
		public short RoleID { get; set; } // Role_ID

        
        public RoleUser()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
