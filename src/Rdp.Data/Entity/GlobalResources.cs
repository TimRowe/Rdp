using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    // tbCOM_Global_Resources
	[Table("tbCOM_Global_Resources")]
    public partial class GlobalResources: BaseEntity
    {
        
		[Column("Id",TypeName ="uniqueidentifier")] [Required] [DatabaseGenerated(DatabaseGeneratedOption.None)]  
		public Guid Id { get; set; } // Id (Primary key)
        
		[Column("Name",TypeName ="nvarchar(50)")] [Required]   
		public string Name { get; set; } // Name
        
		[Column("Lanuage",TypeName ="varchar(20)")] [Required]   
		public string Lanuage { get; set; } // Lanuage
        
		[Column("Value",TypeName ="nvarchar(1000)")] [Required]   
		public string Value { get; set; } // Value
        
		[Column("Update_Time",TypeName ="datetime2")] [Required]   
		public DateTime UpdateTime { get; set; } // Update_Time

        
        public GlobalResources()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
