using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using System;
using Invector.vCharacterController;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    public static BasicSpawner basicSpawner;
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    public GameObject self;
    public vNThirdPersonInput vNInput;
    public vThirdPersonController vControl;

    NetworkRunner _runner;
    [Header("Inputs")]
    private bool _mouseButton0;
    private bool _mouseButton1;
    //[Header("Camera")]
    //[SerializeField] private GameObject _playerCamPrefab;


    void Click()
    {
        StartGame(GameMode.Single);
    }
    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }


    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "Fusion Input Testing",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }


    public void OnConnectedToServer(NetworkRunner runner)
    {

    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {

    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }


    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 3, 1, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);

            // Keep track of the player avatars so we can remove it when they disconnect
            _spawnedCharacters.Add(player, networkPlayerObject);
        }
    }
    IEnumerator WaitSelf()
    {
        yield return new WaitUntil(() => self != null);
        Debug.Log(self);
        vNInput = self.GetComponent<vNThirdPersonInput>();
        vControl = self.GetComponent<vThirdPersonController>();
    }


    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        // Find and remove the players avatar
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

    private void Start()
    {
        basicSpawner = this;
    }

    private void Update()
    {
        _mouseButton0 = _mouseButton0 | Input.GetMouseButton(0);
        _mouseButton1 = _mouseButton1 || Input.GetMouseButton(1);
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();
        //Debug.Log(runner);
        //Debug.Log(runner.LocalPlayer);

        if (self)
        {
            data.velocity = vControl.GetVelocity();
            data.direction = vControl.GetDirection();
            if (Input.GetKey(vNInput.strafeInput))
                data.strafe = Input.GetKey(vNInput.strafeInput);
            data.strafe = false;
            if (Input.GetKey(vNInput.sprintInput))
                data.sprint = Input.GetKey(vNInput.sprintInput);
            data.sprint = false;


        }


        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            data.zAxis = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            data.xAxis = Input.GetAxis("Horizontal");

        //if (Input.GetKeyDown(strafeInput))

            //if (Input.GetKey(KeyCode.W))
            //    data.direction += Vector3.forward;

            //if (Input.GetKey(KeyCode.S))
            //    data.direction += Vector3.back;

            //if (Input.GetKey(KeyCode.A))
            //    data.direction += Vector3.left;

            //if (Input.GetKey(KeyCode.D))
            //    data.direction += Vector3.right;


        if (_mouseButton0) { data.buttons |= NetworkInputData.MOUSEBUTTON1; }
        _mouseButton0 = false;

        if (_mouseButton1) { data.buttons |= NetworkInputData.MOUSEBUTTON2; }
        _mouseButton1 = false;

        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {

    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {

    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }

    
}
