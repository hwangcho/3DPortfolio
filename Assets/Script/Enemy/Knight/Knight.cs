using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Knight : MonoBehaviour
{

    public bool destinyOn; //목표물까지 이동했을때 다시 되돌아가게하는 불값
    public bool targetOn; //플레이어 범위에있는지
    bool Attack; //공격 콤보
    public float attackTime; //공격 딜레이
    NavMeshAgent nav;
    Animator ani;

    GameObject player;
    //목표위치
    [SerializeField]
    Transform destiny1; 
    [SerializeField]
    Transform destiny2;
    [SerializeField]
    BoxCollider attackCollider;

    KnightHealth knighthealth;

    Vector3 dir;
    void Awake()
    {
        attackTime = Time.time - 1f;
        nav = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        knighthealth = GetComponent<KnightHealth>();
    }


    void Update()
    {
        //죽지않았을때 
        if (!knighthealth.Dead)
        {
            OnTarget();
            if (!targetOn)
            {
                NoTarget();

            }
        }



    }
    //플레이어 범위내에 들어왓을때 함수
    void OnTarget()
    {
        //플레이어가 범위내에 들어오면 플레이어에게 뛰어감
        if (Vector3.Distance(player.transform.position, transform.position) < 8 && Vector3.Distance(player.transform.position, transform.position) > 2.2f && Time.time - attackTime > 1f)
        {
            targetOn = true;
            nav.speed = 2.2f; //네비게이션 스피드 변경
            ani.SetBool("Walk", false);//걷기에서 뛰는걸로 애니 변경
            ani.SetBool("Run", true);
            Attack = false; //처음공격은 무조건 첫번째공격을위해 초기화

        }
        //플레이어에게 붙엇을때
        if (Vector3.Distance(player.transform.position, transform.position) <= 2.1f && Time.time - attackTime > 1f)
        {
            ani.SetBool("Run", false);//뛰기에서 걷기로 변경
            ani.SetBool("Walk", true);
            
            //네비게이션으로 몬스터 회전x
            nav.speed = 0;
            dir = player.transform.position - transform.position;//플레이어 방향 받아와서
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(dir.x,0,dir.z)), 4.5f * Time.deltaTime);//몬스터 회전
            
            //레이를 몬스터 앞으로 발사해서 플레이어 체크
            //레이를활용해 공격 딜레이가 됫을때 공격하는게아니라
            //공격 딜레이도되고 플레이어가 앞에있을때 공격
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2.1f, LayerMask.GetMask("Player")))
            {
                //걷기 멈춤
                ani.SetBool("Walk", false);
                //공격 딜레이가 되면 플레이어에게 공격
                if (Time.time - attackTime > 2.1f)
                {
                    if (hit.transform.tag == "Player")
                    {
                        if (!Attack)
                        {
                            //첫번째공격
                            Attack = !Attack; //반전해서 두번쨰 공격하게 바꿈
                            attackTime = Time.time;//공격딜레이 초기화
                            ani.SetTrigger("Attack1");
                        }
                        else if (Attack)
                        {
                            Attack = !Attack;
                            attackTime = Time.time;
                            ani.SetTrigger("Attack2");
                        }
                    }
                }

            }
           


        }
        //플레이어가 멀어지면 다시 제자리로 걸어감
        else if (Vector3.Distance(player.transform.position, transform.position) > 8)
        {
            targetOn = false;
            ani.SetBool("Run", false);



        }
        //목표물 플레이어로 설정
        if (targetOn)
        {
            nav.SetDestination(player.transform.position);
        }


    }
    //타겟이 아닐때 설정해둔 목표위치로 계속 왔다갔다 하게하는 함수
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
        //첫번쨰 목표에왔으면 bool값 활용해서 두번째 위치로 
        if (other.tag == "Destiny")
        {
            destinyOn = !destinyOn;
        }
        //플레이어 공격에맞으면
        if (other.gameObject.layer == 11)
        {
            knighthealth.TakeDamage(Random.Range(StatManager.instance.p_attackMin, StatManager.instance.p_attackMax), player.transform.position, 1);//피격
            other.GetComponentInChildren<HitEffect>().HitEffectOn(this.transform);//이펙트생성
            SoundManager.instance.Play("Hit");//효과음

        }
        //플레이어에게 공격을 맞추면
        if (other.gameObject.tag == "Player")
        {
            other.GetComponentInChildren<PlayerHealth>().TakeDamage(Random.Range(11,15), transform.position, 0);//데미지 랜덤값 피격
            SoundManager.instance.Play("Hit");
            
        }
    }
    //플레이어 스킬 덤벨,원판에 맞으면
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "PlayerDumbbel")
        {
            knighthealth.TakeDamage(Random.Range(StatManager.instance.Skill2Min, StatManager.instance.Skill2Max), player.transform.position, 0);
            SoundManager.instance.Play("Hit");

        }
        if (collision.gameObject.tag == "PlayerPlate")
        {
            SoundManager.instance.Play("Hit");

            knighthealth.TakeDamage(Random.Range(StatManager.instance.Skill1Min, StatManager.instance.Skill1Max), player.transform.position, 0);

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
    //피격 무적시간
    public void NoDamageOn()
    {
        nav.speed = 0; //피격시 멈춤
        nav.isStopped = true; //멈춤
        nav.updatePosition = false;//위치 업데이트 멈춤
        nav.updateRotation = false;//회전 업데이트 멈춤
        nav.velocity = Vector3.zero;//네비 속도 멈춤
        gameObject.layer = 8;//레이어 변경해서 무적
        AttackOFF();//공격 콜라이더 남아있을수있으니 비활성화
        CancelInvoke("NavOn"); //초기화후
        Invoke("NavOn", 0.6f);//인보크로 함수호출
    }
    //레이어 변경해서 무적끝
    public void NoDamageOff()
    {

        gameObject.layer = 12;

    }
    //네비 다시 활성화
    public void NavOn()
    {
        nav.isStopped = false;
        nav.updatePosition = true;
        nav.updateRotation = true;
    }
    //효과음
    public void Atk1Sound()
    {
        SoundManager.instance.Play("EnemyAttack1");
    }
    public void Atk2Sound()
    {
        SoundManager.instance.Play("EnemyAttack2");
    }
}
