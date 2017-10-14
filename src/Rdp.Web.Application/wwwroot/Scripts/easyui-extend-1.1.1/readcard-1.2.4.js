function writeCard(sector, block, no,writeSuccess) {
    //转化为十六进制数据
    var st;//主要用于返回值
    var sb;       // 地址塊號    
    var i, j, data;
    data = '';
    i = no.toString().length;
    for (j = 0; j < i; j++) {
        data = data + no.toString().substring(j, j + 1).charCodeAt(0).toString(16);
    }
    data = data + '00202020202020202020202020202020';
    data = data.substring(0, 32);
    var lSnr; //本用于取序列号，但在javascript只是当成dc_card函数的一个临时变量
    sb = sector * 4 + block;
    st = rd.dc_init(100, 115200);
    if (st <= 0) //返回值小于等于0表示失败
    {
        return 'USB端口未发现读卡设备!';
    }
    st = rd.dc_card(0, lSnr);
    if (st != 0) //返回值小于0表示失败
    {
        rd.dc_exit();
        return '读卡设备有效范围内没有可读取的卡!';
    }
    rd.put_bstrSBuffer_asc = "FFFFFFFFFFFF"; // 加載碼，默認為"FFFFFFFFFFFF" 
    st = rd.dc_load_key(0, sector);
    if (st < 0) //返回值小于0表示失败
    {
        rd.dc_exit();
        return '系统不支持此类型的卡!';
    }
    st = rd.dc_authentication(0, sector);
    if (st < 0) //返回值小于0表示失败
    {
        rd.dc_exit();
        return '系统不支持此类型的卡!';
    }
    rd.put_bstrSBuffer_asc = data;
    st = rd.dc_write(sb);
    if (st < 0) //返回值小于0表示失败
    {
        rd.dc_exit();
        return "读写数据失败,请检查卡是否有效!";
    }
    st = rd.dc_beep(50);
    if (st < 0) //返回值小于0表示失败
    {
        rd.dc_exit();
        return "beep失败!";
    }
    st = rd.dc_exit();
    if (st < 0) //返回值小于0表示失败
    {
        return "退出失败1";
    }
    return writeSuccess;
};
function readCard(sector, block) {
    var isNoBeep = arguments[2] || false;//JS設置默認值
    ///////////////////////////////////////////////////////////////////////////////
    //以下为非接触式读写器函数调用例子 for javascript
    //注意个别函数（例如dc_disp_str）只有当设备满足要求（例如有数码显示）时才有效
    //更详细的帮助可参照32位动态库对应的函数使用说明
    ///////////////////////////////////////////////////////////////////////////////
    var st; //主要用于返回值
    var lSnr; //本用于取序列号，但在javascript只是当成dc_card函数的一个临时变量
    var sb;       // 地址塊號  
    var returnValue;
    sb = sector * 4 + block;

    //初始化USB接口的设备
    //======================简单说明================================
    //icdev=dc_init(0,9600)            //初始化串口1，波特率9600
    //icdev=dc_init(100,9600)        //初始化USB 接口的读写器，此时波特率不起作用，但不能为空。

    //注意： 串口1的port值为0。以此类推，串口2的port值为1。
    //==============================================================

    st = rd.dc_init(100, 115200);
    if (st <= 0) //返回值小于等于0表示失败
    {
        alert('USB端口未发现读卡设备!');
        clearInterval(validation);
        validationDialog.dialog('close');
        return 0;
    }

    // ======================简单说明===============================
    //寻卡，能返回在工作区域内某张卡的序列号
    //第一个参数一般设置为0，表示IDLE模式，一次只对一张卡操作
    //第二个参数在javascript只是当成dc_card函数的一个临时变量，仅在vbscript中调用后能正确返回序列号
    //寻卡函数，用于取放在读写器工作区上的卡片的序列号
    //==============================================================

    st = rd.dc_card(0, lSnr);
    if (st != 0) //返回值小于0表示失败
    {
        rd.dc_exit();
        //alert('读卡设备有效范围内没有可读取的卡!');
        return 0;
    }

    if ((rd.get_bstrRBuffer).length < 1) {
        rd.dc_exit();
        //alert('读卡设备有效范围内没有可读取的卡!');
        return 0;
    }
    //alert("dc_card ok!");
    //alert(rd.get_bstrRBuffer); //序列号为rd.get_bstrRBuffer，一般有不可显示字符出现
    //alert(rd.get_bstrRBuffer_asc); //序列号十六进制ascll码字符串表示为rd.get_bstrRBuffer_asc



    //=====================简单说明==============================
    //将密码装入读写模块RAM中
    //第一个参数为装入密码模式
    //第二个参数为扇区号
    //==============================================================

    rd.put_bstrSBuffer_asc = "FFFFFFFFFFFF"; //在调用dc_load_key必须前先设置属性rd.put_bstrSBuffer或rd.put_bstrSBuffer_asc
    st = rd.dc_load_key(0, sector);
    if (st < 0) //返回值小于0表示失败
    {
        rd.dc_exit();
        alert('系统不支持此类型的卡!');
        return 0;
    }

    //========================简单说明============================
    //校验密码的函数，核对指定扇区的密码，须先进行加载密码操作
    //第一个参数为密码验证模式
    //第二个参数为扇区号
    //==============================================================

    st = rd.dc_authentication(0, sector);
    if (st < 0) //返回值小于0表示失败
    {
        rd.dc_exit();
        alert('系统不支持此类型的卡!');
        return 0;
    }

    //==========================简单说明===========================
    //读数据函数，用于对放在读写器工作区上的卡片进行数据写入的工作，按块读，一次读一块
    //第一个参数为块地址
    //取出的数据放在属性放在rd.put_bstrSBuffer（正常表示）和rd.put_bstrSBuffer_asc（十六进制ascll码字符串表示）中
    //==============================================================

    st = rd.dc_read(sb);
    if (st < 0) //返回值小于0表示失败
    {
        rd.dc_exit();
        alert('读写数据失败,请检查卡是否有效!');
        return 0;
    }
    returnValue = rd.get_bstrRBuffer;
    //=========================简单说明=============================
    //设置设备发出蜂鸣
    //第一个参数为蜂鸣时间，单位是10毫秒
    //详细请查看 "说明文档.chm"-->“函数说明”-->"设备函数"-->"dc_beep"
    //==============================================================

    if (!isNoBeep) {
        st = rd.dc_beep(50);
        if (st < 0) //返回值小于0表示失败
        {
            rd.dc_exit();
            alert("beep失败!");

            return 0;
        }
    }
    //========================简单说明=============================
    //关闭端口
    //关闭由 dc_init 打开的端口    
    //==============================================================	

    st = rd.dc_exit();
    if (st < 0) //返回值小于0表示失败
    {
        alert("退出失败!");
        return 0;
    }
    return returnValue;
};

function readSerialNo() {
    var isNoBeep = arguments[0] || false;//JS設置默認值
    ///////////////////////////////////////////////////////////////////////////////
    //以下为非接触式读写器函数调用例子 for javascript
    //注意个别函数（例如dc_disp_str）只有当设备满足要求（例如有数码显示）时才有效
    //更详细的帮助可参照32位动态库对应的函数使用说明
    ///////////////////////////////////////////////////////////////////////////////
    var st; //主要用于返回值
    var lSnr; //本用于取序列号，但在javascript只是当成dc_card函数的一个临时变量
    var returnValue;


    //初始化USB接口的设备
    //======================简单说明================================
    //icdev=dc_init(0,9600)            //初始化串口1，波特率9600
    //icdev=dc_init(100,9600)        //初始化USB 接口的读写器，此时波特率不起作用，但不能为空。

    //注意： 串口1的port值为0。以此类推，串口2的port值为1。
    //==============================================================

    st = rd.dc_init(100, 115200);
    if (st <= 0) //返回值小于等于0表示失败
    {
        alert('USB端口未发现读卡设备!');
        clearInterval(validation);
        validationDialog.dialog('close');
        return 0;
    }

    // ======================简单说明===============================
    //寻卡，能返回在工作区域内某张卡的序列号
    //第一个参数一般设置为0，表示IDLE模式，一次只对一张卡操作
    //第二个参数在javascript只是当成dc_card函数的一个临时变量，仅在vbscript中调用后能正确返回序列号
    //寻卡函数，用于取放在读写器工作区上的卡片的序列号
    //==============================================================


    st = rd.dc_card(0, lSnr);

    if (st != 0) //返回值小于0表示失败
    {
        rd.dc_exit();
        //alert('读卡设备有效范围内没有可读取的卡!');
        return 0;
    }

    if ((rd.get_bstrRBuffer).length < 1) {
        rd.dc_exit();
        //alert('读卡设备有效范围内没有可读取的卡!');
        return 0;
    }
    returnValue = rd.get_bstrRBuffer_asc;
    if (!isNoBeep) {
        st = rd.dc_beep(50);
        if (st < 0) //返回值小于0表示失败
        {
            rd.dc_exit();
            alert("beep失败!");
            return 0;
        }
    }
    //========================简单说明=============================
    //关闭端口
    //关闭由 dc_init 打开的端口    
    //==============================================================	

    st = rd.dc_exit();
    if (st < 0) //返回值小于0表示失败
    {
        alert("退出失败!");
        return 0;
    }
    return returnValue;
};