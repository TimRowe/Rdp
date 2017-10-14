using Microsoft.AspNetCore.Mvc;
using Rdp.Service;
using Rdp.Core;
using Rdp.Data.Entity;
using Rdp.Core.Util;
using AutoMapper;
using Rdp.Web.Framework.Models;
using Rdp.Resources.Globalization;
using Rdp.Service.Dto;
using Rdp.Web.Framework.Filters;

namespace Rdp.Web.Framework.Controllers
{
    /// <summary>
    /// 用户角色管理
    /// </summary>
    [LoginAuthorize]
    public class RoleUserController : BaseController
    {
        private IRoleUserService _roleUserService;
        private IUserMasterService _userMasterService;

        public RoleUserController(
                IRoleUserService roleUserService,
                IUserMasterService userMasterService)
        {
            _roleUserService = roleUserService;
            _userMasterService = userMasterService;
        }

        public string Search(RoleUserSearchRequestDto searchModel)
        {
            var gridParam = GetGridParams();
            var list = _roleUserService.Search(searchModel, ref gridParam);
            return JSONHelper.ToJson(list, gridParam == null ? list.Count : gridParam.TotalCount);
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Add(RoleUserAddModel model)
        {
            if (!ModelState.IsValid)
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.OperateFail + GetModelInvalidMsg() });

            if(_userMasterService.GetModel(m=>m.UserID == model.UserID) == null)
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.OperateFail + ResMessage.AccountNotExist });

            if (_roleUserService.GetModel(m => m.UserID == model.UserID && m.RoleID == model.RoleID) != null)
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.OperateFail + ResOperate.RepeatAdd });

            var addModel = Mapper.Map<RoleUserAddModel, RoleUser>(model);

            if (_roleUserService.Add(addModel))
                return Json(new ResultInfo() { ErrorNo = 0, ErrorMsg = ResSuggest.AddSuccess });
            else
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.OperateFail });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var model = _roleUserService.GetModel(id);

            if (model == null)
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = "找不到相关记录" });

            if (_roleUserService.Delete(model))
                return Json(new ResultInfo() { ErrorNo = 0, ErrorMsg = ResSuggest.DeleteSuccess });
            else
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.DeleteFail });

        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            var model = _roleUserService.GetModel(id);

            return View("Add", Mapper.Map<RoleUser, RoleUserAddModel>(model));
        }

        [HttpPost]
        public ActionResult Update(RoleUserAddModel model)
        {
            if (!ModelState.IsValid)
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.OperateFail + GetModelInvalidMsg() });

            if (_userMasterService.GetModel(m => m.UserID == model.UserID) == null)
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.OperateFail + ResMessage.AccountNotExist });

            if (_roleUserService.GetModel(m => m.UserID == model.UserID && m.RoleID == model.RoleID) != null)
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.OperateFail + ResOperate.RepeatAdd });

            var updateModel = Mapper.Map<RoleUserAddModel, RoleUser>(model);

            if (_roleUserService.Update(updateModel))
                return Json(new ResultInfo() { ErrorNo = 0, ErrorMsg = ResSuggest.UpdateSuccess });
            else
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.UpdateFail });
        }

    }
}
