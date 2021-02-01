using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class makeRadarObject : MonoBehaviour
{
    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        RadarMap.RegisterMapObject(this.gameObject, image);
    }


    void OnDestroy()
    {
        RadarMap.RemoveMapObject(this.gameObject);
    }
}
