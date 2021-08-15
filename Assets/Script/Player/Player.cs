using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerHealth playerhealth; //플레이어 체력관리 스크립트
    [SerializeField]
    PlayerTarget playertarget; // 플레이어 타겟 관리 스크립트
    //클릭 관련
    float h; 
    float v;
    bool Lmouse;
    bool SpaceKey;

    //카메라,이동 관련
    public Vector2 moveInput;
    Vector3 lookForward;
    Vector3 lookRight;
    public Vector3 moveDir;
    Vector3 RollVec;
    Vector3 dir;
    public float moveSpeed;
    Vector3 Skill1Vec;

    Animator ani;
    Rigidbody rigid;

    //공격받앗는지,구르기 관리
    bool Rolling;
    
    //시간관련 
    public float rollOrRunTime; 
    public float hitTime; 
    public float skill1Time;
    public  float skill2Time;

    [SerializeField]
    GameObject player;

    [SerializeField]
    Transform cameras;

    void Awake()
    {
        
        playerhealth = player.GetComponent<PlayerHealth>();
        rigid = GetComponent<Rigidbody>();
        ani = player.GetComponent<Animator>();
        //시작후 스킬바로 사용할수있게 초기화
        skill1Time = Time.time - 7f; 
        skill2Time = Time.time - 12f;
        //피격시간도 초기화
        hitTime = Time.time-1;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (!PlayerHealth.Death&&!GameManager.instance.Loading&&!GameManager.instance.pauseOn)
        {
            if (!TalkManager.instance.talking)
            {
                KeyPress();
                Move();
                Attack();
                RollAndRun();

                if (!PlayerTarget.targetOn)
                    LookAround();
            }
           

            if (!Inventory.invectoryActivated && !Shop.shopActive&&!TalkManager.instance.talking)
            {
                Skill1();
                Skill2();
            }
           
            
           
        }
     

    }

    //키입력
    void KeyPress()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        SpaceKey = Input.GetKeyDown(KeyCode.Space);

        Lmouse = Input.GetMouseButtonDown(0);
    }
    void Move()
    {


        //타겟이 false 일때 
        if (!PlayerTarget.targetOn) 
            ani.SetBool("Walk", moveInput.magnitude != 0);

        moveInput = new Vector2(h, v); //x축 z축 이동 
        lookForward = new Vector3(cameras.forward.x, 0f, cameras.forward.z).normalized; //카메라 정면
        lookRight = new Vector3(cameras.right.x, 0f, cameras.right.z).normalized; // 카메라 좌우
        moveDir = lookForward * moveInput.y + lookRight * moveInput.x; // 카메라 보는방향으로 앞뒤좌우 이동하게끔

        //스킬1 사용시 이동
        if ( Time.time - skill1Time < 0.8f)
        {     
            moveDir = Skill1Vec; //움직이는 방향 고정해서 이동하게끔 바꿔줌
            transform.position += moveDir * Time.deltaTime * moveSpeed; //고정된 방향으로 이동
             
        }
        //타겟 false일때
        if (!PlayerTarget.targetOn)
        {
            //공격할때(실행중인 애니메이션 이름확인해서) , 스킬사용중,맞을때 이동불가(스킬,피격시간 사용)
            if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && !ani.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && !ani.GetBool("Combo") && Time.time - skill1Time>0.8f && Time.time - skill2Time > 1.2f && Time.time - hitTime > 0.35f )
            {
                //만약 구르면
                if (Rolling)
                {
                    moveDir = RollVec; //움직이는 방향 고정해서 이동하게끔 바꿔줌
                    ani.SetBool("Walk", false);
                    player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation((moveDir)), 8 * Time.deltaTime);//구르려는 방향으로 회전
                    transform.position += moveDir * Time.deltaTime * moveSpeed; //고정된 방향으로 이동
                }
                //구르지않고 키입력이 되어있다면
                if (!Rolling&&moveInput.magnitude != 0)
                {
                    player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation((moveDir)), 12 * Time.deltaTime);//플레이어 회전
                    transform.position += moveDir * Time.deltaTime * moveSpeed;
                }
            }
        }else if (PlayerTarget.targetOn) //타겟 true 일때
        {
            ani.SetBool("Run", false);
            //카메라 계속 타겟몬스터 보게 해줌     
            dir = playertarget.targeting.transform.position - cameras.transform.position; //타겟방향     
            cameras.transform.rotation = Quaternion.Slerp(cameras.transform.rotation, Quaternion.LookRotation(new Vector3(dir.x, -0.6f, dir.z)), 3f * Time.deltaTime); //카메라를 계속 타겟을 바라보게 회전
           
            if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && !ani.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && !ani.GetBool("Combo") && Time.time - skill1Time > 0.9f && Time.time - skill2Time > 1.4f && Time.time - hitTime > 0.35f)
            {
                if (Rolling)
                {
                    moveDir = RollVec;
                    player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation((moveDir)), 8 * Time.deltaTime);
                    transform.position += moveDir * Time.deltaTime * moveSpeed;
                }
                else
                {
                    player.transform.LookAt(new Vector3(playertarget.targeting.transform.position.x, 0, playertarget.targeting.transform.position.z)); //플레이어가 타겟팅 바라보게
                    if (!Rolling && moveInput.magnitude != 0) //구르기x 방향키 눌럿을때 
                    {
                        //애니메이터 블렌드트리 에 x값과 y값 float으로 주어서
                        //애니메이션 변경
                        ani.SetFloat("X", v);
                        ani.SetFloat("Y", h);

                        transform.position += moveDir * Time.deltaTime * moveSpeed;
                    }
                    if (h == 0 && v == 0)
                    {
                        ani.SetFloat("X", 0);
                        ani.SetFloat("Y", 0);
                    }
                }         
            }
    
        }

    }
    //카메라 움직임
    void LookAround()
    {
        if (Input.GetMouseButton(1))
        {
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X")*2, Input.GetAxis("Mouse Y")*2);//마우스 x축과 y축 담음
            Vector3 camAngle = cameras.rotation.eulerAngles;
            float x = camAngle.x - mouseDelta.y;

            //x축 최대 최소값
            if (x < 180f)
            {
                x = Mathf.Clamp(x, -1f, 30f);
            }
            else
            {
                x = Mathf.Clamp(x, 345f, 361f);
            }
            //카메라 회전
            cameras.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, 0);
        }
    }
    void RollAndRun()//구르기와 달리기
    {
        //런 애니가 true 이면 스피드 4
        if (ani.GetBool("Run"))
        {
            moveSpeed = 4;
        }
        //공격애니 실행중 이면서 공격애니 시간 0.6이상일때
        //스킬,피격 시간 조건으로 사용
        if ((!(ani.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.6f)&& 
            !(ani.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.6f))&&!Rolling && Time.time - skill1Time > 0.8f && Time.time - skill2Time > 1.2f && Time.time - hitTime > 0.35f) //공격후딜 and 구르기 아니고 맞는상태 아닐때
        {
            //스페이스바 누를때 시간초기화
            if (Input.GetKeyDown(KeyCode.Space) && StatManager.instance.p_curMP > 0) 
            {
                rollOrRunTime = Time.time;
            }
            // 키를 계속 누르고있으면 달리기
            if (Input.GetKey(KeyCode.Space)&&!PlayerTarget.targetOn) 
            {
                //달릴때 스태미나 사용
                if (Time.time - rollOrRunTime > 0.5f && StatManager.instance.p_curMP > 0)
                {
                    ani.SetBool("Run", true);
                    StatManager.instance.p_curMP -= 0.2f;
                    StatManager.instance.p_MpTime = Time.time-0.1f;
                }
                //만약 스태미나가 없으면
                //달리기 멈춤
                else if (Time.time - rollOrRunTime > 0.5f&&StatManager.instance.p_curMP <= 0)
                {
                    ani.SetBool("Run", false);
                    moveSpeed = 2; //다시 속도줄임
                    ani.ResetTrigger("RunAttack"); //대쉬공격 초기화
                    rollOrRunTime = Time.time;//시간도 초기화
                    StatManager.instance.p_MpTime = Time.time-0.1f;


                }
            }
            //스페이스바 땟을때 
            //시간이 0.5초 밑이면 구르기 , 마나있을때
            if (Input.GetKeyUp(KeyCode.Space) && Time.time - rollOrRunTime < 0.5f && StatManager.instance.p_curMP > 0) 
            {
                //방향킼 누를때만 가능
                if (moveInput.magnitude == 0)
                    return;

                ani.Rebind();//실행중 애니매이터 초기화
                Rolling = true;
                ani.SetTrigger("Roll");

                RollVec = moveDir;//이동방향 넣어줌
                moveSpeed = 4; //구를때 이동속도
                ani.SetBool("Combo", false); //공격중에 구르면 초기화시켜줘야됨

                Invoke("RollOut", 0.62f);
                StatManager.instance.p_curMP -= 6;
                StatManager.instance.p_MpTime = Time.time+0.2f;
            }
            //0.5초 이상이면 걷기로 변경
            else if (Input.GetKeyUp(KeyCode.Space) && Time.time - rollOrRunTime >= 0.5f)
            {
                ani.SetBool("Run", false);    
                moveSpeed = 2;
            }
        }
     
       
    }
    //구르기후 초기화
    public void RollOut()
    {
        moveSpeed = 2f;
        Rolling = false;
    }
    //공격 함수
    void Attack()
    {
        //구르기시간,피격시간,스킬시간 조건 사용
        if (Time.time - rollOrRunTime > 0.5f && Time.time - hitTime > 0.3f && Time.time - skill1Time > 0.8f && Time.time - skill2Time > 1.2f) 
        {

            //처음 공격시작 조건
            if (Lmouse && !ani.GetBool("Combo") && !ani.GetBool("Run") && StatManager.instance.p_curMP > 0&& !Inventory.invectoryActivated&&!Shop.shopActive && !TalkManager.instance.talking)
            {
                ani.SetTrigger("Attack");
                ani.SetBool("Walk", false);//걷는거 멈춤
                ani.SetBool("Combo", true);//콤보 시작
                Invoke("ComboFalse",0.7f);//콤보 끄는 함수
            }
            //첫번째 공격후 두번째 공격으로 갈때
            //attack1 애니가 0.3초보다 클때
            else if (Lmouse && ani.GetBool("Combo") && ani.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.3f && StatManager.instance.p_curMP > 0 && !Shop.shopActive && !Inventory.invectoryActivated && !TalkManager.instance.talking)
            {
                CancelInvoke("ComboFalse"); //실행중이던 invoke 취소
                ani.SetTrigger("Attack");
                ani.SetBool("Combo", true);
                ani.SetBool("Walk", false);
            }
            //2번쨰 공격후 다시 1번째공격넘어갈때
            else if (Lmouse && ani.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && StatManager.instance.p_curMP > 0 && !Shop.shopActive && !Inventory.invectoryActivated && !TalkManager.instance.talking)
            {
                ani.SetTrigger("Attack");
                ani.SetBool("Walk", false);

            }
            //2번쨰 공격 애니가 끝날때까지 공격안하면 콤보 초기화
            if (ani.GetBool("Combo") && ani.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                ani.SetBool("Combo", false);


            }
        }
        //달리기 공격 실행
        if (Lmouse && ani.GetBool("Run") && ani.GetBool("Walk") && StatManager.instance.p_curMP > 0)
        {
            ani.SetTrigger("RunAttack");
        }
    }
    //콤보 초기화
    void ComboFalse()
    {
        ani.SetBool("Combo", false);
    }

    //스킬1 함수
    void Skill1()
    {
        //공격애니중 아닐때, 피격 시간, 스킬쿨타임 등등 조건
        if (Input.GetKeyDown(KeyCode.E) && !ani.GetCurrentAnimatorStateInfo(0).IsName("Attack1") &&
         !ani.GetCurrentAnimatorStateInfo(0).IsName("Attack2") &&!ani.GetBool("Run") && !Rolling && Time.time - hitTime > 0.5f && StatManager.instance.skill1Img.fillAmount == 1 && Time.time - skill2Time >1.2f && StatManager.instance.p_curMP > 0)
        {
            ani.SetTrigger("Skill1");
            Skill1Vec = moveDir;//이동 고정
            moveSpeed = 2f;
            skill1Time = Time.time; //스킬시간 초기화
            StatManager.instance.skill1Img.fillAmount = 0; //스킬쿨타임 초기화
            
           
        }
    }
    //스킬2 함수
    void Skill2()
    {
        if (Input.GetKeyDown(KeyCode.Q) && PlayerTarget.targetOn && !ani.GetCurrentAnimatorStateInfo(0).IsName("Attack1") &&
         !ani.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && Time.time - skill1Time > 0.8f && !Rolling && Time.time - hitTime > 0.5f && StatManager.instance.skill2Img.fillAmount == 1 && StatManager.instance.p_curMP > 0)
        {
            ani.SetTrigger("Skill2");
            skill2Time = Time.time;
            StatManager.instance.skill2Img.fillAmount = 0;
            

        }
    }
 



}
