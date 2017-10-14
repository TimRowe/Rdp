using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    // tbLOG_Operation
	[Table("tbLOG_Operation")]
    public partial class Operation: BaseEntity
    {
        
		[Column("Operation_ID",TypeName ="smallint")] [Required] [DatabaseGenerated(DatabaseGeneratedOption.None)]  
		public short OperationID { get; set; } // Operation_ID (Primary key)
        
		[Column("Operation_Desc",TypeName ="nvarchar(100)")] [Required]   
		public string OperationDesc { get; set; } // Operation_Desc
        
		[Column("Prority",TypeName ="smallint")] [Required]   
		public short Prority { get; set; } // Prority
        
		[Column("Status_Flag",TypeName ="tinyint")] [Required]   
		public byte StatusFlag { get; set; } // Status_Flag

        
        public Operation()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
