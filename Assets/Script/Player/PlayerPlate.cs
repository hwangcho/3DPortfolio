using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlate : MonoBehaviour
{
    [SerializeField]
    GameObject Effect; //피격 이펙트
    void Start()
    {
        Destroy(gameObject, 2);
    }

    
    void FixedUpdate()
    {
        //피격가능 레이어 일때만 원판 돌아감
        if (gameObject.layer == 11)
            transform.Rotate(10, 10, 10);
    }
    //날아갈 방향 설정후 addforce
    public void SetPos(GameObject player,Transform skillPos)
    {
        //플레이어 앞에 빈오브젝트만들어서 어느 방향이어도 플레이어 앞으로 날아가게함
        GetComponent<Rigidbody>().AddForce((skillPos.position-player.transform.position)*11 , ForceMode.Impulse);

    }
    //몬스터 or 바닦,벽 닿으면 레이어 변경해서 맞지않게하고 이펙트생성
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Enemy" && gameObject.layer == 11)
        {

            Instantiate(Effect, collision.transform.transform.position, Quaternion.identity);

            gameObject.layer = 15;
        }
        if ((collision.transform.tag == "Wall"||collision.gameObject.layer == 17) && gameObject.layer == 11)
        {
            SoundManager.instance.Play("Skill");
            gameObject.layer = 15;
            Instantiate(Effect, transform.position, Quaternion.identity);
        }
    }
}
