using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BullHealth : MonoBehaviour
{
    //시작체력 
    public int startingHealth;
    //현재체력
    public int currentHealth;
    public bool Dead = false;

    Rigidbody rigid;
    Animator ani;
    Bull bull;

    [SerializeField]
    Slider hpSlider;//체력슬라이더
    [SerializeField]
    GameObject coin;
    void Start()
    {
        currentHealth = startingHealth;//현재체력 시작체력으로 초기화
        ani = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        bull = GetComponent<Bull>();
        hpSlider.maxValue = startingHealth; //슬라이더 최대값 시작체력으로 변경

    }

    void Update()
    {
        hpSlider.value = currentHealth;//슬라이더 업데이트


    }

    //피격 함수
    public void TakeDamage(int amout, Vector3 EnemyPosition, float pushBack)
    {
        //체력깎음
        currentHealth -= amout;


        //만약 현재 체력이 0이하가되면 죽음
        if (currentHealth <= 0 && !Dead)
        {
            
            PlayerDie();
        }
        else
        {
            //공격맞앗을때 플레이어의 위치에따라 밀라는 위치 다르게
            Vector3 diff = transform.position - EnemyPosition;

            diff = diff / diff.sqrMagnitude;
            rigid.AddForce((new Vector3(diff.x, diff.y, diff.z)) * pushBack, ForceMode.Impulse);

        }
    }
    //죽음 함수
    void PlayerDie()
    {
        ani.Rebind();//실행중 애니 초기화
        PlayerTarget.targetOn = false; //플레이어 타게팅 끔

        StatManager.instance.getEXP(1120); //경험치 획득

        bull.enabled = false;//스크립트 비활성화
        gameObject.layer = 8;//안맞게 레이어 변경
        ani.SetTrigger("Death");
        Destroy(gameObject, 6f);

        Dead = true;

        //코인 30개 생성
        for(int i = 0; i < 30; i++)
        {
            GameObject coins = Instantiate(coin, transform.position + Vector3.up, Quaternion.identity);
            coins.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }

  
}
