using maxstAR;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public enum Mode
{
    IMAGE, QR
}

public class LayoutController : MonoBehaviour
{
    [Header(" [ SCENE LINK STRING ] ")]
    [SerializeField] string main_scene_str;
    [SerializeField] string main_scene_str_local;
    [SerializeField] string login_scene_str;
    [SerializeField] TextMeshProUGUI name_txt_sub, com_txt_sub, pos_txt_sub;

    [Header(" [ AR MODE ] ")]
    [SerializeField] Mode mode;
    //[SerializeField] GameObject arOff_bg; //�ν� ������ Ȱ��ȭ
    [SerializeField] GameObject targetZone_img;
    //[SerializeField] GameObject qaGuide_back;

    [SerializeField] public ARSettingCanvas arSettingCanvas;

    [Header(" [ BUTTON ] ")]
    //[SerializeField] public ButtonController home_btn;
    [SerializeField] ButtonController flashOn_btn;
    [SerializeField] ButtonController flashOff_btn; 
        //qa_sub_btn;

    [Header(" [ CAMERA ] ")]
    [SerializeField] GameObject ARCamera_obj;
    [SerializeField] public Camera modelCamera; //AR �ν� ��Ʈ ī�޶�

    [Header(" [ SCRIPTS RESOURCE ] ")]
    [SerializeField] public ImageTrackerSample imageTrackerSample_cs;
    [SerializeField] public QrCodeTrackerSample qrcodeTrackerSample_cs;
    [SerializeField] UserPasswordManager userPasswordManager;

    //[SerializeField] public GizmoController gizmo;

    private void Start()
    {
        if (MySQLManager.Instance.currentUserData != null)
        {
            name_txt_sub.text = MySQLManager.Instance.currentUserData.userbaseName;
            com_txt_sub.text = "�Ҽ� : " + MySQLManager.Instance.currentUserData.userbaseCompany;
            pos_txt_sub.text = "���� : " + MySQLManager.Instance.currentUserData.userbasePosition;
        }

        ARStateSetting();
    }

    //STATE INIT
    void ARStateInit()
    {
        if (targetZone_img != null) targetZone_img.SetActive(false);

        //BUTTON
        flashOn_btn.gameObject.SetActive(false);
        flashOff_btn.gameObject.SetActive(false);
    }

    public void UserPWChange()
    {
        userPasswordManager.ItemContainerActive();
    }

    public void HomeButtonClick()
    {
        if(MySQLManager.Instance.isLocalConnection)
            CheckSceneData.CheckSceneDataSting = main_scene_str_local;
        else
            CheckSceneData.CheckSceneDataSting = main_scene_str;

        SceneManager.LoadScene("LOADING");
    }

    public void ExitButtonClick()
    {
        MySQLManager.Instance.UpdateUserStatus("/api/tbUserbases/", "N", "");
        Application.Quit();
    }

    public void LogoutButtonClick()
    {
        MySQLManager.Instance.UpdateUserStatus("/api/tbUserbases/", "N", "");
        CheckSceneData.CheckSceneDataSting = login_scene_str;
        SceneManager.LoadScene("LOADING");
    }

    //AR STATE SETTING
    void ARStateSetting()
    {
        InitCamera(); 

        if (targetZone_img != null) targetZone_img.SetActive(true);
        if (ARCamera_obj != null) ARCamera_obj.SetActive(true);

        IsFlashOn(false);

        if (mode == Mode.IMAGE)
        {
            if (imageTrackerSample_cs != null)
                imageTrackerSample_cs.TrackStart(true);
        }
        else if (mode == Mode.QR)
        {
            if (qrcodeTrackerSample_cs != null)
                qrcodeTrackerSample_cs.TrackStart(true);
        }
        else
        {
            Debug.Log("MODE NULL");

            return;
        }
    }

    void InitCamera()
    {
        //AR ī�޶� ���� 
        modelCamera.gameObject.SetActive(false);
    }

    //CANVAS STATE CHANGE
    public void CameraSetting()
    {
        InitCamera();

        modelCamera.gameObject.SetActive(true);
    }

    //AR TARGET SUCCESS
    public void ARTargetSuccess(Type type)
    {
        ARStateInit();
        arSettingCanvas.gameObject.SetActive(true);
        arSettingCanvas.ButtonSetting();
        CameraSetting();

        if (mode == Mode.IMAGE)
            arSettingCanvas.SetTargetInfomation(type);
    }

    public void ARTargetSuccess(string type)
    {
        ARStateInit();
        arSettingCanvas.gameObject.SetActive(true);
        arSettingCanvas.ButtonSetting();

        CameraSetting();

        if (mode == Mode.QR)
            arSettingCanvas.SetTargetInfomation(type.ToString());
    }

    //AR ���ν� 
    public void ReTargetOn()
    {
        // ���� �𵨸� ������ �ʱ�ȭ
        arSettingCanvas.EquipInit();
        arSettingCanvas.EquipInfoInit();
        arSettingCanvas.ButtonInit();

        arSettingCanvas.gameObject.SetActive(false);
        ARStateSetting();
    }

    //false : �÷��� Ŵ / true : �÷��� �� 
    public void IsFlashOn(bool check)
    {
        flashOn_btn.gameObject.SetActive(!check);
        flashOff_btn.gameObject.SetActive(check);

        CameraDevice.GetInstance().SetFlashLightMode(check);
    }

    bool qaCheck = false;
    //���� ��ư 
    public void QAOn()
    {
        qaCheck = !qaCheck;

        targetZone_img.SetActive(!qaCheck);
    }


}
