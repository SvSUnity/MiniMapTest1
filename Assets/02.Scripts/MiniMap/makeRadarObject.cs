using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class makeRadarObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<PhotonView>() != null)
            RadarMap.RegisterMapObject(this.gameObject,GetComponent<PhotonView>().ownerId);
        else
            RadarMap.RegisterMapObject(this.gameObject, 0);
    }
    void OnDisable()
    {
        RadarMap.RemoveMapObject(this.gameObject);
    }


}
