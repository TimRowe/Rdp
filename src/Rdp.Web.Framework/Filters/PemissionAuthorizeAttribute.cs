using System.Web;
using Microsoft.AspNetCore.Mvc;
using Rdp.Service;
using Rdp.Web.Framework.Core;
using Rdp.Service.Dto;
using Microsoft.AspNetCore.Mvc.Filters;
using Rdp.Core.Dependency;

namespace COU.Web.Filter
{
    public class PemissionAuthorizeAttribute : ActionFilterAttribute
    {
        //todo
        private IPrivilegeService _privilegeService;

        public PemissionAuthorizeAttribute()
        {
            _privilegeService = IocObjectManager.GetInstance().Resolve<IPrivilegeService>();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (SessionManager.GetRoleUser() == null && SessionManager.GetLock() == null)
            {
                context.Result = new RedirectResult("/Error/Detail?ErrorNo=1");
                return;
            }

            //页面权限控制
            var roleUser = SessionManager.GetRoleUser();
            var path = context.HttpContext.Request.Path.ToString();

            var privilegeList = _privilegeService.GetUrlPermissionItems(roleUser.UserID, roleUser.RoleID);

            PrivilegeDto privilege = null;

            if (path != "" && privilegeList != null && (privilege = privilegeList.Find(o => o.Url == path)) != null)
            {
                context.HttpContext.Items["UrlPermission"] = privilege;
            }
            else
            {
                context.Result = new RedirectResult("/Error/Detail?ErrorNo=5");
            }
        }
    }
}