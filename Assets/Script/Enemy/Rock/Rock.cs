using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rock : MonoBehaviour
{
    public bool destinyOn;
    public bool targetOn;

    public float attackTime;
    NavMeshAgent nav;
    Animator ani;

    GameObject player;
    [SerializeField]
    Transform destiny1;
    [SerializeField]
    Transform destiny2;
    [SerializeField]
    BoxCollider attackCollider;
    [SerializeField]
    Transform throwTransform; //생성된 덤벨 위치
    [SerializeField]
    GameObject Dumbbel; //덤벨 프리펩
    RockHealth rockHealth;

    public GameObject throwDumbble; 
    Vector3 dir;
    void Awake()
    {
        attackTime = Time.time - 2.8f;
        nav = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        rockHealth = GetComponent<RockHealth>();
    }


    void Update()
    {
        if (!rockHealth.Dead)
        {
            OnTarget();
         
            if (!targetOn)
            {
                NoTarget();

            }
        }
        


    }
    void OnTarget()
    {
        //골렘은 플레이어 발견시 플레이어에게 가지않고
        //제자리에서 회전하면서 덤벨을 던짐
        //가까울땐 펀치사용
        if(targetOn)
        {
                //플레이어를 향해 회전
                    dir = player.transform.position - transform.position;

                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z)), 3f * Time.deltaTime);
                
                
           
            

        }
        //공격딜레이가 되고 거리가 가깝지않으면 덤벨 던짐
        if (Vector3.Distance(player.transform.position, transform.position) < 15 && Vector3.Distance(player.transform.position, transform.position) > 3.2f && Time.time - attackTime > 4f)
        {
            
                
                    ani.SetBool("Walk", false);
                    attackTime = Time.time;
                    ani.SetTrigger("Attack1");
                
                
          
                
            
        }
        //거리가 가까우면 펀치
        if (Vector3.Distance(player.transform.position, transform.position) <= 3.2f )
        {

            //레이를 사용해 앞에있으면ㄴ 펀치공격
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 3.2f, LayerMask.GetMask("Player")))
            {

                if (hit.transform.tag == "Player")
                {
                    if (Time.time - attackTime > 2.5f)
                    {
                        ani.SetBool("Walk", false);

                        attackTime = Time.time;
                        ani.SetTrigger("Attack2");



                    }
                }

            }



        }
        //플레이어가 멀어지면 타겟off
        if (Vector3.Distance(player.transform.position, transform.position) >= 15&&Time.time-attackTime> 2.2f)
        {
            targetOn = false;
     
            attackTime = Time.time-2.8f; //공격시간 초기화



        }
        //플레이어가 가까워지면 타겟on
        else if (Vector3.Distance(player.transform.position, transform.position) < 15)
        {
          

            targetOn = true;
            ani.SetBool("Walk", false);


        }
        //타겟on일때 네비속도 0
        if (targetOn)
        {
            nav.speed = 0;
        }


    }
    //타겟팅 off일때 목표위치로 계속 왓다갔따
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
        //목표위치 까지가면 목표변경
        if (other.tag == "Destiny")
        {
            destinyOn = !destinyOn;
        }
        //플레이어 공격에맞으면
        if (other.gameObject.layer == 11)
        {
            rockHealth.TakeDamage(Random.Range(StatManager.instance.p_attackMin, StatManager.instance.p_attackMax), player.transform.position, 1);
            other.GetComponentInChildren<HitEffect>().HitEffectOn(this.transform);
            SoundManager.instance.Play("Hit");

        }
        //플레이어에게 공격을 맞추면
        if (other.gameObject.tag == "Player")
        {
            other.GetComponentInChildren<PlayerHealth>().TakeDamage(Random.Range(14, 18), transform.position, 5);
            SoundManager.instance.Play("Hit");

        }
    }
    //플레이어 스킬 덤벨,원판에 맞으면
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "PlayerDumbbel")
        {
            rockHealth.TakeDamage(Random.Range(StatManager.instance.Skill2Min, StatManager.instance.Skill2Max), player.transform.position, 0);
            SoundManager.instance.Play("Hit");


        }
        if (collision.gameObject.tag == "PlayerPlate")
        {
            rockHealth.TakeDamage(Random.Range(StatManager.instance.Skill1Min, StatManager.instance.Skill1Max), player.transform.position, 0);
            SoundManager.instance.Play("Hit");


        }
    }
    //공격 콜라이더 활성화
    public void AttackON()
    {
        attackCollider.enabled = true;

    }
    //공격 콜라이더 비활성화
    public void AttackOFF()
    {
        attackCollider.enabled = false;

    }
    //무적시간
    public void NoDamageOn()
    {
        nav.speed = 0;

        nav.isStopped = true;
        nav.updatePosition = false;
        nav.updateRotation = false;
        nav.velocity = Vector3.zero;
        gameObject.layer = 8;
        AttackOFF();
        CancelInvoke("NavOn");
        Invoke("NavOn", 0.6f);
    }
    //무적에서 다시 맞게 변경
    public void NoDamageOff()
    {

        gameObject.layer = 12;

    }

    public void NavOn()
    {
        nav.isStopped = false;
        nav.updatePosition = true;
        nav.updateRotation = true;
    }

    //덤벨 생성
    public void InstantiateThrow()
    {
        throwDumbble = Instantiate(Dumbbel, throwTransform.position, Quaternion.identity);
        throwDumbble.GetComponent<Throwing>().setTransform(throwTransform);//덤벨 고정위치 함수로 넘겨줌
    }
    //효과음
    public void Atk2Sound()
    {
        SoundManager.instance.Play("RockAttack");
    }
    public void Atk1Sound()
    {
        SoundManager.instance.Play("RockThrow");
    }

}
