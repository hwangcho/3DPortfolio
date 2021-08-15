using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class KnightHealth : MonoBehaviour
{
    Vector3 startPos;//시작위치

    //시작 체력
    public int startingHealth = 100;
    //현재 체력
    public int currentHealth;

    public bool Dead = false;

    Rigidbody rigid;
    Animator ani;
    NavMeshAgent nav;

    Knight knight;
    [SerializeField]
    Slider hpSlider;

    //아이템 프리펩들
    [SerializeField]
    GameObject coin;
    [SerializeField]
    GameObject hpShake;
    [SerializeField]
    GameObject mpShake;

    int randomItem; //아이템 랜덤
    int randomCoin;//코인 랜덤


    void Start()
    {
        currentHealth = startingHealth;//체력 초기화
        ani = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        knight = GetComponent<Knight>();
        nav = GetComponent<NavMeshAgent>();
        hpSlider.maxValue = startingHealth;//슬라이더 최대값 변경
        startPos = transform.position;//시작 위치값 선언

    }

    
    void Update()
    {
        hpSlider.value = currentHealth; //체력 슬라이더 업데이트
    }

    //피격 함수
    public void TakeDamage(int amout, Vector3 EnemyPosition, float pushBack)
    {
        //처음엔 체력 슬라이드 숨겨놨다가 
        //피격시 슬라이더 활성화
        if(!hpSlider.gameObject.activeSelf)
            hpSlider.gameObject.SetActive(true);
        //체력깎음
        currentHealth -= amout;
    
        //몬스터가 공격할때는 피격애니메이션 안나오고 
        //공격할수 있게
        if (Time.time - knight.attackTime > 0.5f && !Dead)
        {
            ani.Rebind();
            ani.SetTrigger("Hit");     
        }



        //만약 현재 체력이 0이하가되면 죽음
        if (currentHealth <= 0 && !Dead)
        {
            Die();
        }
        else
        {
            //플레이어와 몬스터의 거리값 찾아서
            //밀쳐냄
            Vector3 diff = transform.position - EnemyPosition;
            diff = diff / diff.sqrMagnitude;
            rigid.AddForce((new Vector3(diff.x, diff.y, diff.z)) * pushBack, ForceMode.Impulse);

        }
    }
    //죽는 함수
    void Die()
    {
        PlayerTarget.targetOn = false; //플레이어의 타게팅 초기화

        StatManager.instance.getEXP(25);//경험치
        ani.Rebind();//애니메이터 초기화후
        ani.SetTrigger("Death");//죽는 애니
        knight.CancelInvoke("NavOn");//죽어서 NavOn되면 죽어서 따라옴 그래서 취소
        knight.enabled = false; //나이트 스크립트 비활성화
        GetComponent<BoxCollider>().enabled = false;//안맞게 박스콜라이더 초기화
        Invoke("enemyActive", 2);//2초후 함수호출

        Dead = true;
        DropItem();//아이템 드랍
    }
    //몬스터 비활성화
    public void Deactive()
    {
        PlayerTarget.targetOn = false;
        knight.CancelInvoke("NavOn");
        knight.enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        enemyActive();
        Dead = true;
        nav.isStopped = true;
        nav.updatePosition = false;
        nav.updateRotation = false;
    }
    //몬스터 재생성
    public void enemyRevive()
    {
        ani.Rebind();//애니메이터 초기화
        knight.enabled = true; //스크립트 활성화
        Dead = false; //죽음 초기화
        nav.isStopped = false;//멈췃던 네비들 다시 활성화
        nav.updatePosition = true;
        nav.updateRotation = true;
        GetComponent<BoxCollider>().enabled = true; 
        gameObject.layer = 12;//레이어도 변경

        //활성화
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        hpSlider.gameObject.SetActive(false);//체력슬라이더는 비활성화
        //체력및 위치 초기화
        currentHealth = startingHealth;
        transform.position = startPos;

    }
    //몬스터 비활성화
    void enemyActive()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        hpSlider.gameObject.SetActive(false);
    }
    //아이템 드랍
    //랜덤 및 switch를 활용해서 랜덤으로 아이템,코인 생성
    void DropItem()
    {
        randomItem = Random.Range(0, 4);
        randomCoin = Random.Range(0, 3);

        GameObject coinPrefeb1;
        GameObject coinPrefeb2;
        GameObject coinPrefeb3;

        GameObject hpPrefeb;
        GameObject mpPrefeb;
        switch (randomItem)
        {
            case 0:
                break;
            case 1:
        hpPrefeb = Instantiate(hpShake,transform.position + new Vector3(0.3f, 0.2f, 0), Quaternion.Euler(-90,0,0));
                hpPrefeb.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
                break;
            case 2:
                mpPrefeb = Instantiate(mpShake, transform.position + new Vector3(0.3f, 0.2f, 0f), Quaternion.Euler(-90, 0, 0));
                mpPrefeb.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);

                break;
            case 3:
                hpPrefeb = Instantiate(hpShake, transform.position + new Vector3(0.3f, 0.2f, 0), Quaternion.Euler(-90, 0, 0));
                hpPrefeb.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
                mpPrefeb = Instantiate(mpShake, transform.position- new Vector3(0.4f, 0.2f, 0), Quaternion.Euler(-90, 0, 0));
                mpPrefeb.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
                break;
            default:
                break;

                
        }
        switch (randomCoin)
        {
            case 0:
                coinPrefeb1 = Instantiate(coin, transform.position, Quaternion.identity);
                coinPrefeb1.GetComponent<Rigidbody>().AddForce(Vector3.up *5, ForceMode.Impulse);

                break;
            case 1:
                coinPrefeb1 = Instantiate(coin, transform.position + new Vector3(0, 0.2f, 0.3f), Quaternion.identity);
                coinPrefeb1.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
                coinPrefeb2 = Instantiate(coin, transform.position - new Vector3(0, 0.2f, 0.3f), Quaternion.identity);
                coinPrefeb2.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);

                break;
            case 2:
                coinPrefeb1 = Instantiate(coin, transform.position + new Vector3(0, 0.2f, 0.3f), Quaternion.identity);
                coinPrefeb1.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
                coinPrefeb2 = Instantiate(coin, transform.position, Quaternion.identity);
                coinPrefeb2.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
                coinPrefeb3 = Instantiate(coin, transform.position - new Vector3(0, 0.2f, 0.3f), Quaternion.identity);
                coinPrefeb3.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);

                break;
            default:
                break;

        }
    }
}
