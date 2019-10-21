using System.Web;
using Microsoft.AspNetCore.Mvc;
using Rdp.Service;
using Rdp.Web.Framework.Core;
using Rdp.Service.Dto;
using Microsoft.AspNetCore.Mvc.Filters;
using Rdp.Core.Dependency;
using System.Collections.Generic;
using System.Linq;

namespace Rdp.Web.Framework.Filters
{
    public class PemissionAuthorizeAttribute : TypeFilterAttribute
    {
        public PemissionAuthorizeAttribute() : base(typeof(PemissionAuthorizeImplAttribute))
        {

        }

        private class PemissionAuthorizeImplAttribute : ActionFilterAttribute
        {//todo
            private IPrivilegeService _privilegeService;

            public PemissionAuthorizeImplAttribute(IPrivilegeService privilegeService)
            {
                _privilegeService = privilegeService;
            }

            public override void OnActionExecuting(ActionExecutingContext context)
            {
                var roleUserList = SessionManager.GetRoleUsers();

                if (roleUserList.Count <= 0 && SessionManager.GetLock() == null)
                {
                    context.Result = new RedirectResult("/Error/Detail?ErrorNo=1");
                    return;
                }

                var roleArray = roleUserList.Select(m=>m.RoleID).ToArray();
                

                //页面权限控制

                var path = context.HttpContext.Request.Path.ToString();

                var privilegeList = _privilegeService.GetUrlPermissionItems(roleUserList[0].UserID, roleArray);

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
}