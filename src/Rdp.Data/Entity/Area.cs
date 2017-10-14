using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    // tbCOM_Area
	[Table("tbCOM_Area")]
    public partial class Area: BaseEntity
    {
        
		[Column("Area_ID",TypeName ="int")] [Required] [DatabaseGenerated(DatabaseGeneratedOption.None)]  
		public int AreaID { get; set; } // Area_ID (Primary key)
        
		[Column("Area_Name",TypeName ="nvarchar(100)")] [Required]   
		public string AreaName { get; set; } // Area_Name
        
		[Column("Parent_ID",TypeName ="int")] [Required]   
		public int ParentID { get; set; } // Parent_ID
        
		[Column("Postal_Code",TypeName ="varchar(50)")] [Required]   
		public string PostalCode { get; set; } // Postal_Code
        
		[Column("View_Order",TypeName ="smallint")] [Required]   
		public short ViewOrder { get; set; } // View_Order
        
		[Column("Level_ID",TypeName ="smallint")] [Required]   
		public short LevelID { get; set; } // Level_ID

        
        public Area()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
