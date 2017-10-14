using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    // tbCOM_Job
	[Table("tbCOM_Job")]
    public partial class Job: BaseEntity
    {
        
		[Column("Job_ID",TypeName ="int")] [Required] [DatabaseGenerated(DatabaseGeneratedOption.None)]  
		public int JobID { get; set; } // Job_ID (Primary key)
        
		[Column("Parent_ID",TypeName ="int")] [Required]   
		public int ParentID { get; set; } // Parent_ID
        
		[Column("Job_Name",TypeName ="nvarchar(100)")] [Required]   
		public string JobName { get; set; } // Job_Name
        
		[Column("Enabled",TypeName ="tinyint")] [Required]   
		public byte Enabled { get; set; } // Enabled
        
		[Column("Description",TypeName ="nvarchar(500)")] [Required]   
		public string Description { get; set; } // Description
        
		[Column("Owner_Login_Name",TypeName ="nvarchar(20)")] [Required]   
		public string OwnerLoginName { get; set; } // Owner_Login_Name
        
		[Column("Database_Name",TypeName ="varchar(20)")] [Required]   
		public string DatabaseName { get; set; } // Database_Name
        
		[Column("Command",TypeName ="nvarchar(4000)")] [Required]   
		public string Command { get; set; } // Command
        
		[Column("Active_Start_Time",TypeName ="datetime")] [Required]   
		public DateTime ActiveStartTime { get; set; } // Active_Start_Time
        
		[Column("Active_End_Time",TypeName ="datetime")] [Required]   
		public DateTime ActiveEndTime { get; set; } // Active_End_Time
        
		[Column("Create_User",TypeName ="varchar(20)")] [Required]   
		public string CreateUser { get; set; } // Create_User
        
		[Column("Create_Time",TypeName ="datetime")] [Required]   
		public DateTime CreateTime { get; set; } // Create_Time

        
        public Job()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
