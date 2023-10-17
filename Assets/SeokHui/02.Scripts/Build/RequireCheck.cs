using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequireCheck : MonoBehaviour
{
    Inventory myInven;
    void Awake()
    { 
        myInven = GameObject.FindObjectOfType<Inventory>();
    }

    public bool Check(Requirement req)
    {
        return myInven.InventoryCheck(req.item, req.itemCount);
    }

}
