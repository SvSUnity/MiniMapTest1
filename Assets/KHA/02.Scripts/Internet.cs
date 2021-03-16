using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Internet : MonoBehaviour
{
    public GameObject NetworkOn;
    public GameObject NetworkOff;
    public GameObject wireless;
    public GameObject AlertView;
    void Start()
    {
        switch (Application.internetReachability)
        {
            case NetworkReachability.NotReachable:
                Debug.Log("네트워크 연결 안됨");
                NetworkOff.SetActive(true);
                AlertView.SetActive(true);
                break;
            case NetworkReachability.ReachableViaCarrierDataNetwork:
                Debug.Log("데이터로 연결됨");
                NetworkOn.SetActive(true);
              //  AlertView.SetActive(true);
                break;
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                Debug.Log("Wifi나 케이블로 연결됨");
                wireless.SetActive(true);
               // AlertView.SetActive(true);
                break;
        }
    }
}

