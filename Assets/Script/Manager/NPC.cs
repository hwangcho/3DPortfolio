using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    int id;//npc의 id를넘어서 누구와 대화하는지 알수있게설정
    [SerializeField]
    GameObject fBtn;//npc 머리위에 F 모양 이미지

    //대화가능 공간에 들어와있으면
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            fBtn.SetActive(true);//이미지 활성화

            if (Input.GetKey(KeyCode.F)&&!TalkManager.instance.talking&& other.GetComponent<Player>().moveInput.magnitude == 0)//f버튼 누름 & 플레이어가 대화중이아니고 & 멈춰있다면
            {
                Inventory.invectoryActivated = false; //인벤토리 비활성화
                other.GetComponentInChildren<Inventory>().CloseInventory();
                TalkManager.instance.TalkSystem(id);//id넘겨서 함수호출
            }



        }
    }
    //대화가능 공간에 나오면 이미지 비활성화
    private void OnTriggerExit(Collider other)
    {
        fBtn.SetActive(false);

    }
}
