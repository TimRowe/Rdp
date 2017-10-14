using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    // tbCOM_Version_Control
	[Table("tbCOM_Version_Control")]
    public partial class VersionControl: BaseEntity
    {
        
		[Column("Key",TypeName ="varchar(50)")] [Required] [DatabaseGenerated(DatabaseGeneratedOption.None)]  
		public string Key { get; set; } // Key (Primary key)
        
		[Column("Version",TypeName ="int")] [Required]   
		public int Version { get; set; } // Version
        
		[Column("Ref_Table",TypeName ="varchar(500)")] [Required]   
		public string RefTable { get; set; } // Ref_Table
        
		[Column("Update_Date",TypeName ="datetime")] [Required]   
		public DateTime UpdateDate { get; set; } // Update_Date

        
        public VersionControl()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }



   
    



   
    



   
    



   
    



   
    



   
    



   
    



   
    



   
    



   
    



   
    



   
    



   
    



   
    



   
    



   
    



   
    



   
    



   
    



   
    



   
    



   
    



   
    

}
// </auto-generated>
