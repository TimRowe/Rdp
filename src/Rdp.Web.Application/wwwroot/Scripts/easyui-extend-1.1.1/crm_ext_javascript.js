var com = com || {};
/**
 * 去字符串空格
 * 
 * @author 卜永济
 */
com.trim = function (str) {
    return str.replace(/(^\s*)|(\s*$)/g, '');
};
com.ltrim = function (str) {
    return str.replace(/(^\s*)/g, '');
};
com.rtrim = function (str) {
    return str.replace(/(\s*$)/g, '');
};

/**
 * 判断开始字符是否是XX
 * 
 * @author 卜永济
 */
com.startWith = function (source, str) {
    var reg = new RegExp("^" + str);
    return reg.test(source);
};
/**
 * 回车事件提交
 * 
 * @author 卜永济
 */
com.enterSubmit = function (inputID, aID) {
    document.getElementsByTagName("input")[inputID].focus();
    document.onkeydown = function () {
        if (typeof (window.event)!="undefined") {
            if (window.event.keyCode == 13) {
                document.getElementsByTagName("a")[aID].focus();
            }
        }
        
    };
};
/**
 * 判断结束字符是否是XX
 * 
 * @author 卜永济
 */
com.endWith = function (source, str) {
    var reg = new RegExp(str + "$");
    return reg.test(source);
};

/**
 * iframe自适应高度
 * 
 * @author 卜永济
 * 
 * @param iframe
 */
com.autoIframeHeight = function (iframe) {
    iframe.style.height = iframe.contentWindow.document.body.scrollHeight + "px";
};

/**
 * 设置iframe高度
 * 
 * @author 卜永济
 * 
 * @param iframe
 */
com.setIframeHeight = function (iframe, height) {
    iframe.height = height;
};
/**
 * 获取当前日期
 * 
 * @author 卜永济
 * 
 * @param iframe
 */
com.getNowDate = function () {
    var now = new Date();
    var year = now.getFullYear(); //年
    var month = now.getMonth() + 1; //月
    var day = now.getDate(); //日
    var clock = year + "-";
    if (month < 10)
        clock += "0";
    clock += month + "-";
    if (day < 10)
        clock += "0";
    clock += day;
    return clock;

};
/**
 * 获取当前时间
 * 
 * @author 卜永济
 * 
 * @param iframe
 */

com.getDate = function () {
    var now = new Date();
    var ret = now.getFullYear();
    ret += ("00" + (now.getMonth() + 1)).slice(-2);
    ret += ("00" + now.getDate()).slice(-2);
    ret += ("00" + now.getHours()).slice(-2);
    ret += ("00" + now.getMinutes()).slice(-2);
    ret += ("00" + now.getSeconds()).slice(-2);
    ret += ("000" + now.getMilliseconds()).slice(-3);
    return ret;
};
/**
 * 获取时间对比
 * 
 * @author 卜永济
 * 
 * @param iframe
 */
com.dateCompare = function (startdate, enddate) {

    if (typeof enddate == "undefined") {
        return false
    }

    var arr = startdate.split("-");
    var starttime = new Date(arr[0], arr[1], arr[2]);
    var starttimes = starttime.getTime();

    var arrs = enddate.split("-");
    var lktime = new Date(arrs[0], arrs[1], arrs[2]);
    var lktimes = lktime.getTime();

    if (starttimes > lktimes) {
        return false;
    } else
        return true;

};
/**
 * 获取日期相差天数
 * 
 * @author 张子源
 * 
 * @param iframe
 */
com.dateDiff = function (startDate, endDate) {
    var eDate = new Date(endDate).getTime();
    var sDate = new Date(startDate).getTime();
    var days = (eDate - sDate) / 60000 / 1440;
    if (days >= 0) {
        return days;
    } else {
        return 0;
    }
};
/**
 * 获取时间是否相等
 * 
 * @author 卜永济
 * 
 * @param iframe
 */
com.dateEqual = function (startdate, enddate) {
    var arr = startdate.split("-");
    var starttime = new Date(arr[0], arr[1], arr[2]);
    var starttimes = starttime.getTime();

    var arrs = enddate.split("-");
    var lktime = new Date(arrs[0], arrs[1], arrs[2]);
    var lktimes = lktime.getTime();
    if (starttimes == lktimes) {
        return true;
    } else
        return false;

};
com.getQueryString = function (name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return null;

};
/**
 * @author 郭华(夏悸)
 * 
 * 生成UUID
 * 
 * @returns UUID字符串
 */
com.random4 = function () {
    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
};
com.UUID = function () {
    return (com.random4() + com.random4() + "-" + com.random4() + "-" + com.random4() + "-" + com.random4() + "-" + com.random4() + com.random4() + com.random4());
};
/**
 * 函数用于移动数据项
 * 
 * @author 张子源
 * 
 * @param e1:转出,e2:转入,strNotIn:不能移动的option
 * 2016-07-01  陳靜   轉移options附加css
 */
com.moveOption = function (e1, e2, strNotIn) {
    for (var i = 0; i < e1.options.length; i++) {
        if (strNotIn.indexOf(e1.options[i].value) < 0) {
            if (e1.options[i].selected) {
                var e = e1.options[i];
                e2.options.add(new Option(e.text, e.value));
                if (e.style.cssText) {
                    e2.options[e2.options.length - 1].style.cssText = e.style.cssText;
                }               
                e1.remove(i);
                i = i - 1;
            }
        }
    }
};
/**
 * 函数用于移动全部数据项
 * 
 * @author 张子源
 * 
 * @param e1:转出,e2:转入,strNotIn:不能移动的option
 * 2016-07-01  陳靜   轉移options附加css
 */
com.moveOptionAll = function (e1, e2, strNotIn) {
    for (var i = 0; i < e1.options.length; i++) {
        if (strNotIn.indexOf(e1.options[i].value) < 0) {
            var e = e1.options[i];
            e2.options.add(new Option(e.text, e.value));
            if (e.style.cssText) {
                e2.options[e2.options.length - 1].style.cssText = e.style.cssText;
            }
            e1.remove(i);
            i = i - 1;
        }
    }
};
/**
 * 获取select和group by的项
 * 
 * @author 张子源
 */
com.getOptionsValue = function () {
    var e1 = document.getElementById('select_cols_id'), 
        flag = true, selectOptions = "", 
        groupbyOptions = "", 
        selectChiName = "",
        selectEngName = "";
    for (var i = 0; i < e1.options.length; i++) {
        if (flag == true) {
            selectOptions = e1.options[i].value.toString();//+ " AS N'" + e1.options[i].text.toString() + "'";
            groupbyOptions = e1.options[i].value.toString();
            var index = e1.options[i].value.toString().indexOf('.');
            selectChiName = e1.options[i].text.toString(); flag = false;
            selectEngName = e1.options[i].value.toString().substr(index + 1); flag = false;
            
        }
        else {
            selectOptions += "," + e1.options[i].value.toString(); //+ " AS N'" + e1.options[i].text.toString() + "'";
            groupbyOptions += "," + e1.options[i].value.toString();
            selectChiName += "," + e1.options[i].text.toString();
            var index = e1.options[i].value.toString().indexOf('.');
            selectEngName += "," + e1.options[i].value.toString().substr(index + 1);
        }
    }
    var resOptions = {
        "selectOptions": selectOptions,
        "groupbyOptions": groupbyOptions,
        "selectChiName": selectChiName,
        "selectEngName": selectEngName
    };
    return resOptions;
};

/**
 * 获取select和group by的项增强版
 * 
 * @author 罗梯
 */
com.getOptionsValueEx = function () {
    var e1 = document.getElementById('select_cols_id'),
        flag = true,
        selectOptions = "",     //全英筛选项，如 Customer_ID,ChineseName
        groupbyOptions = "",    
        selectChiName = "",     //下拉框选中描述，如 会员编号，会员卡号
        selectEngName = "",     //下拉框选中值，如 Customer_ID,Card_No
        selectChiOption = "";   //中文筛选项，如 Customer_ID as N'会员编号', Card_No as N'卡号'
    for (var i = 0; i < e1.options.length; i++) {
        var optionValue = e1.options[i].value.toString();
        var splitStr = "";
        if (flag) {
            flag = false; splitStr = "";
        }
        else {
            splitStr = ",";
        }
        //将 tab.[xx bb]换成 tab.[xx bb] as xx_bb 
        //将 tab.[xx bb]换成 tab.[xx bb] as tab_xx_bb,加上tab标识不同表格
        //var fixedField = optionValue.replace("[", "").replace("]", "").substr(optionValue.indexOf(".")+1).replace(" ", "_");
        var fixedField = optionValue.replace("[", "").replace("]", "").replace(" ", "_").replace(".", "_");
        selectOptions += splitStr + optionValue + " as " + fixedField;
        selectChiName += splitStr + e1.options[i].text.toString();
        groupbyOptions += splitStr + e1.options[i].value.toString();
        selectEngName += splitStr + fixedField;
        selectChiOption += splitStr + optionValue + " AS N'" + e1.options[i].text.toString() + "'";
    }
    var resOptions = {
        "selectOptions": selectOptions,    //内层选择
        "groupbyOptions": groupbyOptions,  //内层组合
        "selectChiName": selectChiName,    //中文描述
        "selectEngName": selectEngName,    //英文描述
        "selectChiOption": selectChiOption //中英文筛选项
    };
    return resOptions;

    //BCS.[Area Description]
};

/**
 * 动态组成datagrid的column
 * 
 * @author 张子源
 * 
 * @param myColumn:动态Column
 */
com.datagridColumn = function (myColumn) {
    var columnValue = new Array();
    var column = "";
    columnValue = (myColumn).split(",");
    for (var i = 0; i < columnValue.length; i++) {
        if (i == columnValue.length - 1) {
            column += "{\"field\":\"" + columnValue[i] + "\",\"title\":\"" + columnValue[i] + "\"}";
        } else {
            column += "{\"field\":\"" + columnValue[i] + "\",\"title\":\"" + columnValue[i] + "\"},";
        }
    }
    column = "[" + column + "]";
    return column;
};

/**
 * 动态组成datagrid的column(子源版本的增强版，增加自定义field)
 * 
 * @author 罗梯
 * 
 * @param myColumn:动态Column
 */
com.datagridColumn = function (myColumnTitle, myColumnValue) {
    var columnValue = new Array();
    var columnTitle = new Array();
    var column = "";
    columnValue = (myColumnValue).split(",");
    columnTitle = (myColumnTitle).split(",");
    for (var i = 0; i < columnValue.length; i++) {
        if (i == columnValue.length - 1) {
            column += "{\"field\":\"" + columnValue[i] + "\",\"title\":\"" + columnTitle[i] + "\"}";
        } else {
            column += "{\"field\":\"" + columnValue[i] + "\",\"title\":\"" + columnTitle[i] + "\"},";
        }
    }
    column = "[" + column + "]";
    return column;
};
/**
 * 兼容输入法的转大写操作
 * 
 * @author 钟周强
 * 
 * @param  id
 */
//com.upperCase = function (x) {
//    var y = document.getElementById(x).value;
//    if (y != y.toUpperCase()) {
//        document.getElementById(x).value = y.toUpperCase();
//    }
//};

/**
 * 获取URL参数
 * 
 * @author 卜永济
 * 
 * @param 参数名称
 */
com.getQueryString = function (name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return null;
};
/**
 * 获取URL参数
 * 
 * @author 卜永济
 * 
 * @param 参数名称
 */
com.getRequest = function (url) {
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        str = str.split("&");
        for (var i = 0; i < str.length; i++) {
            theRequest[str[i].split("=")[0]] = unescape(str[i].split("=")[1]);
        }
    }
    return theRequest;
};
/**
 * 货币格式
 * 
 * @author 张子源
 * 
 * @param 参数名称
 */
com.formatMoney = function(value) {
    if (!value) {
        return '0.00';
    }
    value = value + "";
    var s1 = value, s2 = "";
    var ponitPositon = value.indexOf(".");
    if (ponitPositon >= 0) {
        s1 = value.substring(0, ponitPositon);
        s2 = value.substring(ponitPositon + 1, value.length);
    }
    var p = /(\d+)(\d{3})/;
    while (p.test(s1)) {
        s1 = s1.replace(p, "$1" + "," + "$2");
    }
    if (s2) {
        return s1 + "." + s2;
    } else {
        return s1 + ".00";
    }
};


com.reFormatMoney = function (str) {
    if (typeof (str) != "undefined") {
        return parseFloat(str.replace(/,/g, ''));
    }
};