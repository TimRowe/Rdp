using System.Data;
using System.Text;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Rdp.Core.Util
{
    public class JSONHelper
    {

        /// <summary>
        /// Json序列化,用于发送到客户端
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToJson(object item)
        {
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            JsonConverter[] converters = new JsonConverter[] { timeFormat, new DataTableConverter() };
            return JsonConvert.SerializeObject(item, converters).Replace("\"0\"", "\"\"");
        }


        /// <summary>
        ///  Json反序列化,用于接收客户端Json后生成对应的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static T FromJsonTo<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }


        /// <summary>
        ///  Json反序列化为匿名对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static T FromJsonToAnonymousType<T>(string jsonString, T anonymousTypeObject)
        {
            return JsonConvert.DeserializeAnonymousType(jsonString, anonymousTypeObject);
        }


        /// <summary>
        /// 删除列数据再序列化
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="removeColumns"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToJsonWithRemove(DataTable dt, string removeColumns)
        {
            foreach (string item in removeColumns.Split(','))
            {
                dt.Columns.Remove(dt.Columns[item]);
            }
            return ToJson(dt);
        }
        /// <summary>
        /// 将Datatable转成Json格式，并输出总行数
        /// </summary>
        /// <param name="dt">Datatable</param>
        /// <param name="total">Datatable行数</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToJson(DataTable dt, int total)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"total\"");
            jsonBuilder.Append(":\"");
            jsonBuilder.Append(total);
            jsonBuilder.Append("\",\"rows\":");
            jsonBuilder.Append(ToJson(dt));
            jsonBuilder.Append("}");
            return jsonBuilder.ToString().Replace("\"0\"", "\"\"");
        }


        /// <summary>
        /// 将Object转成Json格式，并输出总行数
        /// </summary>
        /// <param name="dt">Datatable</param>
        /// <param name="total">Datatable行数</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToJson(object item, int total)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"total\"");
            jsonBuilder.Append(":\"");
            jsonBuilder.Append(total);
            jsonBuilder.Append("\",\"rows\":");
            jsonBuilder.Append(ToJson(item));
            jsonBuilder.Append("}");
            return jsonBuilder.ToString().Replace("\"0\"", "\"\"");
        }

        public static string ToJson(DataTable parent, DataTable dt, int total)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"total\"");
            jsonBuilder.Append(":\"");
            jsonBuilder.Append(total);
            jsonBuilder.Append("\",\"rows\":");
            if (dt.Rows.Count > 0)
            {
                jsonBuilder.Append(ToJson(parent).Replace("]", ","));
                jsonBuilder.Append(ToJson(dt).Replace("[", ""));
            }
            else
            {
                jsonBuilder.Append(ToJson(parent));
            }
            jsonBuilder.Append("}");
            return jsonBuilder.ToString().Replace("\"0\"", "\"\"");
        }

        /// <summary>
        /// 将表的列名以JSON的格式输出
        /// </summary>
        /// <param name="dt">表名</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToJsonWithColumn(DataTable dt, int total)
        {
            StringBuilder str = new StringBuilder();
            bool flag = true;
            foreach (DataColumn column in dt.Columns)
            {
                if (flag == true)
                {
                    str.Append(",\"columns\":[");
                    str.Append("{\"field\":\"" + column.ColumnName + "\",\"title\":\"" + column.ColumnName + "\",\"editor\":{\"type\":\"text\"}}");
                    //str.Append() ', ""title:"" + column.ColumnName + }")
                    flag = false;
                }
                else
                {
                    str.Append(",{\"field\":\"" + column.ColumnName + "\",\"title\":\"" + column.ColumnName + "\",\"editor\":{\"type\":\"text\"}}");
                }
            }
            str.Append("]}");
            string json = ToJson(dt, total);
            return json.Substring(0, json.Length-1) + str.ToString();
        }


        /// <summary>
        /// 以Json格式返回提示信息
        /// </summary>
        /// <param name="flag">真假值</param>
        /// <param name="trueSuggest">真的返回字符串</param>
        /// <param name="falseSuggest">假的返回字符串</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToJsonSuggest(bool flag, string trueSuggest, string falseSuggest)
        {
            return "{\"res\":\"" + (flag ? trueSuggest : falseSuggest) + "\"}";
        }

        /// <summary>
        /// 以Json格式返回提示信息
        /// </summary>
        /// <param name="suggest"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToJsonSuggest(string suggest)
        {
            return "{\"res\":\"" + suggest + "\"}";
        }

        /// <summary>
        /// 以Json格式返回提示信息
        /// </summary>
        /// <param name="suggest"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToJsonSuggest(int number, string suggest)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"num\"");
            jsonBuilder.Append(":\"");
            jsonBuilder.Append(number);
            jsonBuilder.Append("\",\"res\":\"");
            jsonBuilder.Append(suggest);
            jsonBuilder.Append("\"}");
            return jsonBuilder.ToString();
        }
    }
}

