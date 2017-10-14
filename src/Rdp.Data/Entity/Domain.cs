using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    // tbCOM_Domain
	[Table("tbCOM_Domain")]
    public partial class Domain: BaseEntity
    {
        
		[Column("Domain_ID",TypeName ="smallint")] [Required] [DatabaseGenerated(DatabaseGeneratedOption.None)]  
		public short DomainID { get; set; } // Domain_ID (Primary key)
        
		[Column("Domain_Desc",TypeName ="nvarchar(10)")] [Required]   
		public string DomainDesc { get; set; } // Domain_Desc
        
		[Column("Default_Language",TypeName ="varchar(20)")] [Required]   
		public string DefaultLanguage { get; set; } // Default_Language
        
		[Column("Sector_Staff",TypeName ="smallint")] [Required]   
		public short SectorStaff { get; set; } // Sector_Staff
        
		[Column("Version",TypeName ="int")] [Required]   
		public int Version { get; set; } // Version
        
		[Column("Status_Flag",TypeName ="tinyint")] [Required]   
		public byte StatusFlag { get; set; } // Status_Flag

        
        public Domain()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
