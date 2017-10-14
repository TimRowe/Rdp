using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    // tbCOM_Information_History
	[Table("tbCOM_Information_History")]
    public partial class InformationHistory: BaseEntity
    {
        
		[Column("Seq",TypeName ="uniqueidentifier")] [Required] [DatabaseGenerated(DatabaseGeneratedOption.None)]  
		public Guid Seq { get; set; } // Seq (Primary key)
        
		[Column("Information_Type",TypeName ="smallint")] [Required]   
		public short InformationType { get; set; } // Information_Type
        
		[Column("CRM_Domain",TypeName ="smallint")] [Required]   
		public short CRMDomain { get; set; } // CRM_Domain
        
		[Column("Customer_ID",TypeName ="int")] [Required]   
		public int CustomerID { get; set; } // Customer_ID
        
		[Column("Channel_ID",TypeName ="smallint")] [Required]   
		public short ChannelID { get; set; } // Channel_ID
        
		[Column("Send_Theme",TypeName ="nvarchar(200)")] [Required]   
		public string SendTheme { get; set; } // Send_Theme
        
		[Column("Send_Content",TypeName ="nvarchar(4000)")] [Required]   
		public string SendContent { get; set; } // Send_Content
        
		[Column("Country_Code",TypeName ="varchar(50)")] [Required]   
		public string CountryCode { get; set; } // Country_Code
        
		[Column("Contact_Way",TypeName ="varchar(100)")] [Required]   
		public string ContactWay { get; set; } // Contact_Way
        
		[Column("Create_By",TypeName ="nvarchar(20)")] [Required]   
		public string CreateBy { get; set; } // Create_By
        
		[Column("Create_Date",TypeName ="datetime2")] [Required]   
		public DateTime CreateDate { get; set; } // Create_Date
        
		[Column("Send_Date",TypeName ="datetime2")] [Required]   
		public DateTime SendDate { get; set; } // Send_Date
        
		[Column("Sent_Date",TypeName ="datetime2")] [Required]   
		public DateTime SentDate { get; set; } // Sent_Date
        
		[Column("Error_MSG",TypeName ="nvarchar(2000)")] [Required]   
		public string ErrorMSG { get; set; } // Error_MSG
        
		[Column("Reply_Content",TypeName ="nvarchar(4000)")] [Required]   
		public string ReplyContent { get; set; } // Reply_Content
        
		[Column("Reply_Date",TypeName ="datetime2")] [Required]   
		public DateTime ReplyDate { get; set; } // Reply_Date
        
		[Column("Is_Vip",TypeName ="smallint")] [Required]   
		public short IsVip { get; set; } // Is_Vip
        
		[Column("Status_ID",TypeName ="smallint")] [Required]   
		public short StatusID { get; set; } // Status_ID

        
        public InformationHistory()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
