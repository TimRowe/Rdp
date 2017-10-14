using Microsoft.AspNetCore.Mvc;
using Rdp.Service;
using Rdp.Core;
using Rdp.Data.Entity;
using Rdp.Core.Util;
using Rdp.Resources.Globalization;
using AutoMapper;
using Rdp.Web.Framework.Models;
using Rdp.Web.Framework.Filters;
using Rdp.Service.Dto;

namespace Rdp.Web.Framework.Controllers
{
    /// <summary>
    /// 程序按钮管理
    /// </summary>
    [LoginAuthorize]
    public class ProgramButtonController : BaseController
    {
        private IProgramButtonService _programButtonService;
        private IProgramService _programService;


        public ProgramButtonController(
            IProgramButtonService buttonService,
            IProgramService programService
            )
        {
            _programButtonService = buttonService;
            _programService = programService;
        }

        public string Search(ProgramButtonSearchRequestDto searchModel)
        {
            var gridParam = GetGridParams();
            var list = _programButtonService.Search(searchModel, ref gridParam);
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
        public JsonResult Add(ProgramButtonAddModel model)
        {
            if (!ModelState.IsValid)
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.OperateFail + GetModelInvalidMsg() });

            var addModel = Mapper.Map<ProgramButtonAddModel, ProgramButton>(model);

            if (_programButtonService.Add(addModel))
                return Json(new ResultInfo() { ErrorNo = 0, ErrorMsg = ResSuggest.AddSuccess });
            else
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.OperateFail });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var model = _programButtonService.GetModel(id);

            if (model == null)
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = "找不到相关记录" });

            if (_programButtonService.Delete(model))
                return Json(new ResultInfo() { ErrorNo = 0, ErrorMsg = ResSuggest.DeleteSuccess });
            else
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.DeleteFail });

        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            var model = _programButtonService.GetModel(id);
            var updateModel = Mapper.Map<ProgramButton, ProgramButtonAddModel>(model);
            updateModel.ParentProgramID = _programService.GetCachedModel(p => p.ProgramID == updateModel.ProgramID).ParentID;
            return View("Add", updateModel);
        }

        [HttpPost]
        public ActionResult Update(ProgramButtonAddModel model)
        {
            if (!ModelState.IsValid)
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.OperateFail + GetModelInvalidMsg() });

            var updateModel = Mapper.Map<ProgramButtonAddModel, ProgramButton>(model);

            if (_programButtonService.Update(updateModel))
                return Json(new ResultInfo() { ErrorNo = 0, ErrorMsg = ResSuggest.UpdateSuccess });
            else
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.UpdateFail });
        }

    }
}
