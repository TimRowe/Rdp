using System;
using System.ComponentModel.DataAnnotations;
using Rdp.Resources;
using Rdp.Resources.Globalization;

/// <summary>
/// RoleMasterModel 的摘要说明
/// </summary>
namespace Rdp.Web.Framework.Models
{
    public class RoleMasterModel
    {
        public RoleMasterModel()
        {
            RoleID = 0;
            RoleDesc = "0";
            IsValidate = 0;
            StatusFlag = 0;
        }
        public int RoleID { get; set; }
        public string RoleDesc { get; set; }
        public int IsValidate { get; set; }
        public int StatusFlag { get; set; }
    }

    public class OperateRoleMasterModel
    {
        public OperateRoleMasterModel()
        {
            RoleID = 0;
            RoleDesc = "0";
            IsValidate = 0;
            StatusFlag = 0;
        }
        public short RoleID { get; set; }
        [Required(ErrorMessageResourceType = typeof(ResMessage),
           ErrorMessageResourceName = "Required")]
        public string RoleDesc { get; set; }
        [Required(ErrorMessageResourceType = typeof(ResMessage),
           ErrorMessageResourceName = "Required")]
        public int IsValidate { get; set; }
        [Required(ErrorMessageResourceType = typeof(ResMessage),
           ErrorMessageResourceName = "Required")]
        public int StatusFlag { get; set; }
    }
}