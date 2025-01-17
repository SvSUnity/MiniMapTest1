﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public static bool inventoryActivated = false;

    public List<int> slotIndex = new List<int>();

    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotsParent;

    // 인벤토리슬롯
    private Slot[] slots;

    int inventoryCnt=0;//아이템이 존재하는 인벤토리 칸수

    // Use this for initialization
    void Awake()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
    }

    public void OnBtn()
    {
        inventoryActivated = !inventoryActivated;

        if (inventoryActivated)
            OpenInventory();
        else
            CloseInventory();
    }

    private void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        //이미존재하는 아이템경우 개수만증가시킴
        if (Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }
        //이미존재하지않는경우 새로운슬롯에할당
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                IncreseIvenCnt();
                return;
            }
        }
    }

    public void IncreseIvenCnt ()
    {
        inventoryCnt++;
    }
    public void DecreseIvenCnt()
    {
        inventoryCnt--;
    }
    public bool IsInvenFull()
    {
        if (inventoryCnt == slots.Length)
            return true;
        else
            return false;
    }


    public bool InventoryCheck(List<Item> _item,List<int> _itemCount)
    {
        bool result = true;

        int cnt = 0;
        foreach(Item item in _item)
        {
            int listIndex = -1;
            for (int i = 0; i < slots.Length; ++i)
            {
                //슬롯에 요구아이템이 존재하는경우
                if (item == slots[i].item)
                {
                    listIndex = _item.IndexOf(item);
                    Debug.Log(listIndex);
                    slotIndex.Add(i);//아이템 존재하는 슬롯의 인덱스 저장
                    result = slots[i].itemCount >= _itemCount[listIndex] ? true : false;
                    if(result)
                        cnt++;
                    break;
                }
                else
                    result = false;
            }
        }



        if (cnt == _item.Count)
        {
            int i = 0;
            foreach (int index in slotIndex)
            {
                slots[index].SetSlotCount (-_itemCount[i++]);
            }
        }
        else
            result = false;


        slotIndex.Clear();
        return result;
    }
}
