using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    // tbCOM_Announcement
	[Table("tbCOM_Announcement")]
    public partial class Announcement: BaseEntity
    {
        
		[Column("Announcement_ID",TypeName ="int")] [Required] [DatabaseGenerated(DatabaseGeneratedOption.None)]  
		public int AnnouncementID { get; set; } // Announcement_ID (Primary key)
        
		[Column("Announcement_Name",TypeName ="nvarchar(500)")] [Required]   
		public string AnnouncementName { get; set; } // Announcement_Name
        
		[Column("Announcement_Url",TypeName ="nvarchar(1000)")] [Required]   
		public string AnnouncementUrl { get; set; } // Announcement_Url
        
		[Column("District",TypeName ="varchar(100)")] [Required]   
		public string District { get; set; } // District
        
		[Column("Create_By",TypeName ="varchar(50)")] [Required]   
		public string CreateBy { get; set; } // Create_By
        
		[Column("Create_Time",TypeName ="datetime")] [Required]   
		public DateTime CreateTime { get; set; } // Create_Time
        
		[Column("Status_Flag",TypeName ="smallint")] [Required]   
		public short StatusFlag { get; set; } // Status_Flag

        
        public Announcement()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
