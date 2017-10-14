using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Rdp.Resources;
using Rdp.Resources.Globalization;

namespace Rdp.Web.Framework.Models
{
    public class UserMasterSearchModel
    {
        public UserMasterSearchModel()
        {
            UserID = "";
            UserName = "";
            Password = "";
            BranchCode = 0;
            IPAddress = "";
            ReaderType = 0;
            EmailAdd = "";
            PasswordExprityDate = Convert.ToDateTime("1900-01-01");
            StatusFlag = 0;
            PasswordExprityDateFrom = Convert.ToDateTime("1900-01-01");
            PasswordExprityDateTo = Convert.ToDateTime("1900-01-01");
        }
        public string UserID { get; set; }
        public string UserName { get; set; } 
        public string Password { get; set; } 
        public int BranchCode { get; set; }
        public string IPAddress { get; set; } 
        public short ReaderType { get; set; } 
        public string EmailAdd{ get; set; } 
        public DateTime PasswordExprityDate{ get; set; } 
        public int StatusFlag { get; set; } 
        public DateTime PasswordExprityDateFrom { get; set; } 
        public DateTime PasswordExprityDateTo { get; set; } 

    }

    public class OperateUserMasterModel
    {
        public OperateUserMasterModel()
        {
            UserID = "";
            UserName = "";
            Password = "";
            BranchCode = 0;
            IPAddress = "";
            ReaderType = 0;
            EmailAdd = "";
            PasswordExprityDate = Convert.ToDateTime("1900-01-01");
            StatusFlag = 0;
            UserIDTemplate = "";
            RoleID = "0";
        }
        [Required(ErrorMessageResourceType = typeof(ResMessage),
           ErrorMessageResourceName = "Required")]
        public string UserID { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResMessage),
           ErrorMessageResourceName = "Required")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResMessage),
           ErrorMessageResourceName = "Required")]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResMessage),
           ErrorMessageResourceName = "Required")]
        public int BranchCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResMessage),
           ErrorMessageResourceName = "Required")]
        public string IPAddress { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResMessage),
           ErrorMessageResourceName = "Required")]
        public short ReaderType { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResMessage),
           ErrorMessageResourceName = "Required")]
        [RegularExpression(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$", ErrorMessageResourceType = typeof(ResMessage), ErrorMessageResourceName = "WrongFormat")]
        public string EmailAdd { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResMessage),
           ErrorMessageResourceName = "Required")]
        public DateTime PasswordExprityDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResMessage),
           ErrorMessageResourceName = "Required")]
        public int StatusFlag { get; set; }

        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessageResourceType = typeof(ResUserMaster), ErrorMessageResourceName = "OnlyEnterNumOrLetter")]
        public string UserIDTemplate { get; set; }

        public string RoleID { get; set; }

    }

    public class AddUserMasterModel
    {
        public AddUserMasterModel()
        {
            UserID = "";
            UserName = "";
            EmailAdd = "";
            UserIDTemplate = "";
        }

        [Required(ErrorMessageResourceType = typeof(ResMessage),
           ErrorMessageResourceName = "Required")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessageResourceType = typeof(ResUserMaster), ErrorMessageResourceName = "OnlyEnterNumOrLetter")]
        [Remote("ValidateUserID", "UserMaster", ErrorMessageResourceType = typeof(ResUserMaster),
           ErrorMessageResourceName = "UserIDExist", AdditionalFields = "UserID")]
        public string UserID { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResMessage),
           ErrorMessageResourceName = "Required")]
        public string UserName { get; set; } 

        [Required(ErrorMessageResourceType = typeof(ResMessage),
           ErrorMessageResourceName = "Required")]
        [RegularExpression(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$", ErrorMessageResourceType = typeof(ResMessage), ErrorMessageResourceName = "WrongFormat")]
        public string EmailAdd{ get; set; } 

        [Required(ErrorMessageResourceType = typeof(ResMessage),
           ErrorMessageResourceName = "Required")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessageResourceType = typeof(ResUserMaster), ErrorMessageResourceName = "OnlyEnterNumOrLetter")]
        public string UserIDTemplate { get; set; }
       
    }
}
