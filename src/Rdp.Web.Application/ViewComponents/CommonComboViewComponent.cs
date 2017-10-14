using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rdp.Service;
using Rdp.Service.Dto;
using Rdp.Web.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tmq.Web1.ViewComponents
{
    public class CommonComboViewComponent : ViewComponent
    {
        private ICodeTableService _codeTableService;

        public CommonComboViewComponent(ICodeTableService codeTableService)
        {
            _codeTableService = codeTableService;
        }

        public async Task<IViewComponentResult> InvokeAsync(CommonComboModel model, FromWayEnum way)
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
            return View("/Views/RdpTemplate/CommonCombo/" + comboClass + ".cshtml", model);
        }
    }
}
