using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    // tbLOG_Program
	[Table("tbLOG_Program")]
    public partial class Program: BaseEntity
    {
        
		[Column("Program_ID",TypeName ="int")] [Required] [DatabaseGenerated(DatabaseGeneratedOption.None)]  
		public int ProgramID { get; set; } // Program_ID (Primary key)
        
		[Column("Program_Name",TypeName ="nvarchar(50)")] [Required]   
		public string ProgramName { get; set; } // Program_Name
        
		[Column("Parent_ID",TypeName ="int")] [Required]   
		public int ParentID { get; set; } // Parent_ID
        
		[Column("Url",TypeName ="varchar(100)")] [Required]   
		public string Url { get; set; } // Url
        
		[Column("Icon",TypeName ="varchar(50)")] [Required]   
		public string Icon { get; set; } // Icon
        
		[Column("Is_Visible",TypeName ="tinyint")] [Required]   
		public byte IsVisible { get; set; } // Is_Visible
        
		[Column("Priority",TypeName ="smallint")] [Required]   
		public short Priority { get; set; } // Priority
        
		[Column("Status_Flag",TypeName ="tinyint")] [Required]   
		public byte StatusFlag { get; set; } // Status_Flag

        
        public Program()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
