using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObjectManager : MonoBehaviour
{
    public static DropObjectManager instance;
    public List<GameObject> dropItems = new List<GameObject>();
    PhotonView pv;
    public GameObject newObject;
    public GameObject reqObject;

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
        reqObject = null;

        CheckExist();
        pv.RPC("CheckExist", PhotonTargets.OthersBuffered, null);

        if (reqObject == null)
        {
            PhotonNetwork.Instantiate("DropObject" , pos, rot,0);
            reqObject = newObject;

        }
        reqObject.SetActive(true);
        reqObject.transform.position = pos;

        return reqObject;
    }

    //리스트내부에 오브젝트가 이미 있는지 체크
    [PunRPC]
    void CheckExist()
    {
        foreach (GameObject go in dropItems)
        {
            if (go.activeSelf == false)
            {
                reqObject = go;
                break;
            }
        }
    }
}
