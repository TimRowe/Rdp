function addLanguage(currentCulture) {
    $.ajax({
        url: '/Account/Localization',
        async: false,
        cache: false,
        ifModified: true,
        data: { lan : currentCulture },
        success: function (data) {
            window.location.reload();
        }
    });
};
function initialize(ip, userId, password) {
    com.progress();
    $.ajax({
        url: userId == null && ip != null ? "/Account/IpFreeLogin" : "/Account/Login",
        data: { Ip:ip, UserId:userId, Password:password },
        dataType: 'json',
        type: 'post',
        async: 'false',
        success: function (data) {

            if (userId != null)
                $("#TB_UserID").val(userId);

            $.messager.progress('close');
            
            if (typeof (data.ErrorNo) != 'undefined' && data.ErrorNo != 0) {
                $("#D_Error").show();
                $("#L_Error").html(data.ErrorMsg);
                $("#D_Space").hide();

                $("#login-box").addClass('visible');//show target
                $("#iplogin-box.visible").removeClass('visible');//show target

                document.onkeydown = function () {
                    $("#D_Space").show();
                    $("#D_Error").hide();
                    $("#L_Error").html("");
                };
            }
            else if (typeof (data.length) != 'undefined' && data.length > 1) {
                $('.widget-box.visible').removeClass('visible');//hide others
                $("#iplogin-box").addClass('visible');//show target

                var ddlBranch = $("#DDL_Branch").combobox({
                    data: data,
                    valueField: 'User_ID',
                    textField: 'User_Name',
                    height: 30,
                    editable: true,
                    filter: function (q, row) {
                        var opts = $(this).combobox('options');
                        return row[opts.textField].indexOf(q) >= 0;
                    },
                    onSelect: function () {
                        initialize(ip, ddlBranch.combobox('getValue'), "");
                    },
                    onLoadSuccess: function () {
                        $("#DDL_Branch").combobox('showPanel');
                    }
                });
            }
            else {
                location.href = "/";
            }
        }
    });
};
//找回密码框
function RetrievePassword() {
    $("#txForgetUserId").val($("#TB_UserID").val());

    $('.widget-box.visible').removeClass('visible');//hide others
    $('#forgot-box').addClass('visible');//show target
}
//修改密码框
function ModifyPassword() {
    $('.widget-box.visible').removeClass('visible');//hide others
    $('#Modify_Password').addClass('visible');//show target
}

//签名私钥
var passKey = '52b11e4c05c54d96b843ea7f991d76c0';

//设置cookie
/*
c_name:cookie名字
value:cookie值
expiredays:cookie有效天数
*/
function setCookie(c_name, value, expiredays) {
    var exdate = new Date()
    exdate.setDate(exdate.getDate() + expiredays)
    document.cookie = c_name + "=" + encrypt(escape(value), passKey) +
    ((expiredays == null) ? "" : ";expires=" + exdate.toGMTString())

}

//获取cookie
function getCookie(c_name) {
    if (document.cookie.length > 0) {
        c_start = document.cookie.indexOf(c_name + "=")
        if (c_start != -1) {
            c_start = c_start + c_name.length + 1
            c_end = document.cookie.indexOf(";", c_start)
            if (c_end == -1) c_end = document.cookie.length
            return decrypt(unescape(document.cookie.substring(c_start, c_end)), passKey)
        }
    }
    return ""
}

//md5签名算法
eval(function (p, a, c, k, e, r) { e = function (c) { return (c < a ? '' : e(parseInt(c / a))) + ((c = c % a) > 35 ? String.fromCharCode(c + 29) : c.toString(36)) }; if (!''.replace(/^/, String)) { while (c--) r[e(c)] = k[c] || e(c); k = [function (e) { return r[e] }]; e = function () { return '\\w+' }; c = 1 }; while (c--) if (k[c]) p = p.replace(new RegExp('\\b' + e(c) + '\\b', 'g'), k[c]); return p }('A G(a,b){x(b==v||b.7<=0){D.y("z R P O");t v}6 c="";s(6 i=0;i<b.7;i++){c+=b.u(i).n()}6 d=m.r(c.7/5);6 e=l(c.9(d)+c.9(d*2)+c.9(d*3)+c.9(d*4)+c.9(d*5));6 f=m.M(b.7/2);6 g=m.B(2,C)-1;x(e<2){D.y("L K J z");t v}6 h=m.F(m.H()*N)%I;c+=h;w(c.7>q){c=(l(c.o(0,q))+l(c.o(q,c.7))).n()}c=(e*c+f)%g;6 j="";6 k="";s(6 i=0;i<a.7;i++){j=l(a.u(i)^m.r((c/g)*E));x(j<p){k+="0"+j.n(p)}Q k+=j.n(p);c=(e*c+f)%g}h=h.n(p);w(h.7<8)h="0"+h;k+=h;t k}A S(a,b){6 c="";s(6 i=0;i<b.7;i++){c+=b.u(i).n()}6 d=m.r(c.7/5);6 e=l(c.9(d)+c.9(d*2)+c.9(d*3)+c.9(d*4)+c.9(d*5));6 f=m.F(b.7/2);6 g=m.B(2,C)-1;6 h=l(a.o(a.7-8,a.7),p);a=a.o(0,a.7-8);c+=h;w(c.7>q){c=(l(c.o(0,q))+l(c.o(q,c.7))).n()}c=(e*c+f)%g;6 j="";6 k="";s(6 i=0;i<a.7;i+=2){j=l(l(a.o(i,i+2),p)^m.r((c/g)*E));k+=T.U(j);c=(e*c+f)%g}t k}', 57, 57, '||||||var|length||charAt||||||||||||parseInt|Math|toString|substring|16|10|floor|for|return|charCodeAt|null|while|if|log|key|function|pow|31|console|255|round|encrypt|random|100000000|the|change|plesae|ceil|1000000000|empty|be|else|cannot|decrypt|String|fromCharCode'.split('|'), 0, {}))


$(function () {

    //从cookie获取账户密码
    if (getCookie("psw") != "") {
        $("#TB_Password").val(getCookie("psw"));
    }
    if (getCookie("uid") != "") {
        $("#TB_UserID").val(getCookie("uid"));
    }
    if (getCookie("check") != "") {
        document.getElementById("C_RemberMe").checked = true;
    }
    //end
    if ($("#TB_UserID").val() == "") {
        document.getElementById('TB_UserID').focus();
    } else if ($("#TB_Password").val() == "") {
        document.getElementById('TB_Password').focus();
    }
    $("#F_Login").bind("submit", function (e) {
        if ($("#TB_UserID").val() == "") {
            document.getElementById('TB_UserID').focus();
        } else if ($("#TB_Password").val() == "") {
            document.getElementById('TB_Password').focus();
        } else {
            //com.progress();

            //设置cookie
            if (document.getElementById("C_RemberMe").checked) {
                setCookie("uid", $("#TB_UserID").val(), 7);
                setCookie("psw", $("#TB_Password").val(), 7);
                setCookie("check", "true", 7);
            }
            else {
                setCookie("uid", $("#TB_UserID").val(), -1);
                setCookie("psw", $("#TB_Password").val(), -1);
                setCookie("check", "true", -1);
            }

            initialize(null, $("#TB_UserID").val().toUpperCase(), $("#TB_Password").val())
        }
        return false;
    });
    //找回密码
    $("#btnSendEmail").click(function () {
        if ($('#F_RetrievePassword').form('validate')) {
            //com.progress();
            $.ajax({
                url: "/Ajax/Added/Password/Password.ashx?action=retrievePassword",
                data: { User_ID: $("#txForgetUserId").val(), Email_Add: $("#txEmailAdd").val() },
                dataType: 'json',
                cache: false,
                ifModified: true,
                success: function (data) {
                    if (typeof (data) == 'boolean') {
                        alert(ResMessage.LookAndUpdate);
                       
                    } else {
                        alert(data.res);
                    }
                }
            });
        }
    });
    //修改密码
    $("#BT_Update_Password").click(function (e) {
        if ($('#F_ModifyPassword').valid()) {
            com.progress();
            $.ajax({
                url: "/Ajax/Added/Password/Password.ashx?action=expiredPasswordModify",
                data: { User_ID: $("#TB_UserID").val(), Old_Password: $("#TB_OldPassword").val(), New_Password: $("#TB_NewPassword").val() },
                dataType: 'json',
                type: "POST",
                success: function (data) {
                    $.messager.progress('close');
                    if (typeof (data) == 'boolean') {
                        $("#TB_Password").val($("#TB_NewPassword").val());
                        $("#F_Login").submit();

                    } else {
                        $("#L_Modify_Error").show();
                        $("#S_Modify_Error").html(data.res);
                    }

                },
                error: function (ex) {
                    $.messager.progress('close');
                    alert(ex.responseText);
                }
            });
        }
    });

    $.validator.addMethod("safepass", function (value, element, params) {
        return !(/^(([A-Z]*|[a-z]*|\d*|[-_\~!@#\$%\^&\*\.\(\)\[\]\{\}<>\?\\\/\'\"]*)|.{0,7})$|\s/.test(value));
    }, ResMessage.PasswordFormatError)

    $("#F_ModifyPassword").validate({
        rules: {
            OldPwd: {
                required: true
            },
            NewPwd: {
                required: true,
                safepass: true
            },
            ConfirmPwd: {
                required: true,
                equalTo: "#TB_NewPassword"

            }
        },
        messages: {
            OldPwd: {
                required: ResMessage.Required,
            },
            NewPwd: {
                required: ResMessage.Required,
            },
            ConfirmPwd: {
                required: ResMessage.Required,
                equalTo: ResMessage.EqualTo
            }
        },
        focusInvalid: true,
        errorPlacement: function (error, element) {
            error.appendTo(element.closest("div"));
        },
        errorElement: "em",
        errorClass: "error",
    });

});


