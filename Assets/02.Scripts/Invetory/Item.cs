using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{

    public string itemName; // 아이템의 이름.
    public ItemType itemType; // 아이템의 유형.
    public Sprite itemImage; // 아이템의 이미지.


    public enum ItemType
    {  
        Equipment,//장비
        Used,//소모품
        Ingredient,//재료
        ETC//기타
    }
}
