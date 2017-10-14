var com = com || {};


/**
 * panel关闭时回收内存，主要用于layout使用iframe嵌入网页时的内存泄漏问题
 * 
 * @author 卜永济
 * 
 * @requires jQuery,EasyUI
 * 
 */
$.extend($.fn.panel.defaults, {
    onBeforeDestroy: function () {
        var frame = $('iframe', this);
        try {
            if (frame.length > 0) {
                for (var i = 0; i < frame.length; i++) {
                    frame[i].src = '';
                    frame[i].contentWindow.document.write('');
                    frame[i].contentWindow.close();
                }
                frame.remove();
                if (navigator.userAgent.indexOf("MSIE") > 0) {// IE特有回收内存方法
                    try {
                        CollectGarbage();
                    } catch (e) {
                    }
                }
            }
        } catch (e) {
        }
    }
});
/**
 * easyui的combobox扩展默认选择第一行，调用方法.combobox('selectedIndex', 0);
 * 
 * @author 卜永济
 * 
 * @requires jQuery,EasyUI
 */
$.extend($.fn.combobox.methods, {
    selectedIndex: function (jq, index) {
        if (!index) {
            index = 0;
        }
        $(jq).combobox({
            onLoadSuccess: function () {
                var opt = $(jq).combobox('options');
                var data = $(jq).combobox('getData');
                for (var i = 0; i < data.length; i++) {
                    if (i == index) {
                        $(jq).combobox('setValue', eval('data[index].' + opt.valueField));
                        break;
                    }
                }
            }
        });
    }
});
/**
 * 进度条显示为数据。而不是百分比
 * 
 * @author 卜永济
 * 
 * @requires jQuery,EasyUI
 */
com.progress = function () {
    $.messager.progress({
        text: ResDisplay.Loading
    });
};


/**
 * 
 * 通用错误提示
 * 
 * 用于datagrid/treegrid/tree/combogrid/combobox/form加载数据出错时的操作
 * 
 * @author 卜永济
 * 
 * @requires jQuery,EasyUI
 */
com.onLoadError = {
    onLoadError: function (xmlHttpRequest) {
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
        var loadError = xmlHttpRequest.responseText || ResMessage.loadError;
        if (parent.$ && parent.$.messager) {
            parent.$.messager.progress('close');

        } else {
            $.messager.progress('close');

        }

        if (parent.com && parent.com.messagerAlert) {
            parent.com.messagerAlert(ResDisplay.Result, loadError);
        }
        else if (com.messagerAlert) {
            com.messagerAlert(ResDisplay.Result, loadError);
        }
        else {
            alert(loadError);
        }

    }
};



$.extend($.fn.datagrid.defaults, com.onLoadError);
$.extend($.fn.treegrid.defaults, com.onLoadError);
$.extend($.fn.tree.defaults, com.onLoadError);
$.extend($.fn.combogrid.defaults, com.onLoadError);
$.extend($.fn.combobox.defaults, com.onLoadError);
$.extend($.fn.form.defaults, com.onLoadError);



/**
 * 扩展tree和combotree，使其支持平滑数据格式
 * 
 * @author 卜永济
 * 
 * @requires jQuery,EasyUI
 * 
 */
com.loadFilter = {
    loadFilter: function (data, parent) {
        var opt = $(this).data().tree.options;
        var idField, textField, parentField;
        if (opt.parentField) {
            idField = opt.idField || 'id';
            textField = opt.textField || 'text';
            parentField = opt.parentField || 'pid';
            var i, l, treeData = [], tmpMap = [];
            for (i = 0, l = data.length; i < l; i++) {
                tmpMap[data[i][idField]] = data[i];
            }
            for (i = 0, l = data.length; i < l; i++) {
                if (tmpMap[data[i][parentField]] && data[i][idField] != data[i][parentField]) {
                    if (!tmpMap[data[i][parentField]]['children'])
                        tmpMap[data[i][parentField]]['children'] = [];
                    data[i]['text'] = data[i][textField];
                    tmpMap[data[i][parentField]]['children'].push(data[i]);
                } else {
                    data[i]['text'] = data[i][textField];
                    treeData.push(data[i]);
                }
            }
            return treeData;
        }
        return data;
    }
};
$.extend($.fn.combotree.defaults, com.loadFilter);
$.extend($.fn.tree.defaults, com.loadFilter);

/**
 * 扩展treegrid，使其支持平滑数据格式
 * 
 * @author 卜永济
 * 
 * @requires jQuery,EasyUI
 * 
 */
$.extend($.fn.treegrid.defaults, {
    loadFilter: function (data, parentId) {
        var opt = $(this).data().treegrid.options;
        var idField, treeField, parentField;
        if (opt.parentField) {
            idField = opt.idField || 'id';
            treeField = opt.textField || 'text';
            parentField = opt.parentField || 'pid';
            var i, l, treeData = [], tmpMap = [];
            for (i = 0, l = data.length; i < l; i++) {
                tmpMap[data[i][idField]] = data[i];
            }
            for (i = 0, l = data.length; i < l; i++) {
                if (tmpMap[data[i][parentField]] && data[i][idField] != data[i][parentField]) {
                    if (!tmpMap[data[i][parentField]]['children'])
                        tmpMap[data[i][parentField]]['children'] = [];
                    data[i]['text'] = data[i][treeField];
                    tmpMap[data[i][parentField]]['children'].push(data[i]);
                } else {
                    data[i]['text'] = data[i][treeField];
                    treeData.push(data[i]);
                }
            }
            return treeData;
        }
        return data;
    }
});
/**
 * 选择datagrid中的一行，并进行操作
 * 
 * @author 卜永济
 * 
 * @requires jQuery,EasyUI
 * 
 */
com.gridSelectOperate = function (grid, fn) {
    var row = grid.datagrid('getRows');
    if (row.length == 1) {
        grid.datagrid('selectRow', 0);
        fn(row);
    } else if (row.length == 0) {
        com.messagerAlert(ResDisplay.Result, ResMessage.NoData);
    } else {
        row = grid.datagrid('getSelections');
        if (row.length > 0) {
            fn(row);
        } else {
            com.messagerAlert(ResDisplay.Result, ResMessage.SelectOne);
        };
    }
};
/**
 * 创建一个模式化的dialog
 * 
 * @author 卜永济
 * 
 * @requires jQuery,EasyUI
 * 
 */
com.modalDialog = function (options) {
    var opts = $.extend({
        id: "dlgCurrent",
        title: '&nbsp;',
        width: 800,
        height: 520,
        modal: true,
        top: 10,
        onClose: function () {
            $(this).dialog('destroy');
        }
    }, options);
    opts.modal = true;// 强制此dialog为模式化，无视传递过来的modal参数
    if (options.url) {
        var ifrId = 'ifr' + com.getDate();
        opts.content = '<iframe id="' + ifrId + '" src="" allowTransparency="true" scrolling="auto" width="98%" height="98%" frameBorder="0" name=""></iframe>';
        var dialogObj = $("<div id='dlgCurrent'/>").dialog(opts);
        $('#' + ifrId).attr('src', options.url);
        return dialogObj;
    } else {
        return $('<div/>').dialog(opts);
    }
};


/**
 * 创建一个模式化的combobox
 * 
 * @author 卜永济
 * 
 * @requires jQuery,EasyUI
 * 
 */
com.cookie = function (key, value, options) {
    if (arguments.length > 1 && (value === null || typeof value !== "object")) {
        options = $.extend({}, options);
        if (value === null) {
            options.expires = -1;
        }
        if (typeof options.expires === 'number') {
            var days = options.expires, t = options.expires = new Date();
            t.setDate(t.getDate() + days);
        }
        return (document.cookie = [encodeURIComponent(key), '=', options.raw ? String(value) : encodeURIComponent(String(value)), options.expires ? '; expires=' + options.expires.toUTCString() : '', options.path ? '; path=' + options.path : '', options.domain ? '; domain=' + options.domain : '', options.secure ? '; secure' : ''].join(''));
    }
    options = value || {};
    var result, decode = options.raw ? function (s) {
        return s;
    } : decodeURIComponent;
    return (result = new RegExp('(?:^|; )' + encodeURIComponent(key) + '=([^;]*)').exec(document.cookie)) ? decode(result[1]) : null;
};

com.comboboxVersion = com.cookie('Version');
com.comboboxWithClear = function (ddlName, jsonName, editable, multiple) {
    $("#" + ddlName).combobox({
        // url: '/Ajax/CodeTable/GetCodeTable.ashx?' + com.comboboxVersion + '&tableName=' + jsonName,
        url: '/CommonCombo/GetList?' + com.comboboxVersion + '&refTable=' + jsonName,
        valueField: 'id',
        textField: 'text',
        method: 'GET',
        type: 'json',
        cache: true,
        editable: editable,
        multiple: multiple,
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        },
        onLoadSuccess: function () {
            $("#" + ddlName).combobox('clear');
            if (typeof ($("#" + ddlName).attr("readonly")) == "undefined" && typeof ($("#" + ddlName).attr("disabled") == "undefined")) {
                $("#" + ddlName).combobox('textbox').bind('focus', function () {
                    $("#" + ddlName).combobox('showPanel');
                });
            }

        }
    });
};


com.comboboxWithValues = function (ddlName, jsonName, editable, multiple, values, CancelOperCallBack) {
    $("#" + ddlName).combobox({
        // url: '/Ajax/CodeTable/GetCodeTable.ashx?' + com.comboboxVersion + '&tableName=' + jsonName,
        url: '/CommonCombo/GetList?' + com.comboboxVersion + '&refTable=' + jsonName,
        valueField: 'id',
        textField: 'text',
        method: 'GET',
        type: 'json',
        cache: true,
        editable: editable,
        multiple: multiple,
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.valueField].toString().indexOf(q) >= 0;
        },
        onLoadSuccess: function () {
            $("#" + ddlName).combobox('setValues', values);
            if (typeof (CancelOperCallBack) != "undefined") {
                var data = $("#" + ddlName).combobox('getData');
                if (data.length > 0) {
                    $("#" + ddlName).combobox('select', data[0]["id"]);
                }
            }
            if (typeof ($("#" + ddlName).attr("readonly")) == "undefined" && typeof ($("#" + ddlName).attr("disabled") == "undefined")) {
                $("#" + ddlName).combobox('textbox').bind('focus', function () {
                    $("#" + ddlName).combobox('showPanel');
                });
            }

            //if (typeof (CancelOperCallBack) != "undefined") {
            //    CancelOperCallBack()
            //}
        },
        loadFilter: function (data) {
            if (typeof (CancelOperCallBack) != "undefined") {
                return CancelOperCallBack(data);
            }

            return data;
        }
    });
};
//com.comboboxVersion+'id='+value+ '不同的值会重新请求
com.comboboxWithOneValue = function (ddlName, jsonName, editable, multiple, value) {
    $("#" + ddlName).combobox({
        //url: '/Ajax/CodeTable/GetCodeTable.ashx?' + com.comboboxVersion + 'id=' + value + '&tableName=' + jsonName + '&whereStr=id= \'\'' + value + ' \'\'',
        url: '/CommonCombo/GetList?' + com.comboboxVersion + '&refTable=' + jsonName + '&whereStr=id= \'\'' + value + ' \'\'',
        valueField: 'id',
        textField: 'text',
        method: 'GET',
        type: 'json',
        panelHeight: 'auto',
        cache: true,
        editable: editable,
        multiple: multiple,
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        },
        onLoadSuccess: function () {
            $("#" + ddlName).combobox('setValues', value);
            if (typeof ($("#" + ddlName).attr("readonly")) == "undefined" && typeof ($("#" + ddlName).attr("disabled") == "undefined")) {
                $("#" + ddlName).combobox('textbox').bind('focus', function () {
                    $("#" + ddlName).combobox('showPanel');
                });
            }
        }
    });
};

com.comboboxWithSearch = function (ddlName, jsonName, editable, multiple, value) {
    var len = arguments[5] || 1;//JS設置默認值
    $("#" + ddlName).combobox({
        // url: '/Ajax/CodeTable/GetCodeTable.ashx?' + com.comboboxVersion + 'id=' + value + '&tableName=' + jsonName + '&whereStr=id like \'\'%25' + value + '%25\'\' or text like N\'\'%25' + value + '%\'\'',
        url: '/CommonCombo/GetList?' + com.comboboxVersion + '&refTable=' + jsonName + '&whereStr=id like \'\'%25' + value + '%25\'\' or text like N\'\'%25' + value + '%\'\'',
        valueField: 'id',
        textField: 'text',
        method: 'GET',
        type: 'json',
        panelHeight: 'auto',
        cache: true,
        editable: editable,
        multiple: multiple,
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        },
        onChange: function () {
            if ($("#" + ddlName).combobox('getValue').length > len) {
                //    $("#" + ddlName).combobox('reload', '/Ajax/CodeTable/GetCodeTable.ashx?' + com.comboboxVersion + 'id=' + escape($("#" + ddlName).combobox('getValue')) + '&tableName=' + jsonName + '&whereStr=id like \'\'%25' + escape($("#" + ddlName).combobox('getValue')) + '%25\'\' or text like N\'\'%25' + escape($("#" + ddlName).combobox('getValue')) + '%\'\'');
                //}
                $("#" + ddlName).combobox('reload', '/CommonCombo/GetList?' + com.comboboxVersion + '&refTable=' + jsonName + '&whereStr=id like \'\'%25' + escape($("#" + ddlName).combobox('getValue')) + '%25\'\' or text like N\'\'%25' + escape($("#" + ddlName).combobox('getValue')) + '%\'\'');
            }
        },
        onLoadSuccess: function () {
            if (typeof ($("#" + ddlName).attr("readonly")) == "undefined" && typeof ($("#" + ddlName).attr("disabled") == "undefined")) {
                $("#" + ddlName).combobox('textbox').bind('focus', function () {
                    $("#" + ddlName).combobox('showPanel');
                });
            };
        }

    });
};
/*2015-12-09*/
//timeout单位:毫秒，例如2000表示2秒
com.comboboxWithSearch = function (ddlName, jsonName, editable, multiple, value, timeout) {
    var len = arguments[5] || 1;//JS設置默認值 
    var thread_id;
    $("#" + ddlName).combobox({
        //url: '/Ajax/CodeTable/GetCodeTable.ashx?' + com.comboboxVersion + 'id=' + value + '&tableName=' + jsonName + '&whereStr=id like \'\'%25' + value + '%25\'\' or text like N\'\'%25' + value + '%\'\' Order By id OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY ',
        url: '/CommonCombo/GetList?' + com.comboboxVersion + '&refTable=' + jsonName + '&whereStr=id like \'\'%25' + value + '%25\'\' or text like N\'\'%25' + value + '%\'\' Order By id OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY ',
        valueField: 'id',
        textField: 'text',
        method: 'GET',
        type: 'json',
        panelHeight: 'auto',
        cache: true,
        editable: editable,
        multiple: multiple,
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        },
        onChange: function () {
            if ($("#" + ddlName).combobox('getValue').length > len) {
                if (thread_id != undefined)
                { clearTimeout(thread_id); }
                thread_id = setTimeout(function () {
                    //    $("#" + ddlName).combobox('reload', '/Ajax/CodeTable/GetCodeTable.ashx?' + com.comboboxVersion + 'id=' + escape($("#" + ddlName).combobox('getValue')) + '&tableName=' + jsonName + '&whereStr=id like \'\'%25' + escape($("#" + ddlName).combobox('getValue')) + '%25\'\' or text like N\'\'%25' + escape($("#" + ddlName).combobox('getValue')) + '%\'\' Order By id OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY');
                    //}, timeout)
                    $("#" + ddlName).combobox('reload', '/CommonCombo/GetList?' + com.comboboxVersion + '&refTable=' + jsonName + '&whereStr=id like \'\'%25' + escape($("#" + ddlName).combobox('getValue')) + '%25\'\' or text like N\'\'%25' + escape($("#" + ddlName).combobox('getValue')) + '%\'\' Order By id OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY');
                }, timeout)
            }
        },
        onLoadSuccess: function () {
            if (typeof ($("#" + ddlName).attr("readonly")) == "undefined" && typeof ($("#" + ddlName).attr("disabled") == "undefined")) {
                $("#" + ddlName).combobox('textbox').bind('focus', function () {
                    $("#" + ddlName).combobox('showPanel');
                });
            };
        }
    });
};
com.comboboxWithIdSearch = function (ddlName, jsonName, editable, multiple, value) {
    var len = arguments[5] || 0;//JS設置默認值
    $("#" + ddlName).combobox({
        // url: '/Ajax/CodeTable/GetCodeTable.ashx?' + com.comboboxVersion + 'id=' + value + '&tableName=' + jsonName + '&whereStr=id like \'\'%25' + value + '%25\'\'',
        url: '/CommonCombo/GetList?' + com.comboboxVersion + '&refTable=' + jsonName + '&whereStr=id like \'\'%25' + value + '%25\'\'',
        valueField: 'id',
        textField: 'text',
        method: 'GET',
        type: 'json',
        panelHeight: 'auto',
        cache: true,
        editable: editable,
        multiple: multiple,
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        },
        onChange: function () {
            if ($("#" + ddlName).combobox('getValue').length > len) {
                //$("#" + ddlName).combobox('reload', '/Ajax/CodeTable/GetCodeTable.ashx?' + com.comboboxVersion + 'id=' + escape($("#" + ddlName).combobox('getValue')) + '&tableName=' + jsonName + '&whereStr=id like \'\'%25' + escape($("#" + ddlName).combobox('getValue')) + '%25\'\'');
                $("#" + ddlName).combobox('reload', '/CommonCombo/GetList?' + com.comboboxVersion + '&refTable=' + jsonName + '&whereStr=id like \'\'%25' + escape($("#" + ddlName).combobox('getValue')) + '%25\'\'');
            }
        },
        onLoadSuccess: function () {
            if (typeof ($("#" + ddlName).attr("readonly")) == "undefined" && typeof ($("#" + ddlName).attr("disabled") == "undefined")) {
                $("#" + ddlName).combobox('textbox').bind('focus', function () {
                    $("#" + ddlName).combobox('showPanel');
                });
            }
        }

    });
};

//注意where中有引号需要多加一个，如：id='A' 应写为：id=''A''
com.comboboxWithWhere = function (ddlName, jsonName, editable, multiple, where) {
    $("#" + ddlName).combobox({
        //url: '/Ajax/CodeTable/GetCodeTable.ashx?' + com.comboboxVersion + '&tableName=' + jsonName + '&whereStr='+ where,
        url: '/CommonCombo/GetList?' + com.comboboxVersion + '&refTable=' + jsonName + '&whereStr=' + where,
        valueField: 'id',
        textField: 'text',
        method: 'GET',
        type: 'json',
        cache: true,
        editable: editable,
        panelHeight: 'auto',
        multiple: multiple,
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        },
        onLoadSuccess: function () {
            if (typeof ($("#" + ddlName).attr("readonly")) == "undefined" && typeof ($("#" + ddlName).attr("disabled") == "undefined")) {
                $("#" + ddlName).combobox('textbox').bind('focus', function () {
                    $("#" + ddlName).combobox('showPanel');
                });
            }
        }
    });
};

com.combobox = function (ddlName, jsonName, editable, multiple) {
    $("#" + ddlName).combobox({
        //url: '/Ajax/CodeTable/GetCodeTable.ashx?' + com.comboboxVersion + '&tableName=' + jsonName,
        url: '/CommonCombo/GetList?' + com.comboboxVersion + '&refTable=' + jsonName,
        valueField: 'id',
        textField: 'text',
        method: 'GET',
        type: 'json',
        cache: true,
        editable: editable,
        panelHeight: 'auto',
        multiple: multiple,
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        },
        onLoadSuccess: function () {
            if (typeof ($("#" + ddlName).attr("readonly")) == "undefined" && typeof ($("#" + ddlName).attr("disabled") == "undefined")) {
                $("#" + ddlName).combobox('textbox').bind('focus', function () {
                    $("#" + ddlName).combobox('showPanel');
                });
            }
        }
    });
};
com.combobox = function (ddlName, jsonName, editable, multiple, panelHeight) {
    $("#" + ddlName).combobox({
        //url: '/Ajax/CodeTable/GetCodeTable.ashx?' + com.comboboxVersion + '&tableName=' + jsonName,
        url: '/CommonCombo/GetList?' + com.comboboxVersion + '&refTable=' + jsonName,
        valueField: 'id',
        textField: 'text',
        method: 'GET',
        type: 'json',
        cache: true,
        editable: editable,
        multiple: multiple,
        panelHeight: panelHeight,
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        },
        onLoadSuccess: function () {
            if (typeof ($("#" + ddlName).attr("readonly")) == "undefined" && typeof ($("#" + ddlName).attr("disabled") == "undefined")) {
                $("#" + ddlName).combobox('textbox').bind('focus', function () {
                    $("#" + ddlName).combobox('showPanel');
                });
            }
        }
    });
};
com.combobox = function (ddlName, jsonName, editable, multiple, panelHeight, required) {
    $("#" + ddlName).combobox({
        //url: '/Ajax/CodeTable/GetCodeTable.ashx?' + com.comboboxVersion + '&tableName=' + jsonName,
        url: '/CommonCombo/GetList?' + com.comboboxVersion + '&refTable=' + jsonName,
        valueField: 'id',
        textField: 'text',
        method: 'GET',
        type: 'json',
        cache: true,
        editable: editable,
        multiple: multiple,
        panelHeight: panelHeight,
        required: required,
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        },
        onLoadSuccess: function () {
            if (typeof ($("#" + ddlName).attr("readonly")) == "undefined" && typeof ($("#" + ddlName).attr("disabled") == "undefined")) {
                $("#" + ddlName).combobox('textbox').bind('focus', function () {
                    $("#" + ddlName).combobox('showPanel');
                });
            }
        }

    });
};

com.Identity = function (partition, requestURL, fun) {
    $.get("/Identity/Codesnippet", { partition: partition, requestUrl: requestURL }, function (data) {
        if (typeof (data) == "string" && data.trim().length != 0) {
            operateWithValidate(fun);
        }
        else {
            fun();
        }
    }, 'html');
}

/**
 * 封装分店行号搜索弹出框的方法
 *调用方法：需要传入需要（在哪里）显示结果的id
 * 
 * @author 钟周强
 * History:
 *  2017-03-03     Tim    定义回调函数，避免过多在子页面中HardCode逻辑，降低模块间耦合度
 * 
 * @requires jQuery easyui
 */
var BranchSearchDialog;
com.branchSearch = function (searchId) {
    BranchSearchDialog = com.modalDialog({
        title: ResDisplay.BranchSearch,
        url: '/Branch/Search?parentDialog=BranchSearchDialog',
        width: 800
    });

    BranchSearchDialog.onValueSelected = function (value) {
        $("#" + searchId).val(value);
        $("#" + searchId).focus();
        $("#" + searchId).validatebox('validate');
        BranchSearchDialog.dialog('close', true);
    }
};

/**
 * 封装职员搜索弹出框的方法
 *调用方法：需要传入需要（在哪里）显示结果的id
 * 
 * @author 钟周强
 * 
 * @requires jQuery easyui
 */
var UserSearchDialog;
com.userSearch = function (searchId) {
    UserSearchDialog = com.modalDialog({
        title: ResDisplay.UserSearch,
        url: '/Web/Added/UserSearch/UserSearch.aspx?parentSearchID=' + searchId
    });;
};


var ShowModalDialog;
com.showModalDialog = function (controller, action, data, callBack) {
    var urlData = "";
    $.each(data, function (name, val) {
        urlData += "&" + name + "=" + val;
    });

    ShowModalDialog = com.modalDialog({
        url: "/" + controller + "/" + action + "?parentDialog=ShowModalDialog" + urlData,
        width: 800,
        height: 600
    });

    ShowModalDialog.onValueSelected = function (value) {
        ShowModalDialog.dialog('close', true);
        if (typeof (callBack) != "undefined") {
            callBack(value);
        }
    }
};

/**
 * 封装根据卡号（Car_No）搜索客户编号（Customer_ID）弹出框的方法
 *调用方法：需要传入需要（在哪里）显示结果的id
 * 
 * @author 钟周强
 * 
 * @requires jQuery easyui
 */
var CustomerSearchDialog;
com.customerSearch = function (searchId) {
    CustomerSearchDialog = com.modalDialog({
        title: ResDisplay.CustomerSearch,
        width: 300,
        height: 150,
        url: '/Web/Added/CustomerSearch/CustomerSearch.aspx?parentSearchID=' + searchId
    });;
};

/**
 *重新封装 $.messager.alert()
 *解决easyui $.messager.alert()点击右上角的叉不能执行回调函数的方法及执行com.enterSubmit后影响回车事件
 * 
 * @author 钟周强

 重载方法包含以下：
 com.messagerAlert = function (title, msg)  

 com.messagerAlert = function (title, msg, fn)  
    默认isCloseExecFn为false  关闭右上角的叉时不执行fn

 com.messagerAlert = function (title, msg, fn, isCloseExecFn)
    isCloseExecFn为true关闭右上角的叉时执行fn，为false关闭右上角的叉时不执行fn

 * 
 * @requires jQuery easyui
 */
com.messagerAlert = function (title, msg) {
    var fn = arguments[2] || "";
    var isCloseExecFn = arguments[3] || false;
    var enterEvent = document.onkeydown;

    var win = isCloseExecFn || fn == "" ? $.messager.alert(title, msg) : $.messager.alert(title, msg, '', fn);
    win.window({
        onBeforeOpen: function () {
            document.onkeydown = 'false';
        },
        onBeforeClose: function () {
            document.onkeydown = enterEvent;
            if (isCloseExecFn && typeof (fn) == 'function') fn();
            win.window('close', true);
            return false;
        }
    });
    win.children("div.messager-button").children("a:first").focus();
};


com.messagerAlert = function (title, msg, fn, isCloseExecFn) {
    var fn = arguments[2] || "";
    var isCloseExecFn = arguments[3] || false;
    var enterEvent = document.onkeydown;

    var win = isCloseExecFn || fn == "" ? $.messager.alert(title, msg) : $.messager.alert(title, msg, '', fn);

};



com.messagerConfirm = function (title, msg, fn) {
    var enterEvent = document.onkeydown;
    var win = $.messager.confirm(title, msg, fn);
    win.window({
        onBeforeOpen: function () {
            document.onkeydown = 'false';
        },
        onBeforeClose: function () {
            document.onkeydown = enterEvent;
            win.window('close', true);
            return false;
        }
    });
    win.children("div.messager-button").children("a:first").focus();
};
/**
 * 解决筛选不能为空
 * 
 * @author 钟周强
 * 
 * @requires jQuery easyui
 */
com.filter = function () {
    if ($('#F_Search').form('validate')) {
        var data = com.serializeObject($('#F_Search'), { toUpperCase: false });
        if (data.length == 0) {
            alert(ResMessage.FormIsEmpty);
        } else {

            //添加grid的列选项 add by Tim 2015-8-20
            if (typeof (grid) != "undefined" && grid) {
                var columnFields = grid.datagrid('getColumnFields').join(",");
                var frozenColumnFields = grid.datagrid('getColumnFields', true).join(",");
                columnFields = columnFields + (frozenColumnFields != "" ? "," : "") + frozenColumnFields;
                if (columnFields != "") {
                    data.length = data.length + 1;
                    data["columnFields"] = columnFields;
                }

                data.length = data.length + 1;
                data["gridFields"] = com.getDatagridFields(grid, true);

                //增加总条数，避免下一页的时候再次load总条数 add by Tim 2017-03-28
                data.length = data.length + 1;
                data["total"] = function () { //?这里为什么要用函数变量，因为下一页的时候不会跑进来，必须通过函数实时计算。
                    var gridData = grid.datagrid('getData');
                    if (gridData != "undefined" && gridData.total != "undefined") {
                        return gridData.total;
                    }
                    else
                        return 0;
                }
            }

            grid.datagrid('load', data);
        }
    }
};
com.reFilter = function () {
    //$('#F_Search input').val('');
    //$('#F_Search textarea').val('');
    $('#F_Search').form('clear');
    grid.datagrid('loadData', []);
};
/**
 * //显示tab页面(存在相同tab，则直接选择)
 * 
 * @author 卜永濟
 * 
 * @requires jQuery easyui
 */

com.showTab = function (tabsId, url, text, iconCls, closable) {
    var tabs = $("#" + tabsId);
    var opts = {
        title: text,
        closable: closable,
        iconCls: iconCls,
        content: com.formatString('<iframe src="{0}" allowTransparency="true" style="border:0;width:100%;height:99%;" frameBorder="0"></iframe>', url),
        border: false,
        fit: true
    };
    if (tabs.tabs('exists', opts.title)) {
        tabs.tabs('select', opts.title);
    } else {
        com.progress();
        tabs.tabs('add', opts);
    }
};


/**
 * 添加easyui tabs的右键菜单事件
 * 
 * @author 钟周强
 * 
 * @requires jQuery easyui

 *参数：tabsMain           为tbs div的ID
        tabRightMenuId     为tab 右键菜单ID
 */
//********************************添加easyui tabs的右键菜单事件【begin】**************************************************
com.tabsOnContextMenu = function (tabsMain, tabRightMenuId) {
    var tabsId = tabsMain;//tabs页签Id
    var tabRightmenuId = tabRightMenuId;//tabs右键菜单Id

    //绑定tabs的右键菜单
    $("#" + tabsId).tabs({
        onContextMenu: function (e, title) {//这时去掉 tabsId所在的div的这个属性：class="easyui-tabs"，否则会加载2次
            e.preventDefault();
            $('#' + tabRightmenuId).menu('show', {
                left: e.pageX,
                top: e.pageY
            }).data("tabTitle", title);
        }
    });

    //实例化menu的onClick事件
    $("#" + tabRightmenuId).menu({
        onClick: function (item) {
            CloseTab(tabsId, tabRightmenuId, item.name);
        }
    });
};
/**
    tab关闭事件
    @param	tabId		tab组件Id
    @param	tabMenuId	tab组件右键菜单Id
    @param	type		tab组件右键菜单div中的name属性值
*/
function CloseTab(tabId, tabMenuId, type) {
    //tab组件对象
    var tabs = $('#' + tabId);
    //tab组件右键菜单对象
    var tabMenu = $('#' + tabMenuId);

    //获取当前tab的标题
    var curTabTitle = tabMenu.data('tabTitle');

    //关闭当前tab
    if (type === 'tab_menu-tabclose') {
        //通过标题关闭tab
        tabs.tabs("close", curTabTitle);
    }

    //关闭全部tab
    else if (type === 'tab_menu-tabcloseall') {
        //获取所有关闭的tab对象
        var closeTabsTitle = getAllTabObj(tabs);
        //循环删除要关闭的tab
        $.each(closeTabsTitle, function () {
            var title = this;
            tabs.tabs('close', title);
        });
    }

    //关闭其他tab
    else if (type === 'tab_menu-tabcloseother') {
        //获取所有关闭的tab对象
        var closeTabsTitle = getAllTabObj(tabs);
        //循环删除要关闭的tab
        $.each(closeTabsTitle, function () {
            var title = this;
            if (title != curTabTitle) {
                tabs.tabs('close', title);
            }
        });
    }

    //关闭当前左侧tab
    else if (type === 'tab_menu-tabcloseleft') {
        //获取所有关闭的tab对象
        var closeTabsTitle = getLeftToCurrTabObj(tabs, curTabTitle);
        //循环删除要关闭的tab
        $.each(closeTabsTitle, function () {
            var title = this;
            tabs.tabs('close', title);
        });
    }

    //关闭当前右侧tab
    else if (type === 'tab_menu-tabcloseright') {
        //获取所有关闭的tab对象
        var closeTabsTitle = getRightToCurrTabObj(tabs, curTabTitle);
        //循环删除要关闭的tab
        $.each(closeTabsTitle, function () {
            var title = this;
            tabs.tabs('close', title);
        });
    }
};

/**
    获取所有关闭的tab对象
    @param	tabs	tab组件
*/
function getAllTabObj(tabs) {
    //存放所有tab标题
    var closeTabsTitle = [];
    //所有所有tab对象
    var allTabs = tabs.tabs('tabs');
    $.each(allTabs, function () {
        var tab = this;
        var opt = tab.panel('options');
        //获取标题
        var title = opt.title;
        //是否可关闭 ture:会显示一个关闭按钮，点击该按钮将关闭选项卡
        var closable = opt.closable;
        if (closable) {
            closeTabsTitle.push(title);
        }
    });
    return closeTabsTitle;
};

/**
    获取左侧第一个到当前的tab
    @param	tabs		tab组件
    @param	curTabTitle	到当前的tab
*/
function getLeftToCurrTabObj(tabs, curTabTitle) {
    //存放所有tab标题
    var closeTabsTitle = [];
    //所有所有tab对象
    var allTabs = tabs.tabs('tabs');
    for (var i = 0; i < allTabs.length; i++) {
        var tab = allTabs[i];
        var opt = tab.panel('options');
        //获取标题
        var title = opt.title;
        //是否可关闭 ture:会显示一个关闭按钮，点击该按钮将关闭选项卡
        var closable = opt.closable;
        if (closable) {
            //alert('title' + title + '  curTabTitle:' + curTabTitle);
            if (title == curTabTitle) {
                return closeTabsTitle;
            }
            closeTabsTitle.push(title);
        }
    }
    return closeTabsTitle;
}
;

/**
    获取当前到右侧最后一个的tab
    @param	tabs		tab组件
    @param	curTabTitle	到当前的tab
*/
function getRightToCurrTabObj(tabs, curTabTitle) {
    //存放所有tab标题
    var closeTabsTitle = [];
    //所有所有tab对象
    var allTabs = tabs.tabs('tabs');
    for (var i = (allTabs.length - 1); i >= 0; i--) {
        var tab = allTabs[i];
        var opt = tab.panel('options');
        //获取标题
        var title = opt.title;
        //是否可关闭 ture:会显示一个关闭按钮，点击该按钮将关闭选项卡
        var closable = opt.closable;
        if (closable) {
            //alert('title' + title + '  curTabTitle:' + curTabTitle);
            if (title == curTabTitle) {
                return closeTabsTitle;
            }
            closeTabsTitle.push(title);
        }
    }
    return closeTabsTitle;
};
//********************************添加easyui tabs的右键菜单事件【end】**************************************************
/**
 * 添加封装的方法
   2015-04-24     罗梯     增加form参数
   2015-10-13     罗梯     优化添加成功后关闭对话框逻辑
 */
com.add = function (tableName, formName) {

    if (typeof (formName) == 'undefined' || formName == "") {
        formName = "#F_Add";
    } else {
        formName = "#" + formName;
    }

    if ($(formName).form('validate')) {
        com.progress();
        var action = arguments[2] || "add";
        $.ajax({
            url: "/Ajax/" + tableName + "/" + tableName + ".ashx?action=" + action,
            data: com.serializeObject($(formName), {
                "toUpperCase": false
            }),
            dataType: 'json',
            success: function (data) {
                $.messager.progress('close');

                if (typeof (parent.grid) != 'undefined') {
                    parent.grid.datagrid('reload', com.serializeObject(parent.$('#F_Search'), {
                        "toUpperCase": false
                    }));
                }

                //弹出信息函数
                var fnMessagerAlert = com.messagerAlert;
                var needClose = false;

                if (typeof (parent.modalDialog) != 'undefined') {
                    //因为需要关闭本窗口，所以调用父类函数
                    fnMessagerAlert = parent.com.messagerAlert;
                    needClose = true;
                }

                fnMessagerAlert(ResDisplay.Result, data.res, function () {
                    if (needClose) parent.modalDialog.dialog('close');
                    else location.href = location.href.replace(/(\S)#/g, '$1');
                }, true);
            }
        });


    }
};

/**
  2016-03-25     陈静     增加MVC Add
 **/
com.addMvc = function (tableName, formName) {

    if (typeof (formName) == 'undefined' || formName == "") {
        formName = "#F_Add";
    } else {
        formName = "#" + formName;
    }

    if ($(formName).form('validate')) {
        com.progress();
        var action = arguments[2] || "add";
        $.ajax({
            url: "/" + tableName + "/" + action,
            method: "post",
            data: com.serializeObject($(formName), {
                "toUpperCase": false
            }),
            dataType: 'json',
            success: function (data) {
                $.messager.progress('close');

                if (typeof (parent.grid) != 'undefined') {
                    parent.grid.datagrid('reload', com.serializeObject(parent.$('#F_Search'), {
                        "toUpperCase": false
                    }));
                }

                //弹出信息函数
                var fnMessagerAlert = com.messagerAlert;
                var needClose = false;

                if (typeof (parent.modalDialog) != 'undefined') {
                    //因为需要关闭本窗口，所以调用父类函数
                    fnMessagerAlert = parent.com.messagerAlert;
                    needClose = true;
                }

                fnMessagerAlert(ResDisplay.Result, data.res, function () {
                    if (needClose) parent.modalDialog.dialog('close');
                    else location.href = location.href.replace(/(\S)#/g, '$1');
                }, true);
            }
        });

    }
};

/**
 * 修改封装的方法
  2015-04-24     罗梯     增加form参数
  2015-05-19     陈静     增加修改成功后关闭对话框
  2015-10-13     罗梯     优化修改成功后关闭对话框逻辑
 */
com.update = function (tableName, formName) {


    if (typeof (formName) == 'undefined' || formName == "") {
        formName = "#F_Update";
    } else {
        formName = "#" + formName;
    }

    if ($(formName).form('validate')) {
        com.progress();
        var action = arguments[2] || "update";
        $.ajax({
            url: "/Ajax/" + tableName + "/" + tableName + ".ashx?action=" + action,
            data: com.serializeObject($(formName)),
            async: false,
            dataType: 'json',
            success: function (data) {
                $.messager.progress('close');

                if (typeof (parent.grid) != 'undefined') {
                    parent.grid.datagrid('reload', com.serializeObject(parent.$('#F_Search'), {
                        "toUpperCase": false
                    }));
                }

                //弹出信息函数
                var fnMessagerAlert = com.messagerAlert;
                var needClose = false;

                if (typeof (parent.modalDialog) != 'undefined') {
                    //因为需要关闭本窗口，所以调用父类函数
                    fnMessagerAlert = parent.com.messagerAlert;
                    needClose = true;
                }

                fnMessagerAlert(ResDisplay.Result, data.res, function () {
                    if (needClose) parent.modalDialog.dialog('close');
                    else location.href = location.href.replace(/(\S)#/g, '$1');
                }, true);
            }
        });

    }
};




/**
  2016-03-25     陈静     增加MVC Update
 **/
com.updateMvc = function (tableName, formName) {


    if (typeof (formName) == 'undefined' || formName == "") {
        formName = "#F_Update";
    } else {
        formName = "#" + formName;
    }

    if ($(formName).form('validate')) {
        com.progress();
        var action = arguments[2] || "update";
        $.ajax({
            url: "/" + tableName + "/" + action,
            data: com.serializeObject($(formName)),
            async: false,
            dataType: 'json',
            success: function (data) {
                $.messager.progress('close');

                if (typeof (parent.grid) != 'undefined') {
                    parent.grid.datagrid('reload', com.serializeObject(parent.$('#F_Search'), {
                        "toUpperCase": false
                    }));
                }

                //弹出信息函数
                var fnMessagerAlert = com.messagerAlert;
                var needClose = false;

                if (typeof (parent.modalDialog) != 'undefined') {
                    //因为需要关闭本窗口，所以调用父类函数
                    fnMessagerAlert = parent.com.messagerAlert;
                    needClose = true;
                }

                fnMessagerAlert(ResDisplay.Result, data.res, function () {
                    if (needClose) parent.modalDialog.dialog('close');
                    else location.href = location.href.replace(/(\S)#/g, '$1');
                }, true);
            }
        });

    }
};

/**
  2017-03-10     罗梯     整合增加data参数用于非form提交
 **/
com.mvcSubmitWithData = function (controller, action, data, callBack, identityVerify) {
    var mvcSubmitFun = function () {
        com.progress();
        $.ajax({
            url: "/" + controller + "/" + action,
            data: data,
            async: false,
            dataType: 'json',
            method: 'POST',
            success: function (data) {
                $.messager.progress('close');

                if (typeof (parent.grid) != 'undefined') {
                    parent.grid.datagrid('reload', com.serializeObject(parent.$('#F_Search'), {
                        "toUpperCase": false
                    }));
                } else if (typeof (grid) != 'undefined') {
                    grid.datagrid('reload', com.serializeObject($('#F_Search'), {
                        "toUpperCase": false
                    }));
                }

                //弹出信息函数
                var fnMessagerAlert = com.messagerAlert;
                var needClose = false;

                if (typeof (parent.modalDialog) != 'undefined') {
                    //因为需要关闭本窗口，所以调用父类函数
                    fnMessagerAlert = parent.com.messagerAlert;
                    needClose = true;
                }

                message = data.res || data.ErrorMsg;

                if (typeof(message) != "undefined") {
                    fnMessagerAlert(ResDisplay.Result, message, function () {
                        if (needClose) parent.modalDialog.dialog('close');
                        else location.href = location.href.replace(/(\S)#/g, '$1');
                    }, true);
                }

                if (typeof (callBack) != 'undefined') {
                    callBack(data);
                }
            }
        });
    }

    if (identityVerify) {
        operateWithValidate(mvcSubmitFun);
    }
    else {
        mvcSubmitFun()
    }
};


/**
  2016-04-11     罗梯     添加form到mvc
  2016-09-12     罗梯     增加callBack
 **/
com.mvcSubmit = function (controller, action, formName, callBack) {

    if (typeof (formName) == 'undefined' || formName == "") {
        formName = "#F_Update";
    } else {
        formName = "#" + formName;
    }
    if ($(formName).form('validate') && $(formName).valid()) {
        com.mvcSubmitWithData(controller, action, com.serializeObject($(formName)), callBack, $("#H_StaffCode").length > 0);
    }
};




/**
 * 修改导出Excel的方法
 2014-02-18     钟周强     修改为若筛选条件为空则不允许导出数据
 2014-02-18     张子源     增加action
 2014-08-30     钟周强     重新封装及重构导出方法，允许带title
 2014-12-03     钟周强     去除 parent.$("#L_UserID").html() 首尾的空字符串，以免报错
 2015-4-27      罗  梯     增加非正常返回值弹出提示
 */
/**************************begin**************************************/
com.batchExport = function (url, action) {
    com.batchExportWithTitle(url, undefined, action);
};

com.batchExportWithTitle = function (url, title, action) {
    if ($('#F_Search').form('validate')) {
        com.progress();
        var data = com.serializeObject($('#F_Search'));
        if (data.length > 0) {
            var path = "/File/Excel/DownLoad/" + $.trim(parent.$("#L_UserID").html().toString()) + "/" + $.trim((typeof (title) == 'undefined' ? document.title : title) + com.getDate()) + ".xls";
            data["action"] = (typeof (action) == 'undefined' ? 'batchExport' : action); //默认action为batchExport
            data["path"] = path;
            data["gridFields"] = com.getDatagridFields(grid);
            data["FieldList"] = com.getDatagridFieldAndTitle(grid);
            $.ajax({
                url: url,
                data: data,
                type: 'post',
                success: function (result) {
                    $.messager.progress('close');
                    //返回值校验，弹出后台错误信息
                    if (typeof (result.res) == "undefined") {
                        location.href = path;
                        return;
                    }

                    com.messagerAlert(ResDisplay.Result, result.res);

                }, error: function (ex) {
                    $.messager.progress('close');
                    com.messagerAlert(ResDisplay.Result, ex.responseText);
                }
            });
        } else {
            $.messager.progress('close');
        }
    }
};
/**************************end**************************************/

/**
 *
  2015-6-17      罗  梯     增加是否有相关列校验
 */
/**************************begin**************************************/
com.getDatagridFieldAndTitle = function (datagrid) {
    var fieldList = "";

    if (datagrid.datagrid('options').frozenColumns.length > 0) {
        var frozenColumns = datagrid.datagrid('options').frozenColumns[0];
        for (var i = 0; i < frozenColumns.length; i++) {
            if (!frozenColumns[i].hidden) {
                fieldList = (fieldList == "" ? "[" : fieldList + ",[") + frozenColumns[i].field + "] AS N'" + frozenColumns[i].title + "'";
            }
        }
    }

    if (datagrid.datagrid('options').columns.length > 0) {
        var columns = datagrid.datagrid('options').columns[0];
        for (var j = 0; j < columns.length; j++) {
            if (!columns[j].hidden) {
                fieldList = (fieldList == "" ? "[" : fieldList + ",[") + columns[j].field + "] AS N'" + columns[j].title + "'";
            }
        }
    }

    return fieldList;
};


com.getDatagridFields = function (datagrid, withHidden) {
    var fieldList = "";

    if (datagrid.datagrid('options').frozenColumns.length > 0) {
        var frozenColumns = datagrid.datagrid('options').frozenColumns[0];
        for (var i = 0; i < frozenColumns.length; i++) {
            if (withHidden || !frozenColumns[i].hidden) {
                fieldList = (fieldList == "" ? "[{ Field:\"" : fieldList + ",{ Field:\"") + frozenColumns[i].field + "\", Title:\"" + frozenColumns[i].title + "\"}";
            }
        }
    }

    if (datagrid.datagrid('options').columns.length > 0) {
        var columns = datagrid.datagrid('options').columns[0];
        for (var j = 0; j < columns.length; j++) {
            if (withHidden || !columns[j].hidden) {
                fieldList = (fieldList == "" ? "[{ Field:\"" : fieldList + ",{ Field:\"") + columns[j].field + "\", Title:\"" + columns[j].title + "\"}";
            }
        }
    }

    if (fieldList.length > 0) {
        fieldList = fieldList + "]";
    }

    return fieldList;
};



/**
 * 等同于原form的load方法，但是这个方法支持{data:{name:''}}形式的对象赋值
 */
$.extend($.fn.form.methods, {
    loadData: function (jq, data) {
        return jq.each(function () {
            load(this, data);
        });

        function load(target, data) {
            if (!$.data(target, 'form')) {
                $.data(target, 'form', {
                    options: $.extend({}, $.fn.form.defaults)
                });
            }
            var opts = $.data(target, 'form').options;

            if (typeof data == 'string') {
                var param = {};
                if (opts.onBeforeLoad.call(target, param) == false)
                    return;

                $.ajax({
                    url: data,
                    data: param,
                    dataType: 'json',
                    success: function (data) {
                        _load(data);
                    },
                    error: function () {
                        opts.onLoadError.apply(target, arguments);
                    }
                });
            } else {
                _load(data);
            }
            function _load(data) {
                var form = $(target);
                var formFields = form.find("input[name],select[name],textarea[name]");
                formFields.each(function () {
                    var name = this.name;
                    var value = jQuery.proxy(function () {
                        try {
                            return eval('this.' + name);
                        } catch (e) {
                            return "";
                        }
                    }, data)();
                    var rr = _checkField(name, value);
                    if (!rr.length) {
                        var f = form.find("input[numberboxName=\"" + name + "\"]");
                        if (f.length) {
                            f.numberbox("setValue", value);
                        } else {
                            $("input[name=\"" + name + "\"]", form).val(value);
                            $("textarea[name=\"" + name + "\"]", form).val(value);
                            $("select[name=\"" + name + "\"]", form).val(value);
                        }
                    }
                    _loadCombo(name, value);
                });
                opts.onLoadSuccess.call(target, data);
                $(target).form("validate");
            }

            function _checkField(name, val) {
                var rr = $(target).find('input[name="' + name + '"][type=radio], input[name="' + name + '"][type=checkbox]');
                rr._propAttr('checked', false);
                rr.each(function () {
                    var f = $(this);
                    if (f.val() == String(val) || $.inArray(f.val(), val) >= 0) {
                        f._propAttr('checked', true);
                    }
                });
                return rr;
            }

            function _loadCombo(name, val) {
                var form = $(target);
                var cc = ['combobox', 'combotree', 'combogrid', 'datetimebox', 'datebox', 'combo'];
                var c = form.find('[comboName="' + name + '"]');
                if (c.length) {
                    for (var i = 0; i < cc.length; i++) {
                        var type = cc[i];
                        if (c.hasClass(type + '-f')) {
                            if (c[type]('options').multiple) {
                                c[type]('setValues', val);
                            } else {
                                c[type]('setValue', val);
                            }
                            return;
                        }
                    }
                }
            }
        }
    }
});

/**
 * 更换主题
 * 
 * @author 卜永济
 * @requires jQuery,EasyUI
 * @param themeName
 */
com.changeTheme = function (themeName) {
    var $easyuiTheme = $('#easyuiTheme');
    var url = $easyuiTheme.attr('href');
    var href = url.substring(0, url.indexOf('themes')) + 'themes/' + themeName + '/easyui.css';
    $easyuiTheme.attr('href', href);

    var $iframe = $('iframe');
    if ($iframe.length > 0) {
        for (var i = 0; i < $iframe.length; i++) {
            var ifr = $iframe[i];
            try {
                $(ifr).contents().find('#easyuiTheme').attr('href', href);
            } catch (e) {
                try {
                    ifr.contentWindow.document.getElementById('easyuiTheme').href = href;
                } catch (e) {
                }
            }
        }
    }

    com.cookie('easyuiTheme', themeName, {
        expires: 7
    });
};

/**
 * 滚动条
 * 
 * @author 卜永济
 * @requires jQuery,EasyUI
 */
com.progressBar = function (options) {
    if (typeof options == 'string') {
        if (options == 'close') {
            $('#syProgressBarDiv').dialog('destroy');
        }
    } else {
        if ($('#syProgressBarDiv').length < 1) {
            var opts = $.extend({
                title: '&nbsp;',
                closable: false,
                width: 300,
                height: 60,
                modal: true,
                content: '<div id="syProgressBar" class="easyui-progressbar" data-options="value:0"></div>'
            }, options);
            $('<div id="syProgressBarDiv"/>').dialog(opts);
            $.parser.parse('#syProgressBarDiv');
        } else {
            $('#syProgressBarDiv').dialog('open');
        }
        if (options.value) {
            $('#syProgressBar').progressbar('setValue', options.value);
        }
    }
};
/**
	 * 扩展树表格级联勾选方法：
	 * @param {Object} container
	 * @param {Object} options
	 * @return {TypeName} 
	 */
$.extend($.fn.treegrid.methods, {
    /**
     * 级联选择
     * @param {Object} target
     * @param {Object} param 
     *		param包括两个参数:
     *			id:勾选的节点ID
     *			deepCascade:是否深度级联
     * @return {TypeName} 
     */
    cascadeCheck: function (target, param) {
        var opts = $.data(target[0], "treegrid").options;
        if (opts.singleSelect)
            return;
        var idField = opts.idField;//这里的idField其实就是API里方法的id参数
        var status = false;//用来标记当前节点的状态，true:勾选，false:未勾选
        var selectNodes = $(target).treegrid('getSelections');//获取当前选中项
        for (var i = 0; i < selectNodes.length; i++) {
            if (selectNodes[i][idField] == param.id)
                status = true;
        }
        //级联选择父节点
        selectParent(target[0], param.id, idField, status);
        selectChildren(target[0], param.id, idField, param.deepCascade, status);
        /**
         * 级联选择父节点
         * @param {Object} target
         * @param {Object} id 节点ID
         * @param {Object} status 节点状态，true:勾选，false:未勾选
         * @return {TypeName} 
         */
        function selectParent(target, id, idField, status) {
            var parent = $(target).treegrid('getParent', id);
            if (parent) {
                var parentId = parent[idField];
                if (status)
                    $(target).treegrid('select', parentId);
                else
                    $(target).treegrid('unselect', parentId);
                selectParent(target, parentId, idField, status);
            }
        }
        /**
         * 级联选择子节点
         * @param {Object} target
         * @param {Object} id 节点ID
         * @param {Object} deepCascade 是否深度级联
         * @param {Object} status 节点状态，true:勾选，false:未勾选
         * @return {TypeName} 
         */
        function selectChildren(target, id, idField, deepCascade, status) {
            //深度级联时先展开节点
            if (!status && deepCascade)
                $(target).treegrid('expand', id);
            //根据ID获取下层孩子节点
            var children = $(target).treegrid('getChildren', id);
            for (var i = 0; i < children.length; i++) {
                var childId = children[i][idField];
                if (status)
                    $(target).treegrid('select', childId);
                else
                    $(target).treegrid('unselect', childId);
                selectChildren(target, childId, idField, deepCascade, status);//递归选择子节点
            }
        }
    }
});
/**
 * 绑定默认值
 * 
 * @author 张子源
 * @requires jQuery,EasyUI
 */
com.comboboxDefaultValue = function (id) {
    $(id).combobox({
        onLoadSuccess: function () {
            var data = $(id).combobox('getData');
            if (data.length > 0) {
                $(id).combobox('select', data[0].id);
            }
        }
    });
};
/**
 * 删除、恢复validate验证
 * 
 * @author 李柏威
 * @requires jQuery,EasyUI
 */
$.extend($.fn.validatebox.methods, {
    remove: function (jq, newposition) {
        return jq.each(function () {
            $(this).removeClass("validatebox-text validatebox-invalid").unbind('focus.validatebox').unbind('blur.validatebox');
        });
    },
    recover: function (jq, newposition) {
        return jq.each(function () {
            var opt = $(this).data().validatebox.options;
            $(this).addClass("validatebox-text").validatebox(opt);
        });
    }
});

