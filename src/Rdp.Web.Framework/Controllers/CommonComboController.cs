using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Rdp.Service;
using Rdp.Web.Framework.Models;
using Rdp.Service.Dto;
using AutoMapper;
using Rdp.Core.Util;
using Rdp.Web.Framework.Filters;

namespace Rdp.Web.Framework.Controllers
{
    /// <summary>
    /// CommonComboController 的摘要说明
    /// </summary>
    public class CommonComboController : BaseController
    {

        private ICodeTableService _codeTableService;
        public CommonComboController(ICodeTableService codeTableService)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            _codeTableService = codeTableService;

        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }




        public ActionResult FromCache(CommonComboModel model, string comboTypes = null)
        {
            var modelDto = new CodeTableDto()
            {
                TableName = model.refTable,
                ValueField = model.valueField,
                TextField = model.textField,
                Where = model.whereStr,
                UseCache = model.useCache
            };
            var itemList = new List<CodeTableItemDto>();
            itemList = _codeTableService.GetCodeTable(modelDto);
            if (itemList != null)
            {
                model.optionList = Mapper.Map<List<CodeTableItemDto>, List<CommonItemModel>>(itemList);
            }
            return PartialView(string.IsNullOrEmpty(comboTypes) ? "CommonCombo" : comboTypes, model);
        }



        [HttpGet]
        public ActionResult FromDB(CommonComboModel model)
        {
            var modelDto = new CodeTableDto()
            {
                TableName = model.refTable,
                ValueField = model.valueField,
                TextField = model.textField,
                Where = model.whereStr
            };
            var itemList = new List<CodeTableItemDto>();
            itemList = _codeTableService.GetGeneralTable(modelDto);
            if (itemList != null)
            {
                model.optionList = Mapper.Map<List<CodeTableItemDto>, List<CommonItemModel>>(itemList);
            }
            return PartialView("CommonCombo", model);
        }


        public ActionResult Create(CommonComboModel model, FromWayEnum way)
        {
            var modelDto = new CodeTableDto()
            {
                TableName = model.refTable,
                ValueField = model.valueField,
                TextField = model.textField,
                Where = model.whereStr,
                UseCache = model.useCache
            };
            var itemList = new List<CodeTableItemDto>();
            switch (way)
            {
                case FromWayEnum.FromCodeTable:
                    itemList = _codeTableService.GetCodeTable(modelDto);
                    break;
                case FromWayEnum.FromGeneralTable:
                    itemList = _codeTableService.GetGeneralTable(modelDto);
                    break;
                default:
                    itemList = null;
                    break;
            }
            if (itemList != null)
            {
                model.optionList = Mapper.Map<List<CodeTableItemDto>, List<CommonItemModel>>(itemList);
            }
            var comboClass = "";
            switch (model.comboType)
            {
                case ComboTypeEnum.EasyUI:
                    comboClass = "CommonCombo";
                    break;
                case ComboTypeEnum.ACE:
                    comboClass = "CommonComboForAce";
                    break;
            }
            return PartialView(comboClass, model);
        }

        [WebCache]
        public string GetList(CommonComboModel model, FromWayEnum way = FromWayEnum.FromCodeTable)
        {
            var modelDto = new CodeTableDto()
            {
                TableName = model.refTable,
                ValueField = model.valueField,
                TextField = model.textField,
                Where = model.whereStr,
                UseCache = model.useCache
            };
            var itemList = new List<CodeTableItemDto>();
            switch (way)
            {
                case FromWayEnum.FromCodeTable:
                    itemList = _codeTableService.GetCodeTable(modelDto);
                    break;
                case FromWayEnum.FromGeneralTable:
                    itemList = _codeTableService.GetGeneralTable(modelDto);
                    break;
                default:
                    itemList = null;
                    break;
            }
            if (itemList != null)
            {
                model.optionList = Mapper.Map<List<CodeTableItemDto>, List<CommonItemModel>>(itemList);
            }
            return JSONHelper.ToJson(model.optionList);
        }
    }
}