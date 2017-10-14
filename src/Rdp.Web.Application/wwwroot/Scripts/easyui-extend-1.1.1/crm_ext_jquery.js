var com = com || {};
com.data = com.data || {};// 用于存放临时的数据或者对象



/**
 * @author 卜永济
 * 
 * 增加命名空间功能
 * 
 * 使用方法：com.ns('jQuery.bbb.ccc','jQuery.eee.fff');
 */
com.ns = function () {
    var o = {}, d;
    for (var i = 0; i < arguments.length; i++) {
        d = arguments[i].split(".");
        o = window[d[0]] = window[d[0]] || {};
        for (var k = 0; k < d.slice(1).length; k++) {
            o = o[d[k + 1]] = o[d[k + 1]] || {};
        }
    }
    return o;
};

/**
 * 将form表单元素的值序列化成对象
 * 
 * @example com.serializeObject($('#formId'))
 * 
 * @author 卜永济
 * 
 * @requires jQuery
 * 
 * @returns object
 * @history:
   2015-4-28 LT  增加配置项 option{"toUpperCase":true,"toLowere": false}
   2015-5-14 LT  默认不变大小写，可通过配置项配置
 */
com.serializeObject = function (form, option) {
    var o = {};
    o.length = 0;
    $.each(form.serializeArray(), function (index) {
        if (this['value'] != undefined && this['value'].length > 0) {// 如果表单项的值非空，才进行序列化操作

            if (typeof (o[this['name']]) == "undefined") {
                o[this['name']] = "";
            }

            if (typeof (option) != "undefined" &&  typeof option.toUpperCase != "undefined" && option.toUpperCase == true) {
                o[this['name']] = o[this['name']] + (o[this['name']] != "" ? "," : "") + $.trim(this['value']).toUpperCase();
            } else if (typeof (option) != "undefined" &&  typeof option.toUpperCase != "undefined" && option.toLowerCase == true) {
                o[this['name']] = o[this['name']] + (o[this['name']] != "" ? "," : "") + $.trim(this['value']).toLowerCase();
            } else {
                o[this['name']] = o[this['name']] + (o[this['name']] != "" ? "," : "") + $.trim(this['value']).toString();
            }
            o.length = o.length + 1;
        }
    });
    return o;
};
com.serializeObjectByString = function (form, option) {
    var str = "";
    $.each(form.serializeArray(), function (index) {
        if (this['value'] != undefined && this['value'].length > 0) {// 如果表单项的值非空，才进行序列化操作
            if (typeof (option) != "undefined" && typeof option.toUpperCase != "undefined" && option.toUpperCase == true) {
                str += (str == "" ? "?" : "&") + this['name'] + '=' + $.trim(this['value']).toUpperCase();
            } else if (typeof (option) != "undefined" && typeof option.toUpperCase != "undefined" && option.toLowerCase == true) {
                str += (str == "" ? "?" : "&") + this['name'] + '=' + $.trim(this['value']).toLowerCase();
            } else {
                str += (str == "" ? "?" : "&") + this['name'] + '=' + $.trim(this['value']);
            }
        }
    });
    return str;
};

/**
 * 增加formatString功能
 * 
 * @author 卜永济
 * 
 * @example com.formatString('字符串{0}字符串{1}字符串','第一个变量','第二个变量');
 * 
 * @returns 格式化后的字符串
 */
com.formatString = function (str) {
    for (var i = 0; i < arguments.length - 1; i++) {
        str = str.replace("{" + i + "}", arguments[i + 1]);
    }
    return str;
};

/**
 * 接收一个以逗号分割的字符串，返回List，list里每一项都是一个字符串
 * 
 * @author 卜永济
 * 
 * @returns list
 */
com.stringToList = function (value) {
    if (value != undefined && value != '') {
        var values = [];
        var t = value.split(',');
        for (var i = 0; i < t.length; i++) {
            values.push('' + t[i]);/* 避免他将ID当成数字 */
        }
        return values;
    } else {
        return [];
    }
};

/**
 * JSON对象转换成String
 * 
 * @param o
 * @returns
 */
com.jsonToString = function (o) {
    var r = [];
    if (typeof o == "string")
        return "\"" + o.replace(/([\'\"\\])/g, "\\$1").replace(/(\n)/g, "\\n").replace(/(\r)/g, "\\r").replace(/(\t)/g, "\\t") + "\"";
    if (typeof o == "object") {
        if (!o.sort) {
            for (var i in o)
                r.push(i + ":" + com.jsonToString(o[i]));
            if (!!document.all && !/^\n?function\s*toString\(\)\s*\{\n?\s*\[native code\]\n?\s*\}\n?\s*$/.test(o.toString)) {
                r.push("toString:" + o.toString.toString());
            }
            r = "{" + r.join() + "}";
        } else {
            for (var i = 0; i < o.length; i++)
                r.push(com.jsonToString(o[i]));
            r = "[" + r.join() + "]";
        }
        return r;
    }
    return o.toString();
};

/**
 * Create a cookie with the given key and value and other optional parameters.
 * 
 * @example com.cookie('the_cookie', 'the_value');
 * @desc Set the value of a cookie.
 * @example com.cookie('the_cookie', 'the_value', { expires: 7, path: '/', domain: 'jquery.com', secure: true });
 * @desc Create a cookie with all available options.
 * @example com.cookie('the_cookie', 'the_value');
 * @desc Create a session cookie.
 * @example com.cookie('the_cookie', null);
 * @desc Delete a cookie by passing null as value. Keep in mind that you have to use the same path and domain used when the cookie was set.
 * 
 * @param String
 *            key The key of the cookie.
 * @param String
 *            value The value of the cookie.
 * @param Object
 *            options An object literal containing key/value pairs to provide optional cookie attributes.
 * @option Number|Date expires Either an integer specifying the expiration date from now on in days or a Date object. If a negative value is specified (e.g. a date in the past), the cookie will be deleted. If set to null or omitted, the cookie will be a session cookie and will not be retained when the the browser exits.
 * @option String path The value of the path atribute of the cookie (default: path of page that created the cookie).
 * @option String domain The value of the domain attribute of the cookie (default: domain of page that created the cookie).
 * @option Boolean secure If true, the secure attribute of the cookie will be set and the cookie transmission will require a secure protocol (like HTTPS).
 * @type undefined
 * 
 * @name com.cookie
 * @cat Plugins/Cookie
 * @author Klaus Hartl/klaus.hartl@stilbuero.de
 * 
 * Get the value of a cookie with the given key.
 * 
 * @example com.cookie('the_cookie');
 * @desc Get the value of a cookie.
 * 
 * @param String
 *            key The key of the cookie.
 * @return The value of the cookie.
 * @type String
 * 
 * @name com.cookie
 * @cat Plugins/Cookie
 * @author Klaus Hartl/klaus.hartl@stilbuero.de
 */

/**
 * 改变jQuery的AJAX默认属性和方法
 * 
 * @author 卜永济
 * 
 * @requires jQuery
 * 
 */
$.ajaxSetup({
    type: 'POST',
    dataFilter: function (data, type) {
        /*@history fixed iis8下返回值是空，返回类型json不会自动改变，jq在这种情况会报异常，这里增加校验，封装默认值 by LT 2016-2-30*/
        if (data == "" && typeof (type) === 'undefined') {
            data = '{"result":"0"}';
            return data;
        }
        return data;
    },
    error: function (xmlHttpRequest, textStatus, errorThrown) {

        var postData = {
            action: "add",
            Url: location.href,
            ErrorCode: xmlHttpRequest.status,
            ErrorMSG: xmlHttpRequest.responseText.substring(xmlHttpRequest.responseText.indexOf("<title>") + 7, xmlHttpRequest.responseText.indexOf("</title>")),
            StackTrace: xmlHttpRequest.responseText
        };
        var indexOfPD = location.href.indexOf("?PD=");
        if (indexOfPD != -1) {
            postData.ProgramID = location.href.substring(indexOfPD + 4, location.href.indexOf("&") == -1 ? location.href.length : location.href.indexOf("&"));
        }

        $.ajax({
            url: '/Error/Add',
            data: postData,
            type: 'post',
            success: function (data) {
            }
        });
        try {
            parent.$.messager.progress('close');
            parent.com.messagerAlert('错误', xmlHttpRequest.responseText + ";" + errorThrown.message);
        } catch (e) {
            alert(xmlHttpRequest.responseText);
        }
    }
});


/**
 * 解决class="iconImg"的img标记，没有src的时候，会出现边框问题
 * 
 * @author 卜永济
 * 
 * @requires jQuery
 */
$(function () {
    $('.iconImg').attr('src', com.pixel_0);
});
/**
 * 验证是否有权限操作
 * 
 * @author 卜永济
 * 
 * @requires jQuery
 */
//todo测试
com.checkRight = function (fn) {
    $.ajax({
        url: '/Privilege/CheckRight',
        data: { userId: $("#TB_StaffCode").val(), programId: com.getQueryString("PD") },
        success: function (data) {
            if (typeof (data.ErrorNo) != 'undefined' && data.ErrorNo != 0) {
                com.messagerAlert(ResDisplay.Tips, data.res);
            } else {
                fn();
            }
        }
    });
};