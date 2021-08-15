using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//스켈레톤
//Knight 스크립트와 거의비슷
//플레이어에게 붙으면 잠시후 폭발
public class Skeleton : MonoBehaviour
{
    public bool destinyOn;
    public bool targetOn;


    NavMeshAgent nav;
    Animator ani;

    GameObject player;
    [SerializeField]
    Transform destiny1;
    [SerializeField]
    Transform destiny2;
    [SerializeField]
    Material originColor;//처음 컬러
    [SerializeField]
    Material setColor;//바뀔 컬러

    [SerializeField]
    GameObject Effect;

    float rgb = 0; //Lerp에 넣을값
    float blinkTime;//깜빡이는 시간
    bool blinking;//무슨색으로 변할지 불값
    SkeletonHealth skeltonhealth;

    bool Kamikaze; //자폭했는지 확인
    Vector3 dir;
    void Awake()
    {

        nav = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        skeltonhealth = GetComponent<SkeletonHealth>();

       
    }


    void Update()
    {
        if (!skeltonhealth.Dead&&!Kamikaze)
        {
            OnTarget();
            if (!targetOn)
            {
                NoTarget();

            }
        }

        if (Kamikaze&&!skeltonhealth.Dead)
        {
            KamikazeSystem();
        }


    }
    //자폭시스템
    void KamikazeSystem()
    {
        
        //blinking 의 불값에따라 rgb 증가할지 감소할지
        if (rgb >= 1)
        {
            blinking = true;

        }
        else if (rgb <= 0)
        {
            blinking = false;


        }
        //시작했을때보다 시간지날수록 빠르게 깜빡거림
        if (blinking)
        {
            rgb -= (Time.time - blinkTime) / 4f;//감소치

        }
        else if (!blinking)
        {

            rgb += (Time.time - blinkTime) / 4f;//증가치


        }
        //몬스터 색깔 lerp 이용해서 변경
        for (int i = 0; i < 3; i++)
        {

            gameObject.transform.GetChild(i).GetComponentInChildren<Renderer>().material.color = Color.Lerp(originColor.color, setColor.color, rgb);

        }
        //2초가 지나면 자폭시작
        if (Time.time - blinkTime > 2f)
        {

            skeltonhealth.Boom();//터졋을때 함수 호출
            SoundManager.instance.Play("Kamikaze");//효과음

            Instantiate(Effect, transform.position+ new Vector3(0,1,0), Quaternion.identity);//폭발 이펙트 생성

        }
    }
    void OnTarget()
    {
        //플레이어가 거리내에 들어오면 빠르게 달려감
        if (Vector3.Distance(player.transform.position, transform.position) < 8 && Vector3.Distance(player.transform.position, transform.position) > 1.2f)
        {
            targetOn = true;
            nav.speed = 3.5f;
            ani.SetBool("Walk", false);
            ani.SetBool("Run", true);


            //플레이어에게 붙는순간 멈추고 자폭준비
        }else if(Vector3.Distance(player.transform.position, transform.position) <= 1.2f)
        {

            ani.SetBool("Run", false);
            nav.isStopped = true;
            nav.updatePosition = false;
            nav.updateRotation = false;
            nav.velocity = Vector3.zero;
            Kamikaze = true;
            blinkTime = Time.time;
        }

        //플레이어가 멀어지면 다시 제자리로 
        else if (Vector3.Distance(player.transform.position, transform.position) > 8)
        {
            targetOn = false;
            ani.SetBool("Run", false);
        }
        //타게팅 되면 목표물설정
        if (targetOn)
        {
            nav.SetDestination(player.transform.position);
        }


    }
    //다시 태어날때 초기화
    public void kamiOFF()
    {
        Kamikaze = false;
        for (int i = 0; i < 3; i++)
        {

            gameObject.transform.GetChild(i).GetComponentInChildren<Renderer>().material.color = originColor.color;

        }

    }
    void NoTarget()
    {
        if (destinyOn)
        {
            nav.speed = 1;
            nav.SetDestination(destiny1.transform.position);
            ani.SetBool("Walk", true);
        }
        else
        {
            nav.speed = 1;
            nav.SetDestination(destiny2.transform.position);
            ani.SetBool("Walk", true);


        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Destiny")
        {
            destinyOn = !destinyOn;
        }
        if (other.gameObject.layer == 11)
        {
            skeltonhealth.TakeDamage(Random.Range(StatManager.instance.p_attackMin, StatManager.instance.p_attackMax), player.transform.position, 1);
            other.GetComponentInChildren<HitEffect>().HitEffectOn(this.transform);
            SoundManager.instance.Play("Hit");

        }


    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "PlayerDumbbel")
        {
            skeltonhealth.TakeDamage(Random.Range(StatManager.instance.Skill2Min, StatManager.instance.Skill2Max), player.transform.position, 0);
            SoundManager.instance.Play("Hit");


        }
        if (collision.gameObject.tag == "PlayerPlate")
        {
            skeltonhealth.TakeDamage(Random.Range(StatManager.instance.Skill1Min, StatManager.instance.Skill1Max), player.transform.position, 0);
            SoundManager.instance.Play("Hit");


        }
    }

    //무적 on
    public void NoDamageOn()
    {
        nav.speed = 0;

        nav.isStopped = true;
        nav.updatePosition = false;
        nav.updateRotation = false;
        nav.velocity = Vector3.zero;
        gameObject.layer = 8;
     
        CancelInvoke("NavOn");
        Invoke("NavOn", 0.8f);
    }
    //무적 off
    public void NoDamageOff()
    {

        gameObject.layer = 12;

    }
    //네비게이션 따라가는거 활성화
    public void NavOn()
    {
        nav.isStopped = false;
        nav.updatePosition = true;
        nav.updateRotation = true;
    }
    
}
