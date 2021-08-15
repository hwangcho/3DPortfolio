using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAniEvent : MonoBehaviour
{
   
    [SerializeField]
    Player player; 
    [SerializeField]
    PlayerTarget playertarget; 
    [SerializeField]
    GameObject Weapon; //무기
    [SerializeField]
    GameObject Skill1Plate; //생성될 스킬1 오브젝트
    [SerializeField]
    GameObject Skill2Dumbbel; // 스킬2 오브젝트
    [SerializeField]
    Transform skill1Pos; //생성 위치
    [SerializeField]
    Transform[] skill2Pos; //생성위치

    GameObject[] skill2 = new GameObject[4]; //스킬2 생성개수

    //레이어 변경으로 무적 함수
    public void NoDamageOn()
    {
        AttackColliderOFF();//공격 끝나기전에 구르면 공격콜라이더 남아있어서 초기화
        gameObject.transform.parent.gameObject.layer = 8;
    }
    //무적 끝
    public void NoDamageOff()
    {
        gameObject.transform.parent.gameObject.layer = 9;
    }
    //공격 콜라이더 활성화
    public void AttackColliderOn()
    {
        Weapon.GetComponent<BoxCollider>().enabled = true;
    }
    //공격 콜라이더 비활성화
    public void AttackColliderOFF()
    {

        Weapon.GetComponent<BoxCollider>().enabled = false;

    }

    //공격시 스테미나 사용
    public void AttackMp()
    {
        StatManager.instance.p_curMP -= 6; //mp 감소
       StatManager.instance.p_MpTime = Time.time+0.3f; //mp 회복시간 초기화

    }
    //스킬1 스테미나 사용
    public void Skill1Mp()
    {
        StatManager.instance.p_curMP -= 10;
        StatManager.instance.p_MpTime = Time.time+0.4f;

    }
    //스킬2 스테미나 
    public void Skill2Mp()
    {
        StatManager.instance.p_curMP -= 10;
        StatManager.instance.p_MpTime = Time.time+1f;

    }


    //스킬2 덤벨 생성
    public void Skill2DumbbelON()
    {
        //4개의 덤벨을 각각 설정해둔 위치에 생성
        for(int i = 0; i < 4; i++)
        {
            skill2[i] = Instantiate(Skill2Dumbbel, skill2Pos[i].position, Quaternion.Euler(0, 90, 0));
            skill2[i].GetComponent<PlayerDumbbel>().SetTargetPos(playertarget.targeting.transform);//생성된 덤벨의 스크립트에 함수에 매개변수를 넘겨줌
        }
    }
    //스킬1 원판 생성
    public void Skill1PlateON()
    {
        GameObject a= Instantiate(Skill1Plate, skill1Pos.position, Quaternion.identity); //원판 생성
        a.GetComponent<PlayerPlate>().SetPos(player.gameObject,skill1Pos);// 위치값 보내줌

    }
    //효과음 들
    public void WalkSound()
    {
        SoundManager.instance.Play("Walk");
    }
    public void RunSound()
    {
        SoundManager.instance.Play("Run");
    }
    public void Atk1Sound()
    {
        SoundManager.instance.Play("Attack1");
    }
    public void Atk2Sound()
    {
        SoundManager.instance.Play("Attack2");
    }
    public void Skill2Sound()
    {
        SoundManager.instance.Play("Skill2");
    }
}