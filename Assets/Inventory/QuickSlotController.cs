using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotController : MonoBehaviour
{
    [SerializeField] private Slot[] quickSlots;  // 퀵슬롯들 (8개)
    [SerializeField] private Transform tf_parent;  // 퀵슬롯들의 부모 오브젝트

    private int selectedSlot;  // 선택된 퀵슬롯의 인덱스 (0~7)

    public Item[] item;
    private ItemEffectDatabase theItemEffectDatabase;

    [SerializeField]
    private GameObject go_SlotsParent;  // Slot들의 부모인 Grid Setting 

    private Slot[] slots;  // 슬롯들 배열

    void Start()
    {
        theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
        quickSlots = tf_parent.GetComponentsInChildren<Slot>();
        selectedSlot = 0;
    }

    void Update()
    {
        TryInputNumber();
       
    }
    private void FixedUpdate()
    {
        item[0] = slots[0].item;
        item[1] = slots[1].item;

        item[2] = slots[2].item;
        item[3] = slots[3].item;
     }
    private void TryInputNumber()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            if (item[0] != null)
            {
                theItemEffectDatabase.UseItem(item[0]);

                if (item[0].itemType == Item.ItemType.Used)
                    slots[0].SetSlotCount(-1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
                      if (item[1] != null)
                      {
                          theItemEffectDatabase.UseItem(item[1]);

                          if (item[1].itemType == Item.ItemType.Used)
                              slots[1].SetSlotCount(-1);
                      }
            if (Input.GetKeyDown(KeyCode.Alpha3))
                 if (item[2] != null)
                 {
                     theItemEffectDatabase.UseItem(item[2]);

                     if (item[2].itemType == Item.ItemType.Used)
                         slots[2].SetSlotCount(-1);
                 }
        if (Input.GetKeyDown(KeyCode.Alpha4))
            if (item[3] != null)
            {
                theItemEffectDatabase.UseItem(item[3]);

                if (item[3].itemType == Item.ItemType.Used)
                    slots[3].SetSlotCount(-1);
            }

    }
    public void AcquireItem(Item _item, int _count = 1)
    {
        if (Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)  // null 이라면 slots[i].item.itemName 할 때 런타임 에러 나서
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
   
    private void ChangeSlot(int _num)
    {
        SelectedSlot(_num);

    }

    private void SelectedSlot(int _num)
    {
        // 선택된 슬롯
        selectedSlot = _num;


    }
}
