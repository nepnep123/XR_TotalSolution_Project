using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WebRTCWrapper;
using UnityEngine.SceneManagement;

public class RealCollaboration : MonoBehaviour
{
    //MAIN
    [SerializeField] string mainScene;
    [SerializeField] string loginScene;
    [SerializeField] Button _quitBtn;

    //SET
    [SerializeField] RawImage _remoteVideo;
    [SerializeField] RealwearCamera _realWearCamera;
    //GET
    [SerializeField] RawImage _pcVideo;

    [SerializeField] GameObject _pcChatObjet;
    [SerializeField] GameObject _mobileChatObject;
    [SerializeField] GameObject _pcChatObjet_WIDE;
    [SerializeField] GameObject _mobileChatObject_WIDE;
    [SerializeField] Transform _chatTr;
    [SerializeField] Transform _wideChatTr;

    [SerializeField] TMP_InputField _inputChat, _inputChat_2;
    [SerializeField] Button _chatButton, _chatButton_2;
    
    Mic _mic;

    [SerializeField] Animator _uiAnim;
    [SerializeField] Button _remoteButton;
    bool _isWide = false;

    private void Start()
    {
        //   Screen.SetResolution(854, 480, true);

        _quitBtn.onClick.AddListener(HomeButtonClick);

        Global.GetComm().OnDisconnected += OnDisconnected;

        _remoteButton.onClick.AddListener(ChangeMode);

        _remoteVideo = _realWearCamera._localTex;
        _pcVideo.texture = Global.GetComm().GetVideo(Global.CONNECT_USER.userData.userbaseId);
        Global.GetComm().GetAudio(Global.CONNECT_USER.userData.userbaseId).Play();

        Global.GetComm().OnChatReceived += ReceiveChat;

        _inputChat.onSubmit.AddListener(OnSubmitChat);
        _inputChat_2.onSubmit.AddListener(OnSubmitChat);

        _chatButton.onClick.AddListener(OnSubmitChat);
        _chatButton_2.onClick.AddListener(OnSubmitChat);

        Global.GetComm().SetVideo(_remoteVideo.texture);

        _mic = WebRTCWrapper.MediaDevices.GetDefaultMic();
        _mic.Run();        
        Global.GetComm().SetAudio(_mic.clip);

    }

    void OnDisconnected(string remote_id)
    {
        MySQLManager.Instance.UpdateUserStatus("/api/tbUserbases/", "Y", "N");

        CheckSceneData.CheckSceneDataSting = mainScene;

        SceneManager.LoadScene("LOADING");
    }


    public void HomeButtonClick()
    {
        MySQLManager.Instance.UpdateUserStatus("/api/tbUserbases/", "Y", "N");

        CheckSceneData.CheckSceneDataSting = mainScene;

        SceneManager.LoadScene("LOADING");
    }

    public void LogoutButtonClick()
    {
        MySQLManager.Instance.UpdateUserStatus("/api/tbUserbases/", "N", "N");

        CheckSceneData.CheckSceneDataSting = loginScene;

        SceneManager.LoadScene("0.START");
    }



    public void ExitButtonClick()
    {
        MySQLManager.Instance.UpdateUserStatus("/api/tbUserbases/", "N", "N");

        Application.Quit();
    }

    void ReceiveChat(string _remoteId,string chat)
    {
        GameObject _obj = Instantiate(_pcChatObjet, _chatTr);
        GameObject _wideObj = Instantiate(_pcChatObjet_WIDE, _wideChatTr);
        _obj.SetActive(true);
        _wideObj.SetActive(true);

        ChatBalloon _chatBalloon = _obj.GetComponent<ChatBalloon>();
        ChatBalloon _chatBalloonWide = _wideObj.GetComponent<ChatBalloon>();
        _chatBalloon.Initialize("OTHER", "", chat);
        _chatBalloonWide.Initialize("OTHER", "", chat);
    }

    void OnSubmitChat()
    {
        string _str;

        if (_isWide)
        {
            _str = _inputChat_2.text;
        }
        else
        {
            _str = _inputChat.text;
        }

        Global.GetComm().SendChatAllRemote(_str);
        GameObject _obj = Instantiate(_mobileChatObject, _chatTr);
        GameObject _wideObj = Instantiate(_mobileChatObject_WIDE, _wideChatTr);
        _obj.SetActive(true);
        _wideObj.SetActive(true);

        _inputChat.text = "";
        _inputChat_2.text = "";

        ChatBalloon chat_baloon = _obj.GetComponent<ChatBalloon>();
        ChatBalloon _chatBalloonWide = _wideObj.GetComponent<ChatBalloon>();
        chat_baloon.Initialize(Global.CURRENT_USER.userData.userbaseName, "", _str);
        _chatBalloonWide.Initialize(Global.CURRENT_USER.userData.userbaseName, "", _str);
    }

    void OnSubmitChat(string _str)
    {
        Global.GetComm().SendChatAllRemote(_str);
        GameObject _obj = Instantiate(_mobileChatObject, _chatTr);
        GameObject _wideObj = Instantiate(_mobileChatObject_WIDE, _wideChatTr);
        _obj.SetActive(true);
        _wideObj.SetActive(true);

        _inputChat.text = "";
        _inputChat_2.text = "";

        ChatBalloon chat_baloon = _obj.GetComponent<ChatBalloon>();
        ChatBalloon _chatBalloonWide = _wideObj.GetComponent<ChatBalloon>();
        chat_baloon.Initialize(Global.CURRENT_USER.userData.userbaseName, "", _str);
        _chatBalloonWide.Initialize(Global.CURRENT_USER.userData.userbaseName, "", _str);
    }



    void ChangeMode()
    {
        if (!_isWide)
        {
            _uiAnim.Play("CollaboWideAnimation");
            _isWide = true;
        }
        else
        {
            _uiAnim.Play("CollaboUiAnimation");
            _isWide = false;
        }
        _uiAnim.enabled = true;
    }

    private void OnApplicationQuit()
    {
        MySQLManager.Instance.UpdateUserStatus("/api/tbUserbases/", "N", "N");
        _mic.Stop();
        Global.GetComm().Close();
        //RTCSender.GET.Close();
    }

}