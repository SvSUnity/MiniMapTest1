using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapObject
{
    public Image icon { get; set; }
    public GameObject owner { get; set; }
}

public class RadarMap : MonoBehaviour
{
    public Transform playerPos;
    float mapScale = 2.0f;

    public static List<MapObject> mapObject = new List<MapObject>();

    public static void RegisterMapObject(GameObject o, Image i)
    {
        Image imagae = Instantiate(i);
        mapObject.Add(new MapObject() { owner = o, icon = i });
    }

    public static void RemoveMapObject(GameObject o)
    {
        List<MapObject> newList = new List<MapObject>();
        foreach(MapObject m in mapObject)
        {
            if (m.owner == o)
            {
                Destroy(m.icon);
                continue;
            }
            else
                newList.Add(m);
        }
        mapObject.RemoveRange(0, mapObject.Count);
        mapObject.AddRange(newList);
    }

    void DrawMapDots()
    {
        foreach(MapObject m in mapObject)
        {
            Vector3 mapPos = (m.owner.transform.position - playerPos.position);
            float dist2Object = Vector3.Distance(playerPos.position, m.owner.transform.position) * mapScale;
            float deltay = Mathf.Atan2(mapPos.x, mapPos.z) * Mathf.Rad2Deg - 270 - playerPos.eulerAngles.y;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
