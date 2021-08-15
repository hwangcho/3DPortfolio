using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }

    public string itemName;
    [TextArea]  // 여러 줄 가능해짐
    public string itemDesc; // 아이템의 설명
    public ItemType itemType;
    public Sprite itemImage;
    public GameObject itemPrefab;

    public string weaponType;
}
