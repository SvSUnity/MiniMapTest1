using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapObject
{
    Image _icon ;
    GameObject _owner;
    public Image icon 
    { 
        get
        {
            return _icon;
        }
        
      
        set
        {
            _icon = value;
        }
    }
    public GameObject owner 
    { 
        get
        {
            return _owner;
        }
        set
        {
            _owner = value;
        }
    }

}

public class RadarMap : MonoBehaviour
{
    public Transform playerPos;
    public float mapScale = 2f;

    public static List<MapObject> mapObject = new List<MapObject>();

    public static void RegisterMapObject(GameObject o)
    {
        //Image image = Instantiate(i);
        Image image = null;
        

        image = MinimapIconManager.instance.GetMinimapIcon(o.tag);
           
        mapObject.Add(new MapObject() { owner = o, icon = image });
    }

    public static void RemoveMapObject(GameObject o)
    {
        Debug.Log(1);
        //List<MapObject> newList = new List<MapObject>();
        for(int i = 0; i< mapObject.Count;i++)
        {
            
            if (mapObject[i].owner == o && mapObject[i].icon !=null)
            {
                //Destroy(mapObject[i].icon.gameObject);

                if (GameObject.FindGameObjectWithTag("MinimapManager") != null)
                {
                    mapObject[i].icon.gameObject.SetActive(false);
                    mapObject[i].icon.gameObject.transform.SetParent(MinimapIconManager.instance.transform);
                    mapObject.RemoveAt(i);
                }
                else
                    return;

                //continue;
            }
            //else
            //    newList.Add(mapObject[i]);
        }
        //mapObject.RemoveRange(0, mapObject.Count);
        //mapObject.AddRange(newList);
    }

    void DrawMapDots()
    {
        foreach(MapObject m in mapObject)
        {
            //플레이어 위치를 중심으로 대상오브젝트의 상대적위치
            Vector3 mapPos = (m.owner.transform.position - playerPos.position);

            //실제 오브젝트 사이의 거리 * 맵스케일
            float dist2Object = Vector3.Distance(playerPos.position, m.owner.transform.position) * mapScale;

            //대상오브젝트의 절대각도
            float deltay = Mathf.Atan2(mapPos.z, mapPos.x) * Mathf.Rad2Deg+45; //+ playerPos.eulerAngles.y;

          
            //미니맵 아이콘x좌표
            mapPos.x = dist2Object * Mathf.Cos(deltay * Mathf.Deg2Rad);
            //미니맵 아이콘y좌표
            mapPos.z = dist2Object * Mathf.Sin(deltay * Mathf.Deg2Rad);

            m.icon.transform.SetParent(this.transform);
            m.icon.transform.position = new Vector3(mapPos.x, mapPos.z, 0) + this.transform.position;
            m.icon.transform.localScale = new Vector3(1, 1, 1);
        }
    }


    // Update is called once per frame
    void Update()
    {
        DrawMapDots();
    }


    [ContextMenu("ShowList")]
    void ShowList()
    {
        foreach(MapObject m in mapObject)
        {
            Debug.Log(m.owner);
        }
    }
}
