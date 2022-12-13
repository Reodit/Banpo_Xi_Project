using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonConnector : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkManager _networkManager;
    [SerializeField] RoomInfo _roomInfo;
    
    string _roomName;
    NetworkRunner _runner;
    LoginUI _loginUI;
    
    #region MonoBehaviour Callbacks

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            _loginUI = FindObjectOfType<LoginUI>();
        }
        if (!_runner)
        {
            _runner = gameObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;
        }
    }

    #endregion

    
    #region Public Callbacks

    public async void StartGame(GameMode gameMode, string sessionName)
    {
        await _runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            SessionName = sessionName,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
    
    #endregion

    #region Private Callbacks

    void UpdateSessionInfo(PlayerRef player)
    {
        Debug.Log($"_runner.SessionInfo.PlayerCount : {_runner.SessionInfo.PlayerCount}");
        Debug.Log($"NetworkManager.Instance : {NetworkManager.Instance}");
        Debug.Log($"_runner.SessionInfo : {_runner.SessionInfo}");
        NetworkManager.Instance.PlayerCount = _runner.SessionInfo.PlayerCount;
        _loginUI.SetPlayer(player);
    }

    #endregion

    #region PhotonFusion CallBacks

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("OnPlayerJoined");
        if (runner.GameMode == GameMode.Host)
        {
            runner.Spawn(_networkManager, Vector3.zero, Quaternion.identity, player);
        }
        // runner.Spawn(_roomInfo, Vector3.zero, Quaternion.identity, player);
        UpdateSessionInfo(player);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"OnPlayerLeft");
        UpdateSessionInfo(player);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.LogError($"OnShutdown : {shutdownReason}");
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("OnConnectedToServer");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        Debug.Log("OnDisconnectedFromServer");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.Log("OnConnectRequest");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log($"OnConnectFailed : {reason}");
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        Debug.Log("OnUserSimulationMessage");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log("OnSessionListUpdated");
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        Debug.Log("OnCustomAuthenticationResponse");
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log("OnHostMigration");
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        Debug.Log("OnReliableDataReceived");
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log("OnSceneLoadDone");
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.Log("OnSceneLoadStart");
    }
    #endregion

    
}
