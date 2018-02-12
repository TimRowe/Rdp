using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    // tbCOM_Code_Table
	[Table("tbCOM_Code_Table")]
    public partial class CodeTable: BaseEntity
    {
        
		[Column("Table_ID",TypeName ="int")] [Required] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  
		public int TableID { get; set; } // Table_ID (Primary key)
        
		[Column("Table_Name",TypeName ="nvarchar(50)")] [Required]   
		public string TableName { get; set; } // Table_Name
        
		[Column("Table_Desc",TypeName ="nvarchar(100)")] [Required]   
		public string TableDesc { get; set; } // Table_Desc
        
		[Column("Database_Name",TypeName ="varchar(20)")] [Required]   
		public string DatabaseName { get; set; } // Database_Name
        
		[Column("Select_Sql",TypeName ="nvarchar(1000)")] [Required]   
		public string SelectSql { get; set; } // Select_Sql
        
		[Column("Select_Parameter",TypeName ="nvarchar(500)")] [Required]   
		public string SelectParameter { get; set; } // Select_Parameter
        
		[Column("Insert_Sql",TypeName ="nvarchar(500)")] [Required]   
		public string InsertSql { get; set; } // Insert_Sql
        
		[Column("Insert_Parameter",TypeName ="nvarchar(500)")] [Required]   
		public string InsertParameter { get; set; } // Insert_Parameter
        
		[Column("Update_Sql",TypeName ="nvarchar(500)")] [Required]   
		public string UpdateSql { get; set; } // Update_Sql
        
		[Column("Update_Parameter",TypeName ="nvarchar(500)")] [Required]   
		public string UpdateParameter { get; set; } // Update_Parameter
        
		[Column("Delete_Sql",TypeName ="nvarchar(500)")] [Required]   
		public string DeleteSql { get; set; } // Delete_Sql
        
		[Column("Delete_Parameter",TypeName ="nvarchar(500)")] [Required]   
		public string DeleteParameter { get; set; } // Delete_Parameter
        
		[Column("Is_Code_Table",TypeName ="tinyint")] [Required]   
		public byte IsCodeTable { get; set; } // Is_Code_Table
        
		[Column("Status_Flag",TypeName ="tinyint")] [Required]   
		public byte StatusFlag { get; set; } // Status_Flag

        
        public CodeTable()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
