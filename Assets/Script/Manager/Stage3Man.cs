using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//보스입장 가능한 방의 몬스터를 모두 죽여야 
//보스방 포탈열리게
public class Stage3Man : MonoBehaviour
{
    [SerializeField]
    GameObject bossPotal;//보스방 포탈
    [SerializeField]
    GameObject[] enemys;//몬스터들

    void Update()
    {
       //몬스터 다죽이면 활성화
        if(!enemys[0].transform.GetChild(0).gameObject.activeSelf&& !enemys[1].transform.GetChild(0).gameObject.activeSelf&&
            !enemys[2].transform.GetChild(0).gameObject.activeSelf&& !enemys[3].transform.GetChild(0).gameObject.activeSelf)
        {
            bossPotal.SetActive(true);
        }
        else
        {
            bossPotal.SetActive(false);
        }


    }
}
