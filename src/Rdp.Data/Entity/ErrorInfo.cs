using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    // tbCOM_Error_Info
	[Table("tbCOM_Error_Info")]
    public partial class ErrorInfo: BaseEntity
    {
        
		[Column("Error_Info_ID",TypeName ="int")] [Required] [DatabaseGenerated(DatabaseGeneratedOption.None)]  
		public int ErrorInfoID { get; set; } // Error_Info_ID (Primary key)
        
		[Column("User_ID",TypeName ="varchar(50)")] [Required]   
		public string UserID { get; set; } // User_ID
        
		[Column("Error_Code",TypeName ="int")] [Required]   
		public int ErrorCode { get; set; } // Error_Code
        
		[Column("Error_MSG",TypeName ="nvarchar(2000)")] [Required]   
		public string ErrorMSG { get; set; } // Error_MSG
        
		[Column("Running_Time",TypeName ="datetime")] [Required]   
		public DateTime RunningTime { get; set; } // Running_Time
        
		[Column("Program_ID",TypeName ="int")] [Required]   
		public int ProgramID { get; set; } // Program_ID
        
		[Column("Url",TypeName ="varchar(500)")] [Required]   
		public string Url { get; set; } // Url
        
		[Column("Exec_Sql",TypeName ="nvarchar(4000)")] [Required]   
		public string ExecSql { get; set; } // Exec_Sql
        
		[Column("Stack_Trace",TypeName ="nvarchar(4000)")] [Required]   
		public string StackTrace { get; set; } // Stack_Trace
        
		[Column("Solve_By",TypeName ="varchar(50)")] [Required]   
		public string SolveBy { get; set; } // Solve_By

        
        public ErrorInfo()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
