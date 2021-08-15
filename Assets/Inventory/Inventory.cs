using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool invectoryActivated = false;  // 인벤토리 활성화 여부. true가 되면 카메라 움직임과 다른 입력을 막을 것이다.

    [SerializeField]
    private GameObject go_InventoryBase; // Inventory_Base 이미지
    [SerializeField]
    private GameObject go_SlotsParent;  // Slot들의 부모인 Grid Setting 

    private Slot[] slots;  // 슬롯들 배열
   
    [SerializeField]
    private GameObject go_QuickSlotParent;  // 퀵슬롯 영역

    private Slot[] quickSlots; // 퀵슬롯의 슬롯들
    private bool isNotPut;
    private ItemEffectDatabase theItemEffectDatabase;

    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
        quickSlots = go_QuickSlotParent.GetComponentsInChildren<Slot>();
        theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();

    }

    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I)&&!PlayerHealth.Death&&!GameManager.instance.Loading&&!GameManager.instance.pauseOn && !TalkManager.instance.talking)
        {
            invectoryActivated = !invectoryActivated;

            if (invectoryActivated)
                OpenInventory();
            else
                CloseInventory();

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (invectoryActivated) {
                invectoryActivated = !invectoryActivated;

                CloseInventory();

            }
        }
    }

    private void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
    }

    public void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
        theItemEffectDatabase.HideToolTip();
    }
    
    //퀵슬롯에 같은템있으면 퀵슬롯으로 먼저 배정하게 만들어줌
    public void AcquireItem(Item _item, int _count = 1)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                if (slots[i].item.itemName == _item.itemName)
                {
                    PutSlot(slots, _item, _count);
                    return;

                }
            }
        }
        for (int i = 0; i < quickSlots.Length; i++)
        {
            if (quickSlots[i].item != null)
            {
                if (quickSlots[i].item.itemName == _item.itemName)
                {
                    PutSlot(quickSlots, _item, _count);
                    return;

                }
            }
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                PutSlot(slots, _item, _count);

                return;
            }
        }
       

    }
    //만약 퀵슬롯에 같은 아이템이 존재하면 퀵슬롯으로
    //퀵슬롯에 같은아이템이 존재하지않으면 인벤토리로


    private void PutSlot(Slot[] _slots, Item _item, int _count)
    {
        if (Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].item != null)
                {
                    if (_slots[i].item.itemName == _item.itemName)
                    {
                        _slots[i].SetSlotCount(_count);
                        isNotPut = false;
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].item == null)
            {
                _slots[i].AddItem(_item, _count);
                isNotPut = false;
                return;
            }
        }

        isNotPut = true;
    }
}
