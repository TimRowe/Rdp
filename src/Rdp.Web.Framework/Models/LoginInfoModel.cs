namespace Rdp.Web.Framework.Models
{
    /// <summary>
    /// 登陆信息模型
    /// </summary>
    public class LoginInfoModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
        
        /// <summary>
        /// 用户密码，未加密
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 登陆IP，需开启Activex控件才能获取
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 网站URL
        /// </summary>
        public string SiteUrl { get; set; }
    }
}