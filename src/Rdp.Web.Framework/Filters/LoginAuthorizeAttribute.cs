using System.Web;
using Microsoft.AspNetCore.Mvc;
using Rdp.Service;
using Rdp.Web.Framework.Core;
using Rdp.Web.Framework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Rdp.Core.Dependency;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Rdp.Web.Framework.Filters
{
    /// <summary>
    /// LoginAuthorizeAttribute 的摘要说明
    /// </summary>
    public class LoginAuthorizeAttribute : ActionFilterAttribute
    {
        //tido
        private IPrivilegeService _privilegeService;

        
        public LoginAuthorizeAttribute()
        {
            _privilegeService = IocObjectManager.GetInstance().Resolve<IPrivilegeService>();

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            UrlHelper _urlHelper = new UrlHelper(context);

            if ((SessionManager.GetRoleUser() == null && SessionManager.GetLock() == null)
                || !_privilegeService.IsPrivilege(SessionManager.GetRoleUser().UserID, SessionManager.GetRoleUser().RoleID))
            {
                var redirectResult = string.IsNullOrEmpty(context.HttpContext.Request.Headers["Referer"].ToString()) ?
                    new RedirectResult("~/Account/Login") :
                    new RedirectResult("~/Error/Detail?ErrorNo=" + ErrorTypeEnum.LostSession);
        

                if (context.HttpContext.Request.Method == "POST")
                    context.Result = new ContentResult() { Content = "{\"redirectUrl\":\"" + _urlHelper.Content(redirectResult.Url) + "\"}", StatusCode = 302, ContentType = "application/json" };
                else
                    context.Result = redirectResult;
            }
        }
    }
}
