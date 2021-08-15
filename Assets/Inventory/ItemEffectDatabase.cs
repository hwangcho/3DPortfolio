using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName;  // 아이템의 이름(Key값으로 사용할 것)
    public string[] part;  // 효과. 어느 부분을 회복하거나 혹은 깎을 포션인지. 포션 하나당 미치는 효과가 여러개일 수 있어 배열.
    public int[] num;  // 수치. 포션 하나당 미치는 효과가 여러개일 수 있어 배열. 그에 따른 수치.
}

public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;

    private const string HP = "HP", MP = "MP", STEROID = "STEROID";

    [SerializeField]
    private GameObject thePlayerStatus;

    [SerializeField]
    private SlotToolTip theSlotToolTip;
    public void UseItem(Item _item)
    {
        if (_item.itemType == Item.ItemType.Equipment)
        {
            return;
        }
        if (_item.itemType == Item.ItemType.Used)
        {
            for (int i = 0; i < itemEffects.Length; i++)
            {
                if (itemEffects[i].itemName == _item.itemName)
                {
                    for (int j = 0; j < itemEffects[i].part.Length; j++)
                    {
                        switch (itemEffects[i].part[j])
                        {
                            case HP:
                                StatManager.instance.IncreaseHP(itemEffects[i].num[j]);
                                break;
                            case MP:
                                StatManager.instance.IncreaseMP(itemEffects[i].num[j]);
                                break;
                            case STEROID:
                                StatManager.instance.IncreaseMaxHPMp(itemEffects[i].num[j]);
                              
                                break;

                            default:

                                break;
                        }
                        Debug.Log(_item.itemName + " 을 사용했습니다.");
                    }
                    return;
                }
            }
        }
    }

    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        theSlotToolTip.ShowToolTip(_item, _pos);
    }


    public void HideToolTip()
    {
        theSlotToolTip.HideToolTip();
    }
}
