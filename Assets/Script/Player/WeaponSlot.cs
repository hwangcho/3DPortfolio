using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    public Item[] item;//장착 아이템
    public Slot[] slots;//장착 슬록
    public GameObject[] plate;//원판 게임오브젝트
    
    void Update()
    {       
            //아이템 리스트를 슬롯 아이템의 리스트와 같게
            item[0] = slots[0].item; 
            item[1] = slots[1].item;
        //첫번째 슬롯의 원판이 장착되면
        if (item[0] != null)
        {
            //아이템타입 확인
            //첫번쨰 원판오브젝트 비활성화중이면
            if (item[0].itemType == Item.ItemType.Equipment && !plate[0].activeSelf)
            {
                plate[0].SetActive(true);
             
            }
            
        }
        //두번째 슬롯의 원판이 장착되면
        else if (item[1] != null)
        {
            if (item[1].itemType == Item.ItemType.Equipment && !plate[0].activeSelf)
            {
                plate[0].SetActive(true);         
            }

        }
        //두개다 비어있으면
        else if(item[0] == null&& item[1] == null)
        {
            plate[0].SetActive(false);



        }
        //두개다 장착중이라면
        if (item[0] != null&& item[1] != null)
        {
            if (item[0].itemType == Item.ItemType.Equipment && item[1].itemType == Item.ItemType.Equipment )
            {
                //두번쨰 원판 활성화
                plate[1].SetActive(true);
            }
        }
        else
        {
            plate[1].SetActive(false);


        }
        //첫번째 원판이 활성화되어있을때 
        if (plate[0].activeSelf&&!plate[1].activeSelf)
        {
            StatManager.instance.equipStr = 1; //착용힘 1
         //두개다 활성화중일때
        }else if(plate[0].activeSelf && plate[1].activeSelf)
            StatManager.instance.equipStr = 2;//착용힘2
        else
        {
            StatManager.instance.equipStr = 0;
        }
    }

}
