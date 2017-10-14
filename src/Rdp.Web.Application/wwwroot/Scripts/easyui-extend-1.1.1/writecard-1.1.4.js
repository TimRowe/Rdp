//將十進制數字轉化為可寫入拍卡器的二進制數據
function getHexadecimal(no) {
    var i, j, data;
    data = '';
    i = no.toString().length;
    for (j = 0; j < i; j++) {
        data = data + no.toString().substring(j, j + 1).charCodeAt(0).toString(16);
    }
    data = data + '00202020202020202020202020202020';
    return data.substring(0, 32);
}
function writeCard(sector, no, isBeep) {
    //转化为十六进制数据
    var st;//主要用于返回值
    var sb;       // 地址塊號    
    var lSnr; //本用于取序列号，但在javascript只是当成dc_card函数的一个临时变量
    sb = sector * 4 ;
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
    rd.put_bstrSBuffer_asc = getHexadecimal(no);
    st = rd.dc_write(sb);
    if (st < 0) //返回值小于0表示失败
    {
        rd.dc_exit();
        return "读写数据失败,请检查卡是否有效!";
    }
    if (isBeep) {
        st = rd.dc_beep(50);
        if (st < 0) //返回值小于0表示失败
        {
            rd.dc_exit();
            return "beep失败!";
        }
    }
    st = rd.dc_exit();
    if (st < 0) //返回值小于0表示失败
    {
        return "退出失败1";
    }
};