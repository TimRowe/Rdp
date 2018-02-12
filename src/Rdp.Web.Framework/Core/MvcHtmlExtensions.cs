using Rdp.Core.Dependency;
using Rdp.Service;
using Rdp.Service.Dto;
using Rdp.Web.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Html;
using System.Data.SqlClient;

namespace Rdp.Web.Framework.Core
{
    
    public class DropDownListOption
    {
        public String RefTable { get; set; }
        public String OptionLabel { get; set; }
        public FromWayEnum FromWay { get; set; }
        public string TextField { get; set; }
        public string ValueField { get; set; }
        public string SelectValue { get; set; }
        public string Where { get; set; }
        public string Order { get; set; }
        public bool UseCache { get; set; }

        public List<SqlParameter> SqlParameters { get; set; }

        public DropDownListOption()
        {
            UseCache = true;
            Where = string.Empty;
        }
    }


    /// <summary>
    /// 扩充MVC的Html方法，用于适配Rdp的view相关控件展示
    /// </summary>
    public static class MvcHtmlExtensions
    {

        public static IHtmlContent DropDownListFor<TModel, TProperty>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            DropDownListOption option,
            IDictionary<string, object> htmlAttributes)
        {

            var codeTableService = IocObjectManager.GetInstance().Resolve<ICodeTableService>();

            var codeTableList = option.FromWay ==
                FromWayEnum.FromGeneralTable ? 
                codeTableService.GetGeneralTable(
                    new CodeTableDto()
                    {
                        TableName = option.RefTable,
                        TextField = option.TextField,
                        ValueField = option.ValueField,
                        Where = option.Where,
                        UseCache = option.UseCache,
                        SqlParameters = option.SqlParameters
                    }
                ):                
                codeTableService.GetCodeTable(
                    new CodeTableDto()
                    {
                        TableName = option.RefTable,
                        Where = option.Where,
                        UseCache = option.UseCache,
                        SqlParameters = option.SqlParameters
                    }
                ) ;

            var list = (from u in codeTableList
                        select new SelectListItem() { Text = u.text, Value = u.id, Selected = option.SelectValue == u.id ? true : false }).ToList();

            return htmlHelper.DropDownListFor(expression, list, option.OptionLabel, htmlAttributes);
        }

        //todo
        public static IHtmlContent DropDownList<TModel>(
            this IHtmlHelper<TModel> htmlHelper,
            DropDownListOption option,
            IDictionary<string, object> htmlAttributes)
        {
            var codeTableService = IocObjectManager.GetInstance().Resolve<ICodeTableService>();

            var codeTableList = option.FromWay ==
                FromWayEnum.FromGeneralTable ? 
                codeTableService.GetGeneralTable(
                    new CodeTableDto()
                    {
                        TableName = option.RefTable,
                        TextField = option.TextField,
                        ValueField = option.ValueField,
                        Where = option.Where,
                        UseCache = option.UseCache,
                        SqlParameters = option.SqlParameters
                    }
                ) :
                codeTableService.GetCodeTable(
                    new CodeTableDto()
                    {
                        TableName = option.RefTable,
                        Where = option.Where,
                        UseCache = option.UseCache,
                        SqlParameters = option.SqlParameters
                    })
                ;

            var list = (from u in codeTableList
                        select new SelectListItem() { Text = u.text, Value = u.id, Selected = option.SelectValue == u.id ? true : false }).ToList();

            return htmlHelper.DropDownList(htmlAttributes["name"].ToString(), list, option.OptionLabel, htmlAttributes);
        }


        /// <summary>
        /// 字符串缩略，用于字符串较长的情况
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="content">文本内容</param>
        /// <param name="maxLen">保留最大长度</param>
        /// <param name="replace">省略符</param>
        /// <returns></returns>
        public static IHtmlContent Ellipsis(this IHtmlHelper htmlHelper, string content, int maxLen, string replace)
        {
            return new HtmlString(content.Length > maxLen? content.Substring(0, maxLen-1) + replace : content);
        }


        public static IHtmlContent CacheVersion(this IHtmlHelper htmlHelper, string key)
        {
            return new HtmlString(ServiceExtensions.FromIoc<IVersionControlService>().GetVersion(key).ToString());
        }



    }
}
