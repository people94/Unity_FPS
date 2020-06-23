using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed = 5.0f;  //이동속도
    CharacterController cc;     //캐릭터 컨트롤러
    float jumpPower = 10;       //점프 속도
    int curJumpCnt = 0;         //점프 카운트
    int maxJumpCnt = 2;

    //중력적용
    public float gravity = -20f;
    float velocityY;    //낙하속도(벨로시티는 방향과 힘을 들고 있다), 속도 : 방향 + 힘 


    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //Vector3 dir = Vector3.forward * h + Vector3.right * v;
        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();    //대각선이동 속도를 상하좌우속도와 동일하게 만들기
        //게임에 따라 일부러 대각선은 빠르게 이동하도록 하는 경우도 있다.
        //이럴때는 벡터의 정규화(노멀라이즈)를 하면 안된다.
        transform.Translate(dir.normalized * speed * Time.deltaTime);

        //카메라가 보는 방향으로 이동시킨다. - TransformDirection
        dir = Camera.main.transform.TransformDirection(dir);
        //transform.Translate(dir * speed * Time.deltaTime);

        //심각한 문제 : 하늘 날라다님, 땅 뚫음, 충돌처리 안됨 - 리지드바디를 단다. 근데 안쓸거다, 연산량이 많아서 
        //CharacterController라는 컴포넌트를 쓸 것 - 중력을 다 무시, 오로지 충돌처리만
        //캐릭터컨트롤러 컴포넌트를 사용한다
        //캐릭터컨트롤러는 충돌감지만 하고 물리가 적용안된다.
        //따라서 충돌감지를 하기 위해서는 반드시
        //캐릭터컨트롤러 컴포넌트가 제공해주는 함수로 이동처리해야 한다.
        //cc.Move(dir * speed * Time.deltaTime);

        //중력적용하기
        //velocityY += gravity * Time.deltaTime;
        //dir.y = velocityY;
        //cc.Move(dir * speed * Time.deltaTime);

        //캐릭터 점프
        //점프버튼을 누르면 수직속도에 점프파워를 넣는다
        //땅에 닿으면 0으로 초기화
        //if(cc.isGrounded)   //땅에 닿았냐? 얘랑
        //{
        //
        //}
        //if (cc.collisionFlags == CollisionFlags.Below)   //어디가 충돌했는지 얘랑 같은 의미
        //{
        //    curJumpCnt = 0;
        //    velocityY = 0;
        //}
        //if (curJumpCnt < maxJumpCnt)
        //{
        //    if (Input.GetButtonDown("Jump"))
        //    {
        //        curJumpCnt++;
        //        velocityY = jumpPower;
        //    }
        //}
        //CollisionFlags.Above;
        //CollisionFlags.Below;
        //CollisionFlags.Sides

        //2단 점프만 가능하도록 만들기
        if (cc.isGrounded)
        {
            velocityY = 0;
            curJumpCnt = 0;
        }
        else
        {
            //땅에 닿지 않은 상태이기 때문에 중력적용하기
            velocityY += gravity * Time.deltaTime;
            dir.y = velocityY;
        }
        if(Input.GetButtonDown("Jump") && curJumpCnt < 2)
        {
            curJumpCnt++;
            velocityY = jumpPower;
        }
        cc.Move(dir * speed * Time.deltaTime);
    }
}