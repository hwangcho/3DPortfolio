using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDumbbel : MonoBehaviour
{
    [SerializeField]
    GameObject Effect;//덤벨 이펙트
    Transform targetPos; //타게팅 위치
    Vector3 dir; //발사위치
    float rollTime; //덤벨 도는시간
    void Start()
    {
        Destroy(gameObject, 3f); //3초뒤 삭제
        gameObject.layer = 15;// 던지기전에는 맞지않게 레이어 변경
        rollTime = Time.time; //시간 초기화
    }


    void FixedUpdate()
    {
        //덤벨 도는 조건
        if(gameObject.layer == 11||Time.time-rollTime <0.7f)
             transform.Rotate(7, 7, 7);

    }

    //몬스터 위치 받아와서 코루틴 실행함수
    public void SetTargetPos(Transform target)
    {
        StartCoroutine(a(target));
 
    }
    //덤벨 던지는 함수
    IEnumerator a(Transform target)
    {
        yield return new WaitForSeconds(0.7f);
        gameObject.layer = 11; //몬스터에 맞는 레이어로 변경

        GetComponent<Rigidbody>().useGravity = true; 
        targetPos = target; //받아온 매개변수 넣어줌
        dir = ((targetPos.position + new Vector3(0, 1.5f, 0)) - transform.position).normalized; //날아갈 위치 선정
        GetComponent<Rigidbody>().AddForce(dir * 20, ForceMode.Impulse);
    }
    //몬스터 or 벽,바닦에 닿으면 이펙트 나오면서 공격불가 레이어로 변경
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Enemy" && gameObject.layer == 11)
        {
           
            Instantiate(Effect, collision.transform.transform.position, Quaternion.identity);

            gameObject.layer = 15;
        }
        if ((collision.transform.tag == "Wall" || collision.gameObject.layer == 17) && gameObject.layer == 11 )
        {
            SoundManager.instance.Play("Skill");

            gameObject.layer = 15;
            Instantiate(Effect, transform.position, Quaternion.identity);
        }
    }
}
