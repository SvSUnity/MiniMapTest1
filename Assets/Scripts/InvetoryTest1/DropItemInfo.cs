using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DropItemInfo : MonoBehaviour
{
    //드랍한 아이템을 다시줏엇을때 슬롯에 넘기기위한 정보목록
    public Item[] itemList;
    Item item;
    int itemCount;
    SpriteRenderer itemSpr;
    Inventory inventory;

    public string currItemName;
    public int currItemCount;
    public TextMeshPro itemCountText;
    PhotonView pv;

    void Awake()
    {
        itemSpr = transform.Find("DropItemImage").GetComponent<SpriteRenderer>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        pv = GetComponent<PhotonView>();
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        pv.ObservedComponents[0] = this;
    }
    void FixedUpdate()
    {
        //동기화를통해 값이 넘어오고 현재 item이 null인경우
        if (currItemName != null && currItemCount != 0 && item == null)
        {
            RPCSetDropItemInfo(currItemName, currItemCount);
        }
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
        if(Item.ItemType.Equipment != item.itemType)
            itemCountText.text = itemCount.ToString();
    }
    void OnTriggerEnter(Collider col)
    {

        if(col.tag == "Player")
        {
            inventory.AcquireItem(item, itemCount);
            gameObject.SetActive(false);
        }
        else if(col.tag == "TeamPlayer")
        {
            gameObject.SetActive(false);
        }
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //로컬 플레이어의 위치 정보를 송신
        if (stream.isWriting)
        {
            //박싱
            stream.SendNext(item.itemName);
            stream.SendNext(itemCount);
        }
        //원격 플레이어의 위치 정보를 수신
        else
        {
            //언박싱
            currItemName = (string)stream.ReceiveNext();
            currItemCount = (int)stream.ReceiveNext();
        }

    }

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        DropObjectManager.instance.newObject = this.gameObject;
        DropObjectManager.instance.dropItems.Add(this.gameObject);
        transform.SetParent(DropObjectManager.instance.transform);
    }
}
