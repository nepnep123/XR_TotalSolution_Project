using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebRTCWrapper;
using UnityEngine.SceneManagement;

public class CommunicatorManager : Singleton<CommunicatorManager>
{
    [SerializeField] CallPopup _callPopup;

    //static CommunicatorManager CommManager;
    void Awake()
    {
        Global.CURRENT_USER = new RemoteAgent(MySQLManager.Instance.currentUserData);

        Global.GetComm().Initialize(Global.CURRENT_USER.userData.userbaseId);
        Global.GetComm().OnCallChecked += OnCallChecked;
        Global.GetComm().OnConnected += OnConnected;
        Global.GetComm().OnConnected += OnConnected;
    }

    private void OnEnable()
    {
        StartCoroutine(startCallCheck());
    }


    bool isPopUp = false;
    IEnumerator startCallCheck()
    {
        while (!isPopUp)
        {
            yield return new WaitForSeconds(1.0f);
            Global.GetComm().CallChecking();
        }
    }

    void OnCallChecked(string caller_id)
    {
        Debug.Log(caller_id);

        if (!string.IsNullOrEmpty(caller_id))
        {
            if (caller_id.Equals("XR0006"))
            {
                CallPopup item = Instantiate(_callPopup, gameObject.transform);

                for (int i = 0; i < MySQLManager.Instance.tbUserbases._embedded.tbUserbases.Count; i++)
                {
                    if(MySQLManager.Instance.tbUserbases._embedded.tbUserbases[i].userbaseId == "XR0006")
                    {
                        item.com_txt.text = MySQLManager.Instance.tbUserbases._embedded.tbUserbases[i].userbaseCompany;
                        item.pos_txt.text = MySQLManager.Instance.tbUserbases._embedded.tbUserbases[i].userbasePosition;
                        item.name_txt.text = MySQLManager.Instance.tbUserbases._embedded.tbUserbases[i].userbaseName;
                    }
                }

                item._callString = caller_id;
                isPopUp = true;
                //CallPopupOn(caller_id);
            }

            // Call을 받는 기능이 필요하면 구현
            // StartCoroutine(DelayedCallChecking());
        }
    }

    public void CallPopupOn(string caller_id)
    {
        Global.GetComm().AcceptCall(caller_id);
    }

    IEnumerator DelayedCallChecking()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            Global.GetComm().CallChecking();
        }
    }

    void OnConnected(string caller_id)
    {
        MySQLManager.Instance.UpdateUserStatus("/api/tbUserbases/", "Y", "Y");

        SceneManager.LoadScene("4.REALCALL");
    }
}