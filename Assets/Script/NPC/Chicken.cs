using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//닭
public class Chicken : MonoBehaviour
{
    Animator ani;
    float aniTime;

    [SerializeField]
    GameObject chickenLeg; //닭다리 아이템 프리펩
    // Start is called before the first frame update
    void Awake()
    {
        aniTime = Time.time;
        ani = GetComponent<Animator>();
        ani.SetBool("Turn Head", true);

    }

    void Update()
    {
        //랜덤값줘서 다음에 실행애니 시간 다르게
        if (Time.time - aniTime > Random.Range(3.1f, 5.5f)&&ani.GetBool("Eat"))
        {
            ani.SetBool("Eat", false);
        ani.SetBool("Turn Head", true);
            aniTime = Time.time;

        }
        if (Time.time - aniTime > Random.Range(2.1f, 6f) && ani.GetBool("Turn Head"))
        {
            ani.SetBool("Turn Head", false);
            ani.SetBool("Eat", true);

            aniTime = Time.time;

        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //플레이어 공격에맞으면
        if (other.gameObject.layer == 11)
        {
            Destroy(gameObject);//닭 삭제
            GameObject chickenleg = Instantiate(chickenLeg, transform.position, Quaternion.identity);//닭다리 생성
            chickenleg.GetComponent<Rigidbody>().AddForce(Vector3.up*5, ForceMode.Impulse);//드롭 된거처럼 addforce
        }
    }

}
