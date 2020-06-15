using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5.0f;  //이동속도
    CharacterController cc;     //캐릭터 컨트롤러

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

        //Vector3 moveDir = Vector3.forward * h + Vector3.right * v;
        Vector3 moveDir = new Vector3(h, 0, v);
        moveDir.Normalize();    //대각선이동 속도를 상하좌우속도와 동일하게 만들기
        //게임에 따라 일부러 대각선은 빠르게 이동하도록 하는 경우도 있다.
        //이럴때는 벡터의 정규화(노멀라이즈)를 하면 안된다.
        transform.Translate(moveDir.normalized * speed * Time.deltaTime);

        //카메라가 보는 방향으로 이동시킨다. - TransformDirection
        moveDir = Camera.main.transform.TransformDirection(moveDir);
        //transform.Translate(moveDir * speed * Time.deltaTime);

        //심각한 문제 : 하늘 날라다님, 땅 뚫음, 충돌처리 안됨 - 리지드바디를 단다. 근데 안쓸거다, 연산량이 많아서 
        //CharacterController라는 컴포넌트를 쓸 것 - 중력을 다 무시, 오로지 충돌처리만
        //캐릭터컨트롤러 컴포넌트를 사용한다
        //캐릭터컨트롤러는 충돌감지만 하고 물리가 적용안된다.
        //따라서 충돌감지를 하기 위해서는 반드시
        //캐릭터컨트롤러 컴포넌트가 제공해주는 함수로 이동처리해야 한다.
        cc.Move(moveDir * speed * Time.deltaTime);
    }
}
