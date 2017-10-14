using System.Web;
using Microsoft.AspNetCore.Mvc;
using Rdp.Service;
using Rdp.Web.Framework.Core;
using Rdp.Web.Framework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Rdp.Core.Dependency;
using Microsoft.AspNetCore.Localization;

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
            if ((SessionManager.GetRoleUser() == null && SessionManager.GetLock() == null)
                || !_privilegeService.IsPrivilege(SessionManager.GetRoleUser().UserID, SessionManager.GetRoleUser().RoleID))
            {
                context.Result = string.IsNullOrEmpty(context.HttpContext.Request.Headers["Referer"].ToString()) ?
                new RedirectResult("/Account/Login") :
                new RedirectResult("/Error/Detail?ErrorNo=" + ErrorTypeEnum.LostSession);
            }
        }
    }
}
