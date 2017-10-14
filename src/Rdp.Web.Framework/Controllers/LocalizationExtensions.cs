/*
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Rdp.Core.Util;
using Rdp.Core.Data;
using Rdp.Web.Framework.Models;
using Rdp.Web.Framework.Core;

namespace Rdp.Web.Framework.Controllers
{
    public static class LocalizationExtensions
    {
        class Item
        {
            public int id = 0;
            public string text = "";
        }

        public static string DescText(string tableName, int id)
        {
            var version = HttpContext.Current.Session["Version"];
            var cacheKey = "CodeTable." + tableName + "." + version;
            var responseBytes = (byte[])HttpContext.Current.Cache[cacheKey];
            var ItemList = new Item[] { };

            if (responseBytes == null || 0 == responseBytes.Length)
            {
                var strSql = String.Format("DECLARE @SQL NVARCHAR(2000) SELECT @SQL = Select_Sql FROM tbCOM_Code_Table WHERE Table_Name = '{0}' EXEC(@SQL)", tableName);
                responseBytes = Utils.Compress(Encoding.UTF8.GetBytes(JSONHelper.ToJson(DbHelperSql.Query(DbHelperSql.DefaultQueryConn, strSql).Tables[0])));
                HttpContext.Current.Cache.Insert(cacheKey, responseBytes, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromDays(30));
            }
            ItemList = JSONHelper.FromJsonToAnonymousType(Encoding.UTF8.GetString(Utils.Decompress(responseBytes)), ItemList);
            foreach (Item i in ItemList)
            {
                if (i.id == id)
                {
                    return i.text;
                }
            }
            return "";
        }


        public static List<CommonItemModel> ComboOptions(string tableName)
        {
            var version = HttpContext.Current.Session["Version"];
            var cacheKey = "CodeTable." + tableName + "." + version;
            var responseBytes = (byte[])HttpContext.Current.Cache[cacheKey];
            var ItemList = new List<CommonItemModel> { };

            if (responseBytes == null || 0 == responseBytes.Length)
            {
                var strSql = String.Format("DECLARE @SQL NVARCHAR(2000) SELECT @SQL = Select_Sql FROM tbCOM_Code_Table WHERE Table_Name = '{0}' EXEC(@SQL)", tableName);
                responseBytes = Utils.Compress(Encoding.UTF8.GetBytes(JSONHelper.ToJson(DbHelperSql.Query(DbHelperSql.DefaultQueryConn, strSql).Tables[0])));
                HttpContext.Current.Cache.Insert(cacheKey, responseBytes, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromDays(30));
            }
            ItemList = JSONHelper.FromJsonToAnonymousType(Encoding.UTF8.GetString(Utils.Decompress(responseBytes)), ItemList);
            return ItemList;
        }



    }
}*/