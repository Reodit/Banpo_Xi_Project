using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    // Networked 속성의 파라미터로 OnChanged라는 string 프로퍼티가 있는데
    // OnChanged는 'static void OnChanged(Changed<MyClass> changed){}' 형식의 콜백이다.
    [Networked(OnChanged = nameof(OnBallSpawned))]
    // NetworkBool처럼 true false 값이 두 가지만 있는 경우 변경사항이 감지되지 않을 수가 있으므로
    // byte/int 로 대체하고 호출할 때마다 값을 변경하는 방식으로 할수도 있다. (시각적 효과와 대역폭의 소모 중요도 차이)
    public NetworkBool spawned { get; set; }
    
    [SerializeField] private Ball _prefabBall;
    [SerializeField] private PhysxBall _prefabPhysxBall;
    [Networked] private TickTimer delay { get; set; }
    
    private NetworkCharacterControllerPrototype _cc;
    private Vector3 _forward;

    Text _messages;
    
    Material _material;
    Material material
    {
        get
        {
            if (!_material)
            {
                _material = GetComponentInChildren<MeshRenderer>().material;
            }
            return _material;
        }
    }
    
    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterControllerPrototype>();
        Debug.Log(_cc);
    }

    void Update()
    {
        // Object.HasInputAuthority는 NetworkObject.Spawn()에서 생성할 때 파라미터로 Runner를 정해주는데 결국 생성이 되었냐에 대한 것이다.
        if (Object.HasInputAuthority && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log(Runner.UserId);
            
            
            // Photon Server Class 코드
            // Runner
            
            //Runner << User

            RpcInfo info = RpcInfo.FromLocal(Runner, RpcChannel.Reliable, RpcHostMode.SourceIsHostPlayer);
            RPC_SendMessage("Hey Mate!");
        }
    }

    // RpcSources : 전송할 수 있는 피어
    // RpcTargets : 피어가 실행되는 피어
    // 1. All : 모두에게 전송 / 세션 내의 모든 피어에 의해서 실행됨(서버 포함)
    // 2. Proxies : 나 말고 전송 / 객체에 대하여 입력 권한 또는 상태 권한을 갖고 있지 않는 피어에 의해 실행됨
    // 3. InputAuthority : 입력 권한 있는 피어만 전송 / 객체에 대한 입력 권한이 있는 피어에 의해 실행됨
    // 4. StateAuthority : 상태 권한 있는 피어만 전송 / 객체에 대한 상태 권한이 있는 피어에 의해 실행됨
    // RpcInfo
    // - Tick : 어떤 곳에서 틱이 전송되었는지
    // - Source : 어떤 플레이어(PlayerRef)가 보냈는지
    // - Channel : Unrealiable 또는 Reliable RPC로 보냈는지 여부
    // - IsInvokeLocal : 이 RPC를 원래 호출한 로컬 플레이어인지의 여부
    // * 공식 문서엔 HostMode를 설정하지 않았지만 이걸 쓰지 않으면 계속 원격 플레이어가 된다. (기본이 서버 모드여서 그런 듯)
    [Rpc(RpcSources.All, RpcTargets.InputAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendMessage(string message, RpcInfo info = default)
    {
        if (!_messages)
        {
            _messages = FindObjectOfType<Text>();
        }
        
        Debug.Log(info.Channel);
        Debug.Log(info.Source.RawEncoded);
        Debug.Log(info.Source.PlayerId);
        Debug.Log(Runner.Simulation.LocalPlayer.PlayerId); 
        if (info.Source == Runner.Simulation.LocalPlayer)
        {
            message = $"You said: {message}\n";
        }
        else
        {
            message = $"Some other player said: {message}\n";
        }

        _messages.text += message;
    }

    // struct인 Changed<T>의 T는 NetworkBehaviour를 상속받는 컴포넌트이다.
    public static void OnBallSpawned(Changed<Player> changed)
    {
        // changed.Behaviour가 T이다. (여기선 Player)
        changed.Behaviour.material.color = Color.white;
    }

    // NetworkBehaviour를 상속하는 SimulationBehaviour에 있는 메서드이다.
    // 색 변화 등을 Update가 아니라 Render에서 수행하는 이유는
    // Render()가 퓨전의 FixedUpdateNetwork 이후에 호출되는 것이 보장되기 때문이다.
    // 또한 Runner.DeltaTime이 아니라 Time.deltaTime을 사용하는 이유는
    // 렌더링 자체는 Fusion 시뮬레이션 단위인 Runner.DeltaTime이 아니고
    // 유니티의 render loop에서 실행되어서 그렇다.
    public override void Render()
    {
        material.color = Color.Lerp(material.color, Color.blue, Time.deltaTime);
    }

    public override void FixedUpdateNetwork()
    {
        if(GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _cc.Move(5 * data.direction * Runner.DeltaTime);
            
            if (data.direction.sqrMagnitude > 0)
                _forward = data.direction;
            
            if (delay.ExpiredOrNotRunning(Runner))
            {
                if ((data.buttons & NetworkInputData.MOUSEBUTTON1) != 0)
                {
                    delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                    Runner.Spawn(_prefabBall,
                        transform.position+_forward, Quaternion.LookRotation(_forward),
                        Object.InputAuthority, (runner, o) =>
                        {
                            // Initialize the Ball before synchronizing it
                            o.GetComponent<Ball>().Init();
                        });
                    spawned = !spawned;
                }
                else if ((data.buttons & NetworkInputData.MOUSEBUTTON2) != 0)
                {
                    delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                    Runner.Spawn(_prefabPhysxBall,
                        transform.position+_forward,
                        Quaternion.LookRotation(_forward),
                        Object.InputAuthority,
                        (runner, o) =>
                        {
                            o.GetComponent<PhysxBall>().Init( 10*_forward );
                        });
                }
            }
        }
    }
}