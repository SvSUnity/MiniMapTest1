using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SelectObjectRay : MonoBehaviour
{
    Ray ray;
    RaycastHit hitInfo;
    GameObject selectEffect;
    int Pid;
    List<Rect> dontTouchArea = new List<Rect>();//터치불가능영역, UI영역
    PlayerMoveCtrl player;

    void Awake()
    {
        selectEffect = transform.Find("selectObject").gameObject;
        dontTouchArea.Add(new Rect(0, 0, Screen.width * 0.3f, Screen.height * 0.5f));
        dontTouchArea.Add(new Rect(Screen.width * 0.7f, 0, Screen.width * 0.3f, Screen.height * 0.3f));
        dontTouchArea.Add(new Rect(Screen.width * 0.9f, 0, Screen.width * 0.1f, Screen.height * 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        //int layerMask = (1 << LayerMask.NameToLayer("Map")) + (1 << LayerMask.NameToLayer("Enemy")) + (1 << LayerMask.NameToLayer("Player"));
        //layerMask = ~layerMask;
        int layerMask = (1 << LayerMask.NameToLayer("Item")) + (1 << LayerMask.NameToLayer("Ground"));

        //레이캐스트가 지면과 아이템만 인식
#if UNITY_EDITOR
        //선택불가영역확인용
        DebugDrawRect(dontTouchArea[0], Color.red);
        DebugDrawRect(dontTouchArea[1], Color.red);
        DebugDrawRect(dontTouchArea[2], Color.red);
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.blue);
        if (Input.GetMouseButtonDown(0) && !Inventory.inventoryActivated)
        {
            Vector2 pos = Input.mousePosition;
            if (!dontTouchArea[0].Contains(pos) && !dontTouchArea[1].Contains(pos))
            {
                if (Physics.Raycast(ray, out hitInfo, 150.0f, layerMask))
                {
                    if (hitInfo.collider.tag == "Item")
                    {
                        player.btnSet(hitInfo.collider.gameObject);
                        selectEffect.SetActive(true);
                        selectEffect.transform.position = new Vector3(hitInfo.transform.position.x, 0.1f, hitInfo.transform.position.z);
                    }
                    else if (hitInfo.collider.tag == "Ground")
                    { print("Tag : Ground");
                        player.btnSet(hitInfo.collider.gameObject);
                        selectEffect.SetActive(false);
                    }
                    else if (hitInfo.collider == null)
                        return;

                }
            }
        }
#endif
#if UNITY_ANDROID
        if (Input.touchCount > 0 && !Inventory.inventoryActivated)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    Vector2 pos = Input.GetTouch(i).position;
                    ray = Camera.main.ScreenPointToRay(Input.touches[i].position);

                    if (!dontTouchArea[0].Contains(pos) && !dontTouchArea[1].Contains(pos))
                    {
                        if (Physics.Raycast(ray, out hitInfo, 150.0f, layerMask))
                        {
                            if (hitInfo.collider.tag == "Item")
                            {
                                player.btnSet(hitInfo.collider.gameObject);
                                selectEffect.SetActive(true);
                                selectEffect.transform.position = new Vector3(hitInfo.transform.position.x, 0.1f, hitInfo.transform.position.z);
                            }
                            else if (hitInfo.collider.tag == "Ground")
                            {
                                player.btnSet(hitInfo.collider.gameObject);
                                selectEffect.SetActive(false);
                            }
                            else if (hitInfo.collider == null)
                                return;
                        }
                    }
                }

            }
        }
#endif
    }
    public void SetPlayerMoveCtrl(GameObject go)
    {
        if (go.GetComponent<PlayerMoveCtrl>() != null)
            player = go.GetComponent<PlayerMoveCtrl>();
    }

    //플레이어로부터 오브젝트 파괴시 호출됨
    public void SelectObjectDestroy()
    {
        selectEffect.SetActive(false);
    }


    //선택불가영역 디버그용
    void DebugDrawRect(Rect rect,Color color)
    {
        //아래
        Debug.DrawLine(new Vector3(rect.x, rect.y), new Vector3(rect.x + rect.width, rect.y), color);
        //왼쪽
        Debug.DrawLine(new Vector3(rect.x, rect.y), new Vector3(rect.x, rect.y+rect.height), color);
        //오른쪽
        Debug.DrawLine(new Vector3(rect.x+rect.width, rect.y), new Vector3(rect.x + rect.width, rect.y + rect.height), color);
        //위
        Debug.DrawLine(new Vector3(rect.x, rect.y + rect.height), new Vector3(rect.x + rect.width, rect.y + rect.height), color);
    }
}
