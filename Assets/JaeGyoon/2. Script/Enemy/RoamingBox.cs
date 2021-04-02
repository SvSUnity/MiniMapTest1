using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamingBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider coll)
    {
        if ( coll.gameObject.tag =="Enemy")
        {
            coll.gameObject.GetComponent<EnemyCtrl>().RoamingCheckStart();
        }
    }

    private void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            coll.gameObject.GetComponent<EnemyCtrl>().RoamingCheckStart();
        }
    }


    //로밍박스 범위를 벗어나야 다시 로밍체크가 가능하도록 설정
    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            coll.gameObject.GetComponent<EnemyCtrl>().CanRoamingCheckStart();
        }
    }

}
