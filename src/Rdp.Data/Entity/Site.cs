using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    // tbLOG_Site
	[Table("tbLOG_Site")]
    public partial class Site: BaseEntity
    {
        
		[Column("Site_ID",TypeName ="tinyint")] [Required] [DatabaseGenerated(DatabaseGeneratedOption.None)]  
		public byte SiteID { get; set; } // Site_ID (Primary key)
        
		[Column("Site_Desc",TypeName ="varchar(50)")] [Required]   
		public string SiteDesc { get; set; } // Site_Desc
        
		[Column("Status_Flag",TypeName ="tinyint")] [Required]   
		public byte StatusFlag { get; set; } // Status_Flag

        
        public Site()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
