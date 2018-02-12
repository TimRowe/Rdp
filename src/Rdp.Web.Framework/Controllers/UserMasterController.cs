using System;
using Rdp.Web.Framework.Filters;
using Microsoft.AspNetCore.Mvc;
using Rdp.Service;
using Rdp.Core.Util;
using Rdp.Core.Security;
using Rdp.Resources;
using Rdp.Data.Entity;
using Microsoft.VisualBasic;
using AutoMapper;
using Rdp.Service.Dto;
using Rdp.Web.Framework.Models;
using Rdp.Resources.Globalization;
using Rdp.Web.Framework.Core;
using Rdp.Core;

namespace Rdp.Web.Framework.Controllers
{
    /// <summary>
    /// UserMasterController 的摘要说明
    /// </summary>

    [LoginAuthorize]
    public class UserMasterController : BaseController
    {
        private IUserMasterService _userMasterService;
        private IRoleUserService _roleUserService;
        private IUserSiteService _userSiteService;
        public UserMasterController(IUserMasterService userMasterService, IRoleUserService roleUserService, IUserSiteService userSiteService)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            _userMasterService = userMasterService;
            _roleUserService = roleUserService;
            _userSiteService = userSiteService;
        }


        [PemissionAuthorize]
        public ViewResult Index()
        {
            return View();
        }

        [PemissionAuthorize]
        public ViewResult Add()
        {
            return View("Operate");
        }


        public ViewResult Update(string userId)
        {
            var model = _userMasterService.GetModel(userId);
            return View("Operate",Mapper.Map<UserMaster, OperateUserMasterModel>(model));
        }

        [HttpPost]
        public string Search(UserMasterSearchModel viewModel)
        {
            var searchModel = Mapper.Map<UserMasterSearchModel, UserMasterSearchDto>(viewModel);
            var gridParam = this.GetGridParams();
            var list = _userMasterService.Search(searchModel, ref gridParam);
            return JSONHelper.ToJson(list, gridParam == null ? list.Count : gridParam.TotalCount);
        }

        [HttpPost]
        [PemissionAuthorize]
        public string Add(OperateUserMasterModel addModel)
        {
            if (!ModelState.IsValid)
                return JSONHelper.ToJsonSuggest(ResSuggest.OperateFail + ResMessage.WrongFormat);

            if(_userMasterService.GetCachedModel(addModel.UserID)!=null)
                return JSONHelper.ToJsonSuggest(ResUserMaster.UserIDExist);

            var userMasterModel = new UserMaster();
            var roleUserModel = new RoleUser();
            var host = "";

            if (addModel.UserIDTemplate != "0"&& addModel.UserIDTemplate != "")
            {
                var templateRoleUserModel = _roleUserService.GetCachedModel(t => t.UserID == addModel.UserIDTemplate);
                host = _userSiteService.GetUserSite(addModel.UserIDTemplate);   //获取服务器域名  

                if (templateRoleUserModel == null)
                    return JSONHelper.ToJsonSuggest(ResUserMaster.EquelPrivilegeUserID + ResMessage.NotExist);
                else
                {
                    roleUserModel.RoleID = templateRoleUserModel.RoleID;
                }
            }
            else if (string.IsNullOrEmpty(addModel.RoleID))
            {
                roleUserModel.RoleID = short.Parse(addModel.RoleID);
                addModel.UserIDTemplate = _roleUserService.GetCachedModel(t => t.RoleID == short.Parse(addModel.RoleID)).UserID;
                host = _userSiteService.GetUserSite(addModel.UserIDTemplate);
            }
            else
                return JSONHelper.ToJsonSuggest(ResUserMaster.PlearsEnterEquelPrivilegeUserIDOrRoleID);

            var randomCode = new RandomCode();
            addModel.Password = randomCode.GetRandomCode("1,2,3,4,5,6,7,8,9", 4) + randomCode.GetRandomCode("A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z", 4);
             
            userMasterModel = Mapper.Map<OperateUserMasterModel, UserMaster>(addModel);
            //userMasterModel.PasswordExprityDate = DateAndTime.DateAdd("d", 90, DateTime.Now.ToShortDateString());

            userMasterModel.PasswordExprityDate = DateTime.Now.AddDays(90);

            roleUserModel.UserID = addModel.UserID;

            var context = ResUserMaster.LoginAddress + "http://" + host + "/Login.aspx " + ResUserMaster.UserID + "：" + userMasterModel.UserID + " " + ResUserMaster.Password + "：" + addModel.Password;
            var bllUserMaster = new UserMasterService();
            if (bllUserMaster.AddUserTemplate(userMasterModel, roleUserModel, addModel.UserIDTemplate, "ctfsystem", addModel.EmailAdd, ResUserMaster.COUPriviligeOpen, context, "HTML"))
                return JSONHelper.ToJsonSuggest(ResSuggest.AddSuccess);
            else
                return JSONHelper.ToJsonSuggest(ResSuggest.AddFail);
        }

        [HttpPost]
        [PemissionAuthorize]
        public string Update(OperateUserMasterModel viewModel)
        {
            if (!ModelState.IsValid)
                return JSONHelper.ToJsonSuggest(ResSuggest.OperateFail + ResMessage.WrongFormat);
            var model = Mapper.Map<OperateUserMasterModel, UserMaster>(viewModel);
            return JSONHelper.ToJsonSuggest(_userMasterService.Update(model) ? ResSuggest.UpdateSuccess : ResSuggest.UpdateFail);
        }

        /// <summary>
        /// 验证用户ID
        /// </summary>
        /// <param name="CardNo"></param>
        /// <returns></returns>
        public JsonResult ValidateUserID(string userID)
        {
            return Json(userID == "" && userID == "0" ? true : _userMasterService.GetCachedModel(userID) == null ? true : false);
        }

        [HttpPost]
        public JsonResult Validate(string userId, string password)
        {
            var user = _userMasterService.GetCachedModel(userId);
            if(user == null)
            {
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResMessage.AccountNotExist });

               
            }

            if(user.Password != DesEncrypt.Encrypt(password))
            {
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResMessage.AccountNotExist });
            }

            return Json(new ResultInfo() { ErrorNo = 0, ErrorMsg = "" });
        }


        [HttpGet]
        public ViewResult ResetPassword()
        {        
            return View();
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="prePasswd"></param>
        /// <param name="newPasswd"></param>
        /// <returns></returns>
        [HttpPost]
        public ViewResult ResetPassword(string oldPassword, string newPassword, string confirmPassword)
        {
            var user = _userMasterService.GetModel(SessionManager.GetUserMaster().UserID);

            if (newPassword == oldPassword)
            {
                return View(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResUserMaster.NewPasswordCannotEquelOldPassword });
            }

            if (newPassword != confirmPassword)
            {
                return View(new ResultInfo() { ErrorNo = -1, ErrorMsg = "两次新密码不一致" });
            }

            if(user.Password != DesEncrypt.Encrypt(oldPassword))
            {
                return View(new ResultInfo() { ErrorNo = 0, ErrorMsg = ResMessage.PasswordError });
            }

            user.Password = DesEncrypt.Encrypt(newPassword);
            _userMasterService.Update(user);

            return View(new ResultInfo() { ErrorNo = 0, ErrorMsg = ResSuggest.UpdateSuccess });
        }
    }
}