$.extend(
    $.fn.validatebox.defaults.rules, {
        minLength: {
            validator: function (value, param) {
                return value.length >= param[0];
            },
            message: '请输入至少{0}位字符'
        },
        maxLength: {
            validator: function (value, param) {
                return value.length <= param[0];
            },
            message: '最多只能输入{0}位字符'
        },
        money: {
            validator: function (value, param) {
                return /^(([1-9]\d{0,9})|0)(\.\d{1,2})?$/.test(value);
            }, message: '金额格式错误'
        },
        weight: {
            validator: function (value, param) {
                return /^(((([1-9]\d{0,9})|0)(\.\d{1,3})?){1,},){0,}(((([1-9]\d{0,9})|0)(\.\d{1,3})?){1,})$/.test(value);
            }, message: '格式错误,如多个请用英文","隔开'
        },
        userID: {
            validator: function (value, param) {
                return /^[A-Za-z0-9]+$/.test(value);
            },
            message: '只能输入字母或数字'
        },
        batchNo: {
            validator: function (value, param) {
                return /^([0-9]{1,},){0,}([0-9]{1,})$/.test(value);
            }, message: '格式错误,如：1,2,3'
        },
        cost: {
            validator: function (value, param) {
                return /^([0-9]+\.?[0-9]*,){0,}([0-9]+\.?[0-9]*)$/.test(value);
            }, message: '格式错误,如：1,2,3'
        },
        number: {
            validator: function (value, param) {
                return /^\d+$/.test(value);
            },
            message: '请输入数字'
        },
        phoneHK: {
            validator: function (value, param) {
                var reg = /^((([2-3]{1})|([5-9]{1}))\d{7})$/;
                return reg.test(value);
            },
            message: '格式错误'
        },
        phoneCN: {
            validator: function (value, param) {
                var reg = /^(((13[0-9]{1})|(15[0-9]{1})|(14[5-7]{1})|(17[6-8]{1})|170|(18[0-9]{1}))\d{8})$/;
                return reg.test(value);
            },
            message: '格式错误'
        },
        telCN: {
            validator: function (value, param) {
                var reg = /^(([1-9]{1}[0-9]{1,2})\d{5})$/;
                return reg.test(value);
            },
            message: '格式错误'
        },
        phoneMO: {
            validator: function (value, param) {
                var reg = /^(((2)|(6))\d{7})$/;
                return reg.test(value);
            },
            message: '格式错误'
        },
        phoneTW: {
            validator: function (value, param) {
                var reg = /^((09)\d{8})$/;
                return reg.test(value);
            },
            message: '格式错误'
        },
        telTW: {
            validator: function (value, param) {
                var reg = /^((([0-9]{1})|([0-9]{3}))\d{5})$/;
                return reg.test(value);
            },
            message: '格式错误'
        },
        barcode: {
            validator: function (value, parm) {
                return /^(([0-9]{2}[A-Z]{1,2}[0-9]{8}){1,},){0,}(([0-9]{2}[A-Z]{1,2}[0-9]{8}){1,})$/.test(value);
            }, message: '格式错误,如多个请用英文","隔开'
        },
        cost: {
            validator: function (value, param) {
                return /^([0-9]+\.?[0-9]*,){0,}([0-9]+\.?[0-9]*)$/.test(value);
            }, message: '格式错误,如：1,2,3'
        },
        mobile: {
            validator: function (value, param) {
                var mobile = /^(((13[0-9]{1})|(15[0-9]{1})|(14[7-7]{1})|(18[0-9]{1}))+\d{8})$/;
                return (value.length == 11 && mobile.test(value));
            }, message: '手机格式错误'
        },
        tel: {
            validator: function (value, param) {
                var tel = /^(((\(0\d{2,3}\)){1}|(0\d{2,3}[-]?){1})?([1-9]{1}[0-9]{2,7}(\-\d{3,4})?))$/;
                return (tel.test(value));
            }, message: '电话格式错误'
        },
        mobileAndTel: {
            validator: function (value, param) {
                var tel = /^(((\(0\d{2,3}\)){1}|(0\d{2,3}[-]?){1})?([1-9]{1}[0-9]{2,7}(\-\d{3,4})?))$/;
                var mobile = /^(((13[0-9]{1})|(15[0-9]{1})|(14[7-7]{1})|(18[0-9]{1}))+\d{8})$/;
                return ((value.length == 11 && mobile.test(value)) || tel.test(value));
            }, message: '联系方式格式错误'
        },
        noSpecialchar: {
            validator: function (value, param) {
                var regex = /[*@#$%^&*()-=_+,./?';":!~`1234567890]/;
                return !regex.test(value);
            }, message: '不能输入特殊字符或数字'
        },
        postCode: {
            validator: function (value, param) {
                var regex = /([0-9]{6})/;
                return (regex.test(value) && value.length == 6);
            }, message: '邮政编码格式错误'
        },
        CHS: {
            validator: function (value, param) {
                return /^[\u0391-\uFFE5]+$/.test(value);
            },
            message: '请输入汉字'
        },
        letter: {
            validator: function (value, param) {
                return /^[A-Za-z]+$/.test(value);
            },
            message: '请输入字母'
        },
        //允许空格
        letterSpacing: {
            validator: function (value, param) {
                return /^[A-Za-z\s]+$/.test(value);
            },
            message: '请输入字母'
        },
        QQ: {
            validator: function (value, param) {
                return /^[1-9]\d{4,10}$/.test(value);
            },
            message: 'QQ号码不正确'
        },
        BranchCode: {
            validator: function (value, param) {
                return /^\d{0,6}$/.test(value);
            },
            message: '请输入六位有效分店号'
        },
        safepass: {
            validator: function (value, param) {
                return safePassword(value);
            },
            message: '密码由字母和数字组成，至少8位'
        },
        noSpecial: {
            validator: function (value, param) {
                var regex = /['"“”‘/’-]/;
                return !regex.test(value);
            }, message: '不含\'、”、-、/'
        },
        equalTo: {
            validator: function (value, param) {
                return value == $(param[0]).val();
            },
            message: '两次输入的字符不一至'
        },
        transferBranch: {
            validator: function (value, param) {
                return value != $(param[0]).val();
            },
            message: '新分店行号不能与旧分店行号相同'
        },
        deployBranch: {
            validator: function (value, param) {
                return value != $(param[0]).val();
            },
            message: '發出請求分行不能与調出分行相同'
        },
        idcard: {
            validator: function (value, param) {
                return idCard(value);
            },
            message: '请输入正确的身份证号码'
        },
        couponNum: {
            validator: function (value, param) {
                return /^[A-z]{2}[0-9]{9}$/.test(value);
            }, message: '格式错误,如2位字母加9位数字,Ex:CN000000001'
        },
        noCouponNumAndRange: {
            validator: function (value, param) {
                return $.trim($(param[0]).val()) == '';
            }, message: '礼券号码及范围只能填其一'
        },
        startWith: {
            validator: function (value, param) {
                return /^\d+$/.test(value.replace(param[0], "")) || value == param[0];
            },
            message: '礼券类型不对'
        },
        firstCouponNum: {
            validator: function (value, param) {
                var lastCouponNum = $.trim($(param[0]).val());
                if (lastCouponNum != '') {
                    var firstCouponCode, firstCouponNum, lastCouponCode, lastCouponNo;
                    lastCouponCode = lastCouponNum.substr(0, 2);
                    lastCouponNo = lastCouponNum.substr(2, 9);
                    firstCouponCode = value.substr(0, 2);
                    firstCouponNum = value.substr(2, 9);
                    return firstCouponCode == lastCouponCode && firstCouponNo <= lastCouponNo;
                } else if (lastCouponNum == '' && value != '') {
                    return false;
                } else {
                    return true;
                }
            }, message: '请按同一类型礼券前小后大顺序'
        },
        firstCouponNo: {
            validator: function (value, param) {
                var lastCouponNo = $.trim($(param[0]).val());
                if (lastCouponNo != '') {
                    return lastCouponNo >= value;
                } else {
                    return true;
                }
            }, message: '请按礼券编号前小后大顺序'
        },
        lastCouponNum: {
            validator: function (value, param) {
                var firstCouponNum = $.trim($(param[0]).val());
                if (firstCouponNum != '') {
                    var firstCouponCode, firstCouponNo, lastCouponCode, lastCouponNo;
                    firstCouponCode = firstCouponNum.substr(0, 2);
                    firstCouponNo = firstCouponNum.substr(2, 9);
                    lastCouponCode = value.substr(0, 2);
                    lastCouponNo = value.substr(2, 9);
                    return firstCouponCode == lastCouponCode && firstCouponNo <= lastCouponNo;
                } else if (firstCouponNum == '' && value != '') {
                    return false;
                } else {
                    return true;
                }
            }, message: '请按同一类型礼券前小后大顺序'
        },
        lastCouponNo: {
            validator: function (value, param) {
                var firstCouponNo = $.trim($(param[0]).val());
                if (firstCouponNo != '') {
                    return firstCouponNo <= value;
                } else {
                    return true;
                }
            }, message: '请按礼券编号前小后大顺序'
        },
        greater: {
            validator: function (value, param) {
                var lastNum = $.trim($(param[0]).val());
                if (lastNum != '') {
                    return lastNum <= value;
                } else {
                    return true;
                }
            }, message: '请遵循前小后大顺序'
        },
        noCouponCode: {
            validator: function (value, param) {
                return $.trim($(param[0]).combobox('getValue')) != '';
            }, message: '請選擇相應的礼券小類'
        }

    }
);
/* 密码由字母和数字组成，至少6位 */
var safePassword = function (value) {
    return !(/^(([A-Z]*|[a-z]*|\d*|[-_\~!@#\$%\^&\*\.\(\)\[\]\{\}<>\?\\\/\'\"]*)|.{0,7})$|\s/.test(value));
};
var idCard = function (value) {
    if (value.length == 18 && 18 != value.length) return false;
    var number = value.toLowerCase();
    var d, sum = 0, v = '10x98765432', w = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2], a = '11,12,13,14,15,21,22,23,31,32,33,34,35,36,37,41,42,43,44,45,46,50,51,52,53,54,61,62,63,64,65,71,81,82,91';
    var re = number.match(/^(\d{2})\d{4}(((\d{2})(\d{2})(\d{2})(\d{3}))|((\d{4})(\d{2})(\d{2})(\d{3}[x\d])))$/);
    if (re == null || a.indexOf(re[1]) < 0) return false;
    if (re[2].length == 9) {
        number = number.substr(0, 6) + '19' + number.substr(6);
        d = ['19' + re[4], re[5], re[6]].join('-');
    } else d = [re[9], re[10], re[11]].join('-');
    if (!isDateTime.call(d, 'yyyy-MM-dd')) return false;
    for (var i = 0; i < 17; i++) sum += number.charAt(i) * w[i];
    return (re[2].length == 9 || number.charAt(17) == v.charAt(sum % 11));
};