using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    // 상태 표현을 위한 열거형 변수 정의
    public enum State  
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE
    }

    public State state = State.IDLE;

    public float attackDist = 2f; // 공격 거리
    public float traceDist = 5f; // 추적 거리
    public float moveSpeed = 2f;
    private float damping = 1f; // 회전 속도 조절 변수

    private Transform playerTr; // 플레이어 위치
    private Transform enemyTr; // 적 위치
    private Animator anim;
    private WaitForSeconds ws;
    private NavMeshAgent agent;
    private Rigidbody rigid;

    public bool isDie = false;

    private Vector3 _traceTarget; // 추적 대상 위치 저장 변수
    
    
    public Vector3 traceTarget
    {
        get {  return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = moveSpeed;
            damping = 7f;
            TraceTarget(_traceTarget);
        }
    }
    public float speed
    {
        get { return agent.velocity.magnitude; }
    }

    private readonly int hashMove = Animator.StringToHash("isMove");
    private readonly int hashSpeed = Animator.StringToHash("speed");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashOffset = Animator.StringToHash("Offset");
    private readonly int hashAttack = Animator.StringToHash("Attack");

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");

        if(player != null)
        {
            playerTr = player.GetComponent<Transform>();
        }
        ws = new WaitForSeconds(0.3f);
        enemyTr = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rigid.sleepThreshold = 0.0f;
        //agent.updateRotation = false;
        agent.autoBraking = false;
        agent.speed = moveSpeed;

    }
    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());

    }
    // 상태 검사 코루틴 (0.3초마다 검사)

    IEnumerator CheckState()
    {
        while (!isDie)
        {
            if (state == State.DIE) // 사망상태면 코루틴 종료
                yield break;

            // 플레이어 - 적 사이 거리 계산
            float dist = Vector3.Distance(playerTr.position, enemyTr.position);

            if(dist <= attackDist)  // 공격 거리 이내일 때
            {
                state = State.ATTACK;
            }
            else if(dist <= traceDist) // 추적 거리 이내일 때
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }

            yield return ws;
        }
    }
    // 상태에 따라 행동을 처리하는 코루틴 함수
    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return ws;

            switch (state)
            {
                case State.IDLE:
                    Stop();
                    anim.SetBool(hashMove, false);
                    break;
                case State.TRACE:
                    traceTarget = playerTr.position;
                    anim.SetBool(hashMove, true);
                    break;
                case State.ATTACK:
                    anim.SetTrigger(hashAttack);
                    Stop();
                    anim.SetBool(hashMove, false);
                    break;
                case State.DIE:
                    this.gameObject.tag = "Untagged";
                    isDie = true;
                    Stop();
                    anim.SetBool(hashDie, true);
                    break;
            }
        } 
         
    }
    public void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;

        agent.destination = pos;
        agent.isStopped = false;


    }
    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }
    public void StartMove()
    {
        agent.isStopped = false;
    }
    void Update()
    {
        //if (!agent.isStopped)
        //{
        //    Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
        //    enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        //}
    }

    //private void OnMouseOver()
    //{
    //    playerTr.GetComponent<PlayerBehaviour>().opponent = this.gameObject;
    //    Cursor.SetCursor(GameManager.instance.cursers[1], Vector2.zero, CursorMode.Auto);
    //}
    //private void OnMouseExit()
    //{
    //    Cursor.SetCursor(GameManager.instance.cursers[0], Vector2.zero, CursorMode.Auto);

    //}

}
