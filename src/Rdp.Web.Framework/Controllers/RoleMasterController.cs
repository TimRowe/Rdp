using Rdp.Core.Util;
using Rdp.Service;
using Microsoft.AspNetCore.Mvc;
using Rdp.Web.Framework.Models;
using COU.Web.Filter;
using AutoMapper;
using Rdp.Data.Entity;
using Rdp.Resources;
using Rdp.Resources.Globalization;

namespace Rdp.Web.Framework.Controllers
{
    /// <summary>
    /// RoleMasterController 的摘要说明
    /// </summary>
    public class RoleMasterController : BaseController
    {
        private IRoleMasterService _roleMasterService;
        public RoleMasterController(IRoleMasterService roleMasterService)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            _roleMasterService = roleMasterService;
        }

        //[PemissionAuthorize]
        public ViewResult Index()
        {
            return View();
        }

        [PemissionAuthorize]
        public ViewResult Add()
        {
            return View("Operate");
        }

        [PemissionAuthorize]
        public ViewResult Update(short roleId)
        {
            var model = _roleMasterService.GetModel(roleId);
            return View("Operate", Mapper.Map<RoleMaster, OperateRoleMasterModel>(model));
        }

        [HttpPost]
        //[PemissionAuthorize]
        public string Search(RoleMasterModel viewModel)
        {
            var gridParams = this.GetGridParams();
            var list = _roleMasterService.GetCachedModels(
                t =>
                {
                    if (viewModel.RoleID != 0 && t.RoleID != viewModel.RoleID)
                    {
                        return false;
                    }
                    return true;

                });
            return JSONHelper.ToJson(list);

        }

        [HttpPost]
        [PemissionAuthorize]
        public string Update(OperateRoleMasterModel viewModel)
        {
            if (!ModelState.IsValid)
                return JSONHelper.ToJsonSuggest(ResSuggest.OperateFail + ResMessage.WrongFormat);
            var model = Mapper.Map<OperateRoleMasterModel, RoleMaster>(viewModel);
            return JSONHelper.ToJsonSuggest(_roleMasterService.Update(model) ? ResSuggest.UpdateSuccess : ResSuggest.UpdateFail);
        }
    }
}