using Rdp.Core.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Rdp.Web.Framework.Datatables
{
    public class DataTableHelper
    {
        public static DataTable ToDataTable(IList list) 
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null) != null && pi.GetValue(list[i], null).GetType() == typeof(DateTime) ?
                            ((DateTime)pi.GetValue(list[i], null)).ToString("yyyy-MM-dd HH:mm:ss") :
                            pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }


    }
}