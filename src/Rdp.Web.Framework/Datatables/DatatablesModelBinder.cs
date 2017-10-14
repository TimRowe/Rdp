using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Rdp.Web.Framework.Datatables;
using Rdp.Web.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Model Binder for DTParameterModel (DataTables)
/// </summary>
public class DatatablesModelBinder : IModelBinder
{
    /// <summary>
    /// 此方法用于显式调用
    /// </summary>
    /// <param name="controllerContext"></param>
    /// <returns></returns>
    public DatatablesParameters BindModel(ControllerContext controllerContext)
    {
        var request = new HttpRequestExtension(controllerContext.HttpContext.Request);
        // Retrieve request data
        int draw = 0, start = 0, length = 0;
        draw = request["draw"] != null ? Convert.ToInt32(request["draw"]) : draw;
        start = request["draw"] != null ? Convert.ToInt32(request["start"]) : start;
        length = request["draw"] != null ? Convert.ToInt32(request["length"]) : length;

        // Search
        var search = new DataTablesSearch
        {
            Value = request["search[value]"] != null ? request["search[value]"] : string.Empty,
            Regex = request["search[regex]"] != null ? Convert.ToBoolean(request["search[regex]"]) : false
        };
        // Order
        var o = 0;
        var order = new List<DataTablesOrder>();
        while (request["order[" + o + "][column]"] != null)
        {
            order.Add(new DataTablesOrder
            {
                Column = request["order[" + o + "][column]"] != null ? Convert.ToInt32(request["order[" + o + "][column]"]) : -1,
                Dir = (request["order[" + o + "][dir]"] != null && request["order[" + o + "][dir]"].ToString().ToUpper() == "DESC") ?
                DataTablesOrderDir.Desc :
                DataTablesOrderDir.Asc
            });
            o++;
        }
        // Columns
        var c = 0;
        var columns = new List<DataTablesColumns>();
        while (request["columns[" + c + "][name]"] != null)
        {
            columns.Add(new DataTablesColumns
            {
                Data = request["columns[" + c + "][data]"],
                Name = request["columns[" + c + "][name]"],
                Orderable = Convert.ToBoolean(request["columns[" + c + "][orderable]"]),
                Searchable = Convert.ToBoolean(request["columns[" + c + "][searchable]"]),
                Search = new DataTablesSearch
                {
                    Value = request["columns[" + c + "][search][value]"],
                    Regex = Convert.ToBoolean(request["columns[" + c + "][search][regex]"])
                }
            });
            c++;
        }

        return new DatatablesParameters
        {
            Draw = draw,
            Start = start,
            Length = length,
            Search = search,
            Order = order,
            Columns = columns
        };
    }

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        throw new NotImplementedException();
    }
}