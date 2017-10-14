using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    // tbLOG_Button
	[Table("tbLOG_Button")]
    public partial class Button: BaseEntity
    {
        
		[Column("Button_ID",TypeName ="int")] [Required] [DatabaseGenerated(DatabaseGeneratedOption.None)]  
		public int ButtonID { get; set; } // Button_ID (Primary key)
        
		[Column("Button_Name",TypeName ="nvarchar(100)")] [Required]   
		public string ButtonName { get; set; } // Button_Name
        
		[Column("Button_Text",TypeName ="nvarchar(200)")] [Required]   
		public string ButtonText { get; set; } // Button_Text
        
		[Column("Button_Class",TypeName ="varchar(100)")] [Required]   
		public string ButtonClass { get; set; } // Button_Class
        
		[Column("Button_Icon",TypeName ="varchar(100)")] [Required]   
		public string ButtonIcon { get; set; } // Button_Icon
        
		[Column("Button_Script",TypeName ="nvarchar(4000)")] [Required]   
		public string ButtonScript { get; set; } // Button_Script
        
		[Column("Button_Data_Options",TypeName ="varchar(200)")] [Required]   
		public string ButtonDataOptions { get; set; } // Button_Data_Options
        
		[Column("Button_Href",TypeName ="nvarchar(200)")] [Required]   
		public string ButtonHref { get; set; } // Button_Href
        
		[Column("Priority",TypeName ="smallint")] [Required]   
		public short Priority { get; set; } // Priority
        
		[Column("Status_Flag",TypeName ="tinyint")] [Required]   
		public byte StatusFlag { get; set; } // Status_Flag

        
        public Button()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
