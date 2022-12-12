using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Cinemachine;
using Invector.vCharacterController;

public class Player : NetworkBehaviour
{
    //private NetworkCharacterControllerPrototype _cc;
    private NetworkCharacterController _cc;



    [SerializeField] private Ball _prefabBall;
    private Vector3 _forward;
    [Networked] private TickTimer delay { get; set; }



    [Header("Camera")]
    [SerializeField] private GameObject _playerCamPrefab;
    GameObject _playerCam;
    CinemachineVirtualCamera _vcam;


    private void Awake()
    {
        //_cc = GetComponent<NetworkCharacterControllerPrototype>();
        _cc = GetComponent<NetworkCharacterController>();
        _forward = transform.forward;        
    }

    private void Start()
    {
        // Create a local cam and set virtual camera
        if (Object.HasInputAuthority)
        {
            _playerCam = Instantiate(_playerCamPrefab, transform.position - _forward, Quaternion.identity);            

            _vcam = _playerCam.transform.GetChild(1).GetComponent<CinemachineVirtualCamera>();
            _vcam.LookAt = transform;
            _vcam.Follow = transform;

            BasicSpawner.basicSpawner.self = gameObject;
            BasicSpawner.basicSpawner.vNInput = GetComponent<vNThirdPersonInput>();
            BasicSpawner.basicSpawner.vControl = GetComponent<vThirdPersonController>();
            BasicSpawner.basicSpawner.GetComponent<NetworkRunner>().SetPlayerObject(Runner.LocalPlayer, Object);
        }

    }
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _cc.Velocity = data.velocity * Runner.DeltaTime;

            //Debug.Log("Data: "+data.velocity);
            //Debug.Log("_cc: "+_cc.Velocity);

            //_cc.Move(5 * data.direction * Runner.DeltaTime);

            if (data.direction.sqrMagnitude > 0)
            {
                // input 입력이 있을 때 해당 direction을 forward로 삼음
                _forward = data.direction;
            }
            //if (delay.ExpiredOrNotRunning(Runner)) // timer 지나기 전이거나 아직 타이머 시작 전일 때만 ball 생성 가능
            //{
            //    if ((data.buttons & NetworkInputData.MOUSEBUTTON1) != 0)
            //    {
            //        delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
            //        Runner.Spawn(_prefabBall, transform.position + _forward, Quaternion.LookRotation(_forward),
            //            Object.InputAuthority,
            //            (runner, o) => { 
            //                // Initialize the Ball before synchronizing it
            //                o.GetComponent<Ball>().Init(); 
            //            }
            //        );
            //    }
            //}

        }
        
        
    }
}