function Logout() {
    location.href = "/Account/Logout";
};

//修改语言
function addLanguage(currentCulture) {
    $.ajax({
        url: '/Account/Localization',
        async: false,
        cache: false,
        ifModified: true,
        data: { lan: currentCulture },
        success: function (data) {
            location.href = "/";
        }
    });
};
var arr = window.location.href.split("/");
var url = "http://" + arr[2];
//添加桌面快捷方式
function toDesktop(sUrl, sName) {
    try {
        var wshShell = new ActiveXObject("WScript.Shell");
        var oUrlLink = wshShell.CreateShortcut(wshShell.SpecialFolders("Desktop") + "\\" + sName + ".url");
        oUrlLink.TargetPath = sUrl;
        oUrlLink.Save();
        com.messagerAlert(ResDisplay.Result, ResSuggest.OperateSuccess);
    }
    catch (e) {
        com.messagerAlert(ResDisplay.Result, ResSuggest.OperateFail);
    }
};
//收藏页面
function bookMark() {
    var title = document.title;
    if (window.sidebar) window.sidebar.addPanel(title, url, "");
    else if (window.opera && window.print) {
        var mbm = document.createElement('a');
        mbm.setAttribute('rel', 'sidebar');
        mbm.setAttribute('href', url);
        mbm.setAttribute('title', title);
        mbm.click();
    }
    else if (document.all) window.external.AddFavorite(url, title);
}


//更改角色
function changeRole(userRoleID) {
    $.ajax({
        url: '/Account/ChangeRole',
        data: { action: "changeRole", userRoleId: userRoleID },
        type: 'post',
        success: function (data) {
            if (data.ErrorNo == 0) {
                location.href = "/";
            }
            else {
                alert(data.ErrorMsg);
            }
            
        }
    });
};
var dialogLogin;
//登录操作
function login() {
    if ($("#TB_Password").val() == "") {
        document.getElementById('TB_Password').focus();
    } else {
        $.ajax({
            url: '/Ajax/UserMaster/UserMaster.ashx',
            data: { action: 'login', User_ID: $("#L_UserID").html(), Password: $("#TB_Password").val() },
            dataType: 'json',
            type: 'post',
            success: function (data) {
                if (typeof (data.res) != 'undefined') {
                    $("#L_Error").html(data.res);
                }
                else {
                    dialogLogin.dialog('close');
                }
            }
        });
    }
};



