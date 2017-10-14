using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    // tbLOG_User_IP
	[Table("tbLOG_User_IP")]
    public partial class UserIP: BaseEntity
    {
        
		[Column("User_IP_ID",TypeName ="int")] [Required] [DatabaseGenerated(DatabaseGeneratedOption.None)]  
		public int UserIPID { get; set; } // User_IP_ID (Primary key)
        
		[Column("User_ID",TypeName ="varchar(50)")] [Required]   
		public string UserID { get; set; } // User_ID
        
		[Column("IP_Address",TypeName ="varchar(20)")] [Required]   
		public string IPAddress { get; set; } // IP_Address
        
		[Column("IP_Address_Sub",TypeName ="varchar(20)")]    
		public string IPAddressSub { get; set; } // IP_Address_Sub
        
		[Column("IP_Min",TypeName ="varchar(20)")]    
		public string IPMin { get; set; } // IP_Min
        
		[Column("IP_Max",TypeName ="varchar(20)")]    
		public string IPMax { get; set; } // IP_Max

        
        public UserIP()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
