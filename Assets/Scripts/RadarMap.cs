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
        Image image = Instantiate(i);
        mapObject.Add(new MapObject() { owner = o, icon = image });
    }

    public static void RemoveMapObject(GameObject o)
    {
        List<MapObject> newList = new List<MapObject>();
        for(int i = 0; i< mapObject.Count;i++)
        {
            if (mapObject[i].owner == o)
            {
                Destroy(mapObject[i].icon.gameObject);
                continue;
            }
            else
                newList.Add(mapObject[i]);
        }
        mapObject.RemoveRange(0, mapObject.Count);
        mapObject.AddRange(newList);
    }

    void DrawMapDots()
    {
        foreach(MapObject m in mapObject)
        {
            Debug.Log(m);
            Vector3 mapPos = (m.owner.transform.position - playerPos.position);
            float dist2Object = Vector3.Distance(playerPos.position, m.owner.transform.position) * mapScale;
            float deltay = Mathf.Atan2(mapPos.x, mapPos.z) * Mathf.Rad2Deg - 270 - playerPos.eulerAngles.y;
            mapPos.x = dist2Object * Mathf.Cos(deltay * Mathf.Deg2Rad) * -1;
            mapPos.z = dist2Object * Mathf.Sin(deltay * Mathf.Deg2Rad);

            m.icon.transform.SetParent(this.transform);
            m.icon.transform.position = new Vector3(mapPos.x, mapPos.z, 0) + this.transform.position;
        }
    }


    // Update is called once per frame
    void Update()
    {
        DrawMapDots();
    }
}
