using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//몬스터 유한상태머신
public class EnemyFSM : MonoBehaviour
{
    //몬스터 상태 이넘문
    enum EnemyState
    {
        Idle, Move, Attack, Return, Damaged, Die
    }

    EnemyState state;
    GameObject target;

    

    /// 유용한 기능

    #region "공통 변수"
    public float speed = 5.0f;      //에너미 이동 속도
    public float maxHP = 10.0f;     //에너미 최대 체력
    private float curHP;            //에너미 현재 체력
    private float distance;         //타겟과의 거리
    Vector3 dir;                    //이동 방향 - 추적이나 리턴
    CharacterController cc;         //캐릭터 컨트롤러
    #endregion

    #region "Idle 상태에 필요한 변수들"
    #endregion

    #region "Move 상태에 필요한 변수들"
    public float chaseSpeed = 5.0f;     //타겟 추적 속도
    public float chaseRange = 15.0f;    //타겟 추적 범위
    #endregion

    #region "Attack 상태에 필요한 변수들"
    public float attackRange = 5.0f;    //타겟 공격 측정 범위
    public float attackTime = 2.0f;     //공격 주기
    private float curTime = 0.0f;       //현재 타이머
    #endregion

    #region "Return 상태에 필요한 변수들"
    public float returnRange = 30.0f;   //최대 추적 범위
    public float returnSpeed = 10.0f;   //리턴할때 속도
    Vector3 returnPos;                  //리턴 지점
    EnemyState returnState;
    #endregion

    #region "Damaged 상태에 필요한 변수들"    
    #endregion

    #region "Die 상태에 필요한 변수들"
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //몬스터 상태 초기화
        state = EnemyState.Idle;

        //타겟 설정
        target = GameObject.Find("Player");

        //몬스터 HP 초기화
        curHP = maxHP;

        //되돌아갈 지점
        returnPos = transform.position;

        //캐릭터 컨트롤러
        cc = GetComponent<CharacterController>();

        
    }

    // Update is called once per frame
    void Update()
    {
        distance = (target.transform.position - transform.position).sqrMagnitude;
        //Debug.Log(distance);           

        if(distance < 1.0f * 1.0f)
        {
            StartCoroutine(Hit());
        }

        if (curHP <= 0)
        {
            state = EnemyState.Die;
        }

        //상태에 따른 행동처리
        switch (state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                Damaged();
                break;
            case EnemyState.Die:
                Die();
                break;
            default:
                break;
        }//end of switch
    }//end of void Update()

    //대기상태
    private void Idle()
    {
        //1. 플레이어와 일정범위가 되면 이동상태로 변경 (탐지)
        //- 플레이어 찾기 (GameObject.Find("Player")
        //- 일정거리 20미터 (거리비교 : Distance, Magnitude 등등) 예를 든거임
        //- 상태변경  state = EnemyState.Move;
        //- 상태전환 출력, 상태전환을 트랜지션이라고 한다.
        Debug.Log("State: Idle");
        if (distance < chaseRange * chaseRange)
        {
            state = EnemyState.Move;
        }
    }

    //플레이어 추격 상태
    private void Move()
    {
        //1. 플레이어를 향해 이동 후 공격범위 안에 들어오면 공격상태로 변경
        //2. 플레이어를 추격하더라도 처음위치에서 일정범위를 넘어가면 리턴상태로 변경
        //- 플레이어 처럼 캐릭터컨트롤러를 이용하기
        //- ex) 공격범위 2미터 1미터
        //- 상태변경
        //- 상태전환 출력
        Debug.Log("State: Move");
        dir = target.transform.position - transform.position;
        dir.Normalize();
        speed = chaseSpeed;
        cc.Move(dir * speed * Time.deltaTime);
        if (distance < attackRange * attackRange)
        {
            state = EnemyState.Attack;
        }

        if (distance > returnRange * returnRange)
        {
            state = EnemyState.Return;
        }
    }

    //공격 상태
    private void Attack()
    {
        //1. 플레이어가 공격범위 안에 있다면 일정한 시간 간격으로 플레이어 공격
        //2. 플레이어가 공겨범위를 벗어나면 이동상태(재추격)로 변경
        //- 공격범위 1미터
        //- 상태변경
        //- 상태전환 출력        
        Debug.Log("State: Attack");

        curTime += Time.deltaTime;
        if(curTime >= attackTime)
        {
            Debug.Log("Attack!!");
            curTime = 0.0f;
        }
        if (distance > attackRange * attackRange)
        {
            state = EnemyState.Idle;
        }
    }

    //복귀상태
    private void Return()
    {
        //1. 몬스터가 플레이어를 추격하더라도 처음 위치에서 일정 범위를 벗어나면 다시 돌아옴
        //- 처음위치에서 일정범위 30미터
        //- 상태변경
        //- 상태전환 출력 
        Debug.Log("State: Return");
        dir = returnPos - transform.position;
        dir.Normalize();
        speed = returnSpeed;
        cc.Move(dir * speed * Time.deltaTime);
        float returnDir = (returnPos - transform.position).sqrMagnitude;
        
        if ( returnDir < 1.0f * 1.0f)
        {
            state = EnemyState.Idle;
        }
    }

    //피격상태 (Any State)
    private void Damaged()
    {
        //코루틴을 사용하자
        //1. 몬스터 체력이 1이상
        //2. 다시 이전상태로 변경
        //- 상태변경
        //- 상태전환 출력
        Debug.Log("State: Damaged");
        curHP--;
       
    }

    //죽음상태 (Any State)
    private void Die()
    {
        //코루틴을 사용하자
        //1. 체력이 0이하
        //2. 몬스터 오브젝트 삭제
        //- 상태변경
        //- 상태전환 출력 (죽었다)
        Debug.Log("State: Die");
        Destroy(gameObject, 2.0f);
    }
    
    IEnumerator Hit()
    {
        returnState = state;
        state = EnemyState.Damaged;

        yield return null;
        state = returnState;
        StopAllCoroutines();
    }
}
