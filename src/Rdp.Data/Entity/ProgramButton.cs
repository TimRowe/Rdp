using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    // tbLOG_Program_Button
	[Table("tbLOG_Program_Button")]
    public partial class ProgramButton: BaseEntity
    {
        
		[Column("Program_Button_ID",TypeName ="int")] [Required] [DatabaseGenerated(DatabaseGeneratedOption.None)]  
		public int ProgramButtonID { get; set; } // Program_Button_ID (Primary key)
        
		[Column("Program_ID",TypeName ="int")] [Required]   
		public int ProgramID { get; set; } // Program_ID
        
		[Column("Button_ID",TypeName ="int")] [Required]   
		public int ButtonID { get; set; } // Button_ID
        
		[Column("Url",TypeName ="varchar(100)")] [Required]   
		public string Url { get; set; } // Url

        
        public ProgramButton()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
