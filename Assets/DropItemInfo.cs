using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItemInfo : MonoBehaviour
{
    //드랍한 아이템을 다시줏엇을때 슬롯에 넘기기위한 정보
    Item item; // 획득한 아이템.
    int itemCount; // 획득한 아이템의 개수.
    Image itemImage; // 아이템의 이미지

    void Awake()
    {
    }

    public void SetDropItemInfo(Slot s)
    {
        item = s.item;
        itemCount = s.itemCount;
        itemImage = s.itemImage;
    }
}
