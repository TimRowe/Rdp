using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    // tbLOG_User_Site
	[Table("tbLOG_User_Site")]
    public partial class UserSite: BaseEntity
    {
        
		[Column("User_Site_ID",TypeName ="int")] [Required]   
		public int UserSiteID { get; set; } // User_Site_ID
        
		[Column("User_ID",TypeName ="varchar(50)")] [Required]   
		public string UserID { get; set; } // User_ID
        
		[Column("Site_ID",TypeName ="tinyint")] [Required]   
		public byte SiteID { get; set; } // Site_ID

        
        public UserSite()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
