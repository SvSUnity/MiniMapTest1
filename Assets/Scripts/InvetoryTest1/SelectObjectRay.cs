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
    Rect dontTouchArea;//터치불가능영역, 조이스틱위치

    void Awake()
    {
        selectEffect = transform.Find("selectObject").gameObject;
        dontTouchArea = new Rect(0, 0, Screen.width * 0.3f, Screen.height * 0.5f);
    }

    // Update is called once per frame
    void Update()
    {


#if UNITY_EDITOR
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.blue);
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Input.mousePosition;
            if (!dontTouchArea.Contains(pos))
            {
                if (Physics.Raycast(ray, out hitInfo, 150.0f))
                {
                    if (hitInfo.collider.tag == "Item")
                    {
                        selectEffect.SetActive(true);
                        selectEffect.transform.position = new Vector3(hitInfo.transform.position.x, 0.1f, hitInfo.transform.position.z);
                    }
                    else if (hitInfo.collider.tag == "Ground")
                        selectEffect.SetActive(false);
                }
            }
        }
#endif
#if UNITY_ANDROID


        if (Input.touchCount > 0 )
        {
            for(int i = 0; i<Input.touchCount;i++)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    Vector2 pos = Input.GetTouch(i).position;
                    ray = Camera.main.ScreenPointToRay(Input.touches[i].position);

                    if (!dontTouchArea.Contains(pos))
                    {
                        if (Physics.Raycast(ray, out hitInfo, 150.0f))
                        {
                            if (hitInfo.collider.tag == "Item")
                            {
                                selectEffect.SetActive(true);
                                selectEffect.transform.position = new Vector3(hitInfo.transform.position.x, 0.1f, hitInfo.transform.position.z);
                            }
                            else if (hitInfo.collider.tag == "Ground")
                                selectEffect.SetActive(false);
                        }
                    }
                }

            }
        }
#endif
    }

}
