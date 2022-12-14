using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    public PlayerRef Player { get; private set; }
    
    [SerializeField] GameObject _loginPanel;
    [SerializeField] Button _loginButton;
    [SerializeField] GameObject _loadingPanel;
    [SerializeField] GameObject _roomPanel;
    [SerializeField] TMP_Text _nickNameText;
    [SerializeField] TMP_InputField _loginInputField;
    [SerializeField] TMP_InputField _roomNameInputField;
    [SerializeField] Button _createButton;
    [SerializeField] Button _joinButton;
    [SerializeField] GameObject _roomListPanel;
    [SerializeField] GameObject _content;
    [SerializeField] PlayerInfo playerInfo;
    [SerializeField] TMP_Text _roomNameText;
    [SerializeField] Button _roomBackButton;
    
    
    string _nickName;
    PhotonConnector _photonConnector;
    PlayfabConnector _playfabConnector;
    Dictionary<PlayerRef, PlayerInfo> _roomInfoDict = new Dictionary<PlayerRef, PlayerInfo>();
    
    static Dictionary<PlayerRef, string> _playerDict = new Dictionary<PlayerRef, string>();

    #region MonoBehaviourCallbacks
    
    void Start()
    {
        _photonConnector = FindObjectOfType<PhotonConnector>();
        _playfabConnector = FindObjectOfType<PlayfabConnector>();
        _loginInputField.onValueChanged.AddListener(OnInputFieldChanged);
        _loginButton.onClick.AddListener(OnClickLoginButton);
        _createButton.onClick.AddListener(OnClickCreateButton);
        _joinButton.onClick.AddListener(OnClickJoinButton);
        _roomBackButton.onClick.AddListener(OnClickBackButton);
        
        
        // NetworkManager.OnlobbyChanged += ChangeLobbyState;
    }
    #endregion

    
    #region Public Callbacks

    public void ActiveLobbyPanel()
    {
        _loadingPanel.SetActive(false);
        _roomPanel.SetActive(true);
    }

    public void SetPlayer(PlayerRef player)
    {
        Player = player;
    }

    public void SetRoomName(string roomName)
    {
        _roomNameText.text = roomName;
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
            _loadingPanel.SetActive(true);
            UpdateNickName();
        }
        
    }

    void UpdateNickName()
    {
        _nickNameText.text = _nickName;
    }
    
    // 방 만들기
    void OnClickCreateButton()
    {
        string sessionName = _roomNameInputField.text;
        _photonConnector.StartGame(GameMode.Host, sessionName);
        _roomPanel.SetActive(false);
        _loadingPanel.SetActive(true);
        
    }
    
    // 방 참가
    void OnClickJoinButton()
    {
        string sessionName = _roomNameInputField.text;
        _photonConnector.StartGame(GameMode.Client, sessionName);
        _roomPanel.SetActive(false);
        _loadingPanel.SetActive(true);
    }

    void OnClickBackButton()
    {
        _loginPanel.SetActive(true);
        _roomPanel.SetActive(false);
    }
    
    void OnClickLeaveButton()
    {
        _photonConnector.LeaveSession();
    }


    void ChangeLobbyState(NetworkManager networkManager)
    {
        if (_roomInfoDict.ContainsKey(Player))
        {
            Destroy(_roomInfoDict[Player].gameObject);
            _roomInfoDict.Remove(Player);
        }
        else
        {
            if (_roomInfoDict.Count < networkManager.PlayerCount)
            {
                foreach (PlayerRef player in networkManager.Runner.ActivePlayers)
                {
                    if (!_roomInfoDict.ContainsKey(player))
                    {
                        PlayerInfo playerInfo = Instantiate(this.playerInfo, Vector3.zero, Quaternion.identity, _content.transform);
                        if (!_playerDict.ContainsKey(player) && networkManager.Runner.LocalPlayer)
                        {
                            _playerDict.Add(player, _nickName);
                        }
                        playerInfo.SetPlayer(player, _playerDict[player]);
                        _roomInfoDict.Add(player, playerInfo);
                    }
                }
            }
        }

        if (_loadingPanel.activeInHierarchy)
        {
            _loadingPanel.SetActive(false);
            _roomListPanel.SetActive(true);
        }
    }

    #endregion

    #region PhotonFusionCallBacks
    
    #endregion
}
