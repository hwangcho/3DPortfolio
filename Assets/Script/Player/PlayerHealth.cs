using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public static bool Death = false;

    Rigidbody rigid;
    Animator ani;

    Player player;

    PlayerAniEvent playeraniEvent;

    void Awake()
    {

        ani = GetComponent<Animator>();
        player =GetComponentInParent<Player>();
        rigid = GetComponentInParent<Rigidbody>();
        playeraniEvent = GetComponent<PlayerAniEvent>();
    }

    //피격 함수
    public void TakeDamage(int amout, Vector3 EnemyPosition, float pushBack)
    {
  
        StatManager.instance.p_curHealth -= amout;//체력감소
        player.hitTime = Time.time; //피격시간 초기화
        //스킬 시간 초기화해줘서
        //피격중에 스킬사용못하게
        player.skill1Time = Time.time -0.8f;
        player.skill2Time = Time.time-1.2f;

        
        player.RollOut();//구르는 도중에 맞았을 경우 대비해 사용
        ani.Rebind();//애니 초기화

        ani.SetTrigger("Hit");//피격애니

        //체력게이지에 변경된 체력값을 표시
        //만약 현재 체력이 0이하가되면 죽음
        if (StatManager.instance.p_curHealth <= 0 && !Death)
        {
            //플레이어가 죽었을때 수행할 명령
        StatManager.instance.p_curHealth = 0;

            PlayerDie();
        }
        else
        {
            //몬스터의 위치값을 받아와서
            //플레이어를 밀쳐냄
            Vector3 diff = transform.position - EnemyPosition;
            diff = diff / diff.sqrMagnitude;
            rigid.AddForce((new Vector3(diff.x, 0, diff.z)) * pushBack, ForceMode.Impulse);
            Invoke("RigidvelocityZero", 0.2f);
        }
    }
    //플레이어 사망
    void PlayerDie()
    {
        ani.Rebind();//애니메이션 초기화
        ani.SetTrigger("Death");//죽는 애니
        gameObject.transform.parent.gameObject.layer = 8;//더이상 피격되지않게 무적레이어
        Death = true;
        StartCoroutine(diepanel());
        Invoke("a",2);

    }
    //모든 몬스터 비활성화
    void a()
    {
        SpawnManager.instance.AllOFF();
    }
    //물리속도 초기화
    void RigidvelocityZero()
    {
        rigid.velocity = Vector3.zero;
    }
    //죽엇을때 나올 패널
    IEnumerator diepanel()
    {
        yield return new WaitForSeconds(1f);
        GameManager.instance.DiePanelOn();

    }
}