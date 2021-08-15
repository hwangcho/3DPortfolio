using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bull : MonoBehaviour
{

    public bool targetOn; //타게팅중인지 

    public float attackTime;//공격 딜레이
    public int randomAttack; //공격 패턴
    public int randomAttackCheck;//공격 중복방지
    float growlTime;//스킬 시간
    float dunbbelTime;//덤벨 생성시간

    NavMeshAgent nav;
    Animator ani;

    Rigidbody rigid;
    Transform m_Target; //플레이어 타겟
    public float m_InitialAngle = 30f; //시작각도

    GameObject player;

    //공격 콜라이더
    [SerializeField]
    BoxCollider attack01Collider;
    [SerializeField]
    BoxCollider attack02Collider;
    [SerializeField]
    BoxCollider jumpCollider;

    //점프공격 이펙트
    [SerializeField]
    GameObject jumpAttackEffect;

    BullHealth bullHealth;

    //덤벨생성 랜덤위치
    int randomX;
    int randomZ;
    public Vector3[] randomPos;
    [SerializeField]
    GameObject bullDumbbel;

    Vector3 dir;
    void Awake()
    {
        //시작시 시간들 초기화
        attackTime = Time.time - 5f;
        growlTime = Time.time - 20f;
        dunbbelTime = Time.time;

        nav = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        bullHealth = GetComponent<BullHealth>();

        rigid = GetComponent<Rigidbody>();
        randomAttackCheck = randomAttack;
   
    }


    void Update()
    {
        //죽지않았을때 행동들
        if (!bullHealth.Dead)
        {
            if(Time.time - growlTime > 3f)
                OnTarget();
            TwoPage();
        }

        //4초간격으로 덤벨생성 패턴
        if(Time.time - growlTime > 3f&& Time.time - growlTime < 14f &&Time.time-dunbbelTime > 4f)
        {
            dunbbelTime = Time.time;//시간 초기화
            SetBlock(10);//10개의 랜덤위치 생성

            for (int i = 0; i < 10; i++)
            {
                Instantiate(bullDumbbel,new Vector3( transform.position.x-randomPos[i].x, 7,transform.position.z - randomPos[i].z) , Quaternion.identity);//덤벨 생성
            }
        }
        //보스에게 죽어 로딩화면 넘어가게되면 보스 삭제
        if (GameManager.instance.Loading)
        {
            Destroy(gameObject);
        }
    }

    //체력반까이면 표효(맵 위에서 덤벨떯어짐)
    void TwoPage()
    {
        //체력 반깎인후 28초마다 실행
        if (bullHealth.startingHealth * 0.5f >= bullHealth.currentHealth && Time.time - growlTime >= 28f && Time.time - attackTime > 2f)
        {
            AttackOFF();//공격중에 실행되면 공격콜라이더 남아있어서 콜라이더 꺼줌
            JumpAttackOff();//점프공격또한 꺼줘야댐
            nav.speed = 0; //네비게이션 속도0으로해서 이상해지는거 수정
            ani.Rebind();//실행중인 애니메이터 초기화
            ani.SetTrigger("Growl");
            //시간들 초기화
            growlTime = Time.time;
            attackTime = Time.time;
            dunbbelTime = Time.time;

        }
    }
    void OnTarget()
    {
        //멀어졋을때 패턴
        if(Vector3.Distance(player.transform.position, transform.position) < 100 && Vector3.Distance(player.transform.position, transform.position) > 4.8f &&nav.enabled == true)
        {
            //9초전까지는 계속 따라감
            if (Time.time - attackTime > 4f)
            {
                nav.speed = 3;
                ani.SetBool("Walk", true);
            }
            //만약 시간이 9초가 넘게되면 플레이어에게 날아가 공격
            if( Time.time - attackTime > 9f)
            {
                
                nav.enabled = false;//네비게이션 꺼줌
                rigid.constraints = RigidbodyConstraints.FreezeRotation; //회전만 true로 모두 체크함

                nav.speed = 0;
                ani.SetBool("Walk", false);
                ani.SetTrigger("JumpAttack03");
                attackTime = Time.time-2f;
                m_Target = GameObject.FindGameObjectWithTag("Player").transform; //점프뛸때 플레이어 위치 저장
                Vector3 velocity = GetVelocity(transform.position, m_Target.position, m_InitialAngle); //날아가는 속도 받아옴
                rigid.velocity = velocity; //넣어줌
            }
        }
        //가까이있을때 패턴
        if (Vector3.Distance(player.transform.position, transform.position) <= 4.8f && Time.time - attackTime > 3f)
        {
            //가까이 붙엇을때 플레이어에게 걸어가지않고
            //플레이어를 바라보게 회전함
            ani.SetBool("Walk", true);
            nav.speed = 0;
            dir = player.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z)), 5f * Time.deltaTime); //플레이어를보게 회전
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 5f, LayerMask.GetMask("Player")) )//레이를 발사해서 플레이어를 찾아냄
            {
                //찾아내면 걷는거 멈추고 
                //공격 시간되면 공격
                ani.SetBool("Walk", false);
                if (Time.time - attackTime > 5f)
                {
                    //랜덤값줘서 공격패턴 다르게함
                    //이전 공격과 중복시 다시 랜덤값 받아옴
                    randomAttack = Random.Range(0, 4);
                    if(randomAttackCheck != randomAttack)
                    {
                        switch (randomAttack)
                        {
                            case 0:
                                ani.SetTrigger("Attack1");
                                ani.SetTrigger("Attack2");
                                ani.SetTrigger("Idle");

                                attackTime = Time.time - 1;
                                randomAttackCheck = randomAttack;

                                break;
                            case 1:
                                ani.SetTrigger("Attack1");
                                ani.SetTrigger("Attack2");
                                ani.SetTrigger("Attack3");
                                attackTime = Time.time;
                                randomAttackCheck = randomAttack;

                                break;
                            case 2:
                                ani.SetTrigger("Attack4");
                                attackTime = Time.time - 1.5f;
                                randomAttackCheck = randomAttack;

                                break;
                            case 3:
                                ani.SetTrigger("JumpAttack");
                                attackTime = Time.time - 1.5f;
                                randomAttackCheck = randomAttack;

                                break;
                        }

                    }

                }

            }



        }
   
        //타게팅중이고 네비가 활성화중일때 
        //플레이어 위치로 이동하게 함
        if (targetOn && nav.enabled == true)
        {
            nav.SetDestination(player.transform.position);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        //플레이어 공격에 맞앗을때
        if (other.gameObject.layer == 11)
        {
            bullHealth.TakeDamage(Random.Range(StatManager.instance.p_attackMin, StatManager.instance.p_attackMax), player.transform.position, 1); //데미지 받음
            other.GetComponentInChildren<HitEffect>().HitEffectOn(this.transform);//피격 이펙트 생성
            SoundManager.instance.Play("Hit");//효과음
        }
        //플레이어를 맞췃을때
        if (other.gameObject.tag == "Player")
        {
            other.GetComponentInChildren<PlayerHealth>().TakeDamage(Random.Range(18, 24), transform.position, 10);//플레이어 데미지 받고
            SoundManager.instance.Play("Hit");//피격음

        }
    }
    //플레이어 스킬 덤벨,원판에 맞앗을때
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "PlayerDumbbel")
        {
            bullHealth.TakeDamage(Random.Range(StatManager.instance.Skill2Min, StatManager.instance.Skill2Max), player.transform.position, 0);
            SoundManager.instance.Play("Hit");

        }
        if (collision.gameObject.tag == "PlayerPlate")
        {
            bullHealth.TakeDamage(Random.Range(StatManager.instance.Skill1Min, StatManager.instance.Skill1Max), player.transform.position, 0);
            SoundManager.instance.Play("Hit");

        }
    }
    //공격 콜라이더 끄고 킴
    //#####
    public void Attack1ON()
    {
        attack01Collider.enabled = true;

    }
    public void Attack2ON()
    {
        attack02Collider.enabled = true;

    }
    public void JumpAttackON()
    {
        jumpCollider.enabled = true;
        Instantiate(jumpAttackEffect, jumpCollider.transform.position, Quaternion.identity);//점프공격 이펙트온

    }
    public void AttackOFF()
    {
        attack01Collider.enabled = false;
        attack02Collider.enabled = false;
        jumpCollider.enabled = false;

    }
    //###############

    //날아가는 점ㅍ프공격 off
    public void JumpAttackOff()
    {
        nav.enabled = true;
        rigid.constraints = RigidbodyConstraints.FreezeAll;
    }

    //포물선으로 타겟으로 날아가게하기위한 vector3반환함수
    public Vector3 GetVelocity(Vector3 player, Vector3 target, float initialAngle)
    {
        float gravity = Physics.gravity.magnitude;
        float angle = initialAngle * Mathf.Deg2Rad;

        Vector3 planarTarget = new Vector3(target.x, 0, target.z);
        Vector3 planarPosition = new Vector3(player.x, 0, player.z);

        float distance = Vector3.Distance(planarTarget, planarPosition);
        float yOffset = player.y - target.y;

        float initialVelocity
            = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity
            = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        float angleBetweenObjects
            = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (target.x > player.x ? 1 : -1);
        Vector3 finalVelocity
            = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        return finalVelocity;
    }

    //이전의 위치값과 중복인지 체크함수
    int  Search(int nEnd, int nX, int nZ)
    {
        for (int i = 0; i < nEnd; i++)
        {
            if (randomPos[i].z == nZ)
            {
                if (randomPos[i].x == nX || (randomPos[i].x + 1) == nX)
                    return 1;
            }
        }
        return 0;
    }
    //블럭위치 정하는 함수
    void SetBlock(int nBlockCount)
    {
        //생성 블럭 개수많큼 포문
        for (int i = 0; i < nBlockCount; i++)
        {
            randomPos[i].y = 8; //y값 고정

            while (true)
            {
                randomX = Random.Range(-7,8);
                randomZ = Random.Range(-7, 8);

                if (Search(i, randomX, randomZ) == 0)//이전의 값과 중복되면 다시 받아옴
                {
                    randomPos[i].x = randomX;
                    randomPos[i].z = randomZ;
                    break;
                }
            }
        }
    }
}
