function OnTest(control,alert_flag, desc1, desc2, desc3, desc4, desc5, desc6) {
    ///////////////////////////////////////////////////////////////////////////////
    //����Ϊ�ǽӴ�ʽ��д�������������� for javascript
    //ע�������������dc_disp_str��ֻ�е��豸����Ҫ��������������ʾ��ʱ����Ч
    //����ϸ�İ����ɲ���32λ��̬���Ӧ�ĺ���ʹ��˵��
    ///////////////////////////////////////////////////////////////////////////////

    var st; //��Ҫ���ڷ���ֵ
    var lSnr; //������ȡ���кţ�����javascriptֻ�ǵ���dc_card������һ����ʱ����
    var sector = 4;	//������
    var block = 16;	//���


    //��ʼ��USB�ӿڵ��豸
    //======================��˵��================================
    //icdev=dc_init(0,9600)            //��ʼ������1��������9600
    //icdev=dc_init(100,9600)        //��ʼ��USB �ӿڵĶ�д������ʱ�����ʲ������ã�������Ϊ�ա�

    //ע�⣺ ����1��portֵΪ0���Դ����ƣ�����2��portֵΪ1��
    //==============================================================

    st = rd.dc_init(100, 115200);
    if (st <= 0) //����ֵС�ڵ���0��ʾʧ��
    {
        if (alert_flag == "true") {
            alert(desc1);
        }

        return false;
    }
    //alert("dc_init ok!");


    // ======================��˵��===============================
    //Ѱ�����ܷ����ڹ���������ĳ�ſ������к�
    //��һ������һ������Ϊ0����ʾIDLEģʽ��һ��ֻ��һ�ſ�����
    //�ڶ���������javascriptֻ�ǵ���dc_card������һ����ʱ����������vbscript�е��ú�����ȷ�������к�
    //Ѱ������������ȡ���ڶ�д���������ϵĿ�Ƭ�����к�
    //==============================================================

    st = rd.dc_card(0, lSnr);
    if (st != 0) //����ֵС��0��ʾʧ��
    {
        //alert(desc2);
        if (alert_flag == "true") { alert(desc2); }
        rd.dc_exit();
        return true;
    }


    if ((rd.get_bstrRBuffer).length < 1) {
        if (alert_flag == "true") { alert(desc2); }
        rd.dc_exit();
        return true;
    }
    //alert("dc_card ok!");
    //alert(rd.get_bstrRBuffer); //���к�Ϊrd.get_bstrRBuffer��һ���в�����ʾ�ַ�����
    //alert(rd.get_bstrRBuffer_asc); //���к�ʮ������ascll���ַ�����ʾΪrd.get_bstrRBuffer_asc



    //=====================��˵��==============================
    //������װ���дģ��RAM��
    //��һ������Ϊװ������ģʽ
    //�ڶ�������Ϊ������
    //==============================================================

    rd.put_bstrSBuffer_asc = "FFFFFFFFFFFF"; //�ڵ���dc_load_key����ǰ����������rd.put_bstrSBuffer��rd.put_bstrSBuffer_asc
    st = rd.dc_load_key(0, sector);
    if (st < 0) //����ֵС��0��ʾʧ��
    {
        alert(desc3);
        rd.dc_exit();
        return true;
    }
    //alert("dc_load_key ok!");



    //========================��˵��============================
    //У������ĺ������˶�ָ�����������룬���Ƚ��м����������
    //��һ������Ϊ������֤ģʽ
    //�ڶ�������Ϊ������
    //==============================================================

    st = rd.dc_authentication(0, sector);
    if (st < 0) //����ֵС��0��ʾʧ��
    {
        alert(desc4);
        rd.dc_exit();
        return true;
    }
    //alert("dc_authentication ok!");



    //==========================��˵��===========================
    //�����ݺ��������ڶԷ��ڶ�д���������ϵĿ�Ƭ��������д��Ĺ������������һ�ζ�һ��
    //��һ������Ϊ���ַ
    //ȡ�������ݷ������Է���rd.put_bstrSBuffer��������ʾ����rd.put_bstrSBuffer_asc��ʮ������ascll���ַ�����ʾ����
    //==============================================================

    st = rd.dc_read(block);
    if (st < 0) //����ֵС��0��ʾʧ��
    {
        alert(desc6);
        document.all(control).value = "";
        rd.dc_exit();
        return true;
    }
    //alert("dc_read ok!");
    //alert(rd.get_bstrRBuffer);
    var return_value = null;

    return_value = rd.get_bstrRBuffer;

    if (IsInteger(return_value.substring(0, 20)) == false) {
        alert(desc5);
        document.all(control).value = "";
        rd.dc_exit();
        return true;
    }

    //alert(rd.get_bstrRBuffer_asc);

    //=========================��˵��=============================
    //�����豸��������
    //��һ������Ϊ����ʱ�䣬��λ��10����
    //��ϸ��鿴 "˵���ĵ�.chm"-->������˵����-->"�豸����"-->"dc_beep"
    //==============================================================


    st = rd.dc_beep(50);
    if (st < 0) //����ֵС��0��ʾʧ��
    {
        //alert("dc_beep error!");
        rd.dc_exit();
        return true;
    }
    //alert("dc_beep ok!");


    //========================��˵��=============================
    //�رն˿�
    //�ر��� dc_init �򿪵Ķ˿�    
    //==============================================================	

    st = rd.dc_exit();
    if (st < 0) //����ֵС��0��ʾʧ��
    {
        //alert("dc_exit error!");
        return true;
    }
    //alert("dc_exit ok!");
    document.all(control).value = return_value.substring(0, 20);
}

function IsInteger(fData) {
    //���Ϊ�գ�����false
    if (IsEmpty(fData))
        return false
    if ((fData.indexOf(".") != -1) || (fData.indexOf("-") != -1))
        return false
    return true
};
function IsEmpty(fData) {
    return ((fData == null) || (fData.length == 0));
};