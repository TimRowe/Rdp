using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rdp.Resources.Globalization;
using Rdp.Web.Framework.Core;

namespace Rdp.Web.Framework.Core
{
    /// <summary>
	/// CombinerResource 主要用于组合资源的动态获取，与HttpCombiner协助使用，即带版本号的js或css文件. Add by LT 2015-12-5
	/// 默认JS获取方法： @CRM.CombinerResource.Instance.DefaultJs%> 
	/// 额外JS获取方法：@CRM.CombinerResource.Instance.GetExternalJs("/Login.js")%>
	/// WebConfig定义Key的获取： @CRM.CombinerResource.Instance.GetConfigJs("javascript")%>
	/// </summary>
	/// <remarks></remarks>
	public class CombinerResource
    {

        private Hashtable _randonVersionMap = new Hashtable();

        private static readonly CombinerResource combinerResource = new CombinerResource();
        /// <summary>
        /// CRM默认Javascript文件Header
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string DefaultJs
        {
            get { return GetConfigJs("javascriptUpdate"); }
        }

        /// <summary>
        /// CRM默认CSS文件Header
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string DefaultCss
        {
            get { return GetConfigCss("cssUpdate"); }
        }

        /// <summary>
        /// 单例化
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static CombinerResource Instance()
        {
            return combinerResource;
        }


        public string GetVersion(ref string strKey)
        {
            if (!_randonVersionMap.Contains(strKey))
            {
                _randonVersionMap[strKey] = (new Random()).Next();
            }

            return _randonVersionMap[strKey].ToString();

        }

        /// <summary>
        /// 重置随机数
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool ResetVersion(string strKey)
        {

            if (_randonVersionMap.Contains(strKey))
            {
                _randonVersionMap[strKey] = (new Random()).Next();
                return true;
            }

            return false;
        }

        /// <summary>
        /// 为当前的资源链接附加上版本号随机数
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string PackVersion(string strKey)
        {
            return strKey + "&Rn=" + GetVersion(ref strKey).ToString();
        }

        /// <summary>
        /// 根据WebConfig中的Key获取对应的Js文件
        /// </summary>
        /// <param name="strConfigKey"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetConfigJs(string strConfigKey)
        {
            return "<script type=\"text/javascript\" src=\"" + PackVersion("/HttpCombiner?s=" + strConfigKey + "&t=text/javascript&v=-1") + "\"></script>";
        }

        /// <summary>
        /// 根据WebConfig中的Key获取对应的CSS文件
        /// </summary>
        /// <param name="strConfigKey"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetConfigCss(string strConfigKey)
        {
            return "<link type=\"text/css\" rel=\"stylesheet\" href=\"" + PackVersion("/HttpCombiner?s=" + strConfigKey + "&t=text/css&v=0") + "\"/>";
        }

        /// <summary>
        /// 获取一些自定义的JS Header, 以逗号隔开路径，例 /js/a.js,/js/b.js
        /// </summary>
        /// <param name="strJsPaths"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetExternalJs(string strJsPaths)
        {
            return "<script type=\"text/javascript\"  src=\"" + PackVersion("/HttpCombiner?ex=" + strJsPaths + "&t=text/javascript&v=-1") + "\"/></script>";
        }

        /// <summary>
        /// 获取一些自定义的Css Header, 以逗号隔开路径，例 /css/a.css,/css/b.css
        /// </summary>
        /// <param name="strJsPaths"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetExternalCss(string strJsPaths)
        {
            return "<link type=\"text/css\" rel=\"stylesheet\" href=\"" + PackVersion("/HttpCombiner?ex=" + strJsPaths + "&t=text/css&v=0") + "\"/>";
        }

    }
}


namespace Resources
{
    /// <summary>
    /// 由于历史原因，导致资源文件都采用@ResVersion.Javascript%>，现需将代码升级
    /// 为@CRM.CombinerResource.Instance.DefaultJs%>方式，为了保持向下兼容性而且秉承少修改
    /// 以前的代码的原则，删除掉资源文件Resources.ResVersion，改用类获取，类变量依赖CombinerResource
    /// </summary>
    /// <remarks></remarks>
    public class ResVersion
    {
        public static string Javascript
        {
            get { return CombinerResource.Instance().DefaultJs; }
        }

        public static string Css
        {
            get { return CombinerResource.Instance().DefaultCss; }
        }

        public static readonly string ComboboxUrl = ResVersion2.ComboboxUrl;
        public static readonly string CPValidateExtendCss = ResVersion2.CPValidateExtendCss;
        //Public Shared ReadOnly Css As String = Resources.ResVersion2.Css
        public static readonly string CssCrmbase = ResVersion2.CssCrmbase;
        public static readonly string CssCss_all = ResVersion2.CssCss_all;
        //Public Shared ReadOnly CssEasyui_142 As String = Resources.ResVersion2.CssEasyui_142
        public static readonly string CssNormal = ResVersion2.CssNormal;
        public static readonly string CssTable_blue = ResVersion2.CssTable_blue;
        //Public Shared ReadOnly Javascript As String = Resources.ResVersion2.Javascript
        public static readonly string JavascriptWithEx = ResVersion2.JavascriptWithEx;
        public static readonly string JPJqueryJs = ResVersion2.JPJqueryJs;
        public static readonly string JPJqueryValidateJs = ResVersion2.JPJqueryValidateJs;
        public static readonly string JPQueryPageJs = ResVersion2.JPQueryPageJs;
        public static readonly string JPValidateExtendJs = ResVersion2.JPValidateExtendJs;
        public static readonly string JsCommon = ResVersion2.JsCommon;
        public static readonly string JsCommonCardPrint = ResVersion2.JsCommonCardPrint;
        //Public Shared ReadOnly JsEasyui_142 As String = Resources.ResVersion2.JsEasyui_142
        public static readonly string JsJquery_172 = ResVersion2.JsJquery_172;
        public static readonly string JsPlace = ResVersion2.JsPlace;
        public static readonly string MyDatePicker = ResVersion2.MyDatePicker;
        public static readonly string ReadCard1 = ResVersion2.ReadCard1;
        public static readonly string ReadCard3 = ResVersion2.ReadCard3;

        public static readonly string REValidateExtendJs = ResVersion2.REValidateExtendJs;
    }
}
