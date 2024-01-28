using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;


#region PM DATA
[System.Serializable]
public struct LinkToDmData //Pm에 할당되는 DM LIST 정리 
{
    public string title;

    public Object pm_data;

    public List<Object> dm_rootName;
}
#endregion

#region DM DATA
//DM 하나에 관련된 데이터 (한 페이지에 담긴 모든 데이터)
[System.Serializable]
public class DmContent
{
    public List<DMItem> contents; //텍스트 내용
    public List<Object> LinkDm; //DM에 연결된 DM 파일 

    public Object OwnDM; //DM 자기자신
    public int OwnDM_Num; //자기자신의 고유의 넘버값 //ContentListSetting.cs에서 값을 할당함 

    public DmContent(List<DMItem> content, List<Object> linkDM, Object ownDM)
    {
        this.contents = content;
        this.LinkDm = linkDM;
        this.OwnDM = ownDM;
    }
}

public enum Data
{
    TEXT, FOLDERCONTENT, FIGURE, TABLE, OPERATION, DIAGRAM, CATALOG, SPACING, NONE
}

public enum TEXTTYPE
{
    TEXT, WARNING, CAUTION, NOTE
}

//DM 데이터 텍스트, 테이블 데이터, 다이어그램 
[System.Serializable]
public class DMItem
{
    public Data data;
    public bool isTitle; //타이틀 항목인지 체크 

    public float spacing;//빈객체 넣어줄때

    public TEXTTYPE texttype;
    public string dmItem_text; //오로지 텍스트 
    public TextDefine.TEXTALIGN textAlign; //텍스트 정렬
    public TextDefine.TEXTSTYLE textStyle; //텍스트 스타일
    public float textSize; //텍스트 사이즈 
    public Color textColor; //텍스트 컬러 

    public DMItem_Figure figure; //사진 또는 객체 (버튼형식)
    public DMItem_Table table; //DM 테이블 내용 
    public DMItem_OperButtonItems operBtn;
    public List<DMItem_DiagramItemSetting> diagram; //다이어그램
    public DMItem_Catalog catalog; //도면 (모델링 링크)

    //DATA.TEXT, DATA.TITLE
    public DMItem(Data _data, bool _isTitle, string _dmItem_text, TEXTTYPE _texttype, TextDefine.TEXTALIGN _textAlign, TextDefine.TEXTSTYLE _textStyle, float _textSize, Color _textColor)
    {
        this.data = _data;
        this.isTitle = _isTitle;

        this.texttype = _texttype;
        this.dmItem_text = _dmItem_text;
        this.textAlign = _textAlign;
        this.textStyle = _textStyle;
        this.textSize = _textSize;
        this.textColor = _textColor;
    }

    //빈객체 넣어줄때
    public DMItem(Data _data, bool _isTitle, float _spacing)
    {
        this.data = _data;
        this.isTitle = _isTitle;

        this.spacing = _spacing;
    }

    public DMItem(Data _data, bool _isTitle, DMItem_Figure _figure)
    {
        this.data = _data;
        this.isTitle = _isTitle;
        this.figure = _figure;
    }

    public DMItem(Data _data, bool _isTitle, DMItem_Table _table)
    {
        this.data = _data;
        this.isTitle = _isTitle;
        this.table = _table;
    }

    public DMItem(Data _data, bool _isTitle, DMItem_OperButtonItems _operBtn)
    {
        this.data = _data;
        this.isTitle = _isTitle;
        this.operBtn = _operBtn;
    }

    public DMItem(Data _data, bool _isTitle, List<DMItem_DiagramItemSetting> _diagram)
    {
        this.data = _data;
        this.isTitle = _isTitle;
        this.diagram = _diagram;
    }

    public DMItem(Data _data, bool _isTitle, DMItem_Catalog _catalog)
    {
        this.data = _data;
        this.isTitle = _isTitle;
        this.catalog = _catalog;
    }
}

[System.Serializable]
public class DMItem_FolderContent
{
    public string header_txt;

    public TEXTTYPE texttype;
    public TextDefine.TEXTALIGN textAlign; //텍스트 정렬
    public TextDefine.TEXTSTYLE textStyle; //텍스트 스타일
    public float textSize; //텍스트 사이즈 
    public Color textColor; //텍스트 컬러 

    public DMItem_FolderContent(string _header_txt, TEXTTYPE _texttype, TextDefine.TEXTALIGN _textAlign, TextDefine.TEXTSTYLE _textStyle, float _textSize, Color _textColor)
    {
        header_txt = _header_txt;

        this.texttype = _texttype;
        this.textAlign = _textAlign;
        this.textStyle = _textStyle;
        this.textSize = _textSize;
        this.textColor = _textColor;
    }
}


[System.Serializable]
public class DMItem_Table
{
    public List<ColSpec> colspec; //열 최대 개수 
    public float sumColWidth;

    public int header_lineCnt; //헤더 행 개수
    public int content_lineCnt; //컨텐츠 행 개수 
    public List<DMItem_Table_Colume> header_colume;
    public List<DMItem_Table_Colume> content_colume; //테이블 아이템 단위 

    public DMItem_Table(ColSpec[] _colspec, float _sumColWidth, int _header_lineCnt, int _content_lineCnt, DMItem_Table_Colume[] _header_colume, DMItem_Table_Colume[] _content_colume)
    {
        colspec = new List<ColSpec>();
        colspec.AddRange(_colspec);

        this.sumColWidth = _sumColWidth;
        this.header_lineCnt = _header_lineCnt;
        this.content_lineCnt = _content_lineCnt;

        header_colume = new List<DMItem_Table_Colume>();
        header_colume.AddRange(_header_colume);

        content_colume = new List<DMItem_Table_Colume>();
        content_colume.AddRange(_content_colume);
    }
}

[System.Serializable]
public class DMItem_UseToolTable
{
    public List<DMItem_UseToolTable_Item> colume; //테이블 아이템 단위 
    public int lineCnt; //열
    public int rowCnt; //행

    public DMItem_UseToolTable(DMItem_UseToolTable_Item[] _colume, int _lineCnt, int _rowCnt)
    {
        colume = new List<DMItem_UseToolTable_Item>();
        colume.AddRange(_colume);

        lineCnt = _lineCnt;
        rowCnt = _rowCnt;
    }
}

[System.Serializable]
public class DMItem_UseToolTable_Item
{
    public TextDefine.TEXTALIGN textAlign;
    public float width;
    public string colume_txt;

    public DMItem_UseToolTable_Item(TextDefine.TEXTALIGN _textAlign, float _width, string _colume_txt)
    {
        this.textAlign = _textAlign;
        this.width = _width;
        this.colume_txt = _colume_txt;
    }
}

[System.Serializable]
public class DMItem_SubTable
{
    public List<string> colume; //테이블 컬럼 텍스트 

    public DMItem_SubTable(string[] _colume)
    {
        colume = new List<string>();
        colume.AddRange(_colume);
    }
}

public enum OperationItems
{
    OPERATION, WARNING, CAUTION, NOTE
}

//DM 데이터 텍스트, 테이블 데이터, 다이어그램 
[System.Serializable]
public class OperationItemOption
{
    public OperationItems OperationItem_type;
    public string operationItem_text; //오로지 텍스트 

    public TextDefine.TEXTALIGN textAlign; //텍스트 정렬
    public TextDefine.TEXTSTYLE textStyle; //텍스트 스타일
    public float textSize; //텍스트 사이즈 
    public Color textColor; //텍스트 컬러 

    public OperationItemOption(OperationItems _OperationItem_type, string _operationItem_text, TextDefine.TEXTALIGN _textAlign, TextDefine.TEXTSTYLE _textStyle, float _textSize, Color _textColor)
    {
        OperationItem_type = _OperationItem_type;
        operationItem_text = _operationItem_text;

        this.textAlign = _textAlign;
        this.textStyle = _textStyle;
        this.textSize = _textSize;
        this.textColor = _textColor;
    }
}

[System.Serializable]
public class DMItem_OperButtonItems
{
    public string operTitle; //이걸로 모델링 교체 예정 
    public string operSubTitle; //장탈/장착
    public List<OperationItemOption> colume; //테이블 컬럼 텍스트 

    public DMItem_OperButtonItems(string _operTitle, string _operSubTitle, OperationItemOption[] _colume)
    {
        operTitle = _operTitle;
        operSubTitle = _operSubTitle;

        colume = new List<OperationItemOption>();
        colume.AddRange(_colume);
    }
}

//사진 또는 객체
[System.Serializable]
public class DMItem_Figure
{
    public string title;
    public string id;
    public string infoEntityIdent;

    public DMItem_Figure(string _title, string _id, string _infoEntityIdent)
    {
        this.title = _title;
        this.id = _id;
        this.infoEntityIdent = _infoEntityIdent;
    }
}

//열의 속성 
[System.Serializable]
public class ColSpec
{
    public string colname; //열의 ID 값 (이값으로 가로정렬과 길이를 알수 있다.) 
    public float colwidth;


    public ColSpec(string _colname, float _colwidth)
    {
        this.colname = _colname;
        this.colwidth = _colwidth;
    }
}

//테이블 아이템 단위
[System.Serializable]
public class DMItem_Table_Colume
{
    public int morerows; //몇번째 행까지 가져가는지 
    public string namest; //ColSpec - colname 과 비교해서 열의 사이즈를 결정
    public string nameend; //ColSpec - colname 과 비교해서 열의 사이즈를 결정
    public float totalColwidth; //namest - nameend 총합 너비 계산 

    public TextDefine.TEXTALIGN textAlign;

    public string colume_txt;

    public bool isEmpty = false;

    public DMItem_Table_Colume(int _morerows, string _namest, string _nameend, float _totalColwidth, TextDefine.TEXTALIGN _textAlign, string _colume_txt, bool _isEmpty)
    {
        this.morerows = _morerows;

        this.namest = _namest;
        this.nameend = _nameend;
        this.totalColwidth = _totalColwidth;

        this.textAlign = _textAlign;

        this.colume_txt = _colume_txt;

        this.isEmpty = _isEmpty;
    }
}

[System.Serializable]
public class DMItem_Catalog //한줄당 데이터를 관리해야된다. 
{
    public int lineCnt; //행
    public int rowCnt;//열의 개수 
    public List<float> item_width;

    public List<Catalog_Item> catalog_items;


    public DMItem_Catalog(int _lineCnt, int _rowCnt, List<float> _item_width, Catalog_Item[] _catalog_items)
    {
        this.lineCnt = _lineCnt;
        this.rowCnt = _rowCnt;
        this.item_width = _item_width;

        //새로 할당이 필요할때 
        catalog_items = new List<Catalog_Item>();
        catalog_items.AddRange(_catalog_items);
    }
}

//테이블 한 행
[System.Serializable]
public class Catalog_Item
{
    //OWN DATA
    public string subSystemCode; //어떤 모델링인지 코드 네임
    public string subsubSystemCode;
    public string assyCode; //필요한 모델링 코드
    public string id; //한장비의 고유 번호 (모델링 또는 부품 표현)

    //TABLE DATA
    public string figureNumber; //계통/그림번호
    public string itemSeqNumberValue; //품목 번호
    public string partNumberValue; //부품번호
    public string emp;
    //국가 재고번호 생략
    public string manufacturerCodeValue; //생산자 부호
    public string descrForPart; //설명
    public string quantityPerNextHigherAssy; //단위당 구성 수량
    public string sourceMaintRecoverability; //근원정비 복구성 부호 

    public Catalog_Item(string _subSystemCode, string _subsubSystemCode, string _assyCode, string _id, string _figureNumber, string _itemSeqNumberValue, string _partNumberValue, string _emp,
        string _manufacturerCodeValue, string _descrForPart, string _quantityPerNextHigherAssy, string _sourceMaintRecoverability)
    {
        this.subSystemCode = _subSystemCode;
        this.subsubSystemCode = _subsubSystemCode;
        this.assyCode = _assyCode;
        this.id = _id;
        this.figureNumber = _figureNumber;
        this.itemSeqNumberValue = _itemSeqNumberValue;
        this.partNumberValue = _partNumberValue;
        this.emp = _emp;
        this.manufacturerCodeValue = _manufacturerCodeValue;
        this.descrForPart = _descrForPart;
        this.quantityPerNextHigherAssy = _quantityPerNextHigherAssy;
        this.sourceMaintRecoverability = _sourceMaintRecoverability;
    }
}

#region DIAGRAM
[System.Serializable]
public class DMItem_DiagramItemSetting //isolationMainProcedure/isolationStep or isolationMainProcedure/isolationProcedureEnd
{
    public bool endTrigger = false; //마지막 결말이 나올때 
    public string id; //isolationStep or isolationProcedureEnd 
    public string listOfChice_id;
    public string yes_answer_id; //id 값이 들어간다.
    public string no_answer_id; //id 값이 들어간다.
    public string quest_str; //isolationStepQuestion (결함이 해소되었는가?)
    public List<DMItem_DiagramItemInfo> actionItems; //action/randomList/listItem/para (고장원인 or 조치사항) - 해당 답 DM

    //isolationMainProcedure/isolationStep
    public DMItem_DiagramItemSetting(bool _endTrigger, string _id, string _listOfChoice, string _yes_answer_id, string _no_answer_id, string _quest_str, List<DMItem_DiagramItemInfo> _actionItems)
    {
        this.endTrigger = _endTrigger;
        this.id = _id; //필수 
        this.listOfChice_id = _listOfChoice;
        this.yes_answer_id = _yes_answer_id;
        this.no_answer_id = _no_answer_id;
        this.quest_str = _quest_str;
        this.actionItems = _actionItems; //endItem 포함
    }
}

public enum DiagramItemList
{
    고장원인,
    조치사항,
    결과
}

[System.Serializable]
public class DMItem_DiagramItemInfo
{
    public DiagramItemList itemList; // 고장원인, 조치사항, 결과

    public string para_txt; //고장원인 or 조치사항 

    public Object linkDM; //링크 DM이 있는 경우에만 할당 //각 아이템 텍스트마다 연결되는 DM이 존재한다. 

    public DMItem_DiagramItemInfo(DiagramItemList _itemList, string _para_txt, Object _linkDM)
    {
        this.itemList = _itemList;
        this.para_txt = _para_txt;
        this.linkDM = _linkDM;
    }
}
#endregion

#endregion

#region UI BUTTON 
//BUTTON SETTING
[System.Serializable]
public class ItemTree
{
    public int menuNum;//메뉴 번호 
    public string title;
    //기본은 0 depth
    public bool isSub_1depth; //1 depth
    public bool isSub_2depth; //2 depth
    public bool isSub_3depth; //3 depth

    public bool hasChild;

    public ItemTree(int _menuNum, string index, bool depth_1, bool depth_2, bool depth_3, bool hasChild)
    {
        this.menuNum = _menuNum;
        this.title = index;
        this.isSub_1depth = depth_1;
        this.isSub_2depth = depth_2;
        this.isSub_3depth = depth_3;
        this.hasChild = hasChild; //자식이 있는지 없는지
    }
}
#endregion

public class XMLManager : MonoBehaviour
{
    [Header(" [ TREE ] ")]
    [SerializeField] public LinkToDmData[] _linkToDmData_Unit;//Pm에 할당되는 DM LIST 정리 
    [HideInInspector] public List<ItemTree> itemTrees_list_Unit = new List<ItemTree>();
    [SerializeField] public List<DmContent> dmContents_Unit = new List<DmContent>();

    [SerializeField] public DmContent dmAbbre_Unit;

    List<string> pm_content_fileName; //PM 마다 필요한 XML 파일명 

    [Header(" [ XML RESOURCE ] ")]
    [SerializeField] string root_pm_option_Unit;
    [SerializeField] string root_dm_sub_Unit;

    //장절항
    //Object[] txtAsset_pm_Field;
    Object[] txtAsset_pm_Unit;
    //DM
    //Object[] txtAsset_dm_Field;
    Object[] txtAsset_dm_Unit;

    //약어 
    [SerializeField] Object txtAsset_abbre_Unit;

    [SerializeField] UIManager UM;

    [HideInInspector] public int menuNum = 0;


    string title_Han = "가 나 다 라 마 바 사 아 자 차 카 타 파 하";
    string[] title_Han_words;

    private void Awake()
    {
        title_Han_words = title_Han.Split(' ');
    }


    //부대정비( 순서 -> 메인PM 데이터 추출 -> DM 데이터 추출 -> DM 포문으로 PM과 연동 )
    public void XMLData_Upload_PM_Unit()
    {
        //Debug.Log("GET UNIT XML RESORUCE");
        //장절항 데이터 추출 
        txtAsset_pm_Unit = Resources.LoadAll(root_pm_option_Unit, typeof(TextAsset));

        //DM 데이터 추출 
        txtAsset_dm_Unit = Resources.LoadAll(root_dm_sub_Unit, typeof(TextAsset));

        string rootName = ""; //PM 파일을 구분하기위한 값 

        string[] split_0_depth = new string[txtAsset_pm_Unit.Length];
        string[] split_1_depth = new string[txtAsset_pm_Unit.Length];

        _linkToDmData_Unit = new LinkToDmData[txtAsset_pm_Unit.Length];

        //pm 1개씩 체크 
        for (int i = 0; i < txtAsset_pm_Unit.Length; i++)
        {
            XmlDocument xmlDoc = new XmlDocument();
            TextAsset empTextAsset = (TextAsset)txtAsset_pm_Unit[i];
            xmlDoc.LoadXml(empTextAsset.text);

            _linkToDmData_Unit[i].title = xmlDoc.GetElementsByTagName("pmTitle")[0].InnerText;

            _linkToDmData_Unit[i].pm_data = txtAsset_pm_Unit[i];
            _linkToDmData_Unit[i].dm_rootName = new List<Object>();

            //파일이름 추출 메인 
            XmlElement root_depth_0 = xmlDoc.SelectSingleNode("pm/identAndStatusSection/pmAddress/pmIdent/pmCode") as XmlElement;
            rootName = root_depth_0.GetAttribute("pmNumber");
            //rootName_3_depth[i] = root_depth_0.GetAttribute("pmVolume");

            split_0_depth[i] = rootName.Substring(0, 1);
            split_1_depth[i] = rootName.Substring(2, 1);

            //PM Content 영역 
            XmlNodeList pm_content = xmlDoc.SelectNodes("pm/content/pmEntry/dmRef/dmRefIdent");

            //초기화 및 할당 
            pm_content_fileName = new List<string>(new string[pm_content.Count]);

            for (int z = 0; z < pm_content.Count; z++)
            {
                XmlElement pm_linkCode_child_0 = pm_content[z].SelectSingleNode("dmCode") as XmlElement;
                XmlElement pm_linkCode_child_1 = pm_content[z].SelectSingleNode("issueInfo") as XmlElement;

                pm_content_fileName[z] = "DMC-" + pm_linkCode_child_0.GetAttribute("modelIdentCode") + "-" + pm_linkCode_child_0.GetAttribute("systemDiffCode")
                        + "-" + pm_linkCode_child_0.GetAttribute("systemCode") + "-" + pm_linkCode_child_0.GetAttribute("subSystemCode") + pm_linkCode_child_0.GetAttribute("subSubSystemCode")
                        + "-" + pm_linkCode_child_0.GetAttribute("assyCode") + "-" + pm_linkCode_child_0.GetAttribute("disassyCode") + pm_linkCode_child_0.GetAttribute("disassyCodeVariant")
                        + "-" + pm_linkCode_child_0.GetAttribute("infoCode") + pm_linkCode_child_0.GetAttribute("infoCodeVariant") + "-" + pm_linkCode_child_0.GetAttribute("itemLocationCode")
                        + "_" + pm_linkCode_child_1.GetAttribute("issueNumber") + "-" + pm_linkCode_child_1.GetAttribute("inWork") + "_KO-KR";

                //Debug.Log("pm : " + pm_content_fileName[z]);

                for (int k = 0; k < txtAsset_dm_Unit.Length; k++)
                {
                    if (pm_content_fileName[z] == txtAsset_dm_Unit[k].name)
                    {
                        _linkToDmData_Unit[i].dm_rootName.Add(txtAsset_dm_Unit[k]);
                    }
                }
            }

            XmlNodeList subBtn_node = xmlDoc.SelectNodes("pm/content/pmEntry/dmRef/dmRefAddressItems/dmTitle");
            XmlElement subBtn_node_elements_0;
            XmlElement subBtn_node_elements_1;

            //첫번째는 비교 불가 
            if (i == 0)
            {
                menuNum = 0; //첫번쨰 메뉴 

                //0 뎁스에 자식이 있음
                itemTrees_list_Unit.Add(new ItemTree(menuNum, _linkToDmData_Unit[i].title, false, false, false, true));
                //바로 아래 컨텐츠 자식이 있는 경우 
                for (int j = 0; j < subBtn_node.Count; j++)
                {
                    subBtn_node_elements_0 = subBtn_node[j].SelectSingleNode("techName") as XmlElement;
                    subBtn_node_elements_1 = subBtn_node[j].SelectSingleNode("infoName") as XmlElement;

                    //1 depth
                    if (subBtn_node_elements_0 == null)
                    {
                        itemTrees_list_Unit.Add(new ItemTree(menuNum, subBtn_node_elements_1.InnerText, true, false, false, false));
                    }
                    else if (subBtn_node_elements_1 == null)
                    {
                        itemTrees_list_Unit.Add(new ItemTree(menuNum, subBtn_node_elements_0.InnerText, true, false, false, false));
                    }
                    else
                    {
                        itemTrees_list_Unit.Add(new ItemTree(menuNum, subBtn_node_elements_0.InnerText + " " +
                            subBtn_node_elements_1.InnerText, true, false, false, false));
                    }
                }

                continue;
            }
            else
            {
                if (split_0_depth[i] != split_0_depth[i - 1])
                {
                    menuNum++;
                }
            }

            //"X0X" 항목안에 리스트 (이전 버튼과 같은 항목인 경우) - 2,3 depth
            if (split_0_depth[i] == split_0_depth[i - 1])
            {
                if (subBtn_node != null)
                {
                    //2_depth로 들어가는 경우 
                    if (split_1_depth[i] != split_1_depth[i - 1])
                    {
                        //2 depth
                        itemTrees_list_Unit.Add(new ItemTree(menuNum, _linkToDmData_Unit[i].title, true, true, false, true));
                        for (int j = 0; j < subBtn_node.Count; j++)
                        {
                            subBtn_node_elements_0 = subBtn_node[j].SelectSingleNode("techName") as XmlElement;
                            subBtn_node_elements_1 = subBtn_node[j].SelectSingleNode("infoName") as XmlElement;

                            //3 depth
                            if (subBtn_node_elements_0 == null)
                            {
                                itemTrees_list_Unit.Add(new ItemTree(menuNum, subBtn_node_elements_1.InnerText, true, true, true, false));
                            }
                            else if (subBtn_node_elements_1 == null)
                            {
                                itemTrees_list_Unit.Add(new ItemTree(menuNum, subBtn_node_elements_0.InnerText, true, true, true, false));
                            }
                            else
                            {
                                itemTrees_list_Unit.Add(new ItemTree(menuNum, subBtn_node_elements_0.InnerText + " " +
                                    subBtn_node_elements_1.InnerText, true, true, true, false));
                            }
                        }
                    }
                    //1_depth에서 끝나는 경우 
                    else
                    {
                        //1 depth
                        itemTrees_list_Unit.Add(new ItemTree(menuNum, _linkToDmData_Unit[i].title, true, false, false, true));
                        for (int j = 0; j < subBtn_node.Count; j++)
                        {
                            subBtn_node_elements_0 = subBtn_node[j].SelectSingleNode("techName") as XmlElement;
                            subBtn_node_elements_1 = subBtn_node[j].SelectSingleNode("infoName") as XmlElement;

                            //2 depth
                            if (subBtn_node_elements_0 == null)
                            {
                                itemTrees_list_Unit.Add(new ItemTree(menuNum, subBtn_node_elements_1.InnerText, true, true, false, false));
                            }
                            else if (subBtn_node_elements_1 == null)
                            {
                                itemTrees_list_Unit.Add(new ItemTree(menuNum, subBtn_node_elements_0.InnerText, true, true, false, false));
                            }
                            else
                            {
                                itemTrees_list_Unit.Add(new ItemTree(menuNum, subBtn_node_elements_0.InnerText + " " +
                                    subBtn_node_elements_1.InnerText, true, true, false, false));
                            }
                        }
                    }
                }
                else
                {
                    //1 depth
                    itemTrees_list_Unit.Add(new ItemTree(menuNum, _linkToDmData_Unit[i].title, true, false, false, true));
                }
            }
            //0, 1 depth
            else
            {
                //메인 타이틀 (pm) - 0 depth (10000, 20000, 30000 ...)
                if (rootName.Substring(1, 1) == "0")
                {
                    //0 depth
                    itemTrees_list_Unit.Add(new ItemTree(menuNum, _linkToDmData_Unit[i].title, false, false, false, true));
                    //DM 포함
                    if (subBtn_node != null)
                    {
                        for (int j = 0; j < subBtn_node.Count; j++)
                        {
                            subBtn_node_elements_0 = subBtn_node[j].SelectSingleNode("techName") as XmlElement;
                            subBtn_node_elements_1 = subBtn_node[j].SelectSingleNode("infoName") as XmlElement;

                            //1 depth
                            if (subBtn_node_elements_0 == null)
                            {
                                itemTrees_list_Unit.Add(new ItemTree(menuNum, subBtn_node_elements_1.InnerText, true, false, false, false));
                            }
                            else if (subBtn_node_elements_1 == null)
                            {
                                itemTrees_list_Unit.Add(new ItemTree(menuNum, subBtn_node_elements_0.InnerText, true, false, false, false));
                            }
                            else
                            {
                                itemTrees_list_Unit.Add(new ItemTree(menuNum, subBtn_node_elements_0.InnerText + " " +
                                    subBtn_node_elements_1.InnerText, true, false, false, false));
                            }
                        }
                    }
                }
            }
        }

        DMData_Parcing_Unit();

        DMData_Parcing_Abbre();

        //메뉴 생성
        UM.detailSettingCanvas.itemListSetting.CreateTableList();
    }

    List<DMItem> empList; //DM 데이터 긁어서 가져오는 저장체 / 추출 내용 (텍스트 OR 테이블 순서대로 들어간다)
    DMItem_Table empList_table;

    DMItem_Catalog empList_catalog; //카탈로그 데이터 
    //XmlElement titleRoot; //타이틀 루트
    XmlElement root, sideRoot; // 메인 루트, 사이드 메인 루트 
    string text_sum;

    void DMData_Parcing_Unit()
    {
        //PM 데이터 
        for (int i = 0; i < _linkToDmData_Unit.Length; i++)
        {
            //DM 데이터 
            for (int j = 0; j < _linkToDmData_Unit[i].dm_rootName.Count; j++)
            {
                XmlDocument xmlDoc = new XmlDocument();
                TextAsset txtAsset = (TextAsset)_linkToDmData_Unit[i].dm_rootName[j];
                xmlDoc.LoadXml(txtAsset.text);

                empList = new List<DMItem>();

                // ----------------------------------------------------TITLE----------------------------------------------------
                XmlElement title_root = xmlDoc.SelectSingleNode("dmodule/identAndStatusSection/dmAddress/dmAddressItems") as XmlElement; //기본 타이틀 루트

                if (title_root.SelectSingleNode("dmTitle/infoName") != null)
                {
                    string[] textLine = title_root.SelectSingleNode("dmTitle/infoName").InnerText.Split('\n');
                    text_sum = "";

                    for (int q = 0; q < textLine.Length; q++)
                    {
                        text_sum += textLine[q];
                    }

                    if (text_sum != "")
                    {
                        empList.Add(new DMItem(Data.TEXT, true, text_sum, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.CENTER, TextDefine.TEXTSTYLE.BOLD, 35.0f, new Color(0.0627451f, 0.05882353f, 0.3960785f, 1)));
                    }
                }


                // ----------------------------------------------------CONTENT----------------------------------------------------
                XmlElement default_root = xmlDoc.SelectSingleNode("dmodule/content") as XmlElement; //기본 루트 

                if (default_root.SelectSingleNode("description/levelledPara") != null)
                {
                    root = default_root.SelectSingleNode("description/levelledPara") as XmlElement;
                }
                else if (default_root.SelectSingleNode("description") != null)
                {
                    root = default_root.SelectSingleNode("description") as XmlElement;
                }
                else if (default_root.SelectSingleNode("procedure") != null)
                {
                    root = default_root.SelectSingleNode("procedure") as XmlElement;
                }
                else if (default_root.SelectSingleNode("refs") != null)
                {
                    root = default_root.SelectSingleNode("refs") as XmlElement;
                }
                else if (default_root.SelectSingleNode("illustratedPartsCatalog") != null)
                {
                    root = default_root.SelectSingleNode("illustratedPartsCatalog") as XmlElement;
                }
                else if (default_root.SelectSingleNode("faultIsolation") != null)
                {
                    root = default_root.SelectSingleNode("faultIsolation") as XmlElement;
                }
                else
                {
                    root = null;
                    Debug.Log("Can not find Root Name : " + _linkToDmData_Unit[i].dm_rootName[j].name);
                    //continue;
                }

                //Debug.Log("DATA PARSING : " + _linkToDmData_Unit[i].dm_rootName[j].name);

                //TITLE
                if (root.SelectNodes("title").Count != 0) //기술 제원 
                {
                    empList.Add(new DMItem(Data.SPACING, true, 20.0f));

                    XmlNodeList root_title = root.SelectNodes("title");
                    for (int k = 0; k < root_title.Count; k++)
                    {
                        empList.Add(new DMItem(Data.TEXT, true, (k + 1) + ". " + root_title[k].InnerText, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.BOLD, 34.0f, new Color(0.06666667f, 0.06666667f, 0.06666667f, 1)));
                    }
                }

                //warning
                if (root.SelectNodes("warning").Count != 0)
                {
                    XmlNodeList root_ele_0 = root.SelectNodes("warning");

                    for (int g = 0; g < root_ele_0.Count; g++)
                    {
                        empList.Add(new DMItem(Data.SPACING, true, 20.0f));

                        XmlNodeList root_ele_1 = root_ele_0[g].SelectNodes("warningAndCautionPara");

                        if (root_ele_1 != null)
                        {
                            text_sum = "";
                            for (int q = 0; q < root_ele_1.Count; q++)
                            {
                                string[] textLine = root_ele_1[q].InnerText.Split('\n');

                                for (int z = 0; z < textLine.Length; z++)
                                {
                                    text_sum += textLine[z];
                                    text_sum += ' ';
                                }

                                text_sum += "\n";
                            }

                            if (text_sum != "")
                            {
                                empList.Add(new DMItem(Data.TEXT, true, text_sum, TEXTTYPE.WARNING, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                            }
                        }

                        empList.Add(new DMItem(Data.SPACING, true, 20.0f));
                    }
                }

                //caution
                if (root.SelectNodes("caution").Count != 0)
                {
                    XmlNodeList root_ele_0 = root.SelectNodes("caution");

                    for (int g = 0; g < root_ele_0.Count; g++)
                    {
                        empList.Add(new DMItem(Data.SPACING, true, 20.0f));

                        XmlNodeList root_ele_1 = root_ele_0[g].SelectNodes("warningAndCautionPara");

                        if (root_ele_1 != null)
                        {
                            text_sum = "";
                            for (int q = 0; q < root_ele_1.Count; q++)
                            {
                                string[] textLine = root_ele_1[q].InnerText.Split('\n');

                                for (int z = 0; z < textLine.Length; z++)
                                {
                                    text_sum += textLine[z];
                                    text_sum += ' ';
                                }

                                text_sum += "\n";
                            }

                            if (text_sum != "")
                            {
                                empList.Add(new DMItem(Data.TEXT, true, text_sum, TEXTTYPE.CAUTION, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                            }
                        }

                        empList.Add(new DMItem(Data.SPACING, true, 20.0f));
                    }
                }

                //note
                if (root.SelectNodes("note").Count != 0)
                {
                    XmlNodeList root_ele_0 = root.SelectNodes("note");

                    for (int g = 0; g < root_ele_0.Count; g++)
                    {
                        empList.Add(new DMItem(Data.SPACING, true, 20.0f));

                        XmlNodeList root_ele_1 = root_ele_0[g].SelectNodes("notePara");

                        if (root_ele_1 != null)
                        {
                            text_sum = "";
                            for (int q = 0; q < root_ele_1.Count; q++)
                            {
                                string[] textLine = root_ele_1[q].InnerText.Split('\n');

                                for (int z = 0; z < textLine.Length; z++)
                                {
                                    text_sum += textLine[z];
                                    text_sum += ' ';
                                }

                                text_sum += "\n";
                            }

                            if (text_sum != "")
                            {
                                empList.Add(new DMItem(Data.TEXT, true, text_sum, TEXTTYPE.NOTE, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                            }
                        }

                        empList.Add(new DMItem(Data.SPACING, true, 20.0f));
                    }
                }

                //PARA
                if (root.SelectNodes("para").Count != 0)
                {
                    XmlNodeList root_para = root.SelectNodes("para");

                    for (int k = 0; k < root_para.Count; k++)
                    {
                        //sequentialList
                        if (root_para[k].SelectNodes("sequentialList").Count != 0)
                        {
                            XmlNodeList root_para_sequentialList = root_para[k].SelectNodes("sequentialList");
                            for (int h = 0; h < root_para_sequentialList.Count; h++)
                            {
                                XmlNodeList root_para_sequentialList_listItem = root_para_sequentialList[h].SelectNodes("listItem/para");

                                for (int l = 0; l < root_para_sequentialList_listItem.Count; l++)
                                {
                                    empList.Add(new DMItem(Data.TEXT, true, title_Han_words[l] + ". " + root_para_sequentialList_listItem[l].InnerText, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                    empList.Add(new DMItem(Data.SPACING, true, 20.0f));
                                }
                            }
                        }
                        //없으면 그냥 텍스트 
                        else
                        {
                            string[] textLine = root_para[k].InnerText.Split('\n');
                            text_sum = "";

                            for (int q = 0; q < textLine.Length; q++)
                            {
                                text_sum += textLine[q];
                                text_sum += ' ';
                            }

                            empList.Add(new DMItem(Data.TEXT, true, k + 1 + ") " + text_sum, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                        }
                        empList.Add(new DMItem(Data.SPACING, true, 20.0f));
                    }
                }

                //FIGURE
                if (root.SelectNodes("figure").Count != 0)
                {
                    XmlNodeList root_figure = root.SelectNodes("figure");

                    for (int k = 0; k < root_figure.Count; k++)
                    {
                        empList.Add(new DMItem(Data.SPACING, true, 20.0f));

                        XmlElement figure_title = root_figure[k].SelectSingleNode("title") as XmlElement;
                        XmlElement figure_graphics = root_figure[k].SelectSingleNode("graphic") as XmlElement;

                        string figure_graphics_id = figure_graphics.GetAttribute("id");
                        string figure_graphics_infoEntityIdent = figure_graphics.GetAttribute("infoEntityIdent");

                        DMItem_Figure figureData = new DMItem_Figure(figure_title.InnerText, figure_graphics_id, figure_graphics_infoEntityIdent);

                        empList.Add(new DMItem(Data.FIGURE, true, figureData));

                        empList.Add(new DMItem(Data.SPACING, true, 20.0f));
                    }
                }

                //LEVELLEDPARA
                if (root.SelectNodes("levelledPara").Count != 0)
                {
                    //dmodule/content/description/levelledPara/levelledPara
                    XmlNodeList root_1 = root.SelectNodes("levelledPara");

                    for (int k = 0; k < root_1.Count; k++)
                    {
                        //TITLE
                        if (root_1[k].SelectNodes("title").Count != 0)
                        {
                            empList.Add(new DMItem(Data.SPACING, true, 20.0f));

                            XmlNodeList root_1_title = root_1[k].SelectNodes("title");

                            for (int z = 0; z < root_1_title.Count; z++)
                            {
                                empList.Add(new DMItem(Data.TEXT, true, "1" + "." + (k + 1) + ". " + root_1_title[z].InnerText, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.BOLD, 30.0f, new Color(0.1333333f, 0.1333333f, 0.1333333f, 1))); //222222
                            }
                        }

                        //FIGURE
                        if (root_1[k].SelectNodes("figure").Count != 0)
                        {
                            XmlNodeList root_figure = root_1[k].SelectNodes("figure");

                            for (int o = 0; o < root_figure.Count; o++)
                            {
                                empList.Add(new DMItem(Data.SPACING, true, 20.0f));

                                XmlElement figure_title = root_figure[o].SelectSingleNode("title") as XmlElement;
                                XmlElement figure_graphics = root_figure[o].SelectSingleNode("graphic") as XmlElement;

                                string figure_graphics_id = figure_graphics.GetAttribute("id");
                                string figure_graphics_infoEntityIdent = figure_graphics.GetAttribute("infoEntityIdent");

                                DMItem_Figure figureData = new DMItem_Figure(figure_title.InnerText, figure_graphics_id, figure_graphics_infoEntityIdent);

                                empList.Add(new DMItem(Data.FIGURE, true, figureData));

                                empList.Add(new DMItem(Data.SPACING, true, 20.0f));
                            }
                        }

                        //PARA
                        if (root_1[k].SelectNodes("para").Count != 0)
                        {
                            XmlNodeList root_1_para = root_1[k].SelectNodes("para");

                            for (int z = 0; z < root_1_para.Count; z++)
                            {
                                string[] textLine = root_1_para[z].InnerText.Split('\n');
                                text_sum = "";

                                for (int q = 0; q < textLine.Length; q++)
                                {
                                    text_sum += textLine[q];
                                    text_sum += ' ';
                                }

                                empList.Add(new DMItem(Data.TEXT, true, text_sum, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                            }
                        }

                        //warning
                        if (root_1[k].SelectNodes("warning").Count != 0)
                        {
                            XmlNodeList root_ele_0 = root_1[k].SelectNodes("warning");

                            for (int g = 0; g < root_ele_0.Count; g++)
                            {
                                empList.Add(new DMItem(Data.SPACING, true, 20.0f));

                                XmlNodeList root_ele_1 = root_ele_0[g].SelectNodes("warningAndCautionPara");

                                if (root_ele_1 != null)
                                {
                                    text_sum = "";
                                    for (int q = 0; q < root_ele_1.Count; q++)
                                    {
                                        string[] textLine = root_ele_1[q].InnerText.Split('\n');

                                        for (int z = 0; z < textLine.Length; z++)
                                        {
                                            text_sum += textLine[z];
                                            text_sum += ' ';
                                        }

                                        text_sum += "\n";
                                    }

                                    if (text_sum != "")
                                        empList.Add(new DMItem(Data.TEXT, true, text_sum, TEXTTYPE.WARNING, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                }

                                empList.Add(new DMItem(Data.SPACING, true, 20.0f));
                            }
                        }

                        //caution
                        if (root_1[k].SelectNodes("caution").Count != 0)
                        {
                            XmlNodeList root_ele_0 = root_1[k].SelectNodes("caution");

                            for (int g = 0; g < root_ele_0.Count; g++)
                            {
                                empList.Add(new DMItem(Data.SPACING, true, 20.0f));

                                XmlNodeList root_ele_1 = root_ele_0[g].SelectNodes("warningAndCautionPara");

                                if (root_ele_1 != null)
                                {
                                    text_sum = "";
                                    for (int q = 0; q < root_ele_1.Count; q++)
                                    {
                                        string[] textLine = root_ele_1[q].InnerText.Split('\n');

                                        for (int z = 0; z < textLine.Length; z++)
                                        {
                                            text_sum += textLine[z];
                                            text_sum += ' ';
                                        }

                                        text_sum += "\n";
                                    }

                                    if (text_sum != "")
                                        empList.Add(new DMItem(Data.TEXT, true, text_sum, TEXTTYPE.CAUTION, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                }

                                empList.Add(new DMItem(Data.SPACING, true, 20.0f));
                            }
                        }

                        //note
                        if (root_1[k].SelectNodes("note").Count != 0)
                        {
                            XmlNodeList root_ele_0 = root_1[k].SelectNodes("note");

                            for (int g = 0; g < root_ele_0.Count; g++)
                            {
                                empList.Add(new DMItem(Data.SPACING, true, 20.0f));

                                XmlNodeList root_ele_1 = root_ele_0[g].SelectNodes("notePara");

                                if (root_ele_1 != null)
                                {
                                    text_sum = "";
                                    for (int q = 0; q < root_ele_1.Count; q++)
                                    {
                                        string[] textLine = root_ele_1[q].InnerText.Split('\n');

                                        for (int z = 0; z < textLine.Length; z++)
                                        {
                                            text_sum += textLine[z];
                                            text_sum += ' ';
                                        }

                                        text_sum += "\n";
                                    }

                                    if (text_sum != "")
                                        empList.Add(new DMItem(Data.TEXT, true, text_sum, TEXTTYPE.NOTE, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                }

                                empList.Add(new DMItem(Data.SPACING, true, 20.0f));
                            }
                        }

                        //TABLE 
                        if (root_1[k].SelectSingleNode("table") != null)
                        {
                            empList.Add(new DMItem(Data.SPACING, true, 20.0f));

                            XmlElement root_1_table = root_1[k].SelectSingleNode("table") as XmlElement;
                            CreateTableData(root_1_table);
                        }

                        // 여기서부터 모든 데이터를 묶어야될듯? (Folder Data)

                        //LEVELLEDPARA
                        if (root_1[k].SelectNodes("levelledPara").Count != 0)
                        {
                            //dmodule/content/description/levelledPara/levelledPara/levelledPara
                            XmlNodeList root_3 = root_1[k].SelectNodes("levelledPara");

                            for (int z = 0; z < root_3.Count; z++)
                            {
                                //TITLE
                                // 버튼형식으로 생성 후 밑에 있는 데이터를 제어 가능하게 해야된다. 
                                if (root_3[z].SelectNodes("title").Count != 0)
                                {
                                    empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                    XmlNodeList root_3_title = root_3[z].SelectNodes("title");

                                    for (int g = 0; g < root_3_title.Count; g++)
                                    {
                                        empList.Add(new DMItem(Data.FOLDERCONTENT, false, "1" + "." + (k + 1) + "." + (z + 1) + ". " + root_3_title[g].InnerText, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.MEDIUM, 27.0f, new Color(0.2f, 0.2f, 0.2f, 1))); //3333333
                                    }
                                }

                                //FIGURE
                                if (root_3[z].SelectNodes("figure").Count != 0)
                                {
                                    XmlNodeList root_figure = root_3[z].SelectNodes("figure");

                                    for (int o = 0; o < root_figure.Count; o++)
                                    {
                                        empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                        XmlElement figure_title = root_figure[o].SelectSingleNode("title") as XmlElement;
                                        XmlElement figure_graphics = root_figure[o].SelectSingleNode("graphic") as XmlElement;

                                        string figure_graphics_id = figure_graphics.GetAttribute("id");
                                        string figure_graphics_infoEntityIdent = figure_graphics.GetAttribute("infoEntityIdent");

                                        DMItem_Figure figureData = new DMItem_Figure(figure_title.InnerText, figure_graphics_id, figure_graphics_infoEntityIdent);

                                        empList.Add(new DMItem(Data.FIGURE, false, figureData));

                                        empList.Add(new DMItem(Data.SPACING, false, 20.0f));
                                    }
                                }

                                //warning
                                if (root_3[z].SelectNodes("warning").Count != 0)
                                {
                                    XmlNodeList root_ele_0 = root_3[z].SelectNodes("warning");

                                    for (int g = 0; g < root_ele_0.Count; g++)
                                    {
                                        empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                        XmlNodeList root_ele_1 = root_ele_0[g].SelectNodes("warningAndCautionPara");

                                        if (root_ele_1 != null)
                                        {
                                            text_sum = "";
                                            for (int q = 0; q < root_ele_1.Count; q++)
                                            {
                                                string[] textLine = root_ele_1[q].InnerText.Split('\n');

                                                for (int h = 0; h < textLine.Length; h++)
                                                {
                                                    text_sum += textLine[h];
                                                    text_sum += ' ';
                                                }

                                                text_sum += "\n";
                                            }

                                            if (text_sum != "")
                                                empList.Add(new DMItem(Data.TEXT, false, text_sum, TEXTTYPE.WARNING, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                        }

                                        empList.Add(new DMItem(Data.SPACING, false, 20.0f));
                                    }
                                }

                                //caution
                                if (root_3[z].SelectNodes("caution").Count != 0)
                                {
                                    XmlNodeList root_ele_0 = root_3[z].SelectNodes("caution");

                                    for (int g = 0; g < root_ele_0.Count; g++)
                                    {
                                        empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                        XmlNodeList root_ele_1 = root_ele_0[g].SelectNodes("warningAndCautionPara");

                                        if (root_ele_1 != null)
                                        {
                                            text_sum = "";
                                            for (int q = 0; q < root_ele_1.Count; q++)
                                            {
                                                string[] textLine = root_ele_1[q].InnerText.Split('\n');

                                                for (int h = 0; h < textLine.Length; h++)
                                                {
                                                    text_sum += textLine[h];
                                                    text_sum += ' ';
                                                }

                                                text_sum += "\n";
                                            }

                                            if (text_sum != "")
                                                empList.Add(new DMItem(Data.TEXT, false, text_sum, TEXTTYPE.CAUTION, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                        }

                                        empList.Add(new DMItem(Data.SPACING, false, 20.0f));
                                    }
                                }

                                //note
                                if (root_3[z].SelectNodes("note").Count != 0)
                                {
                                    XmlNodeList root_ele_0 = root_3[z].SelectNodes("note");

                                    for (int g = 0; g < root_ele_0.Count; g++)
                                    {
                                        empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                        XmlNodeList root_ele_1 = root_ele_0[g].SelectNodes("notePara");

                                        if (root_ele_1 != null)
                                        {
                                            text_sum = "";
                                            for (int q = 0; q < root_ele_1.Count; q++)
                                            {
                                                string[] textLine = root_ele_1[q].InnerText.Split('\n');

                                                for (int h = 0; h < textLine.Length; h++)
                                                {
                                                    text_sum += textLine[h];
                                                    text_sum += ' ';
                                                }

                                                text_sum += "\n";
                                            }

                                            if (text_sum != "")
                                                empList.Add(new DMItem(Data.TEXT, false, text_sum, TEXTTYPE.NOTE, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                        }

                                        empList.Add(new DMItem(Data.SPACING, false, 20.0f));
                                    }
                                }

                                //PARA
                                if (root_3[z].SelectNodes("para").Count != 0)
                                {
                                    XmlNodeList root_para = root_3[z].SelectNodes("para");

                                    for (int e = 0; e < root_para.Count; e++)
                                    {
                                        //sequentialList
                                        if (root_para[e].SelectNodes("sequentialList").Count != 0)
                                        {
                                            XmlNodeList root_para_randomList = root_para[e].SelectNodes("sequentialList");
                                            for (int h = 0; h < root_para_randomList.Count; h++)
                                            {
                                                XmlNodeList root_para_randomList_listItem = root_para_randomList[h].SelectNodes("listItem/para");

                                                for (int l = 0; l < root_para_randomList_listItem.Count; l++)
                                                {
                                                    empList.Add(new DMItem(Data.TEXT, false, title_Han_words[l] + ". " + root_para_randomList_listItem[l].InnerText, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                                    empList.Add(new DMItem(Data.SPACING, false, 20.0f));
                                                }
                                            }
                                        }
                                        //없으면 그냥 텍스트 
                                        else
                                        {
                                            string[] textLine = root_para[e].InnerText.Split('\n');
                                            text_sum = "";

                                            for (int q = 0; q < textLine.Length; q++)
                                            {
                                                text_sum += textLine[q];
                                                text_sum += ' ';
                                            }

                                            empList.Add(new DMItem(Data.TEXT, false, e + 1 + ") " + text_sum, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                        }
                                        empList.Add(new DMItem(Data.SPACING, false, 20.0f));
                                    }
                                }

                                //TABLE 
                                if (root_3[z].SelectSingleNode("table") != null)
                                {
                                    empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                    XmlElement root_4 = root_3[z].SelectSingleNode("table") as XmlElement;

                                    CreateTableData(root_4);
                                }

                                //LEVELLEDPARA
                                //dmodule/content/description/levelledPara/levelledPara/levelledPara/levelledPara
                                if (root_3[z].SelectNodes("levelledPara").Count != 0)
                                {
                                    XmlNodeList root_4 = root_3[z].SelectNodes("levelledPara");

                                    for (int g = 0; g < root_4.Count; g++)
                                    {
                                        XmlElement root__title = root_4[g].SelectSingleNode("title") as XmlElement;
                                        XmlNodeList root_warning = root_4[g].SelectNodes("warning");
                                        XmlNodeList root_note = root_4[g].SelectNodes("note");
                                        XmlNodeList root_caution = root_4[g].SelectNodes("caution");
                                        XmlNodeList root_levelledPara = root_4[g].SelectNodes("levelledPara");
                                        XmlElement root_table = root_4[g].SelectSingleNode("table") as XmlElement;
                                        XmlNodeList root_para = root_4[g].SelectNodes("para");

                                        //TITLE
                                        if (root__title != null)
                                        {
                                            empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                            empList.Add(new DMItem(Data.TEXT, false, "1" + "." + (k + 1) + "." + (z + 1) + "." + (g + 1) + ". " + root__title.InnerText, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.MEDIUM, 27.0f, new Color(0.2f, 0.2f, 0.2f, 1)));
                                        }

                                        //FIGURE
                                        if (root_4[g].SelectNodes("figure").Count != 0)
                                        {
                                            XmlNodeList root_figure = root_4[g].SelectNodes("figure");

                                            for (int o = 0; o < root_figure.Count; o++)
                                            {
                                                empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                                XmlElement figure_title = root_figure[o].SelectSingleNode("title") as XmlElement;
                                                XmlElement figure_graphics = root_figure[o].SelectSingleNode("graphic") as XmlElement;

                                                string figure_graphics_id = figure_graphics.GetAttribute("id");
                                                string figure_graphics_infoEntityIdent = figure_graphics.GetAttribute("infoEntityIdent");

                                                DMItem_Figure figureData = new DMItem_Figure(figure_title.InnerText, figure_graphics_id, figure_graphics_infoEntityIdent);

                                                empList.Add(new DMItem(Data.FIGURE, false, figureData));

                                                empList.Add(new DMItem(Data.SPACING, false, 20.0f));
                                            }
                                        }

                                        //TABLE 
                                        if (root_table != null)
                                        {
                                            empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                            CreateTableData(root_table);
                                        }

                                        //warning
                                        if (root_warning != null)
                                        {
                                            for (int a = 0; a < root_warning.Count; a++)
                                            {
                                                empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                                XmlNodeList root_ele = root_warning[a].SelectNodes("warningAndCautionPara");

                                                if (root_ele != null)
                                                {
                                                    text_sum = "";
                                                    for (int q = 0; q < root_ele.Count; q++)
                                                    {
                                                        string[] textLine = root_ele[q].InnerText.Split('\n');

                                                        for (int s = 0; s < textLine.Length; s++)
                                                        {
                                                            text_sum += textLine[s];
                                                            text_sum += ' ';
                                                        }

                                                        text_sum += "\n";
                                                    }

                                                    if (text_sum != "")
                                                        empList.Add(new DMItem(Data.TEXT, false, text_sum, TEXTTYPE.WARNING, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                                }

                                                empList.Add(new DMItem(Data.SPACING, false, 20.0f));
                                            }
                                        }

                                        //caution
                                        if (root_caution != null)
                                        {
                                            for (int a = 0; a < root_caution.Count; a++)
                                            {
                                                empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                                XmlNodeList root_ele = root_caution[a].SelectNodes("warningAndCautionPara");

                                                if (root_ele != null)
                                                {
                                                    text_sum = "";
                                                    for (int q = 0; q < root_ele.Count; q++)
                                                    {
                                                        string[] textLine = root_ele[q].InnerText.Split('\n');

                                                        for (int s = 0; s < textLine.Length; s++)
                                                        {
                                                            text_sum += textLine[s];
                                                            text_sum += ' ';
                                                        }

                                                        text_sum += "\n";
                                                    }

                                                    if (text_sum != "")
                                                        empList.Add(new DMItem(Data.TEXT, false, text_sum, TEXTTYPE.CAUTION, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                                }

                                                empList.Add(new DMItem(Data.SPACING, false, 20.0f));
                                            }
                                        }

                                        //note
                                        if (root_note != null)
                                        {
                                            for (int a = 0; a < root_note.Count; a++)
                                            {
                                                empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                                XmlNodeList root_ele = root_note[a].SelectNodes("notePara");

                                                if (root_ele != null)
                                                {
                                                    text_sum = "";
                                                    for (int q = 0; q < root_ele.Count; q++)
                                                    {
                                                        string[] textLine = root_ele[q].InnerText.Split('\n');

                                                        for (int s = 0; s < textLine.Length; s++)
                                                        {
                                                            text_sum += textLine[s];
                                                            text_sum += ' ';
                                                        }

                                                        text_sum += "\n";
                                                    }

                                                    if (text_sum != "")
                                                        empList.Add(new DMItem(Data.TEXT, false, text_sum, TEXTTYPE.NOTE, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                                }

                                                empList.Add(new DMItem(Data.SPACING, false, 20.0f));
                                            }
                                        }

                                        //para
                                        if (root_para != null)
                                        {
                                            for (int h = 0; h < root_para.Count; h++)
                                            {
                                                string[] textLine = root_para[h].InnerText.Split('\n');
                                                text_sum = "";

                                                for (int q = 0; q < textLine.Length; q++)
                                                {
                                                    text_sum += textLine[q];
                                                    text_sum += ' ';
                                                }

                                                empList.Add(new DMItem(Data.TEXT, false, text_sum, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                            }
                                        }

                                        //levelledPara
                                        if (root_levelledPara != null)
                                        {
                                            for (int l = 0; l < root_levelledPara.Count; l++)
                                            {
                                                XmlElement root_levelledPara_title = root_levelledPara[l].SelectSingleNode("title") as XmlElement;
                                                XmlElement root_levelledPara_table = root_levelledPara[l].SelectSingleNode("table") as XmlElement;
                                                XmlNodeList root_levelledPara_para = root_levelledPara[l].SelectNodes("para");

                                                //title
                                                if (root_levelledPara_title != null)
                                                {
                                                    empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                                    empList.Add(new DMItem(Data.TEXT, false, "1" + "." + (k + 1) + "." + (z + 1) + "." + (g + 1) + "." + (l + 1) + ". " +
                                                        root_levelledPara_title.InnerText, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.MEDIUM, 24.0f, new Color(0.2f, 0.2f, 0.2f, 1)));
                                                }

                                                //FIGURE
                                                if (root_levelledPara[l].SelectNodes("figure").Count != 0)
                                                {
                                                    XmlNodeList root_figure = root_levelledPara[l].SelectNodes("figure");

                                                    for (int o = 0; o < root_figure.Count; o++)
                                                    {
                                                        empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                                        XmlElement figure_title = root_figure[o].SelectSingleNode("title") as XmlElement;
                                                        XmlElement figure_graphics = root_figure[o].SelectSingleNode("graphic") as XmlElement;

                                                        string figure_graphics_id = figure_graphics.GetAttribute("id");
                                                        string figure_graphics_infoEntityIdent = figure_graphics.GetAttribute("infoEntityIdent");

                                                        DMItem_Figure figureData = new DMItem_Figure(figure_title.InnerText, figure_graphics_id, figure_graphics_infoEntityIdent);

                                                        empList.Add(new DMItem(Data.FIGURE, false, figureData));

                                                        empList.Add(new DMItem(Data.SPACING, false, 20.0f));
                                                    }
                                                }

                                                //table
                                                if (root_levelledPara_table != null)
                                                {
                                                    empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                                    CreateTableData(root_levelledPara_table as XmlElement);
                                                }

                                                //para
                                                if (root_levelledPara_para != null)
                                                {
                                                    for (int h = 0; h < root_levelledPara_para.Count; h++)
                                                    {
                                                        string[] textLine = root_levelledPara_para[h].InnerText.Split('\n');
                                                        text_sum = "";

                                                        for (int q = 0; q < textLine.Length; q++)
                                                        {
                                                            text_sum += textLine[q];
                                                            text_sum += ' ';
                                                        }

                                                        empList.Add(new DMItem(Data.TEXT, false, text_sum, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                                    }
                                                }

                                                //warning
                                                if (root_levelledPara[l].SelectNodes("warning").Count != 0)
                                                {
                                                    XmlNodeList root_ele_0 = root_levelledPara[l].SelectNodes("warning");

                                                    for (int a = 0; a < root_ele_0.Count; a++)
                                                    {
                                                        empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                                        XmlNodeList root_ele_1 = root_ele_0[a].SelectNodes("warningAndCautionPara");

                                                        if (root_ele_1 != null)
                                                        {
                                                            text_sum = "";
                                                            for (int q = 0; q < root_ele_1.Count; q++)
                                                            {
                                                                string[] textLine = root_ele_1[q].InnerText.Split('\n');

                                                                for (int s = 0; s < textLine.Length; s++)
                                                                {
                                                                    text_sum += textLine[s];
                                                                    text_sum += ' ';
                                                                }

                                                                text_sum += "\n";
                                                            }

                                                            if (text_sum != "")
                                                                empList.Add(new DMItem(Data.TEXT, false, text_sum, TEXTTYPE.WARNING, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                                        }

                                                        empList.Add(new DMItem(Data.SPACING, false, 20.0f));
                                                    }
                                                }

                                                //caution
                                                if (root_levelledPara[l].SelectNodes("caution").Count != 0)
                                                {
                                                    XmlNodeList root_ele_0 = root_levelledPara[l].SelectNodes("caution");

                                                    for (int a = 0; a < root_ele_0.Count; a++)
                                                    {
                                                        empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                                        XmlNodeList root_ele_1 = root_ele_0[a].SelectNodes("warningAndCautionPara");

                                                        if (root_ele_1 != null)
                                                        {
                                                            text_sum = "";
                                                            for (int q = 0; q < root_ele_1.Count; q++)
                                                            {
                                                                string[] textLine = root_ele_1[q].InnerText.Split('\n');

                                                                for (int s = 0; s < textLine.Length; s++)
                                                                {
                                                                    text_sum += textLine[s];
                                                                    text_sum += ' ';
                                                                }

                                                                text_sum += "\n";
                                                            }

                                                            if (text_sum != "")
                                                                empList.Add(new DMItem(Data.TEXT, false, text_sum, TEXTTYPE.CAUTION, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                                        }

                                                        empList.Add(new DMItem(Data.SPACING, false, 20.0f));
                                                    }
                                                }

                                                //note
                                                if (root_levelledPara[l].SelectNodes("note").Count != 0)
                                                {
                                                    XmlNodeList root_ele_0 = root_levelledPara[l].SelectNodes("note");

                                                    for (int a = 0; a < root_ele_0.Count; a++)
                                                    {
                                                        empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                                        XmlNodeList root_ele_1 = root_ele_0[a].SelectNodes("notePara");

                                                        if (root_ele_1 != null)
                                                        {
                                                            text_sum = "";
                                                            for (int q = 0; q < root_ele_1.Count; q++)
                                                            {
                                                                string[] textLine = root_ele_1[q].InnerText.Split('\n');

                                                                for (int s = 0; s < textLine.Length; s++)
                                                                {
                                                                    text_sum += textLine[s];
                                                                    text_sum += ' ';
                                                                }

                                                                text_sum += "\n";
                                                            }

                                                            if (text_sum != "")
                                                                empList.Add(new DMItem(Data.TEXT, false, text_sum, TEXTTYPE.NOTE, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                                        }

                                                        empList.Add(new DMItem(Data.SPACING, false, 20.0f));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //PROCEDURALSTEP
                if (root.SelectNodes("proceduralStep") != null)
                {
                    XmlNodeList root_1 = root.SelectNodes("proceduralStep");

                    for (int k = 0; k < root_1.Count; k++)
                    {
                        //warning
                        if (root_1[k].SelectNodes("warning").Count != 0)
                        {
                            XmlNodeList root_ele_0 = root_1[k].SelectNodes("warning");

                            for (int a = 0; a < root_ele_0.Count; a++)
                            {
                                empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                XmlNodeList root_ele_1 = root_ele_0[a].SelectNodes("warningAndCautionPara");

                                if (root_ele_1 != null)
                                {
                                    text_sum = "";
                                    for (int q = 0; q < root_ele_1.Count; q++)
                                    {
                                        string[] textLine = root_ele_1[q].InnerText.Split('\n');

                                        for (int s = 0; s < textLine.Length; s++)
                                        {
                                            text_sum += textLine[s];
                                            text_sum += ' ';
                                        }

                                        text_sum += "\n";
                                    }

                                    if (text_sum != "")
                                        empList.Add(new DMItem(Data.TEXT, false, text_sum, TEXTTYPE.WARNING, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                }

                                empList.Add(new DMItem(Data.SPACING, false, 20.0f));
                            }
                        }

                        //caution
                        if (root_1[k].SelectNodes("caution").Count != 0)
                        {
                            XmlNodeList root_ele_0 = root_1[k].SelectNodes("caution");

                            for (int a = 0; a < root_ele_0.Count; a++)
                            {
                                empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                XmlNodeList root_ele_1 = root_ele_0[a].SelectNodes("warningAndCautionPara");

                                if (root_ele_1 != null)
                                {
                                    text_sum = "";
                                    for (int q = 0; q < root_ele_1.Count; q++)
                                    {
                                        string[] textLine = root_ele_1[q].InnerText.Split('\n');

                                        for (int s = 0; s < textLine.Length; s++)
                                        {
                                            text_sum += textLine[s];
                                            text_sum += ' ';
                                        }

                                        text_sum += "\n";
                                    }

                                    if (text_sum != "")
                                        empList.Add(new DMItem(Data.TEXT, false, text_sum, TEXTTYPE.CAUTION, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                }

                                empList.Add(new DMItem(Data.SPACING, false, 20.0f));
                            }
                        }

                        //note
                        if (root_1[k].SelectNodes("note").Count != 0)
                        {
                            XmlNodeList root_ele_0 = root_1[k].SelectNodes("note");

                            for (int a = 0; a < root_ele_0.Count; a++)
                            {
                                empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                XmlNodeList root_ele_1 = root_ele_0[a].SelectNodes("notePara");

                                if (root_ele_1 != null)
                                {
                                    text_sum = "";
                                    for (int q = 0; q < root_ele_1.Count; q++)
                                    {
                                        string[] textLine = root_ele_1[q].InnerText.Split('\n');

                                        for (int s = 0; s < textLine.Length; s++)
                                        {
                                            text_sum += textLine[s];
                                            text_sum += ' ';
                                        }

                                        text_sum += "\n";
                                    }

                                    if (text_sum != "")
                                        empList.Add(new DMItem(Data.TEXT, false, text_sum, TEXTTYPE.NOTE, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                }

                                empList.Add(new DMItem(Data.SPACING, false, 20.0f));
                            }
                        }

                        //PARA
                        if (root_1[k].SelectNodes("para").Count != 0)
                        {
                            XmlNodeList root_1_para = root_1[k].SelectNodes("para");

                            for (int z = 0; z < root_1_para.Count; z++)
                            {
                                string[] textLine = root_1_para[z].InnerText.Split('\n');
                                text_sum = "";

                                for (int q = 0; q < textLine.Length; q++)
                                {
                                    text_sum += textLine[q];
                                    text_sum += ' ';
                                }

                                empList.Add(new DMItem(Data.TEXT, false, text_sum, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                            }
                        }
                    }
                }

                //DMREF
                if (root.SelectNodes("dmRef").Count != 0)
                {
                    XmlNodeList root_1 = root.SelectNodes("dmRef");

                    for (int k = 0; k < root_1.Count; k++)
                    {
                        //DMREFLDENT(연결되는 DM)


                        //DMREFADDRESSITEMS
                        if (root_1[k].SelectNodes("dmRefAddressItems").Count != 0)
                        {
                            XmlElement root_2 = root_1[k].SelectSingleNode("dmRefAddressItems/dmTitle") as XmlElement;

                            for (int z = 0; z < root_2.SelectNodes("techName").Count; z++)
                            {
                                empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                empList.Add(new DMItem(Data.TEXT, false, root_2.SelectNodes("techName")[z].InnerText, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.MEDIUM, 27.0f, new Color(0.2f, 0.2f, 0.2f, 1)));
                                empList.Add(new DMItem(Data.TEXT, false, root_2.SelectNodes("infoName")[z].InnerText, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                            }
                        }
                    }
                }

                //TABLE
                if (root.SelectNodes("table").Count != 0)
                {
                    for (int k = 0; k < root.SelectNodes("table").Count; k++)
                    {
                        empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                        CreateTableData(root.SelectNodes("table")[k] as XmlElement);
                    }
                }

                // -- root : content/procedure -- 
                //PRELIMINARYRQMTS
                if (root.SelectSingleNode("preliminaryRqmts") != null)
                {
                    XmlNodeList root_1 = root.SelectSingleNode("preliminaryRqmts").SelectNodes("reqCondGroup/reqCondNoRef/reqCond");

                    //reqCondGroup
                    if (root_1 != null)
                    {
                        empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                        for (int k = 0; k < root_1.Count; k++)
                        {
                            empList.Add(new DMItem(Data.TEXT, false, (k + 1) + ". " + root_1[k].InnerText, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                        }
                    }

                    //reqPersons
                    if (root.SelectSingleNode("preliminaryRqmts/reqPersons") != null)
                    {
                        //person
                        XmlElement root_2 = root.SelectSingleNode("preliminaryRqmts/reqPersons/person") as XmlElement;

                        //man
                        empList.Add(new DMItem(Data.SPACING, false, 20.0f));
                        empList.Add(new DMItem(Data.TEXT, false, "소요인원 : " + root_2.GetAttribute("man"), TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.MEDIUM, 27.0f, new Color(0.2f, 0.2f, 0.2f, 1)));

                        //woman ? 추가되나?

                        //personnel
                        empList.Add(new DMItem(Data.SPACING, false, 20.0f));
                        XmlNodeList root_3 = root.SelectNodes("preliminaryRqmts/reqPersons/personnel/personCategory");
                        for (int k = 0; k < root_3.Count; k++)
                        {
                            XmlElement root_3_ele = root_3[k] as XmlElement;
                            empList.Add(new DMItem(Data.TEXT, false, " • " + root_3_ele.GetAttribute("personCategoryCode"), TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                        }
                    }

                    //reqSupportEquips
                    if (root.SelectSingleNode("preliminaryRqmts/reqSupportEquips") != null)
                    {
                        //TOOL TABLE 데이터 할당 
                        //supportEquipDescr
                        DMItem_UseToolTable empTable;
                        List<DMItem_UseToolTable_Item> empTableItem = new List<DMItem_UseToolTable_Item>();
                        List<float> empfloat = new List<float>();
                        float sum = 0;

                        empfloat.Add(0.3f);
                        empfloat.Add(1.0f);
                        empfloat.Add(0.4f);
                        empfloat.Add(0.2f);

                        //테이블 칼럼마다 길이 비율 추출
                        for (int k = 0; k < empfloat.Count; k++)
                        {
                            sum += empfloat[k];
                        }

                        //Width * 500 / Sum
                        for (int k = 0; k < empfloat.Count; k++)
                        {
                            empfloat[k] = empfloat[k] * caltallog_width / sum;
                        }

                        XmlNodeList root_2 = root.SelectSingleNode("preliminaryRqmts/reqSupportEquips").SelectNodes("supportEquipDescrGroup/supportEquipDescr");
                        if (root_2 != null)
                        {
                            //header 추가 
                            empTableItem.Add(new DMItem_UseToolTable_Item(TextDefine.TEXTALIGN.CENTER, empfloat[0], "구 분"));
                            empTableItem.Add(new DMItem_UseToolTable_Item(TextDefine.TEXTALIGN.CENTER, empfloat[1], "품 명"));
                            empTableItem.Add(new DMItem_UseToolTable_Item(TextDefine.TEXTALIGN.CENTER, empfloat[2], "품 번"));
                            empTableItem.Add(new DMItem_UseToolTable_Item(TextDefine.TEXTALIGN.CENTER, empfloat[3], "수 량"));

                            for (int k = 0; k < root_2.Count; k++)
                            {
                                //DMItem_SubTable empList_subtable;
                                //List<string> text_data = new List<string>(); //갯수는 지정된다. 

                                XmlElement partName_root = root_2[k] as XmlElement;
                                XmlElement name = root_2[k].SelectSingleNode("name") as XmlElement;
                                XmlElement partNum = root_2[k].SelectSingleNode("identNumber/partAndSerialNumber/partNumber") as XmlElement;
                                XmlElement count_root = root_2[k].SelectSingleNode("reqQuantity ") as XmlElement;

                                empTableItem.Add(new DMItem_UseToolTable_Item(TextDefine.TEXTALIGN.CENTER, empfloat[0], partName_root.GetAttribute("materialUsage"))); //지원장비인지 일반공구인지 판단 
                                empTableItem.Add(new DMItem_UseToolTable_Item(TextDefine.TEXTALIGN.CENTER, empfloat[1], name.InnerText));
                                empTableItem.Add(new DMItem_UseToolTable_Item(TextDefine.TEXTALIGN.CENTER, empfloat[2], partNum.InnerText));
                                empTableItem.Add(new DMItem_UseToolTable_Item(TextDefine.TEXTALIGN.CENTER, empfloat[3], count_root.InnerText));
                            }

                            empTable = new DMItem_UseToolTable(empTableItem.ToArray(), empfloat.Count, root_2.Count + 1); //헤더 추가 
                        }
                    }

                    //reqSafety
                    if (root.SelectSingleNode("preliminaryRqmts/reqSafety") != null)
                    {
                        XmlNodeList root_note = root.SelectSingleNode("preliminaryRqmts/reqSafety").SelectNodes("safetyRqmts/note");
                        XmlNodeList root_warning = root.SelectSingleNode("preliminaryRqmts/reqSafety").SelectNodes("safetyRqmts/warning");
                        XmlNodeList root_caution = root.SelectSingleNode("preliminaryRqmts/reqSafety").SelectNodes("safetyRqmts/caution");

                        //warning
                        if (root_warning != null)
                        {
                            for (int a = 0; a < root_warning.Count; a++)
                            {
                                empList.Add(new DMItem(Data.SPACING, true, 20.0f));

                                XmlNodeList root_ele = root_warning[a].SelectNodes("warningAndCautionPara");

                                if (root_ele != null)
                                {
                                    text_sum = "";
                                    for (int q = 0; q < root_ele.Count; q++)
                                    {
                                        string[] textLine = root_ele[q].InnerText.Split('\n');

                                        for (int s = 0; s < textLine.Length; s++)
                                        {
                                            text_sum += textLine[s];
                                            text_sum += ' ';
                                        }

                                        text_sum += "\n";
                                    }

                                    if (text_sum != "")
                                        empList.Add(new DMItem(Data.TEXT, true, text_sum, TEXTTYPE.WARNING, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                }

                                empList.Add(new DMItem(Data.SPACING, true, 20.0f));
                            }
                        }

                        //caution
                        if (root_caution != null)
                        {
                            for (int a = 0; a < root_caution.Count; a++)
                            {
                                empList.Add(new DMItem(Data.SPACING, true, 20.0f));

                                XmlNodeList root_ele = root_caution[a].SelectNodes("warningAndCautionPara");

                                if (root_ele != null)
                                {
                                    text_sum = "";
                                    for (int q = 0; q < root_ele.Count; q++)
                                    {
                                        string[] textLine = root_ele[q].InnerText.Split('\n');

                                        for (int s = 0; s < textLine.Length; s++)
                                        {
                                            text_sum += textLine[s];
                                            text_sum += ' ';
                                        }

                                        text_sum += "\n";
                                    }

                                    if (text_sum != "")
                                        empList.Add(new DMItem(Data.TEXT, true, text_sum, TEXTTYPE.CAUTION, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                }

                                empList.Add(new DMItem(Data.SPACING, true, 20.0f));
                            }
                        }

                        //note
                        if (root_note != null)
                        {
                            for (int a = 0; a < root_note.Count; a++)
                            {
                                empList.Add(new DMItem(Data.SPACING, true, 20.0f));

                                XmlNodeList root_ele = root_note[a].SelectNodes("notePara");

                                if (root_ele != null)
                                {
                                    text_sum = "";
                                    for (int q = 0; q < root_ele.Count; q++)
                                    {
                                        string[] textLine = root_ele[q].InnerText.Split('\n');

                                        for (int s = 0; s < textLine.Length; s++)
                                        {
                                            text_sum += textLine[s];
                                            text_sum += ' ';
                                        }

                                        text_sum += "\n";
                                    }

                                    if (text_sum != "")
                                        empList.Add(new DMItem(Data.TEXT, true, text_sum, TEXTTYPE.NOTE, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                }

                                empList.Add(new DMItem(Data.SPACING, true, 20.0f));
                            }
                        }
                    }
                }

                // ----------------------------------------------------OPERATION----------------------------------------------------

                //MAINPROCEDURE
                if (root.SelectNodes("mainProcedure/proceduralStep").Count != 0)
                {
                    empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                    //Debug.Log("들어오긴함");
                    XmlNodeList root_1 = root.SelectNodes("mainProcedure/proceduralStep");
                    XmlElement title = title_root.SelectSingleNode("dmTitle/techName") as XmlElement;
                    XmlElement subTitle = title_root.SelectSingleNode("dmTitle/infoName") as XmlElement;
                    int paraCnt = 1;

                    DMItem_OperButtonItems empList_operBtnItem;
                    List<OperationItemOption> operationItem_Data = new List<OperationItemOption>();

                    //mainProcedure/proceduralStep
                    //절차 중간중간에 팝업창이 포함된다. 
                    for (int k = 0; k < root_1.Count; k++)
                    {
                        //warning
                        if (root_1[k].SelectNodes("warning").Count != 0)
                        {
                            XmlNodeList root_ele_0 = root_1[k].SelectNodes("warning");

                            for (int g = 0; g < root_ele_0.Count; g++)
                            {
                                //empList.Add(new DMItem(Data.SPACING, true, 20.0f));

                                XmlNodeList root_ele_1 = root_ele_0[g].SelectNodes("warningAndCautionPara");

                                if (root_ele_1 != null)
                                {
                                    text_sum = "";
                                    for (int q = 0; q < root_ele_1.Count; q++)
                                    {
                                        string[] textLine = root_ele_1[q].InnerText.Split('\n');

                                        for (int h = 0; h < textLine.Length; h++)
                                        {
                                            text_sum += textLine[h];
                                            text_sum += ' ';
                                        }

                                        text_sum += "\n";
                                    }

                                    if (text_sum != "")
                                        operationItem_Data.Add(new OperationItemOption(OperationItems.WARNING, text_sum, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                }

                                //empList.Add(new DMItem(Data.SPACING, true, 20.0f));
                            }
                        }

                        //caution
                        if (root_1[k].SelectNodes("caution").Count != 0)
                        {
                            XmlNodeList root_ele_0 = root_1[k].SelectNodes("caution");

                            for (int g = 0; g < root_ele_0.Count; g++)
                            {
                                //empList.Add(new DMItem(Data.SPACING, true, 20.0f));

                                XmlNodeList root_ele_1 = root_ele_0[g].SelectNodes("warningAndCautionPara");

                                if (root_ele_1 != null)
                                {
                                    text_sum = "";
                                    for (int q = 0; q < root_ele_1.Count; q++)
                                    {
                                        string[] textLine = root_ele_1[q].InnerText.Split('\n');

                                        for (int h = 0; h < textLine.Length; h++)
                                        {
                                            text_sum += textLine[h];
                                            text_sum += ' ';
                                        }

                                        text_sum += "\n";
                                    }

                                    if (text_sum != "")
                                        operationItem_Data.Add(new OperationItemOption(OperationItems.CAUTION, text_sum, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                }

                                //empList.Add(new DMItem(Data.SPACING, true, 20.0f));
                            }
                        }

                        //note
                        if (root_1[k].SelectNodes("note").Count != 0)
                        {
                            XmlNodeList root_ele_0 = root_1[k].SelectNodes("note");

                            for (int g = 0; g < root_ele_0.Count; g++)
                            {
                                //empList.Add(new DMItem(Data.SPACING, true, 20.0f));

                                XmlNodeList root_ele_1 = root_ele_0[g].SelectNodes("notePara");

                                if (root_ele_1 != null)
                                {
                                    text_sum = "";
                                    for (int q = 0; q < root_ele_1.Count; q++)
                                    {
                                        string[] textLine = root_ele_1[q].InnerText.Split('\n');

                                        for (int h = 0; h < textLine.Length; h++)
                                        {
                                            text_sum += textLine[h];
                                            text_sum += ' ';
                                        }

                                        text_sum += "\n";
                                    }

                                    if (text_sum != "")
                                        operationItem_Data.Add(new OperationItemOption(OperationItems.NOTE, text_sum, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                }

                                //empList.Add(new DMItem(Data.SPACING, true, 20.0f));
                            }
                        }

                        //proceduralStep
                        if (root_1[k].SelectNodes("proceduralStep").Count != 0)
                        {
                            //warning
                            if (root_1[k].SelectNodes("proceduralStep/warning").Count != 0)
                            {
                                XmlNodeList root_ele_0 = root_1[k].SelectNodes("proceduralStep/warning");

                                for (int g = 0; g < root_ele_0.Count; g++)
                                {
                                    empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                    XmlNodeList root_ele_1 = root_ele_0[g].SelectNodes("warningAndCautionPara");

                                    if (root_ele_1 != null)
                                    {
                                        text_sum = "";
                                        for (int q = 0; q < root_ele_1.Count; q++)
                                        {
                                            string[] textLine = root_ele_1[q].InnerText.Split('\n');

                                            for (int h = 0; h < textLine.Length; h++)
                                            {
                                                text_sum += textLine[h];
                                                text_sum += ' ';
                                            }

                                            text_sum += "\n";
                                        }

                                        if (text_sum != "")
                                            empList.Add(new DMItem(Data.TEXT, false, text_sum, TEXTTYPE.WARNING, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                    }

                                    empList.Add(new DMItem(Data.SPACING, false, 20.0f));
                                }
                            }

                            //caution
                            if (root_1[k].SelectNodes("proceduralStep/caution").Count != 0)
                            {
                                XmlNodeList root_ele_0 = root_1[k].SelectNodes("proceduralStep/caution");

                                for (int g = 0; g < root_ele_0.Count; g++)
                                {
                                    empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                    XmlNodeList root_ele_1 = root_ele_0[g].SelectNodes("warningAndCautionPara");

                                    if (root_ele_1 != null)
                                    {
                                        text_sum = "";
                                        for (int q = 0; q < root_ele_1.Count; q++)
                                        {
                                            string[] textLine = root_ele_1[q].InnerText.Split('\n');

                                            for (int h = 0; h < textLine.Length; h++)
                                            {
                                                text_sum += textLine[h];
                                                text_sum += ' ';
                                            }

                                            text_sum += "\n";
                                        }

                                        if (text_sum != "")
                                            empList.Add(new DMItem(Data.TEXT, false, text_sum, TEXTTYPE.CAUTION, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                    }

                                    empList.Add(new DMItem(Data.SPACING, false, 20.0f));
                                }
                            }

                            //note
                            if (root_1[k].SelectNodes("proceduralStep/note").Count != 0)
                            {
                                XmlNodeList root_ele_0 = root_1[k].SelectNodes("proceduralStep/note");

                                for (int g = 0; g < root_ele_0.Count; g++)
                                {
                                    empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                                    XmlNodeList root_ele_1 = root_ele_0[g].SelectNodes("notePara");

                                    if (root_ele_1 != null)
                                    {
                                        text_sum = "";
                                        for (int q = 0; q < root_ele_1.Count; q++)
                                        {
                                            string[] textLine = root_ele_1[q].InnerText.Split('\n');

                                            for (int h = 0; h < textLine.Length; h++)
                                            {
                                                text_sum += textLine[h];
                                                text_sum += ' ';
                                            }

                                            text_sum += "\n";
                                        }

                                        if (text_sum != "")
                                            empList.Add(new DMItem(Data.TEXT, false, text_sum, TEXTTYPE.NOTE, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                    }

                                    empList.Add(new DMItem(Data.SPACING, false, 20.0f));
                                }
                            }

                            //para
                            if (root_1[k].SelectNodes("proceduralStep/para").Count != 0)
                            {
                                for (int z = 0; z < root_1[k].SelectNodes("proceduralStep/para").Count; z++)
                                {
                                    XmlElement root_1_1 = root_1[k].SelectNodes("proceduralStep/para")[z] as XmlElement;

                                    empList.Add(new DMItem(Data.TEXT, false, z + 1 + ". " + root_1_1.InnerText, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                }
                            }

                            //proceduralStep
                            if (root_1[k].SelectNodes("proceduralStep").Count != 0)
                            {
                                for (int z = 0; z < root_1[k].SelectNodes("proceduralStep").Count; z++)
                                {
                                    XmlElement root_1_1 = root_1[k].SelectNodes("proceduralStep")[z].SelectSingleNode("para") as XmlElement;

                                    empList.Add(new DMItem(Data.TEXT, false, z + 1 + ". " + root_1_1.InnerText, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                                }
                            }
                        }

                        //mainProcedure/proceduralStep/para
                        //para
                        string operation_str = "";
                        if (root_1[k].SelectSingleNode("para") != null)
                        {
                            StringBuilder paraMax = new StringBuilder();

                            paraMax.Append(root_1[k].SelectSingleNode("para").InnerText);
                            paraMax.Replace("\n", "");

                            operation_str = paraCnt + ". " + paraMax.ToString();
                            paraCnt += 1;

                            operationItem_Data.Add(new OperationItemOption(OperationItems.OPERATION, operation_str, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                        }
                    }

                    empList_operBtnItem = new DMItem_OperButtonItems(title.InnerText, subTitle.InnerText, operationItem_Data.ToArray());

                    empList.Add(new DMItem(Data.OPERATION, false, empList_operBtnItem));

                    empList_operBtnItem = null;
                    operationItem_Data.Clear();
                }

                // ----------------------------------------------------CATALOG----------------------------------------------------

                //CATALOGSEQNUMBER
                if (root.SelectNodes("catalogSeqNumber").Count != 0)
                {
                    //figure 
                    //catalogSeqNumber 
                    CreateCatalogTableData(root.SelectNodes("catalogSeqNumber"), root.SelectNodes("figure"));
                }

                // ----------------------------------------------------SIDEDATA----------------------------------------------------
                //[ DIAGRAM CONTENT ]
                //FAULTLSOLATION
                if (default_root.SelectSingleNode("faultIsolation") != null)
                {
                    sideRoot = default_root.SelectSingleNode("faultIsolation/faultIsolationProcedure") as XmlElement;

                    //fault:faultCode ? 

                    //FAULTDESCR
                    if (sideRoot.SelectNodes("faultDescr").Count != 0)
                    {
                        for (int k = 0; k < sideRoot.SelectNodes("faultDescr").Count; k++)
                        {
                            XmlNodeList root_1 = sideRoot.SelectNodes("faultDescr/descr");
                            for (int z = 0; z < root_1.Count; z++)
                            {
                                empList.Add(new DMItem(Data.TEXT, false, root_1[z].InnerText, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                            }
                        }
                    }

                    //isolationProcedure
                    if (sideRoot.SelectNodes("isolationProcedure").Count != 0)
                    {
                        //preliminaryRqmts (데이터 X)

                        //isolationMainProcedure (다이어 그램)

                        XmlNodeList root_1 = sideRoot.SelectNodes("isolationProcedure/isolationMainProcedure");
                        if (root_1 != null)
                        {
                            CreateDiagramData(root_1);
                        }

                        //closeRqmts (데이터 X)
                    }
                }
                else
                    sideRoot = null;

                //테이블이 포함된 데이터 삽입
                dmContents_Unit.Add(new DmContent(empList, null, _linkToDmData_Unit[i].dm_rootName[j]));

                //if (empList.Count == 0)
                //    Debug.Log("DATA PARSING : " + _linkToDmData_Unit[i].dm_rootName[j].name + "  FAIL !!");
                //else
                //    Debug.Log("DATA PARSING : " + _linkToDmData_Unit[i].dm_rootName[j].name + "  SUCCESS !!");

            }
        }
    }

    //Title, Para, Table으로만 구성 
    void DMData_Parcing_Abbre()
    {
        XmlDocument xmlDoc = new XmlDocument();
        TextAsset txtAsset = (TextAsset)txtAsset_abbre_Unit;
        xmlDoc.LoadXml(txtAsset.text);
        empList = new List<DMItem>();

        // ----------------------------------------------------TITLE----------------------------------------------------
        XmlElement title_root = xmlDoc.SelectSingleNode("dmodule/identAndStatusSection/dmAddress/dmAddressItems") as XmlElement; //기본 타이틀 루트
                                                                                                                                 //XmlElement title_link = xmlDoc.SelectSingleNode("dmodule/identAndStatusSection/dmAddress/dmIdent/dmCode") as XmlElement;

        //string title_skip = title_link.GetAttribute("infoCode");

        if (title_root.SelectSingleNode("dmTitle/infoName") != null)
        {
            string[] textLine = title_root.SelectSingleNode("dmTitle/infoName").InnerText.Split('\n');
            text_sum = "";

            for (int q = 0; q < textLine.Length; q++)
            {
                text_sum += textLine[q];
            }

            if (text_sum != "")
            {
                empList.Add(new DMItem(Data.TEXT, true, text_sum, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.CENTER, TextDefine.TEXTSTYLE.BOLD, 35.0f, new Color(0.0627451f, 0.05882353f, 0.3960785f, 1)));
            }
        }


        // ----------------------------------------------------CONTENT----------------------------------------------------
        XmlElement default_root = xmlDoc.SelectSingleNode("dmodule/content") as XmlElement; //기본 루트 

        if (default_root.SelectSingleNode("description/levelledPara") != null)
        {
            root = default_root.SelectSingleNode("description/levelledPara") as XmlElement;
        }
        else
        {
            root = null;

            return;
        }
        //TITLE
        if (root.SelectNodes("title").Count != 0) //기술 제원 
        {
            empList.Add(new DMItem(Data.SPACING, true, 20.0f));

            XmlNodeList root_title = root.SelectNodes("title");
            for (int k = 0; k < root_title.Count; k++)
            {
                empList.Add(new DMItem(Data.TEXT, true, (k + 1) + ". " + root_title[k].InnerText, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.BOLD, 34.0f, new Color(0.06666667f, 0.06666667f, 0.06666667f, 1)));
            }
        }
        //PARA
        if (root.SelectNodes("para").Count != 0)
        {
            XmlNodeList root_para = root.SelectNodes("para");

            for (int k = 0; k < root_para.Count; k++)
            {
                //sequentialList
                if (root_para[k].SelectNodes("sequentialList").Count != 0)
                {
                    XmlNodeList root_para_sequentialList = root_para[k].SelectNodes("sequentialList");
                    for (int h = 0; h < root_para_sequentialList.Count; h++)
                    {
                        XmlNodeList root_para_sequentialList_listItem = root_para_sequentialList[h].SelectNodes("listItem/para");

                        for (int l = 0; l < root_para_sequentialList_listItem.Count; l++)
                        {
                            empList.Add(new DMItem(Data.TEXT, true, title_Han_words[l] + ". " + root_para_sequentialList_listItem[l].InnerText, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                            empList.Add(new DMItem(Data.SPACING, true, 20.0f));
                        }
                    }
                }
                //없으면 그냥 텍스트 
                else
                {
                    string[] textLine = root_para[k].InnerText.Split('\n');
                    text_sum = "";

                    for (int q = 0; q < textLine.Length; q++)
                    {
                        text_sum += textLine[q];
                        text_sum += ' ';
                    }

                    empList.Add(new DMItem(Data.TEXT, true, k + 1 + ") " + text_sum, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.LEFT, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                }
                empList.Add(new DMItem(Data.SPACING, true, 20.0f));
            }
        }
        //TABLE
        if (root.SelectNodes("table").Count != 0)
        {
            for (int k = 0; k < root.SelectNodes("table").Count; k++)
            {
                empList.Add(new DMItem(Data.SPACING, false, 20.0f));

                CreateTableData(root.SelectNodes("table")[k] as XmlElement);
            }
        }
        sideRoot = null;

        //테이블이 포함된 데이터 삽입
        dmAbbre_Unit = new DmContent(empList, null, null);
    }


    [Header(" [ CONTENTLISTSETTING.CS에 TABLE WIDTH 값이랑 동일 ] ")]
    [SerializeField] float caltallog_width;

    //테이블 분리 필요하다 중간에 버튼을 추가해야된다. (누르면 컨텐츠 영역에 사진 or 모델링이 나온다.)
    void CreateCatalogTableData(XmlNodeList root, XmlNodeList title_root)
    {
        //int lineCnt = root.Count + 1;//행 개수 (헤더 포함)
        int rowCnt = 8; //열 개수 (정해짐 8개)
        List<float> table_width = new List<float>(rowCnt); //개수 8개 정해짐

        List<string> tableTitle = new List<string>(title_root.Count); //테이블 타이틀 
        int titleCount = 1;
        //bool trigger = false;

        float sum = 0;

        List<Catalog_Item> empCatalog_items = new List<Catalog_Item>();

        //정해진 사이즈가 없어서 임의로 진행한다. 
        table_width.Add(0.5f);
        table_width.Add(0.5f);
        table_width.Add(1.0f);
        table_width.Add(1.0f);
        table_width.Add(0.5f);
        table_width.Add(1.5f);
        table_width.Add(0.5f);
        table_width.Add(0.5f);

        //테이블 칼럼마다 길이 비율 추출
        for (int k = 0; k < table_width.Count; k++)
        {
            sum += table_width[k];
        }

        //Width * 500 / Sum
        for (int k = 0; k < table_width.Count; k++)
        {
            table_width[k] = table_width[k] * caltallog_width / sum;
        }

        ////title 
        //for (int i = 0; i < title_root.Count; i++)
        //{
        //    XmlElement title_root_1 = title_root[i].SelectSingleNode("title") as XmlElement;

        //    tableTitle.Add(title_root_1.InnerText);
        //}

        //empList.Add(new DMItem(Data.SPACING, false, 30.0f)); //공백 생성

        ////HEADER
        //empList.Add(new DMItem(Data.TEXT, false, "< " + tableTitle[0] + " >", TEXTTYPE.TEXT, TextDefine.TEXTALIGN.CENTER, TextDefine.TEXTSTYLE.MEDIUM, 27.0f, new Color(0.2f, 0.2f, 0.2f, 1)));
        //한 행 생성
        empCatalog_items.Add(new Catalog_Item("", "", "", "", "계통/그림번호", "품목번호", "부품번호", "국가재고번호", "생산자 부호", "설명(NOMENCLATURE)",
            "단위 당 구성수량", "근원정비 복구성 부호"));



        //catalogSeqNumber 부터 시작 한줄 생성 
        for (int i = 0; i < root.Count; i++)
        {
            //국가 재고 번호(해당 장비마다 몸체는 국가 번호 9999-99-999-9999값을 가지므로 버튼 이벤트를 잃게된다. 
            string emp = "";

            emp = "9999-99-999-9999"; //몸체

            //----------------------------OWN DATA-----------------------------------
            XmlElement catalogSeqNumber = root[i] as XmlElement;
            //catalogSeqNumber - subSystemCode(어떤 모델링인지 고유의 번호)
            string subSystemCode = catalogSeqNumber.GetAttribute("subSystemCode");
            string subsubSystemCode = catalogSeqNumber.GetAttribute("subSubSystemCode");

            //catalogSeqNumber - figureNumber(한 장비의 파트 종류) == 테이블이 교체된다.(모델링또한 마찬가지)
            string figureNumber = catalogSeqNumber.GetAttribute("figureNumber");

            string assyCode = catalogSeqNumber.GetAttribute("assyCode"); //다른 페이지 

            //catalogSeqNumber - id (한장비의 고유의 번호 (단 한장비안에서만 고유하다))
            string id = catalogSeqNumber.GetAttribute("id");

            //----------------------------TABLE DATA-----------------------------------

            //계통/그림번호
            //figureNumber 데이터 삽입 

            //품목 번호
            //itemSeqNumber - itemSeqNumberValue (Single)
            XmlElement itemSeqNumber = root[i].SelectSingleNode("itemSeqNumber") as XmlElement;
            string itemSeqNumberValue = itemSeqNumber.GetAttribute("itemSeqNumberValue");

            //quantityPerNextHigherAssy (Single) //단위당 구성 수량
            XmlElement quantityPerNextHigherAssy = root[i].SelectSingleNode("itemSeqNumber/quantityPerNextHigherAssy") as XmlElement;
            string quantityPerNextHigherAssy_txt = quantityPerNextHigherAssy.InnerText;

            //생산자 부호, 부품번호
            //partRef - manufacturerCodeValue, partNumberValue (Single)
            XmlElement partRef = root[i].SelectSingleNode("itemSeqNumber/partRef") as XmlElement;
            string manufacturerCodeValue = partRef.GetAttribute("manufacturerCodeValue");
            string partNumberValue = partRef.GetAttribute("partNumberValue");

            //설명
            //partSegment/itemIdentData/descrForPart (Single)
            XmlElement descrForPart = root[i].SelectSingleNode("itemSeqNumber/partSegment/itemIdentData/descrForPart") as XmlElement;
            string descrForPart_txt = descrForPart.InnerText;

            //근원정비 복구성 부호 
            //locationRcmdSegment/locationRcmd/sourceMaintRecoverability (Single)
            XmlElement sourceMaintRecoverability = root[i].SelectSingleNode("itemSeqNumber/locationRcmdSegment/locationRcmd/sourceMaintRecoverability") as XmlElement;

            string sourceMaintRecoverability_txt;
            if (sourceMaintRecoverability != null)
                sourceMaintRecoverability_txt = sourceMaintRecoverability.InnerText;
            else
                sourceMaintRecoverability_txt = "";


            //헤더 제외 시작 
            //figureNumber가 이전 데이터랑 다를 경우 헤더 생성 필요 
            if (i != 0)
            {
                XmlElement A = root[i - 1] as XmlElement;
                XmlElement B = root[i] as XmlElement;

                string A_txt = A.GetAttribute("figureNumber");
                string B_txt = B.GetAttribute("figureNumber");

                //테이블 교체 시점 
                if (A_txt != B_txt)
                {
                    //테이블 생성
                    empList_catalog = new DMItem_Catalog(empCatalog_items.Count, rowCnt, table_width, empCatalog_items.ToArray());

                    empList.Add(new DMItem(Data.CATALOG, false, empList_catalog));

                    if (empCatalog_items != null)
                        empCatalog_items.Clear();

                    // 새로운 테이블 시작 --------------------------------------------------------------------------------------------------

                    empList.Add(new DMItem(Data.SPACING, false, 30.0f)); //공백 생성

                    //타이틀 추가 
                    empList.Add(new DMItem(Data.TEXT, false, tableTitle[titleCount], TEXTTYPE.TEXT, TextDefine.TEXTALIGN.CENTER, TextDefine.TEXTSTYLE.BOLD, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
                    ++titleCount;

                    //한 행 생성
                    empCatalog_items.Add(new Catalog_Item("", "", "", "", "계통/그림번호", "품목번호", "부품번호", "국가재고번호", "생산자 부호", "설명(NOMENCLATURE)",
                        "단위 당 구성수량", "근원정비 복구성 부호"));

                    emp = "9999-99-999-9999"; //몸체

                    //한 행 생성
                    empCatalog_items.Add(new Catalog_Item(subSystemCode, subsubSystemCode, assyCode, id, figureNumber, itemSeqNumberValue, partNumberValue,
                        emp, manufacturerCodeValue, descrForPart_txt, quantityPerNextHigherAssy_txt, sourceMaintRecoverability_txt));

                }
                else
                {
                    emp = "";
                    //한 행 생성
                    empCatalog_items.Add(new Catalog_Item(subSystemCode, subsubSystemCode, assyCode, id, figureNumber, itemSeqNumberValue, partNumberValue,
                        emp, manufacturerCodeValue, descrForPart_txt, quantityPerNextHigherAssy_txt, sourceMaintRecoverability_txt));
                }
            }
            else
            {
                //한 행 생성
                empCatalog_items.Add(new Catalog_Item(subSystemCode, subsubSystemCode, assyCode, id, figureNumber, itemSeqNumberValue, partNumberValue,
                    emp, manufacturerCodeValue, descrForPart_txt, quantityPerNextHigherAssy_txt, sourceMaintRecoverability_txt));
            }
            //FOR END
        }

        //마지막 테이블 생성
        empList_catalog = new DMItem_Catalog(empCatalog_items.Count, rowCnt, table_width, empCatalog_items.ToArray());
        empList.Add(new DMItem(Data.CATALOG, false, empList_catalog));
    }

    List<DMItem_Table_Colume> _headerColume;
    List<DMItem_Table_Colume> _contentColume;
    void CreateTableData(XmlElement root)
    {
        float _sumColWidth = 0f;

        //TITLE
        if (root.SelectSingleNode("title") != null)
        {
            empList.Add(new DMItem(Data.TEXT, false, "< " + root.SelectSingleNode("title").InnerText + " >", TEXTTYPE.TEXT, TextDefine.TEXTALIGN.CENTER, TextDefine.TEXTSTYLE.MEDIUM, 27.0f, new Color(0.2f, 0.2f, 0.2f, 1)));
            empList.Add(new DMItem(Data.SPACING, false, 15.0f));
        }

        //PARA
        if (root.SelectSingleNode("para") != null)
        {
            string[] textLine = root.SelectSingleNode("para").InnerText.Split('\n');
            text_sum = "";

            for (int q = 0; q < textLine.Length; q++)
            {
                text_sum += textLine[q];
                text_sum += ' ';
            }

            empList.Add(new DMItem(Data.TEXT, false, text_sum, TEXTTYPE.TEXT, TextDefine.TEXTALIGN.CENTER, TextDefine.TEXTSTYLE.NORMAL, 24.0f, new Color(0.345098f, 0.345098f, 0.345098f, 1)));
        }

        //colspec
        List<ColSpec> _colspec = new List<ColSpec>(); // 열의 개수 

        XmlNodeList colspec_root = root.SelectNodes("tgroup/colspec"); //열의 개수 
        for (int k = 0; k < colspec_root.Count; k++)
        {
            XmlElement colspec_root_el = colspec_root[k] as XmlElement;

            string _colname = colspec_root_el.GetAttribute("colname");

            string tmp = colspec_root_el.GetAttribute("colwidth").Replace("*", "");
            float _colwidth = float.Parse(tmp);
            _sumColWidth += _colwidth;

            _colspec.Add(new ColSpec(_colname, _colwidth));
        }

        XmlNodeList headers = root.SelectNodes("tgroup/thead/row");  //헤더의 행의 개수 
        XmlNodeList contents = root.SelectNodes("tgroup/tbody/row"); //컨텐츠의 행의 개수 


        //header 데이터 값 추출 
        _headerColume = SetColumeData(headers, _colspec, true);
        //content 데이터 값 추출 
        _contentColume = SetColumeData(contents, _colspec, false);


        empList_table = new DMItem_Table(_colspec.ToArray(), _sumColWidth, headers.Count, contents.Count, _headerColume.ToArray(), _contentColume.ToArray());

        //테이블 데이터 추가될 시 텍스트 없이 삽입된다.
        empList.Add(new DMItem(Data.TABLE, false, empList_table));
    }

    public struct MorerowsColume
    {
        public int morerowsCnt;
        public DMItem_Table_Colume morerowsData;
    }

    //2차원 배열 데이터 셋팅 
    List<DMItem_Table_Colume> SetColumeData(XmlNodeList contents, List<ColSpec> _colspec, bool isheaderTrigger)
    {
        List<DMItem_Table_Colume> empListData = new List<DMItem_Table_Colume>();

        MorerowsColume[] morerowsColume = new MorerowsColume[_colspec.Count]; //열의 개수에 맞춰서 생성

        for (int x = 0; x < morerowsColume.Length; x++)
        {
            morerowsColume[x].morerowsCnt = 0;
            morerowsColume[x].morerowsData = null;
        }

        bool morerowsTrigger = false; //체크를 통해서 다음 행에 데이터를 대신 삽입한다. 
        int morerowsStartNum = -1; //기본값 상태 


        for (int x = 0; x < contents.Count; x++) //행의 개수 
        {
            morerowsTrigger = false;

            XmlNodeList colume = contents[x].SelectNodes("entry"); //한 행의 열의 개수 

            int colume_count = 0; //데이터를 추출하기 위해 

            for (int y = 0; y < _colspec.Count; y++) //열의 맥스치로 돌려서 구분한다.
            {
                if (morerowsColume[y].morerowsCnt > 0)
                {
                    morerowsStartNum = y; //복사준비 
                    morerowsColume[morerowsStartNum].morerowsCnt -= 1;

                    //복사본 추가 
                    empListData.Add(morerowsColume[morerowsStartNum].morerowsData);

                    if (morerowsColume[morerowsStartNum].morerowsData.nameend != "" || morerowsColume[morerowsStartNum].morerowsData.namest != "")
                    {
                        bool trigger = false;

                        for (int g = 0; g < _colspec.Count; g++)
                        {
                            if (morerowsColume[morerowsStartNum].morerowsData.namest == _colspec[g].colname)
                            {
                                if (morerowsColume[morerowsStartNum].morerowsData.namest == morerowsColume[morerowsStartNum].morerowsData.nameend)
                                {
                                    break;
                                }

                                trigger = true;
                            }
                            else
                            {
                                if (trigger)
                                {
                                    empListData.Add(new DMItem_Table_Colume(0, null, null, 0f, TextDefine.TEXTALIGN.CENTER, "", true)); //반 공백을 채워넣는다. 

                                    y += 1;

                                    if (morerowsColume[morerowsStartNum].morerowsData.nameend == _colspec[g].colname)
                                    {
                                        trigger = false;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    XmlElement colume_emp = colume[colume_count] as XmlElement;

                    //colume data (한 컬럼에 들어가는 데이터)
                    int _morerows = 0;
                    string _namest = "";
                    string _nameend = "";
                    float _totalColwidth = 0f;
                    TextDefine.TEXTALIGN _align = TextDefine.TEXTALIGN.CENTER;
                    string _colume_txt = "";

                    //무조건 0 이 기본값으로 출력된다. 
                    if (colume_emp.GetAttribute("morerows") != null)
                    {
                        int.TryParse(colume_emp.GetAttribute("morerows"), out _morerows);

                        if (_morerows > 0)
                        {
                            morerowsTrigger = true; //복사 진행 

                            morerowsStartNum = y;
                        }
                    }

                    // ------------------------------------_totalColwidth 계산 ------------------------------------

                    if (colume_emp.GetAttribute("namest") != null)
                        _namest = colume_emp.GetAttribute("namest");

                    if (colume_emp.GetAttribute("nameend") != null)
                        _nameend = colume_emp.GetAttribute("nameend");


                    if (_nameend != "" || _namest != "")
                    {
                        bool trigger = false;

                        for (int g = 0; g < _colspec.Count; g++)
                        {
                            if (_namest == _colspec[g].colname)
                            {
                                if (_namest == _nameend)
                                {
                                    _totalColwidth += _colspec[g].colwidth;

                                    break;
                                }

                                _totalColwidth += _colspec[g].colwidth;

                                trigger = true;
                            }
                            else
                            {
                                if (trigger)
                                {
                                    _totalColwidth += _colspec[g].colwidth;

                                    empListData.Add(new DMItem_Table_Colume(0, null, null, 0f, TextDefine.TEXTALIGN.CENTER, "", true)); //반 공백을 채워넣는다. 


                                    y += 1;

                                    if (_nameend == _colspec[g].colname)
                                    {
                                        trigger = false;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        _totalColwidth = _colspec[y].colwidth;
                    }

                    // --------------------------------------------------------------------------------------------

                    if (colume_emp.GetAttribute("align") != null)
                    {
                        if (colume_emp.GetAttribute("align") == "center")
                        {
                            _align = TextDefine.TEXTALIGN.CENTER;
                        }
                        else if (colume_emp.GetAttribute("align") == "left")
                        {
                            _align = TextDefine.TEXTALIGN.LEFT;
                        }
                        else if (colume_emp.GetAttribute("align") == "right")
                        {
                            _align = TextDefine.TEXTALIGN.RIGHT;
                        }
                        else
                            _align = TextDefine.TEXTALIGN.CENTER;
                    }

                    if (colume_emp.SelectNodes("para") != null)
                    {
                        XmlNodeList root_para = colume_emp.SelectNodes("para");
                        for (int e = 0; e < root_para.Count; e++)
                        {
                            //randomList
                            if (root_para[e].SelectNodes("randomList").Count != 0)
                            {
                                XmlNodeList root_para_randomList = root_para[e].SelectNodes("randomList");
                                for (int h = 0; h < root_para_randomList.Count; h++)
                                {
                                    XmlNodeList root_para_randomList_listItem = root_para_randomList[h].SelectNodes("listItem/para");
                                    for (int l = 0; l < root_para_randomList_listItem.Count; l++)
                                    {
                                        string[] textLine = root_para_randomList_listItem[l].InnerText.Split('\n');
                                        text_sum = "";

                                        for (int q = 0; q < textLine.Length; q++)
                                        {
                                            text_sum += textLine[q];
                                            text_sum += ' ';
                                        }

                                        _colume_txt += text_sum;
                                        _colume_txt += "\n";
                                    }
                                }
                            }
                            //없으면 그냥 텍스트 
                            else
                            {
                                string[] textLine = root_para[e].InnerText.Split('\n');
                                text_sum = "";

                                for (int q = 0; q < textLine.Length; q++)
                                {
                                    text_sum += textLine[q];
                                    text_sum += ' ';
                                }

                                _colume_txt += root_para[e].InnerText;

                                if (e != colume_emp.SelectNodes("para").Count)
                                    _colume_txt += "\n";
                            }
                        }
                    }

                    else
                        _colume_txt = ""; //없을때 기본값 

                    empListData.Add(new DMItem_Table_Colume(_morerows, _namest, _nameend, _totalColwidth, _align, _colume_txt, false));


                    //다음 행나오기전에 morerows 데이터 저장 
                    if (morerowsTrigger)
                    {
                        //복사 
                        if (isheaderTrigger)
                        {
                            morerowsColume[morerowsStartNum].morerowsCnt = _morerows;
                            morerowsColume[morerowsStartNum].morerowsData = new DMItem_Table_Colume(_morerows, _namest, _nameend, _totalColwidth, _align, "", false);
                        }
                        else
                        {
                            morerowsColume[morerowsStartNum].morerowsCnt = _morerows;
                            morerowsColume[morerowsStartNum].morerowsData = new DMItem_Table_Colume(_morerows, _namest, _nameend, _totalColwidth, _align, _colume_txt, false);
                        }

                        colume_count += 1;
                        morerowsTrigger = false;
                    }
                    else
                    {
                        colume_count += 1;
                    }
                }
            }
        }
        return empListData;
    }

    void CreateDiagramData(XmlNodeList root)
    {
        List<DMItem_DiagramItemSetting> emp_diagramItemSetting = new List<DMItem_DiagramItemSetting>();

        //isolationProcedure/isolationMainProcedure
        for (int i = 0; i < root.Count; i++)
        {
            XmlNodeList isolationStep = root[i].SelectNodes("isolationStep");
            XmlNodeList isolationProcedureEnd = root[i].SelectNodes("isolationProcedureEnd");

            if (isolationStep != null)
            {
                //isolationProcedure/isolationMainProcedure/isolationStep
                for (int j = 0; j < isolationStep.Count; j++)
                {
                    XmlElement _id = isolationStep[j] as XmlElement;
                    XmlNodeList root_1 = isolationStep[j].SelectNodes("action/randomList/listItem"); //고장원인 or 조치사항 

                    string id, listOfChoice = null, yes_ans = null, no_ans = null, quest;

                    id = _id.GetAttribute("id");

                    List<DMItem_DiagramItemInfo> tmpActionItem = new List<DMItem_DiagramItemInfo>();

                    //listItem
                    for (int k = 0; k < root_1.Count; k++)
                    {
                        //para
                        XmlElement root_2 = root_1[k].SelectNodes("para")[0] as XmlElement;
                        //연결되는 DM 데이터 
                        XmlNodeList root_2_1 = root_1[k].SelectNodes("para/dmRef");

                        //조치사항
                        if (root_2_1.Count != 0)
                        {
                            tmpActionItem.Add(new DMItem_DiagramItemInfo(DiagramItemList.조치사항, root_2.InnerText, null)); //DM이 정확하게 검색이 되지 않는다. 
                            //dmRefAddressItems ? (일단 보류 
                        }
                        //고장원인
                        else
                        {
                            tmpActionItem.Add(new DMItem_DiagramItemInfo(DiagramItemList.고장원인, root_2.InnerText, null)); //고장원인
                        }
                    }

                    //isolationStepQuestion
                    quest = isolationStep[j].SelectSingleNode("isolationStepQuestion").InnerText;

                    if (quest == "") quest = null;

                    //isolationStepAnswer
                    XmlElement _listOfChoice = isolationStep[j].SelectSingleNode("isolationStepAnswer/listOfChoices/choice") as XmlElement;

                    if (_listOfChoice != null)
                    {
                        if (_listOfChoice.GetAttribute("nextActionRefId") != null)
                            listOfChoice = _listOfChoice.GetAttribute("nextActionRefId");
                        else listOfChoice = null;
                    }

                    XmlElement _yes_ans = isolationStep[j].SelectSingleNode("isolationStepAnswer/yesNoAnswer/yesAnswer") as XmlElement;

                    if (_yes_ans != null)
                    {
                        if (_yes_ans.GetAttribute("nextActionRefId") != null)
                            yes_ans = _yes_ans.GetAttribute("nextActionRefId");
                        else yes_ans = null;
                    }

                    XmlElement _no_ans = isolationStep[j].SelectSingleNode("isolationStepAnswer/yesNoAnswer/noAnswer") as XmlElement;

                    if (_no_ans != null)
                    {
                        if (_no_ans.GetAttribute("nextActionRefId") != null)
                            no_ans = _no_ans.GetAttribute("nextActionRefId");
                        else no_ans = null;
                    }


                    //isolationStep 추가 
                    emp_diagramItemSetting.Add(new DMItem_DiagramItemSetting(false, id, listOfChoice, yes_ans, no_ans, quest, tmpActionItem));
                }
            }

            if (isolationProcedureEnd != null)
            {
                //isolationProcedure/isolationMainProcedure/isolationProcedureEnd
                for (int j = 0; j < isolationProcedureEnd.Count; j++)
                {
                    XmlElement _id = isolationProcedureEnd[j] as XmlElement;
                    XmlNodeList root_1 = isolationProcedureEnd[j].SelectNodes("action/randomList/listItem");

                    string id;
                    List<DMItem_DiagramItemInfo> end_answer = new List<DMItem_DiagramItemInfo>();

                    id = _id.GetAttribute("id");

                    //listItem
                    for (int k = 0; k < root_1.Count; k++)
                    {
                        //para
                        XmlElement root_2 = root_1[k].SelectNodes("para")[0] as XmlElement;
                        //연결되는 DM 데이터 
                        XmlNodeList root_2_1 = root_1[k].SelectNodes("para/dmRef");

                        //조치사항
                        if (root_2_1.Count != 0)
                        {
                            XmlElement linkdm = root_2_1[0].SelectSingleNode("dmRefIdent/dmCode") as XmlElement;

                            end_answer.Add(new DMItem_DiagramItemInfo(DiagramItemList.결과, root_2.InnerText, null)); //DM이 정확하게 검색이 되지 않는다. 

                            //dmRefAddressItems ? (일단 보류 
                        }
                        //고장원인
                        else
                        {
                            end_answer.Add(new DMItem_DiagramItemInfo(DiagramItemList.결과, root_2.InnerText, null)); //고장원인
                        }
                    }

                    //isolationProcedureEnd 추가 
                    emp_diagramItemSetting.Add(new DMItem_DiagramItemSetting(true, id, null, null, null, null, end_answer));
                }
            }
        }

        empList.Add(new DMItem(Data.DIAGRAM, false, emp_diagramItemSetting));
    }
}



