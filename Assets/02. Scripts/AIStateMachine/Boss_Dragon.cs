using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum Boss_Dragon_States
{
    Idle = 0, 
    NormalAttack, 
    BreathOnAir, 
    Flying, 
    ClawAttack, 
    Defend, 
    BreathOnLand,
    Die,
    Screaming,
    Chase
}


[RequireComponent (typeof(NavMeshAgent))]
public class Boss_Dragon : BaseGameEntity
{
    [SerializeField] private int hp;            // 체력
    [SerializeField] private int ap;            // 공격력 
    [SerializeField] private Phase currentPhase;                 // 현재 페이즈
    [SerializeField] private GameObject[] players;
    [SerializeField] private EnemyAggroformat mEnemyAggroformat;
    private const float calculateDestinationterm = 0.2f;
    public EnemyAggro mEnemyAggro { get; private set; }
    public NavMeshAgent mNavMeshAgent { get; private set; }
    public bool isArrivedtoTarget { get; private set; }
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

    public Animator Animator
    {
        private set => animator = value;
        get => animator;
    }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        mNavMeshAgent = GetComponent<NavMeshAgent>();

    }

    public override void Setup(string name)
    {
        // 기반 클래스의 Setup 메소드 호출 (ID, 이름, 색상 설정)
        base.Setup(name);

        // 생성되는 오브젝트 이름 설정
        gameObject.name = $"{ID:D2}_Student_{name}";

        // Student가 가질 수 있는 상태 개수만큼 메모리 할당, 각 상태에 클래스 메모리 할당
        states = new State[10];
        states[(int)Boss_Dragon_States.Idle] = new Boss_DragonStates.Idle();
        states[(int)Boss_Dragon_States.NormalAttack] = new Boss_DragonStates.NormalAttack();
        states[(int)Boss_Dragon_States.Flying] = new Boss_DragonStates.Flying();
        states[(int)Boss_Dragon_States.Defend] = new Boss_DragonStates.Defend();
        states[(int)Boss_Dragon_States.ClawAttack] = new Boss_DragonStates.ClawAttack();
        states[(int)Boss_Dragon_States.Chase] = new Boss_DragonStates.Chase();
        states[(int)Boss_Dragon_States.Die] = new Boss_DragonStates.Die();
        states[(int)Boss_Dragon_States.Screaming] = new Boss_DragonStates.Screaming();
        states[(int)Boss_Dragon_States.BreathOnAir] = new Boss_DragonStates.BreathOnAir();
        states[(int)Boss_Dragon_States.BreathOnLand] = new Boss_DragonStates.BreathOnLand();

        for (int i = 0; i < players.Length; ++i)
        {
            players[i] = Instantiate(players[i], this.transform.position + new Vector3(0, 0, 10), Quaternion.identity);
        }
        
        mEnemyAggro = new EnemyAggro(null, players);
        mEnemyAggro.InitCurrentPlayers();

        // 기본 상태 설정
        ChangeState(Boss_Dragon_States.Idle);

        hp = 100;
        ap = 0;
        currentPhase = Phase.Normal;
        Debug.Log("SetUpComplete");
    }

    public IEnumerator UpdateDestination()
    {
        isArrivedtoTarget = false;
        while (mNavMeshAgent.remainingDistance > Double.Epsilon)
        {
            mNavMeshAgent.SetDestination(mEnemyAggro.Target.transform.position);
            yield return new WaitForSeconds(calculateDestinationterm);
        }
        
        isArrivedtoTarget = true;
    }
    
    public override void Updated()
    {
        if (currentState != null)
        {
            currentState.Execute(this);
        }
    }

    public void SetCurrentTarget()
    {
        mEnemyAggro.SetTarget(mEnemyAggroformat, this.transform.position);
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

