using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    // tbCOM_Batch_Generator
	[Table("tbCOM_Batch_Generator")]
    public partial class BatchGenerator: BaseEntity
    {
        
		[Column("Code",TypeName ="varchar(50)")] [Required] [DatabaseGenerated(DatabaseGeneratedOption.None)]  
		public string Code { get; set; } // Code (Primary key)
        
		[Column("Batch",TypeName ="bigint")] [Required]   
		public long Batch { get; set; } // Batch

        
        public BatchGenerator()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
