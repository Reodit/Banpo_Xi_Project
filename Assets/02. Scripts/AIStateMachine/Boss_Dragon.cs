using System;
using System.Collections;
using Boss_DragonStates;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(NavMeshAgent))]
public class Boss_Dragon : BaseGameEntity
{
    public int HP;                                              // 체력
    [SerializeField]
    int MaxHP = 10000;
    [SerializeField]
    int page2HP = 7000;
    [SerializeField]
    int page3HP = 4000;



    [SerializeField] private int ap;                            // 공격력 
    [SerializeField] private Phase currentPhase;                 // 현재 페이즈
    private EnemyAggroformat mEnemyAggroformat;
    private GameObject[] players;
    private const float CALCULATE_DESTINATIONTERM = 0.2f;
    public EnemyAggro mEnemyAggro { get; private set; }
    public NavMeshAgent mNavMeshAgent { get; private set; }
    public bool IsInvincible;
    public bool IsCurrentAnimaitionStart;
    public bool IsPlayerExistNearby;
    public float idleTime = 3f;
    // Dragon이 가지고 있는 모든 상태, 현재 상태.
    private State[] states;
    private State currentState;
    private Animator animator;
    private EnemyFOV enemyFOV;

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

    private void Start()
    {
        enemyFOV.InitFOV();
    }

    public override void Setup(string name)
    {
        // 기반 클래스의 Setup 메소드 호출 (ID, 이름, 색상 설정)
        base.Setup(name);

        // 생성되는 오브젝트 이름 설정
        gameObject.name = $"{ID:D2}{name}";

        // Dragon 상태 개수만큼 메모리 할당, 각 상태에 클래스 메모리 할당
        states = new State[11];
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
        states[(int)Boss_Dragon_States.ChaseOnAir] = new Boss_DragonStates.ChaseOnAir();

        players = GameObject.FindGameObjectsWithTag("Player");
        
        mEnemyAggro = new EnemyAggro(null, players);
        mEnemyAggro.InitCurrentPlayers();

        HP = MaxHP;
        ap = 0;
        currentPhase = Phase.Normal;
        IsInvincible = false;

        enemyFOV = new EnemyFOV();
        // 기본 상태 설정
        ChangeState(Boss_Dragon_States.Idle);
        Debug.Log("SetUpComplete");
    }

    private IEnumerator UpdateDestination()
    {
        mNavMeshAgent.SetDestination(mEnemyAggro.Target.transform.position);

        while (mNavMeshAgent.remainingDistance > 0.1f)
        {
            mNavMeshAgent.SetDestination(mEnemyAggro.Target.transform.position);
            yield return new WaitForSeconds(CALCULATE_DESTINATIONTERM);
        }
        IsPlayerExistNearby = true;
    }
    
    public void DestinationCoroutineStart()
    {
        StartCoroutine(UpdateDestination());
    }

    public void CurrentAnimtionPlayCheck()
    {
        if (Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f &&
            !IsCurrentAnimaitionStart)
        {
            IsCurrentAnimaitionStart = true;
        }
    }
    
    public override void Updated()
    {
        if (currentState != null)
        {
            Debug.Log(currentPhase);
            CurrentAnimtionPlayCheck();
            currentState.Execute(this);
            
            if (currentPhase == Phase.Normal && HP <= page2HP)
            {
                IsInvincible = true;
                currentPhase = Phase.FireAttackPhase;
                ForcedChangeState(Boss_Dragon_States.Screaming);
            }
            else if (currentPhase == Phase.FireAttackPhase && HP <= page3HP)
            {
                IsInvincible = true;
                currentPhase = Phase.FlyAttackPhase;
                ForcedChangeState(Boss_Dragon_States.Screaming);
            }
            else if (currentPhase == Phase.FlyAttackPhase && HP <= 0)
            {
                IsInvincible = true;
                currentPhase = Phase.Die;
                ForcedChangeState(Boss_Dragon_States.Die);
            }
        }
    }

    public void ForcedChangeState(Boss_Dragon_States newforceState)
    {
        ChangeState(newforceState);
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
            IsCurrentAnimaitionStart = false;
            currentState.Exit(this);
        }

        // 새로운 상태로 변경하고, 새로 바뀐 상태의 Enter() 메소드 호출
        currentState = states[(int)newState];
        currentState.Enter(this);
    }
}

