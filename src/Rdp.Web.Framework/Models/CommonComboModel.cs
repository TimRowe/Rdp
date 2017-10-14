using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Rdp.Web.Framework.Models
{

    public enum ComboTypeEnum
    {
        EasyUI,
        ACE
    }

    /// <summary>
    /// 请求途径
    /// </summary>
    public enum FromWayEnum
    {
        FromCodeTable,
        FromGeneralTable
    }


    /// <summary>
    /// ComboItemModel 的摘要说明
    /// </summary>
    public class CommonComboModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string refTable { get; set; }
        public string valueField { get; set; }
        public string textField { get; set; }
        public string whereStr { get; set; }
        public bool editable { get; set; }
        public bool multiple { get; set; }
        public bool required { get; set; }
        public bool disabled { get; set; }
        public string cls { get; set; }
        public string placeholder { get; set; }
        public string style { get; set; }
        public string extend { get; set; }
        public bool useCache { get; set; }
        public ComboTypeEnum comboType { get; set; }
        public bool @readonly { get; set; }
        public List<CommonItemModel> optionList { get; set; }

        public CommonComboModel()
        {
            editable = false;
            multiple = false;
            required = false;
            disabled = false;
            useCache = true;
            cls = "";
            placeholder = "";
            comboType = ComboTypeEnum.EasyUI;
            @readonly = false;
        }
    }

    public class CommonItemModel
    {
        public string id;
        public string text = "";

        public CommonItemModel()
        {
        }


    }

}