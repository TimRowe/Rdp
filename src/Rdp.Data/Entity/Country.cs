using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    // tbCOM_Country
	[Table("tbCOM_Country")]
    public partial class Country: BaseEntity
    {
        
		[Column("Country_Code",TypeName ="char(2)")] [Required] [DatabaseGenerated(DatabaseGeneratedOption.None)]  
		public string CountryCode { get; set; } // Country_Code (Primary key)
        
		[Column("Country_No",TypeName ="varchar(50)")] [Required]   
		public string CountryNo { get; set; } // Country_No
        
		[Column("Country_Desc",TypeName ="nvarchar(50)")] [Required]   
		public string CountryDesc { get; set; } // Country_Desc
        
		[Column("Priority",TypeName ="smallint")] [Required]   
		public short Priority { get; set; } // Priority
        
		[Column("Status_Flag",TypeName ="smallint")] [Required]   
		public short StatusFlag { get; set; } // Status_Flag

        
        public Country()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
