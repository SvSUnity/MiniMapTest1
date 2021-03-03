using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prefabObject : MonoBehaviour
{
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    [ContextMenu("DestroyTest")]
    void Boom()
    {
        // 자신과 네트워크상의 아바타들까지 모두 소멸
        PhotonNetwork.Destroy(this.gameObject);
    }

}
