using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum FinalReportMode
{
    ALL,
    inspecReportClf,//분류
    mtmenuInfoEq, //장비
    mtmenuInfoRepnm, //제목
    mtmenuInfoEqnpn, //장비명
    mtmenuInfoEqnm, //장비번호
    mtmenuInfoComp, //구성품
    inspector,//점검자
    inspecReportReq, //생성자
    NONE
}

public class FinalReportManager : MonoBehaviour
{
    [Header("[ FinalReportData ]")]
    [SerializeField] FinalReportData_Item finalReportDataItem_pre;
    [SerializeField] Transform finalReportDataItem_tr;

    [SerializeField] List<FinalReportData_Item> finalReportDataItems = new List<FinalReportData_Item>();

    [SerializeField] public WebViewManager webViewManager;
    [SerializeField] public Button refrash_btn;

    [Header("[ SEARCHING FUNCTION ]")]
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] FinalReportMode _Mode = FinalReportMode.NONE;

    private void Start()
    {
        _Mode = FinalReportMode.ALL;

        refrash_btn.onClick.AddListener(() => DataRefresh(refrash_btn));
        refrash_btn.GetComponent<Animator>().enabled = false;
    }

    bool isTrigger = false;
    private void OnEnable()
    {
        _Mode = FinalReportMode.ALL;
        OnEndEdit();

        if (!isTrigger)
        {
            StartCoroutine(LoadData());
            isTrigger = true;
        }
        else
        {
            DataRefresh(refrash_btn);
        }
    }

    IEnumerator LoadData()
    {
        IEnumerator getData;
        getData = MySQLManager.Instance.GetReportData_Cor("/json/comm/commonSelect.do");
        yield return StartCoroutine(getData);

        if (MySQLManager.Instance.reportDataList.Count == 0) yield break;

        for (int i = 0; i < MySQLManager.Instance.reportDataList.Count; i++)
        {
            AddItem(MySQLManager.Instance.reportDataList[i]);
        }

        SortGameObject();
    }

    public void SortGameObject()
    {
        //SORT 
        for (int i = 0; i < finalReportDataItems.Count; i++)
        {
            string tmpi = "";

            if (finalReportDataItems[i].reportData.inspecReportRegdate != null)
            {
                //2023-11-01 13:37:32
                string[] split = finalReportDataItems[i].reportData.inspecReportRegdate.Split('.');

                tmpi = split[0].Replace("-", "").Replace(":", "").Replace(" ", "");
                //Debug.Log(tmpi);

                //20231211150000
                finalReportDataItems[i].sortCnt = Int64.Parse(tmpi);
            }
        }

        finalReportDataItems.Sort(compare1);

        bool trigger = false;
        for (int i = 0; i < finalReportDataItems.Count; i++)
        {
            trigger = !trigger;
            finalReportDataItems[i].transform.SetSiblingIndex(i);
            finalReportDataItems[i].SetColumeColor(trigger);
        }
    }

    int compare1(FinalReportData_Item a, FinalReportData_Item b)
    {
        return a.sortCnt > b.sortCnt ? -1 : 1;
    }

    public void DataRefresh(Button btn)
    {
        StartCoroutine(DataRefresh_Cor(btn));
    }

    //데이터 리프레쉬
    IEnumerator DataRefresh_Cor(Button btn)
    {
        btn.interactable = false;
        btn.GetComponent<Animator>().enabled = true;

        for (int z = 0; z < finalReportDataItems.Count; z++)
        {
            Destroy(finalReportDataItems[z].gameObject);
        }
        finalReportDataItems.Clear();

        IEnumerator getData = LoadData();
        yield return StartCoroutine(getData);

        btn.interactable = true;
        btn.GetComponent<Animator>().enabled = false;

        Debug.Log("REFRASH DATA");
    }

    public void AddItem(ReportData reportData)
    {
        finalReportDataItems.Add(Instantiate(finalReportDataItem_pre, finalReportDataItem_tr));
        finalReportDataItems[finalReportDataItems.Count - 1].manager = gameObject.GetComponent<FinalReportManager>();

        finalReportDataItems[finalReportDataItems.Count - 1].SetText(reportData);
    }

    #region SEARCHING FUN

    public void OnValueChanged()
    {
        if (dropdown.value == 0)
        {
            _Mode = FinalReportMode.ALL;
        } 
        else if (dropdown.value == 1)
        {
            _Mode = FinalReportMode.inspecReportClf;
        }
        else if (dropdown.value == 2)
        {
            _Mode = FinalReportMode.mtmenuInfoEq;
        }
        else if (dropdown.value == 3)
        {
            _Mode = FinalReportMode.mtmenuInfoRepnm;
        }
        else if (dropdown.value == 4)
        {
            _Mode = FinalReportMode.mtmenuInfoEqnpn;
        }
        else if (dropdown.value == 5)
        {
            _Mode = FinalReportMode.mtmenuInfoEqnm;
        }
        else if (dropdown.value == 6)
        {
            _Mode = FinalReportMode.mtmenuInfoComp;
        }
        else if (dropdown.value == 7)
        {
            _Mode = FinalReportMode.inspector;
        }
        else if (dropdown.value == 8)
        {
            _Mode = FinalReportMode.inspecReportReq;
        }
        else
        {
            _Mode = FinalReportMode.NONE;
        }
    }

    public void OnSelect()
    {
        inputField.text = "";
    }

    //검색 버튼 클릭 (이벤트) //추가 필요 
    public void OnEndEdit()
    {
        if (finalReportDataItems.Count == 0) return;
        if (_Mode == FinalReportMode.NONE) return;

        var search_split = inputField.text.ToString();

        //검색한 기록이 없는 상태로 클릭했을때 
        if (search_split.Length == 0)
        {
            foreach (FinalReportData_Item item in finalReportDataItems) item.gameObject.SetActive(true);
        }
        else
        {
            switch (_Mode)
            {
                case FinalReportMode.ALL:
                    for (int i = 0; i < finalReportDataItems.Count; i++)
                    {
                        for (int j = 0; j < finalReportDataItems[i].allSearchTexts.Count; j++)
                        {
                            string items_sp = finalReportDataItems[i].allSearchTexts[j];
                            bool trigger = false;

                            for (int z = 0; z < items_sp.Length - (search_split.Length - 1); z++)
                            {
                                string test = "";

                                for (int k = z; k < z + search_split.Length; k++)
                                {
                                    test += items_sp[k];
                                }

                                if (test == search_split)
                                {
                                    finalReportDataItems[i].gameObject.SetActive(true);
                                    trigger = true;

                                    break;
                                }
                            }

                            if (!trigger)
                            {
                                finalReportDataItems[i].gameObject.SetActive(false);
                            }
                        }
                    }
                    break;
                case FinalReportMode.inspecReportClf:
                    for (int i = 0; i < finalReportDataItems.Count; i++)
                    {
                        CheckText(search_split, finalReportDataItems[i].reportData.inspecReportClf, i);
                    }
                    break;
                case FinalReportMode.mtmenuInfoEq:
                    for (int i = 0; i < finalReportDataItems.Count; i++)
                    {
                        CheckText(search_split, finalReportDataItems[i].reportData.mtmenuInfoEq, i);
                    }
                    break;

                case FinalReportMode.mtmenuInfoRepnm:
                    for (int i = 0; i < finalReportDataItems.Count; i++)
                    {
                        CheckText(search_split, finalReportDataItems[i].reportData.mtmenuInfoRepnm, i);
                    }
                    break;

                case FinalReportMode.mtmenuInfoEqnpn:
                    for (int i = 0; i < finalReportDataItems.Count; i++)
                    {
                        CheckText(search_split, finalReportDataItems[i].reportData.mtmenuInfoEqnpn, i);
                    }
                    break;

                case FinalReportMode.mtmenuInfoEqnm:
                    for (int i = 0; i < finalReportDataItems.Count; i++)
                    {
                        CheckText(search_split, finalReportDataItems[i].reportData.mtmenuInfoEqnm, i);
                    }
                    break;
                case FinalReportMode.mtmenuInfoComp:
                    for (int i = 0; i < finalReportDataItems.Count; i++)
                    {
                        CheckText(search_split, finalReportDataItems[i].reportData.mtmenuInfoComp, i);
                    }
                    break;
                case FinalReportMode.inspector:
                    for (int i = 0; i < finalReportDataItems.Count; i++)
                    {
                        CheckText(search_split, finalReportDataItems[i].reportData.inspector, i);
                    }
                    break;
                case FinalReportMode.inspecReportReq:
                    for (int i = 0; i < finalReportDataItems.Count; i++)
                    {
                        CheckText(search_split, finalReportDataItems[i].reportData.inspecReportReq, i);
                    }
                    break;
            }
        }
    }

    void CheckText(string search_split, string _text, int i)
    {
        string items_sp = _text;
        bool trigger = false;

        for (int z = 0; z < items_sp.Length - (search_split.Length - 1); z++)
        {
            string test = "";

            for (int k = z; k < z + search_split.Length; k++)
            {
                test += items_sp[k];
            }

            if (test == search_split)
            {
                finalReportDataItems[i].gameObject.SetActive(true);
                trigger = true;

                break;
            }
        }

        if (!trigger)
        {
            finalReportDataItems[i].gameObject.SetActive(false);
        }
    }

    #endregion
}
