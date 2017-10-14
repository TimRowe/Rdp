namespace Rdp.Web.Framework.Models
{
    public enum ErrorTypeEnum
    {
        AccessError = 0,
        LostSession = 1,
        GetIPFailed = 2,
        UnassignedBranch = 3,
        ServerConnectionFailed = 4,
        NotAccessPage = 5,
        PageNotFound = 404
    }

    /// <summary>
    /// ErrorModel 的摘要说明
    /// </summary>
    public class ErrorModel
    {
        public ErrorTypeEnum ErrorNo { get; set; }
        public string ErrorMsg { get; set; }
        public string RedirectUrl { get; set; }

        public ErrorModel()
        {
            ErrorNo = ErrorTypeEnum.AccessError;
            ErrorMsg = "";
            RedirectUrl = "";
        }
    }
}