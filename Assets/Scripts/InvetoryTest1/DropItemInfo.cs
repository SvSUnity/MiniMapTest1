using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DropItemInfo : MonoBehaviour
{
    //드랍한 아이템을 다시줏엇을때 슬롯에 넘기기위한 정보목록
    public Item[] itemList;
    Item item;
    int itemCount;
    SpriteRenderer itemSpr;
    Inventory inventory;

    string currItemName;
    int currItemCount;
    Item currItem;

    PhotonView pv;
    void Awake()
    {
        itemSpr = transform.Find("DropItemImage").GetComponent<SpriteRenderer>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        pv = GetComponent<PhotonView>();
        pv.ObservedComponents[0] = this;
    }

    public void SetDropItemInfo(Slot slotInfo)
    {
        RPCSetDropItemInfo(slotInfo.item.itemName, slotInfo.itemCount);
        pv.RPC("RPCSetDropItemInfo", PhotonTargets.Others, slotInfo.item.itemName, slotInfo.itemCount);

    }

    [PunRPC]
    void RPCSetDropItemInfo(string _itemName, int _itemCount)
    {
        foreach(Item i in itemList )
        {
            if (_itemName == i.itemName)
                item = i;
        }
        itemCount = _itemCount;
        itemSpr.sprite = item.itemImage;
    }
    void OnTriggerEnter(Collider col)
    {

        if(col.tag == "Player")
        {
            inventory.AcquireItem(item, itemCount);
            pv.TransferOwnership(PhotonNetwork.player.ID);
            PhotonNetwork.Destroy(gameObject);
        }
    }

    //void onPhotonInstantiate(PhotonMessageInfo info)
    //{
    //    currItem = item;
    //    currItemCount = itemCount;
    //}
}
