using UnityEngine;

public enum Boss_Dragon_States { Idle = 0, NormalAttack, Dragon_Breath, Flying, Walk, Chase }

[RequireComponent (typeof(Animator))]
public class Boss_Dragon : BaseGameEntity
{
    [SerializeField] private int hp;            // 체력
    [SerializeField] private int ap;            // 공격력 
    [SerializeField] private Phase currentPhase;                 // 현재 페이즈
    [SerializeField] private float[] playersDistance;
    [SerializeField] private float minDistance;
    [SerializeField] private GameObject[] players;
    [SerializeField] private bool isAnimationPlaying;
    public bool isAttackEnd = false;
    public float animationNormalValue = 0f;

    // Dragon이 가지고 있는 모든 상태, 현재 상태.
    private State[] states;
    private State currentState;
    [SerializeField] private Animator animator;

    public int HP
    {
        set => hp = 100;
        get => hp;
    }

    public int AP
    {
        set => ap = 100;
        get => ap;
    }

    public Phase CurrentPhase
    {
        set => currentPhase = value;
        get => currentPhase;
    }

    public float MinDistance
    {
        private set => minDistance = value;
        get => minDistance;
    }

    public bool IsAnimationPlaying
    {
        private set => isAnimationPlaying = value;
        get => isAnimationPlaying;
    }
    public Animator Animator
    {
        private set => animator = value;
        get => animator;
    }

    public override void Setup(string name)
    {
        // 기반 클래스의 Setup 메소드 호출 (ID, 이름, 색상 설정)
        base.Setup(name);

        // 생성되는 오브젝트 이름 설정
        gameObject.name = $"{ID:D2}_Student_{name}";

        // Student가 가질 수 있는 상태 개수만큼 메모리 할당, 각 상태에 클래스 메모리 할당
        states = new State[5];
        states[(int)Boss_Dragon_States.Idle] = new Boss_DragonStates.Idle();
        states[(int)Boss_Dragon_States.NormalAttack] = new Boss_DragonStates.NormalAttack();
        states[(int)Boss_Dragon_States.Dragon_Breath] = new Boss_DragonStates.Dragon_Breath();
        states[(int)Boss_Dragon_States.Flying] = new Boss_DragonStates.Flying();
        states[(int)Boss_Dragon_States.Walk] = new Boss_DragonStates.Walk();

        // 기본 상태 설정
        ChangeState(Boss_Dragon_States.Idle);

        playersDistance = new float[players.Length];

        for (int i = 0; i < players.Length; ++i)
        {
            players[i] = Instantiate(players[i], new Vector3(0, 0, 20), Quaternion.identity);
            playersDistance[i] = 0f;
        }

        hp = 100;
        ap = 0;

        minDistance = 15;
        animator = GetComponent<Animator>();
        currentPhase = Phase.Normal;
    }

    public override void Updated()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players.Length == 0)
            {
                Debug.LogError("Player is null");
            }

            playersDistance[i] = Vector3.Distance(this.transform.position, players[i].transform.position);
        }

        minDistance = Mathf.Min(playersDistance);

        if (currentState != null)
        {
            currentState.Execute(this);
        }
    }

    public void ResetNormalValue()
    {
        animationNormalValue = 0f;
    }

    public void ChangeState(Boss_Dragon_States newState)
    {
        // 새로 바꾸려는 상태가 비어있으면 상태를 바꾸지 않는다
        if (states[(int)newState] == null)
        {
            return;
        }

        // 현재 재생중인 상태가 있으면 Exit() 메소드 호출
        if (currentState != null)
        {
            currentState.Exit(this);
        }

        // 새로운 상태로 변경하고, 새로 바뀐 상태의 Enter() 메소드 호출
        currentState = states[(int)newState];
        currentState.Enter(this);
    }
}

