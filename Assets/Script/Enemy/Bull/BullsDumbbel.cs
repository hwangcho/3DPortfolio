using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullsDumbbel : MonoBehaviour
{
    //이펙트
    [SerializeField]
    GameObject Effect;

    void Start()
    {
        //4초뒤 삭제
        Destroy(gameObject, 4f);
    }

    void FixedUpdate()
    {
        //피격가능 레이어일때만 회전
        //땅에닿으면 회전x and 플레이어 맞춰도 회전x
        if ( gameObject.layer == 14)
        {
            transform.Rotate(new Vector3(6, 6, 6));
        }
    }
    //플레이어 or 바닦에 맞으면 피격 이펙트 나오고
    //더이상 피격안되게 레이어 변경
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player" && gameObject.layer == 14)
        {
            SoundManager.instance.Play("Hit");//효과음 플레이

            collision.gameObject.GetComponentInChildren<PlayerHealth>().TakeDamage(Random.Range(18, 25), transform.position, 15);//플레이어 피격함수 호출
            Instantiate(Effect, collision.transform.transform.position, Quaternion.identity);//이펙트 생성
            gameObject.layer = 15;//레이어 변경
        }
        if ((collision.transform.tag == "Wall" || collision.gameObject.layer == 17) && gameObject.layer == 14)
        {
            SoundManager.instance.Play("Skill");

            gameObject.layer = 15;
            Instantiate(Effect, transform.position, Quaternion.identity);
        }
    }
}
