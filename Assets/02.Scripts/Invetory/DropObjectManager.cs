using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObject
{
    GameObject _reqObject;
    GameObject _newObject;

    public GameObject newObject
    {
        get
        {
            return _newObject;
        }
        set
        {
            _newObject = value;
        }
    }

    public GameObject reqObject
    {
        get
        {
            return _reqObject;
        }
        set
        {
            _reqObject = value;
        }
    }
}


public class DropObjectManager : MonoBehaviour
{
    public static DropObjectManager instance;
    List<GameObject> dropItems = new List<GameObject>();
    PhotonView pv;

    public DropObject dropObject = new DropObject();

    // Start is called before the first frame update
    void Awake()
    {
        if(DropObjectManager.instance == null)
        {
            DropObjectManager.instance = this;
        }
        pv = GetComponent<PhotonView>();
        pv.ObservedComponents[0] = this;
    }
    void CreateDropItem(int cnt)
    {
        for (int i = 0; i < cnt; i++)
        {
            GameObject dropItem = PhotonNetwork.Instantiate("DropObject", transform.position, transform.rotation, 0) as GameObject;
            dropItem.transform.SetParent(transform);
            dropItem.SetActive(false);
            dropItems.Add(dropItem);
        }
    }

    public GameObject GetDropItem(Vector3 pos, Quaternion rot)
    {
        dropObject.reqObject = null;

        CheckExist();
        pv.RPC("CheckExist", PhotonTargets.OthersBuffered, null);
        if (dropObject.reqObject == null)
        {
            dropObject.newObject = PhotonNetwork.Instantiate("DropObject", pos, rot, 0);
        }
        SetReqObejct(pos);
        pv.RPC("SetReqObejct", PhotonTargets.OthersBuffered, pos);

        return dropObject.reqObject;
    }

    //리스트내부에 오브젝트가 이미 있는지 체크
    [PunRPC]
    void CheckExist()
    {
        foreach (GameObject go in dropItems)
        {
            if (go.activeSelf == false)
            {
                dropObject.reqObject = go;
                break;
            }

        }
    }

    [PunRPC]
    void SetReqObejct(Vector3 pos)
    {
        GameObject go = dropObject.reqObject;
        go.SetActive(true);
        go.transform.position = pos;
    }

    public void ListAdd(GameObject go)
    {
        dropItems.Add(go);
    }
}
