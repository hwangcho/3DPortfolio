using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//수정할거 많음
//타겟중인 몬스터와 일정 거리이상 떨어지면 타겟 풀리게 만들기
//타겟 거리 가까운거 측정해서 타겟팅에 넣어주기

public class PlayerTarget : MonoBehaviour
{
    public static bool targetOn; //타게팅중인지
    public GameObject targeting; //타게팅 몬스터

    [SerializeField]
    GameObject player;


    public LayerMask layermask; //타겟가능 레이어

    void Start()
    {
        targeting = null;

    }

    void Target()
    {
        //타게팅이 없거나
        //타게팅이 비활성화 되거나
        //타게팅과의 거리가 멀어지면 타게팅,targetOn 초기화
        if(targeting == null||targeting.activeSelf == false || Vector3.Distance(player.transform.position, targeting.transform.position) > 9)
        {
            targeting = null;
            targetOn = false;
        }
        //마우스휠 클릭시 
        //스킬2 사용하자마자 푸는거 방지
        //레이를 발사해 타게팅
        if (Input.GetMouseButtonDown(2)&& StatManager.instance.skill2Img.fillAmount > 0.05f)
        {

           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//카메라에서 레이발사
           RaycastHit hit;    
           Debug.DrawRay(ray.origin,ray.direction*1000f,Color.red);
            //타겟중이 아닐때
            if (!targetOn)
            {
                if (Physics.Raycast(ray, out hit, 1000,layermask))//몬스터 레이어만 검출
                {
                    if(Vector3.Distance(player.transform.position, hit.transform.position) < 9)//타게팅과 플레이어의 거리
                    {
                        targetOn = true;
                        player.GetComponent<Animator>().SetBool("Walk", false);//타겟시 walk 애니 false
                        targeting = hit.transform.gameObject;//레이에 검출된 몬스터 타게팅에 넣음

                    }

                }
            }
            //타겟중일땐 타겟을 풀어줌
            else
            {
                targetOn = false;
                targeting = null;
            }
           
        }

    }
    void Update()
    {

        Target();
        player.GetComponent<Animator>().SetBool("Target", targetOn);

    }
}
