using Microsoft.AspNetCore.Mvc;
using Rdp.Core.Data;
using Rdp.Core.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Rdp.Web.Framework.Datatables
{
    public class DataTablesHelper
    {
        /// <summary>
        /// 将Datatables的参数适配成系统所需参数
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <returns></returns>
        public static Tuple<GridParams, DatatablesParameters>  GetGridParams(ControllerContext controllerContext)
        {
            var binder = new DatatablesModelBinder();
            var dataTablesParams = binder.BindModel(controllerContext);
            var gridParams = new GridParams();
            gridParams.Columns = new List<GridColumn>();
            gridParams.PageSize = dataTablesParams.Length;
            gridParams.PageIndex = Math.Max(dataTablesParams.Start / Math.Max(dataTablesParams.Length, 1) + 1, 1);
            gridParams.SortDirection = dataTablesParams.OrderDir;
            gridParams.SortField = dataTablesParams.OrderBy;

            foreach(var e in dataTablesParams.Columns)
            {
                gridParams.Columns.Add(new GridColumn { Field = e.Data, Title = e.Name });
            }

            return new Tuple<GridParams, DatatablesParameters>(gridParams, dataTablesParams); ;
        }

        public static string ToJson(DataTable dt, int total, int draw)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"draw\"");
            jsonBuilder.Append(":");
            jsonBuilder.Append(draw);
            jsonBuilder.Append(",\"recordsTotal\":");
            jsonBuilder.Append(total);
            jsonBuilder.Append(",\"recordsFiltered\":");
            jsonBuilder.Append(total);
            jsonBuilder.Append(",\"data\":");
            jsonBuilder.Append(JSONHelper.ToJson(dt));
            jsonBuilder.Append("}");
            return jsonBuilder.ToString().Replace("\"0\"", "\"\"");
        }

        public static string ToJson(IList list, int total, int draw)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"draw\"");
            jsonBuilder.Append(":");
            jsonBuilder.Append(draw);
            jsonBuilder.Append(",\"recordsTotal\":");
            jsonBuilder.Append(total);
            jsonBuilder.Append(",\"recordsFiltered\":");
            jsonBuilder.Append(total);
            jsonBuilder.Append(",\"data\":");
            jsonBuilder.Append(JSONHelper.ToJson(list));
            jsonBuilder.Append("}");
            return jsonBuilder.ToString().Replace("\"0\"", "\"\"");
        }



    }
}