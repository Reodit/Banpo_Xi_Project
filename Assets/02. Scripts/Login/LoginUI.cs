using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [SerializeField] GameObject _loginPanel;
    [SerializeField] Button _loginButton;
    [SerializeField] GameObject _roomPanel;
    [SerializeField] TMP_Text _nickNameText;
    [SerializeField] TMP_InputField _loginInputField;
    [SerializeField] TMP_InputField _roomNameInputField;
    [SerializeField] Button _createButton;
    [SerializeField] Button _joinButton;
    [SerializeField] GameObject _content;
    [SerializeField] GameObject _roomInfo;

    string _nickName;
    PhotonConnector _photonConnector;
    PlayfabConnector _playfabConnector;
    List<RoomInfo> _roomList = new List<RoomInfo>();


    #region MonoBehaviourCallbacks
    
    void Start()
    {
        _photonConnector = FindObjectOfType<PhotonConnector>();
        _playfabConnector = FindObjectOfType<PlayfabConnector>();
        _loginInputField.onValueChanged.AddListener(OnInputFieldChanged);
        _loginButton.onClick.AddListener(OnClickLoginButton);
        _createButton.onClick.AddListener(OnClickCreateButton);
        _joinButton.onClick.AddListener(OnClickJoinButton);
        
    }
    #endregion

    
    #region Public Callbacks

    
    public void InstantiateRoomInfo()
    {
        Instantiate(_roomInfo, Vector3.zero, Quaternion.identity, _content.transform); 
    }
    
    #endregion

    #region Private Callbacks

    void OnInputFieldChanged(string nickName)
    {
        _loginInputField.text = nickName;
        PlayerPrefs.SetString("NICKNAME" , nickName);
    }

    void OnClickLoginButton()
    {
        _nickName = _playfabConnector.Login();
        if (!string.IsNullOrEmpty(_nickName))
        {
            _loginPanel.SetActive(false);
            _roomPanel.SetActive(true);
            UpdateNickName();
        }
    }

    void UpdateNickName()
    {
        _nickNameText.text = PlayerPrefs.GetString("NICKNAME");
    }
    
    // 방 만들기
    void OnClickCreateButton()
    {
        string sessionName = _roomNameInputField.text;
        _photonConnector.StartGame(GameMode.Host, sessionName);
        // 룸 참가 후 UI 넣기
        
    }
    
    // 방 참가
    void OnClickJoinButton()
    {
        string sessionName = _roomNameInputField.text;
        _photonConnector.StartGame(GameMode.Client, sessionName);
        // 룸 참가 후 UI 넣기
    }
    
    

    

    #endregion

    #region PhotonFusionCallBacks
    
    
    
    #endregion

    
}
