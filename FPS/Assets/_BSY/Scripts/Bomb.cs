using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    //폭탄의 역할
    //예전 총알은 생성하면 지 스스로 날라가다 충돌하면 터졌다
    //하지만 폭탄은 생성되자마자 스스로 이동하면 안된다.
    //폭탄은 플레이어가 직접 던져야 한다
    //폭탄이 다른 오브젝트들과 충돌하면 터져야 한다.

    public GameObject fxFactory;

    private void OnCollisionEnter(Collision collision)
    {
        //폭발이펙트 보여주기
        GameObject fx = Instantiate(fxFactory);
        fx.transform.position = transform.position;
        //혹시나 이펙트 오브젝트가 사라지지 않는경우
        //Destroy(fx, 2.0f); //2초후에 폭발이펙트 삭제
        //다른 오브젝트도 삭제하기
        //자기자신 파괴시키기(맨마지막에 처리)
    }
}
