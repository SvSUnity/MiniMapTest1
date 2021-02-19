using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{

    public Item item; // 획득한 아이템.
    public int itemCount; // 획득한 아이템의 개수.
    public Image itemImage; // 아이템의 이미지.

    public bool onDropMessage = false;


    // 필요한 컴포넌트.
    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_CountImage;


    // 이미지의 투명도 조절.
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // 아이템 획득
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if (item.itemType != Item.ItemType.Equipment)
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }

        SetColor(1);
    }

    // 아이템 개수 조정.
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }

    // 슬롯 초기화.
    private void ClearSlot()
    {

        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            itemImage.gameObject.SetActive(false);

            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //드롭이벤트가 발생하지않은경우
        if (!DragSlot.instance.dragSlot.onDropMessage)
        {
            Transform playerPos = GameObject.FindGameObjectWithTag("Player").transform;
            DropItemInfo dropObj = PhotonNetwork.Instantiate("DropObject", playerPos.position, playerPos.rotation, 0).GetComponent<DropItemInfo>();
            dropObj.SetDropItemInfo(DragSlot.instance.dragSlot);
            ClearSlot();
        }

        DragSlot.instance.dragSlot.onDropMessage = false;
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;



    }

    //다른 IDropHandler 를 가진 오브젝트에 마우스클릭을 뗐을때호출됨
    //현재는 다른 슬롯에 드래그드롭했을때 슬롯이 교체됨
    public void OnDrop(PointerEventData eventData)
    {
        DragSlot.instance.dragSlot.onDropMessage = true;
        DragSlot.instance.dragSlot.itemImage.gameObject.SetActive(true);
        if (DragSlot.instance.dragSlot != null)
            ChangeSlot();

    }

    private void ChangeSlot()
    {
        Item _tempItem = item;
        int _tempItemCount = itemCount;
        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (_tempItem != null)
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        else
            DragSlot.instance.dragSlot.ClearSlot();
    }
}
