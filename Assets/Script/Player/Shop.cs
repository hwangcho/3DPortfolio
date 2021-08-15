using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static bool shopActive = false; //상점 사용중인지
    [SerializeField]
    private Inventory theInventory;
    [SerializeField]
    GameObject shopBase;
    
    public ItemPickup[] item;



    void Update()
    {
        TryOpenShop();
    }

    //샵 활성화 비활성화
     void TryOpenShop()
    {
   
            if (shopActive)
                OpenShop();
            else
                CloseShop();     
    }

    //상점 활성화
    private void OpenShop()
    {
        shopBase.SetActive(true);
    }
    //상점 비활성화
    public void CloseShop()
    {
        shopBase.SetActive(false);

    }
    //아이템 클릭시 구매 함수들
    public void HPShake()
    {
        if (StatManager.instance.coin >= 70)//돈이 가격보다 같거나 많을때
        {
            theInventory.AcquireItem(item[0].item);//인벤토리에 추가
            StatManager.instance.coin -= 70;//돈 감소
        }
        else
            return;
    }
    public void MPShake()
    {
        if (StatManager.instance.coin >= 70)
        {
            theInventory.AcquireItem(item[1].item);
            StatManager.instance.coin -= 70;
        }
        else
            return;
    }
    public void ChickenLeg()
    {
        if (StatManager.instance.coin >= 100)
        {
            theInventory.AcquireItem(item[2].item);
            StatManager.instance.coin -= 100;
        }
        else
            return;
    }
    public void Plate()
    {
        if (StatManager.instance.coin >= 100)
        {
            theInventory.AcquireItem(item[3].item);
            StatManager.instance.coin -= 100;
        }
        else
            return;
    }
}
