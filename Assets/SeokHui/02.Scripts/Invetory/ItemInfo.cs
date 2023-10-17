using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public Item item;


    void OnDestroy()
    {
        if(GameObject.FindGameObjectWithTag("selectObject") != null)
        {
            SelectObjectRay so = GameObject.FindGameObjectWithTag("selectObject").GetComponent<SelectObjectRay>();
            so.SelectObjectDestroy();//오브젝트가 파괴됬음을 알림
        }

    }
}