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
    /// 程序管理
    /// </summary>
    [LoginAuthorize]
    public class ProgramController : BaseController
    {
        private IProgramService _programService;
        

        public ProgramController(IProgramService programService)
        {
            _programService = programService;
        }

        public string Search(ProgramSearchRequestDto searchModel)
        {
            var gridParam = GetGridParams();
            var list = _programService.Search(searchModel, ref gridParam);
            return JSONHelper.ToJson(list, gridParam == null ? list.Count : gridParam.TotalCount);
        }

        [HttpGet]
        public JsonResult List(int parentId)
        {
            var list = _programService.GetCachedModels(m => m.ParentID == parentId);
            return Json(list);
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Add(ProgramAddModel model)
        {
            if (!ModelState.IsValid)
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.OperateFail + GetModelInvalidMsg() });

            var addModel = Mapper.Map<ProgramAddModel, Program>(model);

            if (_programService.Add(addModel))
                return Json(new ResultInfo() { ErrorNo = 0, ErrorMsg = ResSuggest.AddSuccess });
            else
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.OperateFail });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var model = _programService.GetModel(id);

            if (model == null)
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = "找不到相关记录" });

            if (_programService.Delete(model))
                return Json(new ResultInfo() { ErrorNo = 0, ErrorMsg = ResSuggest.DeleteSuccess });
            else
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.DeleteFail });

        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            var model = _programService.GetModel(id);

            return View("Add", Mapper.Map<Program, ProgramAddModel>(model));
        }

        [HttpPost]
        public ActionResult Update(ProgramAddModel model)
        {
            if (!ModelState.IsValid)
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.OperateFail + GetModelInvalidMsg() });

            var updateModel = Mapper.Map<ProgramAddModel, Program>(model);

            if (_programService.Update(updateModel))
                return Json(new ResultInfo() { ErrorNo = 0, ErrorMsg = ResSuggest.UpdateSuccess });
            else
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.UpdateFail });
        }

    }
}
