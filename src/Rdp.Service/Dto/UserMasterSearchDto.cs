using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rdp.Service.Dto
{
    public class UserMasterSearchDto
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int BranchCode { get; set; }
        public string IPAddress { get; set; }
        public int ReaderType { get; set; }
        public string EmailAdd { get; set; }
        public DateTime PasswordExprityDate { get; set; }
        public int StatusFlag { get; set; }
        public DateTime PasswordExprityDateFrom { get; set; }
        public DateTime PasswordExprityDateTo { get; set; }
    }
}
