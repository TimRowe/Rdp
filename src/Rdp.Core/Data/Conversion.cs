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

            dynamic list = new List<T>();
            dynamic type = typeof(T);
            foreach (DataRow dr in dt.Rows)
            {
                T item = new T();
                dynamic propertys = item.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        dynamic filedName = column.ColumnName.Replace("_", "");
                        if (filedName.Contains(pi.Name))
                        {
                            if (!pi.CanWrite)
                            {
                                continue;
                            }

                            if (!DBNull.Value.Equals(dr[column.ColumnName]))
                            {
                                pi.SetValue(item, Convert.ChangeType(dr[column.ColumnName], pi.PropertyType), null);
                            }

                            break; // TODO: might not be correct. Was : Exit For
                        }
                    }
                }
                list.Add(item);
            }

            return list;

        }

    }
}
