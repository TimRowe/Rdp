using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Rdp.Core.Data
{
    public class Conversion
    {

        public static List<T> ConvertToList<T>(DataTable dt) where T : new()
        {
            var list = new List<T>(dt.Rows.Count);
            var propertys = typeof(T).GetProperties();

            foreach (DataRow dr in dt.Rows)
            {
                T item = new T();

                foreach (PropertyInfo pi in propertys)
                {
                    var index = -1;

                    for (var i = 0; i < dt.Columns.Count; ++i)
                    {
                        if (dt.Columns[i].ColumnName.Replace("_", "") == pi.Name)
                        {
                            index = i;
                            break;
                        }
                    }

                    if (index == -1)
                        continue;

                    if (!pi.CanWrite)
                        continue;

                    if (dr[index] == null || DBNull.Value.Equals(dr[index]))
                        continue;

                    pi.SetValue(item, Convert.ChangeType(dr[index], pi.PropertyType), null);
                }

                list.Add(item);
            }

            return list;
        }

    }
}
