using System;
using Microsoft.AspNetCore.Mvc;
using Rdp.Service;
using Rdp.Core;
using Rdp.Core.Security;
using Rdp.Web.Framework.Core;
using Rdp.Data.Entity;
using System.Data;
using Rdp.Resources.Globalization;
using Rdp.Web.Framework.Models;

namespace Rdp.Web.Framework.Controllers
{
    /// <summary>
    /// 账户管理控制器
    /// </summary>
    public class AccountController : BaseController
    {
        private IUserSiteService _userSiteService;
        private IUserMasterService _userMasterService;
        private IDomainService _domainService;
        private IRoleUserService _roleUserService;

        public AccountController(
            IUserSiteService userSiteService,
            IUserMasterService userMasterService,
            IDomainService domainService,
            IRoleUserService roleUserService
            )
        {
            _userSiteService = userSiteService;
            _userMasterService = userMasterService;
            _domainService = domainService;
            _roleUserService = roleUserService;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// IP免登陆
        /// </summary>
        /// <param name="Ip"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult IpFreeLogin(string Ip)
        {
            var dt = _userMasterService.Login(Ip);

            if (dt.Rows.Count == 1)
            {
                ViewBag.FromIpFreeLogin = true;
                return Login(new LoginInfoModel() { UserId = dt.Rows[0]["User_ID"].ToString(), Ip = Ip });
            }

            return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = "IP免登陆失败，请手动登陆" });
        }

        /// <summary>
        /// 更改系统语言
        /// </summary>
        /// <param name="lan"></param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        public JsonResult Localization(string lan)
        {
            CookieManager.RemoveLanguage();
            CookieManager.AddLanguage(lan);
            return Json(new ResultInfo() { ErrorNo = 0, ErrorMsg = "" });
        }

        /// <summary>
        /// 系统登陆
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Login(LoginInfoModel loginInfo)
        {
            loginInfo.SiteUrl = Request.Host.ToString();
            UserMaster userMaster;

            //用户密码登陆
            if (loginInfo.Ip == null)
            {
                userMaster = _userMasterService.GetModel(p => p.UserID == loginInfo.UserId);

                if (userMaster == null)
                    return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResMessage.AccountNotExist });

                if (userMaster.Password != DesEncrypt.Encrypt(loginInfo.Password))
                    return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResMessage.PasswordError + "<a href='javascript:void(0);' onclick='RetrievePassword();'>" + ResIndex.RetrievePassword + "</a>" });
            }
            else
            {   //IP免登陆
                if (ViewBag.FromIpFreeLogin != true)
                {
                    var dt = _userMasterService.Login(loginInfo.Ip);
                    var bFind = false;

                    foreach (DataRow row in dt.Rows)
                        if (row["User_ID"].ToString() == loginInfo.UserId) bFind = true;

                    if (!bFind)
                        return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResMessage.AccountInvaliable });
                }

                userMaster = _userMasterService.GetCachedModel(p => p.UserID == loginInfo.UserId);
                if (userMaster == null)
                    return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResMessage.AccountNotExist });
            }
            //账户是否有效
            if (userMaster.StatusFlag != 0)
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResMessage.AccountInvaliable });

            //网址是否有效
            var userSite = _userSiteService.GetModel(userMaster.UserID, loginInfo.SiteUrl);
            //if (userSite == null)
            //    return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResMessage.SiteTips });

            //密码是否过期
            if (userMaster.PasswordExprityDate < DateTime.Now)
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResMessage.PasswordExpired + "<a href='javascript:void(0);' onclick='ModifyPassword();'>" + ResIndex.ModifyPassword + "</a>" });

            //清空旧缓存
            SessionManager.Logout();
            CookieManager.RemoveVersion();

            //加入角色和用户信息
            SessionManager.AddRoleUser(_roleUserService.GetCachedModel(p => p.UserID == loginInfo.UserId));
            SessionManager.AddUserMaster(userMaster);
            var domain = _domainService.GetModel(p => p.StatusFlag == 0);
            if (domain != null)
            {
                SessionManager.AddDomain(domain);
                CookieManager.AddVersion(domain.Version.ToString());
            }

            return Json(new ResultInfo() { ErrorNo = 0, ErrorMsg = "" });
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Logout()
        {
            SessionManager.Logout();
            return RedirectToAction("Login");
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeRole(int userRoleId)
        {
            var roleUser = _roleUserService.GetModel(userRoleId);

            if (roleUser == null)
            {
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResWarning.Warn_No_Data });
            }

            SessionManager.AddRoleUser(roleUser);
            return Json(new ResultInfo() { ErrorNo = 0, ErrorMsg = "" });
        }

    }
}
