using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnAction : MonoBehaviour
{
    GameObject actionBtn;
    Text btnText;

    void Awake()
    {
        //액션버튼
        actionBtn = GameObject.FindGameObjectWithTag("actionBtn");
        //테스트용 버튼텍스트
        btnText = actionBtn.transform.Find("Text").GetComponent<Text>();
    }


    public void btnSet(GameObject go)
    {
        if(go.tag=="item")
        {
            ItemInfo itemInfo = go.GetComponent<ItemInfo>();
            switch(itemInfo.item.name)
            {
                case "Rcok":

                    break;
            }
        }
    }
    void btnActionSet()
    {

    }
    
}
