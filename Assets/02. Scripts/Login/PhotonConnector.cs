using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonConnector : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkRunner _runner { get; private set; }
    
    [SerializeField] NetworkManager _networkManager;
    [SerializeField] NetworkObject _networkObject;
    string _roomName;
    LoginUI _loginUI;

    Dictionary<PlayerRef, NetworkObject> _playerDict = new Dictionary<PlayerRef, NetworkObject>();

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
            SceneManager = LevelManager.Instance
        });
    }

    public void LoadSceneAsync(int index)
    {
        if (_runner.LocalPlayer)
        {
            _runner.SetActiveScene(SceneManager.GetActiveScene().buildIndex + index);
        }
    }

    public void LeaveSession()
    {
        _runner.Shutdown();
        LoadSceneAsync(-1);
    }
    
    

    #endregion

    #region Private Callbacks

    void UpdateSessionInfo(PlayerRef player)
    {
        _loginUI.SetPlayer(player);
        Debug.Log($"_runner.ActivePlayers.Count() {_runner.ActivePlayers.Count()}");
        _networkManager.PlayerCount = _runner.ActivePlayers.Count();
    }

    #endregion

    #region PhotonFusion CallBacks

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("OnPlayerJoined");
        if (runner.GameMode == GameMode.Host)
        {
            Debug.Log($"_networkManager : {_networkManager}");
            _networkManager = runner.Spawn(_networkManager, Vector3.zero, Quaternion.identity, player);
        }

        UpdateSessionInfo(player);
        if (runner.LocalPlayer)
        {
            _runner.SetActiveScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"OnPlayerLeft");
        UpdateSessionInfo(player);
        if (_playerDict.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _playerDict.Remove(player);
        }
        
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
        if (runner.LocalPlayer)
        {
            Debug.Log($"_networkObject : {_networkObject}");
            NetworkObject networkObject = runner.Spawn(_networkObject, new Vector3(56.12501f, -12.77073f, 233.6112f), Quaternion.identity, runner.LocalPlayer);
            _playerDict.Add(runner.LocalPlayer, networkObject);
        }
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.Log("OnSceneLoadStart");
    }
    #endregion
}
