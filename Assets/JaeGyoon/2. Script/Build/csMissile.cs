using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csMissile : MonoBehaviour
{

    public float missileSpeed = 20f; // 미사일 속도
    public float missileRange = 20f; // 미사일 최대 사정거리 ( 넘어가면 소멸 )

    public int missileDMG = 10; // 미사일 데미지

    //public GameObject ExploPtcl; // 폭발 이펙트

    private float dist; // 총알이 날아간 거리






    public int playerId = -1;




    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5.0f); // 총알이 맞든 안맞든 5초뒤에는 소멸
    }

    // Update is called once per frame
    void Update()
    {

        // GetComponent<Rigidbody>().AddForce(Vector3.forward * speed); // 0,0,1 ( z 값이 1 * speed 만큼 증가 ) // 이러면 월드 좌표로 날라감.
        // GetComponent<Rigidbody>().AddForce(transform.forward * speed); // 이러면 총알의 위치가 변경될때마다 이상하게 날라감.
        // GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed); // 결국 이게 정답이다. 월드 좌표를 로컬 좌표로 바꿔주는 리얼리티브 포스

        // 하지만 이런 방법도 있다.

        transform.Translate(Vector3.forward * Time.deltaTime * missileSpeed);

        dist += Time.deltaTime * missileSpeed; // 델타타임으로 정해진 속도로 총알이 날아감.


        if (dist >= missileRange) // 날아간 거리가 미사일 사정거리보다 같거나 커지면 폭발하고 사라짐.
        {
            //Instantiate(ExploPtcl, transform.position, transform.rotation);
            Destroy(gameObject);
        }

    }

    //private void OnCollisionEnter(Collision collision)
    //{

    //    if ( collision.transform.tag == "Enemy")
    //    {
    //        Destroy(gameObject);
    //    }

    //    //Instantiate(ExploPtcl, transform.position, transform.rotation);


    //    //Destroy(gameObject);
    //}


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            Destroy(gameObject);
        }

        //Instantiate(ExploPtcl, transform.position, transform.rotation);


        //Destroy(gameObject);
    }

}
