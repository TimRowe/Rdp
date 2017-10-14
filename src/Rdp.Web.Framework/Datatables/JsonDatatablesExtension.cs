using Rdp.Core.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Rdp.Web.Framework.Datatables
{
    public static class JsonDatatablesExtension
    {
        public static string ToJsonForDataTables(this JSONHelper jsonHelper, DataTable dt, int total, int draw)
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
    }
}