using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Threading;
using Rdp.Service;
using Rdp.Web.Framework.Core;
using Rdp.Web.Framework.Filters;
using System;

namespace Rdp.Web.Framework.Controllers
{ 
    /// <summary>
    /// HomeController 的摘要说明
    /// </summary>
    [LoginAuthorize]
    public class HomeController : BaseController
    {
        private IProgramService _programService;
        private IRoleUserService _roleUserService;
        private IRoleMasterService _roleMasterService;

        public HomeController(IProgramService programService, IRoleUserService roleUserService, IRoleMasterService roleMasterService)
        {
            _programService = programService;
            _roleUserService = roleUserService;
            _roleMasterService = roleMasterService;
        }


        public ActionResult Index()
        {
            var roleUser = SessionManager.GetRoleUser();
            var menu = _programService.GetNavigationItem(roleUser);

            //菜单简繁体转换
            //var localID = Thread.CurrentThread.CurrentUICulture.Name == "zh-CN" ? 0 : 2;
            //VbStrConv strTransfer = localID == 0? Microsoft.VisualBasic.VbStrConv.SimplifiedChinese : Microsoft.VisualBasic.VbStrConv.TraditionalChinese;
            //foreach (var e in menu.ChildMenus)
            //{
            //    if(e.CurrentMenu != null)
            //        e.CurrentMenu.ProgramName = Strings.StrConv(e.CurrentMenu.ProgramName, strTransfer, localID);

            //    foreach(var e1 in e.ChildMenus)
            //        e1.CurrentMenu.ProgramName = Strings.StrConv(e1.CurrentMenu.ProgramName, strTransfer, localID);
            //}

            //获取用户信息
            var user = SessionManager.GetUserMaster();
            var roleUserList = _roleUserService.GetModels(t => t.UserID == roleUser.UserID);
            var roleLists = _roleMasterService.GetModels(t=> t.StatusFlag == 0).ToList();

            //传到View
            ViewBag.roleUserList = roleUserList;
            ViewBag.roleLists = roleLists;
            ViewBag.roleUser = roleUser;
            ViewBag.user = user;

            return View(menu);
            
        }
    }

}