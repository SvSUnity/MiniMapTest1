using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackPoint : MonoBehaviour
{
    public int power;
    public Collider co;

    public GameObject enemyAttackEffect;

    //충돌이 일어나면 코루틴으로 연속충돌을 방지한다




    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "TeamPlayer")
        {

            GameObject enemyblood1 = Instantiate(enemyAttackEffect, coll.transform.position, Quaternion.identity) as GameObject;

            StartCoroutine(this.ResetColl());
        }

    }

    IEnumerator ResetColl()
    {
        co.enabled = false;
        yield return new WaitForSeconds(0.5f);
        co.enabled = true;
    }
}
