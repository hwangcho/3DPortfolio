using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInven : MonoBehaviour
{
    [SerializeField]
    private Inventory theInventory; 

    [SerializeField]
    GameObject itemText;//아이템 획득 텍스트
    [SerializeField]
    GameObject itemTextParent;//생성될 아이템텍스트 부모


    private void OnCollisionEnter(Collision collision)
    {
        //아이템 획득시
        if (collision.gameObject.tag == "Item")
        {

            theInventory.AcquireItem(collision.transform.GetComponent<ItemPickup>().item);//인벤토리 아이템추가
            Destroy(collision.gameObject);//주은 아이템 삭제
            SoundManager.instance.Play("ItemPickUp");//효과음

            GameObject itemtext = Instantiate(itemText); //아이템 텍스트 생성
            itemtext.transform.SetParent(itemTextParent.transform);//부모 설정
            itemtext.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);//스케일 설정
            itemtext.GetComponent<Text>().text = collision.transform.GetComponent<ItemPickup>().item.itemName + " 획득";//텍스트 설정
            Destroy(itemtext, 1);//1초후 삭제

        }
        //동전을 먹엇을때
        if (collision.gameObject.tag == "Coin")
        {
            SoundManager.instance.Play("ItemPickUp");///효과음

            Destroy(collision.gameObject);//삭제
            StatManager.instance.coin += Random.Range(50,101);//돈 랜덤
        }
    }
}
