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
    /// 按钮管理
    /// </summary>
    [LoginAuthorize]
    public class ButtonController : BaseController
    {
        private IButtonService _buttonService;
        

        public ButtonController(IButtonService buttonService)
        {
            _buttonService = buttonService;
        }

        public string Search(ButtonSearchRequestDto searchModel)
        {
            var gridParam = GetGridParams();
            var list = _buttonService.Search(searchModel, ref gridParam);
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
        public JsonResult Add(ButtonAddModel model)
        {
            if (!ModelState.IsValid)
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.OperateFail + GetModelInvalidMsg() });

            var addModel = Mapper.Map<ButtonAddModel, Button>(model);

            if (_buttonService.Add(addModel))
                return Json(new ResultInfo() { ErrorNo = 0, ErrorMsg = ResSuggest.AddSuccess });
            else
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.OperateFail });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var model = _buttonService.GetModel(id);

            if (model == null)
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResWarning.Warn_No_Data });

            if (_buttonService.Delete(model))
                return Json(new ResultInfo() { ErrorNo = 0, ErrorMsg = ResSuggest.DeleteSuccess });
            else
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.DeleteFail });

        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            var model = _buttonService.GetModel(id);

            return View("Add", Mapper.Map<Button, ButtonAddModel>(model));
        }

        [HttpPost]
        public ActionResult Update(ButtonAddModel model)
        {
            if (!ModelState.IsValid)
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.OperateFail + GetModelInvalidMsg() });

            var updateModel = Mapper.Map<ButtonAddModel, Button>(model);

            if (_buttonService.Update(updateModel))
                return Json(new ResultInfo() { ErrorNo = 0, ErrorMsg = ResSuggest.UpdateSuccess });
            else
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResSuggest.UpdateFail });
        }

    }
}
