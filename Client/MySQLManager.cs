using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using MySql.Data.MySqlClient;
using System.Xml;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;
using System;

//훈련 데이터
public class EquipInfoData
{
    TbMtmenuInfoData tbMtmenuInfoes; //예방정비
    TbMtmenuBreakInfoData tbMtmenuBreakInfo; //고장정비
}

public class MySQLManager : Singleton<MySQLManager>
{
    [Header("[ LOCAL CONNECTION ] ")]
    [SerializeField] public bool isLocalConnection = false;
    [SerializeField] public TextAsset userInfoData_json, qrData_json, tecPDFData_json, madePDFData_json;
    USBDataConnection usbDataConnection;

    //"http://211.54.146.2:8080"
    //"http://192.168.0.5:8080"
    [Header("[ DB CONNECTION ] ")]
    [SerializeField] public string apiLink = "";
    [SerializeField] public string apiLink_ExPort = "";
    [SerializeField] public string user_ExURL = "/api/tbUserbases";
    [SerializeField] public string qr_ExURL = "/api/tbQrs";
    [SerializeField] public string tecPDF_ExURL = "/api/tbTcmenuInfoes";
    [SerializeField] public string madePDF_ExURL = "/api/tbMfDatas";
    [SerializeField] public string tbRcCollabo_ExURL = "/api/tbRcCollaboes";
    [SerializeField] public string tbContManages_ExURL = "/api/tbContManages";
    [SerializeField] public string tbMtmenuInfoes_ExURL = "/api/tbMtmenuInfoes";
    [SerializeField] public string tbMtmenuInfoDetail_ExURL = "/api/tbMtmenuInfoDetails";
    [SerializeField] public string tbMtmenuInfoItem_ExURL = "/api/tbMtmenuInfoItems";
    [SerializeField] public string tbMtmenuBreakInfo_ExURL = "/api/tbMtmenuBreakInfoes";
    [SerializeField] public string TbS1000d_ExURL = "/api/tbS1000ds";

    [SerializeField] public UserInfoData tbUserbases;
    [SerializeField] public QRData tbQrs;
    [SerializeField] public TecPDFData tbTcmenuInfoes;
    [SerializeField] public MadePDFData tbMfDatas;
    [SerializeField] public TbRcCollaboData tbRcCollaboData;
    [SerializeField] public TbContManageData tbContManages;
    [SerializeField] public TbMtmenuInfoData tbMtmenuInfoes;
    [SerializeField] public TbMtmenuInfoDetailData tbMtmenuInfoDetail = new TbMtmenuInfoDetailData();
    [SerializeField] public TbMtmenuBreakInfoData tbMtmenuBreakInfo;
    [SerializeField] public TbS1000dData tbS1000dData;

    //보고서
    [SerializeField] public List<ReportData> reportDataList;

    [Header("[ INSERT SETTING ] ")]
    [SerializeField] public string tbMtmenuInsp_ExURL = "/api/tbMtmenuInsps";
    [SerializeField] public string tbMtmenuDetailInsp_ExURL = "/api/tbMtmenuDetailInsps";
    [SerializeField] public TbMtmenuInspData tbMtmenuInsp;
    [SerializeField] public TbMtmenuDetailInspData tbMtmenuDetailInsp;



    //LOGIN USER DATA
    public TbUserbasis currentUserData;

	[Header("[ INSERT DATA ] ")]
	public TbMtmenuInsp tbMtmenuInsp_ins;
	//public List<TbMtmenuDetailInsp> tbMtmenuDetailInsp_ins;

	private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if(usbDataConnection == null)
            usbDataConnection = FindObjectOfType<USBDataConnection>();
    }

    void Start()
    {
        DataLoad(apiLink + user_ExURL);

        //DataLoad(apiLink + TbS1000d_ExURL);
    }

    #region LOCAL CONNECTION
    public void DataLoadJSON(TextAsset ta)
    {
        StartCoroutine(DataLoadJSON_Cor(ta));
    }

    IEnumerator DataLoadJSON_Cor(TextAsset _ta)
    {
        if (_ta == null) yield break;

        if (_ta == userInfoData_json)
        {
            tbUserbases = JsonConvert.DeserializeObject<UserInfoData>(_ta.text);
            Debug.Log("============USER INFO DATA SUCCESS============");

            if (tbUserbases == null) Debug.Log("userInfoData NULL");
        }
        else if (_ta == qrData_json)
        {
            tbQrs = JsonConvert.DeserializeObject<QRData>(_ta.text);
            Debug.Log("============QR DATA SUCCESS============");

            if (tbQrs == null) Debug.Log("qrData NULL");
        }
        else if (_ta == tecPDFData_json)
        {
            tbTcmenuInfoes = JsonConvert.DeserializeObject<TecPDFData>(_ta.text);
            Debug.Log("============TECPDF DATA SUCCESS============");

            if (tbTcmenuInfoes == null) Debug.Log("tecPDFData NULL");
        }
        else if (_ta == madePDFData_json)
        {
            tbMfDatas = JsonConvert.DeserializeObject<MadePDFData>(_ta.text);
            Debug.Log("============MADEPDF DATA SUCCESS============");

            if (tbMfDatas == null) Debug.Log("madePDFData NULL");
        }
    }

    #endregion

    public void UserDataSetting(TbUserbasis userData)
    {
        currentUserData = new TbUserbasis(userData.userbaseId, userData.userbasePassword, 
            userData.userbaseName, userData.userbaseCompany, userData.userbasePosition, 
            userData.userbaseDepartment, userData.userbaseEmail, userData.userbaseImage,
            userData.userbasePic, userData.userbaseExpert, userData.userbaseManager, 
            userData.connStatus, userData.coopStatus,
            userData._links);
    }

    #region GET DATA POST

    public void DataLoad(string url)
    {
        StartCoroutine(DataLoad_Cor(url));
    }

    public IEnumerator DataLoad_Cor(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);

        using (www)
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("ConnectionError : " + www.error);
            }
            else if (www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("ProtocolError : " + www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);

                if (url == apiLink + user_ExURL)
                {
                    tbUserbases = JsonConvert.DeserializeObject<UserInfoData>(www.downloadHandler.text);
                    Debug.Log("============userInfoData SUCCESS============");

                    if (tbUserbases == null) Debug.Log("userInfoData NULL");
                }
                else if (url == apiLink + qr_ExURL)
                {
                    tbQrs = JsonConvert.DeserializeObject<QRData>(www.downloadHandler.text);
                    Debug.Log("============qrData SUCCESS============");

                    if (tbQrs == null) Debug.Log("qrData NULL");

                    //ID PK 값을 가져오기 위한 구문
                    char separatorChar = '/';
                    string[] textLines;
                    for (int i = 0; i < tbQrs._embedded.tbQrs.Count; i++)
                    {
                        textLines = tbQrs._embedded.tbQrs[i]._links.self.href.Split(separatorChar);

                        if (textLines.Length != 0)
                        {
                            tbQrs._embedded.tbQrs[i]._id = textLines[textLines.Length - 1];
                        }
                        else
                        {
                            tbQrs._embedded.tbQrs[i]._id = "";
                        }
                    }
                }
                else if (url == apiLink + tecPDF_ExURL)
                {
                    tbTcmenuInfoes = JsonConvert.DeserializeObject<TecPDFData>(www.downloadHandler.text);
                    Debug.Log("============tecPDFData SUCCESS============");

                    if (tbTcmenuInfoes == null) Debug.Log("tecPDFData NULL");

                    //ID PK 값을 가져오기 위한 구문
                    char separatorChar = '/';
                    string[] textLines;
                    for (int i = 0; i < tbTcmenuInfoes._embedded.tbTcmenuInfoes.Count; i++)
                    {
                        textLines = tbTcmenuInfoes._embedded.tbTcmenuInfoes[i]._links.self.href.Split(separatorChar);

                        if (textLines.Length != 0)
                        {
                            tbTcmenuInfoes._embedded.tbTcmenuInfoes[i]._id = textLines[textLines.Length - 1];
                        }
                        else
                        {
                            tbTcmenuInfoes._embedded.tbTcmenuInfoes[i]._id = "";
                        }
                    }
                }
                else if (url == apiLink + madePDF_ExURL)
                {
                    tbMfDatas = JsonConvert.DeserializeObject<MadePDFData>(www.downloadHandler.text);
                    Debug.Log("============madePDFData SUCCESS============");

                    if (tbMfDatas == null) Debug.Log("madePDFData NULL");

                    //ID PK 값을 가져오기 위한 구문
                    char separatorChar = '/';
                    string[] textLines;
                    for (int i = 0; i < tbMfDatas._embedded.tbMfDatas.Count; i++)
                    {
                        textLines = tbMfDatas._embedded.tbMfDatas[i]._links.self.href.Split(separatorChar);

                        if (textLines.Length != 0)
                        {
                            tbMfDatas._embedded.tbMfDatas[i]._id = textLines[textLines.Length - 1];
                            //Debug.Log("id : " + tbMfDatas._embedded.tbMfDatas[i]._id);
                        }
                        else
                        {
                            tbMfDatas._embedded.tbMfDatas[i]._id = "";
                        }
                    }
                }
                else if (url == apiLink + tbRcCollabo_ExURL)
                {
                    tbRcCollaboData = JsonConvert.DeserializeObject<TbRcCollaboData>(www.downloadHandler.text);
                    Debug.Log("============tbRcCollaboData SUCCESS============");

                    if (tbRcCollaboData == null) Debug.Log("tbRcCollaboData NULL");

                    //ID PK 값을 가져오기 위한 구문
                    char separatorChar = '/';
                    string[] textLines;
                    for (int i = 0; i < tbRcCollaboData._embedded.tbRcCollaboes.Count; i++)
                    {
                        textLines = tbRcCollaboData._embedded.tbRcCollaboes[i]._links.self.href.Split(separatorChar);

                        if (textLines.Length != 0)
                        {
                            tbRcCollaboData._embedded.tbRcCollaboes[i]._id = textLines[textLines.Length - 1];
                            //Debug.Log("id : " + tbContManages._embedded.tbContManages[i]._id);
                        }
                        else
                        {
                            tbRcCollaboData._embedded.tbRcCollaboes[i]._id = "";
                        }
                    }
                }
                else if (url == apiLink + tbContManages_ExURL)
                {
                    tbContManages = JsonConvert.DeserializeObject<TbContManageData>(www.downloadHandler.text);
                    Debug.Log("============tbContManages SUCCESS============");

                    if (tbContManages == null) Debug.Log("tbContManages NULL");

                    //ID PK 값을 가져오기 위한 구문
                    char separatorChar = '/';
                    string[] textLines;
                    for (int i = 0; i < tbContManages._embedded.tbContManages.Count; i++)
                    {
                        textLines = tbContManages._embedded.tbContManages[i]._links.self.href.Split(separatorChar);

                        if (textLines.Length != 0)
                        {
                            tbContManages._embedded.tbContManages[i]._id = textLines[textLines.Length - 1];
                            //Debug.Log("id : " + tbContManages._embedded.tbContManages[i]._id);
                        }
                        else
                        {
                            tbContManages._embedded.tbContManages[i]._id = "";
                        }
                    }
                }
                else if (url == apiLink + TbS1000d_ExURL)
                {
                    tbS1000dData = JsonConvert.DeserializeObject<TbS1000dData>(www.downloadHandler.text);
                    Debug.Log("============tbS1000dData SUCCESS============");

                    if (tbS1000dData == null) Debug.Log("tbS1000dData NULL");

                    //ID PK 값을 가져오기 위한 구문
                    char separatorChar = '/';
                    string[] textLines;
                    for (int i = 0; i < tbS1000dData._embedded.tbS1000ds.Count; i++)
                    {
                        textLines = tbS1000dData._embedded.tbS1000ds[i]._links.self.href.Split(separatorChar);

                        if (textLines.Length != 0)
                        {
                            tbS1000dData._embedded.tbS1000ds[i]._id = textLines[textLines.Length - 1];
                            Debug.Log("id : " + tbS1000dData._embedded.tbS1000ds[i]._id);
                        }
                        else
                        {
                            tbS1000dData._embedded.tbS1000ds[i]._id = "";
                        }
                    }
                }
            }
        }
    }
    public void MtMenuDataLoad(string url)
    {
        StartCoroutine(MtMenuDataLoad_Cor(url));
    }

    public void GetReportData()
    {
        StartCoroutine(GetReportData_Cor("/json/comm/commonSelect.do"));
    }

    //Join된 테이블 데이터를 가져오기위한 PARAM
    public class Param
    {
        public string sqlId;
        public string searchTxt;
    }

    public IEnumerator GetReportData_Cor(string url)
    {
        Param param = new Param();

        param.sqlId = "es_commonQry.selectIRList";
        param.searchTxt = "%";

        string serialized = JsonConvert.SerializeObject(param, Newtonsoft.Json.Formatting.None);
        byte[] ser_bytes = Encoding.UTF8.GetBytes(serialized);

        UnityWebRequest www = UnityWebRequest.Post(apiLink + url, serialized);

        www.downloadHandler = new DownloadHandlerBuffer();
        www.uploadHandler = new UploadHandlerRaw(ser_bytes);
        www.uploadHandler.contentType = "application/json";

        yield return www.SendWebRequest();

        //Debug.Log(www.downloadHandler.text);

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("Failed ReportData" + www.error);
            yield break;
        }
        else
        {
            string res_json = www.downloadHandler.text;
            reportDataList = JsonConvert.DeserializeObject<List<ReportData>>(res_json);

            if (reportDataList.Count != 0)
            {
                for (int i = 0; i < reportDataList.Count; i++)
                {
                    //http://211.54.146.2/#/report-viewer/ff8081818c572a7b018c5cabc7e7000c/%EC%98%88%EB%B0%A9%EC%A0%95%EB%B9%84
                    reportDataList[i].report_api = apiLink_ExPort + "/#/report-viewer/" + reportDataList[i].id
                        + "/" + reportDataList[i].inspecReportClf;

                    //"2023-12-06T15:00:00.000+00:00"
                    string[] split_str = reportDataList[i].inspTimeFrom.Split('.');
                    string tmpi = split_str[0].Replace("T", " ");

                    reportDataList[i].inspTimeFrom = tmpi;
                    //Debug.Log("inspTimeFrom : " + tmpi);

                    split_str = reportDataList[i].inspecReportRegdate.Split('.');
                    tmpi = split_str[0].Replace("T", " ");

                    reportDataList[i].inspecReportRegdate = tmpi;
                    //Debug.Log("inspecReportRegdate : " + tmpi);
                }
            }

            Debug.Log("============ReportData SUCCESS============");
        }
    }
    public IEnumerator MtMenuDataLoad_Cor(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);

        using (www)
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);

                if (url == apiLink + tbMtmenuInfoes_ExURL)
                {
                    tbMtmenuInfoes = JsonConvert.DeserializeObject<TbMtmenuInfoData>(www.downloadHandler.text);
                    Debug.Log("============tbMtmenuInfoes SUCCESS============");

                    if (tbMtmenuInfoes == null) Debug.Log("tbMtmenuInfoes NULL");

                    //ID PK 값을 가져오기 위한 구문
                    char separatorChar = '/';
                    string[] textLines;
                    for (int i = 0; i < tbMtmenuInfoes._embedded.tbMtmenuInfoes.Count; i++)
                    {
                        textLines = tbMtmenuInfoes._embedded.tbMtmenuInfoes[i]._links.self.href.Split(separatorChar);

                        if (textLines.Length != 0)
                        {
                            tbMtmenuInfoes._embedded.tbMtmenuInfoes[i]._id = textLines[textLines.Length - 1];
                            //Debug.Log("id : " + tbMtmenuInfoes._embedded.tbMtmenuInfoes[i]._id);
                        }
                        else
                        {
                            tbMtmenuInfoes._embedded.tbMtmenuInfoes[i]._id = "";
                        }
                    }
                }
                else if (url == apiLink + tbMtmenuBreakInfo_ExURL)
                {
                    tbMtmenuBreakInfo = JsonConvert.DeserializeObject<TbMtmenuBreakInfoData>(www.downloadHandler.text);
                    Debug.Log("============tbMtmenuBreakInfo SUCCESS============");

                    if (tbMtmenuBreakInfo == null) Debug.Log("tbMtmenuBreakInfo NULL");

                    //ID PK 값을 가져오기 위한 구문
                    char separatorChar = '/';
                    string[] textLines;
                    for (int i = 0; i < tbMtmenuBreakInfo._embedded.tbMtmenuBreakInfoes.Count; i++)
                    {
                        textLines = tbMtmenuBreakInfo._embedded.tbMtmenuBreakInfoes[i]._links.self.href.Split(separatorChar);

                        if (textLines.Length != 0)
                        {
                            tbMtmenuBreakInfo._embedded.tbMtmenuBreakInfoes[i]._id = textLines[textLines.Length - 1];
                            //Debug.Log("id : " + tbMtmenuInfoes._embedded.tbMtmenuInfoes[i]._id);
                        }
                        else
                        {
                            tbMtmenuBreakInfo._embedded.tbMtmenuBreakInfoes[i]._id = "";
                        }
                    }
                }
                else
                {
                    Debug.Log("============" + url + " : NO MATCH============");
                }
            }
        }
    }

    IEnumerator waitEvent;
    public IEnumerator SelectMtMenuDetailData_Cor(string mtInfoId)
    {
        string detailURL = "http://211.54.146.2/api/tbMtmenuInfoDetails/search/findByTbMtmenuId?tbMtmenuId=";

        UnityWebRequest www = UnityWebRequest.Get(detailURL + mtInfoId);

        //초기화 
        tbMtmenuInfoDetail = new TbMtmenuInfoDetailData();

        using (www)
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                tbMtmenuInfoDetail = JsonConvert.DeserializeObject<TbMtmenuInfoDetailData>(www.downloadHandler.text);
                //Debug.Log("============tbMtmenuInfoDetail SUCCESS============ : ID : " + mtInfoId);

                if (tbMtmenuInfoDetail._embedded.tbMtmenuInfoDetails.Count == 0)
                {
                    Debug.Log("tbMtmenuInfoDetail NULL");
                    yield break;
                }

                //ID PK 값을 가져오기 위한 구문 
                char separatorChar = '/';
                string[] textLines;
                for (int i = 0; i < tbMtmenuInfoDetail._embedded.tbMtmenuInfoDetails.Count; i++)
                {
                    //Debug.Log("Search Detail : " + tbMtmenuInfoDetail._embedded.tbMtmenuInfoDetails[i].mtmenuInfoCdDiv);

                    textLines = tbMtmenuInfoDetail._embedded.tbMtmenuInfoDetails[i]._links.self.href.Split(separatorChar);

                    if (textLines.Length != 0)
                    {
                        tbMtmenuInfoDetail._embedded.tbMtmenuInfoDetails[i]._id = textLines[textLines.Length - 1];

                    }
                    else
                    {
                        tbMtmenuInfoDetail._embedded.tbMtmenuInfoDetails[i]._id = "";
                    }

                    waitEvent = SelectMtMenuDetailItemData_Cor(tbMtmenuInfoDetail._embedded.tbMtmenuInfoDetails[i]._id,
                        tbMtmenuInfoDetail._embedded.tbMtmenuInfoDetails[i].tbMtmenuInfoItem);

                    yield return StartCoroutine(waitEvent);
                }
            }
        }
    }

    IEnumerator SelectMtMenuDetailItemData_Cor(string mtInfoDetailId, List<TbMtmenuInfoItem> item)
    {
        string detailItemURL = "http://211.54.146.2/api/tbMtmenuInfoItems/search/findByTbMtmenuInfoDetailId?tbMtmenuInfoDetailId=";

        UnityWebRequest www = UnityWebRequest.Get(detailItemURL + mtInfoDetailId);

        TbMtmenuInfoItemData emp = new TbMtmenuInfoItemData();

        using (www)
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                emp = JsonConvert.DeserializeObject<TbMtmenuInfoItemData>(www.downloadHandler.text);

                //Debug.Log("============tbMtmenuInfoItem SUCCESS============ ID : " + mtInfoDetailId);

                if (emp._embedded.tbMtmenuInfoItems.Count == 0)
                {
                    Debug.Log("mtInfoDetailId : " + mtInfoDetailId + " / tbMtmenuInfoItem NULL");
                    yield break;
                }

                //ID PK 값을 가져오기 위한 구문 
                char separatorChar = '/';
                string[] textLines;
                for (int i = 0; i < emp._embedded.tbMtmenuInfoItems.Count; i++)
                {
                    textLines = emp._embedded.tbMtmenuInfoItems[i]._links.self.href.Split(separatorChar);

                    if (textLines.Length != 0)
                    {
                        emp._embedded.tbMtmenuInfoItems[i]._id = textLines[textLines.Length - 1];
                    }
                    else
                    {
                        emp._embedded.tbMtmenuInfoItems[i]._id = "";
                    }

                    item.Add(emp._embedded.tbMtmenuInfoItems[i]);
                }

            }
        }
    }

    #endregion

    #region UPDATE DATA PUT
    public void UpdateUserPw(string url, string pw)
    {
        ///api/tbUserbases/
        StartCoroutine(UpdateUserPw_Cor(url, pw));
    }
    IEnumerator UpdateUserPw_Cor(string url, string pw)
    {
        if(currentUserData == null) yield break;

        currentUserData.userbasePassword = pw;

        string serialized = JsonConvert.SerializeObject(currentUserData, Newtonsoft.Json.Formatting.None);
        byte[] ser_bytes = Encoding.UTF8.GetBytes(serialized);

        UnityWebRequest www = UnityWebRequest.Put(apiLink + url + currentUserData.userbaseId, serialized);

        //UnityWebRequest www = new UnityWebRequest(apiLink + "/api/tbUserbases/" + currentUserData.userbaseId, "Put");
        //Debug.Log("www url : " + apiLink + "/api/tbUserbases/" + currentUserData.userbaseId);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.uploadHandler = new UploadHandlerRaw(ser_bytes);
        www.uploadHandler.contentType = "application/json";

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("failed PW CHANGE" + www.error);
            yield break;
        }
        else
        {
            string res_json = www.downloadHandler.text;
            Debug.Log("PW CHANGE : " + res_json);
        }
    }

    //coon : 로그인시 (접속상태) coop : 원격협업 유무
    public void UpdateUserStatus(string url, string coonstatus, string coopstatus)
    {
        ///api/tbUserbases/
        StartCoroutine(UpdateUserStatus_Cor(url, coonstatus, coopstatus));
    }
    IEnumerator UpdateUserStatus_Cor(string url, string coonstatus, string coopstatus)
    {
        if (currentUserData == null) yield break;

        if (coonstatus != "")
            currentUserData.connStatus = coonstatus;
        if (coopstatus != "")
            currentUserData.coopStatus = coopstatus;

        string serialized = JsonConvert.SerializeObject(currentUserData, Newtonsoft.Json.Formatting.None);
        byte[] ser_bytes = Encoding.UTF8.GetBytes(serialized);

        UnityWebRequest www = UnityWebRequest.Put(apiLink + url + currentUserData.userbaseId, serialized);

        //UnityWebRequest www = new UnityWebRequest(apiLink + "/api/tbUserbases/" + currentUserData.userbaseId, "Put");
        //Debug.Log("www url : " + apiLink + "/api/tbUserbases/" + currentUserData.userbaseId);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.uploadHandler = new UploadHandlerRaw(ser_bytes);
        www.uploadHandler.contentType = "application/json";

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            if (coonstatus != "")
                Debug.LogWarning("failed coonstatus CHANGE" + www.error);
            else if (coopstatus != "")
                Debug.LogWarning("failed coopstatus CHANGE" + www.error);
            yield break;
        }
        else
        {
            string res_json = www.downloadHandler.text;
            if (coonstatus != "")
                Debug.Log("coonstatus CHANGE : " + res_json);
            else if (coopstatus != "")
                Debug.Log("coopstatus CHANGE : " + res_json);

        }
    }

    public void UpdateTbS1000d(string url, TbS1000d tbS1000d, string s1000dFvreq, string s1000dFvrcode, string s1000dFvdmn,
        string s1000dFvregt)
    {
        StartCoroutine(UpdateTbS1000d_Cor(url, tbS1000d, s1000dFvreq, s1000dFvrcode, s1000dFvdmn, s1000dFvregt));
    }

    IEnumerator UpdateTbS1000d_Cor(string url, TbS1000d tbS1000d, string s1000dFvreq, string s1000dFvrcode, string s1000dFvdmn,
        string s1000dFvregt)
    {
        if (tbS1000d == null) yield break;
        tbS1000d.s1000dFvreq = s1000dFvreq;
        tbS1000d.s1000dFvrcode = s1000dFvrcode;
        tbS1000d.s1000dFvdmn = s1000dFvdmn;
        tbS1000d.s1000dFvregt = s1000dFvregt;


        string serialized = JsonConvert.SerializeObject(tbS1000d, Newtonsoft.Json.Formatting.None);
        byte[] ser_bytes = Encoding.UTF8.GetBytes(serialized);

        UnityWebRequest www = UnityWebRequest.Put(apiLink + url + tbS1000d._id, serialized);

        www.downloadHandler = new DownloadHandlerBuffer();
        www.uploadHandler = new UploadHandlerRaw(ser_bytes);
        www.uploadHandler.contentType = "application/json";

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("failed tbS1000d CHANGE" + www.error);
            yield break;
        }
        else
        {
            string res_json = www.downloadHandler.text;
            Debug.Log("tbS1000d CHANGE : " + res_json);
        }
    }

    #endregion

    #region INSERT DATA POST
    public void InsertTbMtmenuInsp(string url, TbMtmenuInsp item)
	{
        StartCoroutine(InsertTbMtmenuInsp_Cor(url, item));
	}

    //보고서 데이터 저장 /api/tbMtmenuInsps값을 던진다 
    public IEnumerator InsertTbMtmenuInsp_Cor(string url, TbMtmenuInsp item)
    {
        //string dateString = "2022-03-30 15:30:00";

        tbMtmenuInsp_ins = new TbMtmenuInsp();
        tbMtmenuInsp_ins.mtmenuInfoId = item.mtmenuInfoId;
        tbMtmenuInsp_ins.inspector = item.inspector;
        tbMtmenuInsp_ins.department = item.department;
        tbMtmenuInsp_ins.rank = item.rank;
        tbMtmenuInsp_ins.inspTimeFrom = item.inspTimeFrom;
        tbMtmenuInsp_ins.inspTimeTo = item.inspTimeTo;
        tbMtmenuInsp_ins.measures = item.measures;
        tbMtmenuInsp_ins.notposb = item.notposb;
        tbMtmenuInsp_ins.mxtrepeNm = item.mxtrepeNm;
        tbMtmenuInsp_ins.mxtrepeNum = item.mxtrepeNum;
        tbMtmenuInsp_ins.testEqNm = item.testEqNm;
        tbMtmenuInsp_ins.testEqNum = item.testEqNum;
        tbMtmenuInsp_ins.toolNm = item.toolNm;
        tbMtmenuInsp_ins.toolNum = item.toolNum;
        tbMtmenuInsp_ins.consumNm = item.consumNm;
        tbMtmenuInsp_ins.consumNum = item.consumNum;

        string serialized = JsonConvert.SerializeObject(tbMtmenuInsp_ins, Newtonsoft.Json.Formatting.None);
        byte[] ser_bytes = Encoding.UTF8.GetBytes(serialized);
        UnityWebRequest www = UnityWebRequest.Post(apiLink + url, serialized);

        www.downloadHandler = new DownloadHandlerBuffer();
        www.uploadHandler = new UploadHandlerRaw(ser_bytes);
        www.uploadHandler.contentType = "application/json";

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("failed to TbMtmenuInsp info. " + www.error);
            yield break;
        }
        else
        {
            string res_json = www.downloadHandler.text;
            //Debug.Log(res_json);

            tbMtmenuInsp_ins = JsonConvert.DeserializeObject<TbMtmenuInsp>(res_json);

            if (tbMtmenuInsp_ins == null) Debug.Log("tbMtmenuInsp NULL");

            Debug.Log(tbMtmenuInsp_ins._links.self.href);
            //ID PK 값을 가져오기 위한 구문 
            char separatorChar = '/';
            string[] textLines;

            textLines = tbMtmenuInsp_ins._links.self.href.Split(separatorChar);

            if (textLines.Length != 0)
            {
                tbMtmenuInsp_ins._id = textLines[textLines.Length - 1];
            }
            else
            {
                tbMtmenuInsp_ins._id = "";
            }

            Debug.Log("DB TbMtmenuInsp INSERT SUCCESS : " + res_json);
        }
    }

    public void InsertTbMtmenuDetailInsp(string url, List<MtMenuDetailInspItem> item)
    {
        for (int i = 0; i < item.Count; i++)
        {
            StartCoroutine(InsertTbMtmenuDetailInsp_Cor(url, item[i].detailInsp));
        }
    }

    //보고서 데이터 저장 n개의 /api/tb_mtmenu_detail_ins값을 던진다 
    IEnumerator InsertTbMtmenuDetailInsp_Cor(string url, TbMtmenuDetailInsp item)
    {
        TbMtmenuDetailInsp setDetail = new TbMtmenuDetailInsp();
        setDetail.mtmenuInspId = item.mtmenuInspId;
        setDetail.mtmenuDetailId = item.mtmenuDetailId;
        setDetail.mtmenuItemId = item.mtmenuItemId;

        string serialized = JsonConvert.SerializeObject(setDetail, Newtonsoft.Json.Formatting.None);
        byte[] ser_bytes = Encoding.UTF8.GetBytes(serialized);
        UnityWebRequest www = UnityWebRequest.Post(apiLink + url, serialized);

        www.downloadHandler = new DownloadHandlerBuffer();
        www.uploadHandler = new UploadHandlerRaw(ser_bytes);
        www.uploadHandler.contentType = "application/json";

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("failed to TbMtmenuInsp info. " + www.error);
            yield break;
        }
        else
        {
            string res_json = www.downloadHandler.text;
            Debug.Log("DB TbMtmenuDetailInsp INSERT SUCCESS : " + res_json);
        }
    }

    #endregion


    //이미지 가져오는 예시 
    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://www.my-server.com/image.png");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }

    void OnApplicationQuit()
    {
        UpdateUserStatus("/api/tbUserbases/", "N", "N");
        Debug.Log("APPLICATION QUIT");
    }
}
