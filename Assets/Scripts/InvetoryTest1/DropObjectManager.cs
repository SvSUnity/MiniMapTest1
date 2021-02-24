using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObjectManager : MonoBehaviour
{
    public static DropObjectManager instance;
    public List<GameObject> dropItems = new List<GameObject>();
    PhotonView pv;
    public GameObject newObject;

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
        GameObject reqObject = null;

        foreach (GameObject go in dropItems)
        {
            if (go.activeSelf == false)
            {
                reqObject = go;
                break;
            }
        }
        if (reqObject == null)
        {

            object[] data = new object[1];
            data[0] = this.name;
            Test(pos, rot, data);
            //pv.RPC("Test", PhotonTargets., pos, rot, data);
            reqObject = newObject;

        }
        reqObject.SetActive(true);
        reqObject.transform.position = pos;

        return reqObject;
    }
    [PunRPC]
    void Test(Vector3 pos, Quaternion rot, object[] data)
    {
        newObject = PhotonNetwork.Instantiate("DropObject", pos, rot, 0, data);
    }

    //void OnPhotonInstantiate(PhotonMessageInfo info)
    //{
    //    newObject.transform.SetParent(transform);
    //    dropItems.Add(newObject);
    //}

    public void  SetNewObject()
    {
        newObject.transform.SetParent(this.transform);
        dropItems.Add(newObject);
    }

}
