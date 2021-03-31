using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : MonoBehaviour
{
    //몬스터 라이프
    public int life = 100;
    //자신의 트렌스폼
    private Transform myTr;
    //혈흔 효과 프리팹
    public GameObject enemyBloodEffect;
    //혈흔데칼 효과 프리팹
    public Transform enemyBloodDecal;

    //EnemyCtrl 연결 레퍼런스
    public EnemyCtrl enemy;
    //생명력 바 연결 레퍼런스 (특정 컴포넌트 아니면 Renderer 로 연결가능)
    //public MeshRenderer lifeBar;



    public PhotonView pv = null;



    void Awake()
    {
        //레퍼런스 할당
        myTr = GetComponent<Transform>();


        // 다른 연결 방식도 있다는걸 알려주는 포톤 뷰 컴포넌트 레퍼런스 할당
        pv = PhotonView.Get(this);
    }

    //포탑에 의해서 Enemy가 총알과 맞았을 때 호출되는 콜백 함수
    //void OnCollisionEnter(Collision coll)
    //{
    //    if (coll.gameObject.tag == "Bullet")
    //    {
    //        //총알이 맞은 위치를 위한 ContactPoint 구조체 선언 및 할당
    //        ContactPoint contact = coll.contacts[0];

    //        //혈흔 효과 함수를 호출
    //        CreateBlood(contact.point);

    //        // 포톤 추가
    //        // 각 요소를 변수로 저장
    //        int pow = coll.gameObject.GetComponent<csMissile>().missileDMG;
    //        int id = coll.gameObject.GetComponent<csMissile>().playerId;

    //        //(포톤 추가)모든 네트웍 유저의 몬스터에 RPC 데이타를 전송하며 RPC 함수를 호출, 로컬 플레이어는 로컬 Deamage 함수를 바로 호출 
    //        pv.RPC("Deamage", PhotonTargets.AllBuffered, pow, id);






    //        ////맞은 총알의 파워를 가져와 Enemy의 life를 감소
    //        //life -= coll.gameObject.GetComponent<BulletCtrl>().power;
    //        ////로컬적인 개념으로 머트리얼 셋팅
    //        //lifeBar.material.SetFloat("_Progress", life / 100.0f);


    //        //// 생명력이 바닥이면 죽이자
    //        //if (life <= 0)
    //        //{
    //        //    enemy.EnemyDie();
    //        //}







    //        //몬스터 타격 루틴을 위한 호출
    //        enemy.HitEnemy();
    //    }
    //}


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            CreateBlood(other.transform.position);

            // 포톤 추가
            // 각 요소를 변수로 저장
            int pow = other.gameObject.GetComponent<csMissile>().missileDMG;




            //(포톤 추가)모든 네트웍 유저의 몬스터에 RPC 데이타를 전송하며 RPC 함수를 호출, 로컬 플레이어는 로컬 Deamage 함수를 바로 호출 
            //pv.RPC("Deamage", PhotonTargets.MasterClient, pow);
            Deamage(pow);



            ////맞은 총알의 파워를 가져와 Enemy의 life를 감소
            //life -= coll.gameObject.GetComponent<BulletCtrl>().power;
            ////로컬적인 개념으로 머트리얼 셋팅
            //lifeBar.material.SetFloat("_Progress", life / 100.0f);


            //// 생명력이 바닥이면 죽이자
            //if (life <= 0)
            //{
            //    enemy.EnemyDie();
            //}







            //몬스터 타격 루틴을 위한 호출
            enemy.HitEnemy();
        }
    }



    //포톤 추가////////////////////////////////////////
    //포톤 클라우드를 위한 어트리뷰트로 함수 선언 
    [PunRPC]
    //데미지 함수
    void Deamage(int dam)
    {
        //맞은 총알의 파워를 가져와 Enemy의 life를 감소
        life -= dam;
        //로컬적인 개념으로 머트리얼 셋팅
        //lifeBar.material.SetFloat("_Progress", life / 100.0f);


        // 생명력이 바닥이면 죽이자
        if (life <= 0 )
        {
            // 포톤 추가
            //자신을 파괴시킨 적 네트워크 베이스의 스코어를 증가시키는 함수를 호출함
            //StartCoroutine(this.SaveKillCount(id));
            enemy.EnemyDie();
        }
    }

















    //Enemy가 Ray에 맞았을 때 호출되는 콜백 함수
    void OnCollision(object[] _params)
    {
        //Debug.Log(string.Format("info {0} : {1}", _params[0], _params[1]));

        //혈흔 효과 함수를 호출
        CreateBlood((Vector3)_params[0]);








        //맞은 총알의 파워를 가져와 Enemy의 life를 감소
        life -= (int)_params[1];
        //로컬적인 개념으로 머트리얼 셋팅
        //lifeBar.material.SetFloat("_Progress", life / 100.0f);

        // 생명력이 바닥이면 죽이자
        if (life <= 0)
        {
            enemy.EnemyDie();
        }

    }

    //// 드럼통 폭발 몬스터 사망 처리
    //public void OnCollisionBarrel(Vector3 firePos)
    //{
    //    //혈흔 효과 함수를 호출
    //    CreateBlood(firePos);

    //    //Enemy의 life를 0 으로
    //    life = 0;
    //    //로컬적인 개념으로 머트리얼 셋팅
    //    lifeBar.material.SetFloat("_Progress", life / 100.0f);

    //    enemy.EnemyBarrelDie(firePos);
    //}

    // 블러드 연출을 시작해주는 코루틴함수 호출
    void CreateBlood(Vector3 pos)
    {
        //혈흔 효과를 위한 코루틴 함수 호출
        StartCoroutine(this.CreateBloodEffects(pos));
    }

    IEnumerator CreateBloodEffects(Vector3 pos)
    {
        //blood effect 생성
        //GameObject enemyblood1 = Instantiate(enemyBloodEffect, pos, Quaternion.identity) as GameObject;
        //만약 블러드 이펙트에 오브젝트 삭제 컴포넌트가 없을시...
        //Destroy(enemyblood1, 1.5f);











        //만약 혈흔 프리팹에 차일드 오브젝트를(혈흔) up 방향으로 미리 올려놨다면...
        //혈흔데칼의 생성되는 위치는 바닥에서 조금 올린 위치로 만들어야 바닥에 묻히지 않는다
        //Vector3 decalPos = myTr.position + (Vector3.up * 0.1f);

        //혈흔데칼의 회전을 Y 축으로 랜덤으로 설정
        Quaternion decalRot = Quaternion.Euler(0, Random.Range(0, 360), 0);
        //혈흔데칼의 크기를 랜덤으로 설정
        float scale = Random.Range(1.0f, 2.5f);

        Vector3 Decal = new Vector3(myTr.position.x, myTr.position.y + 1f, myTr.position.z);


        //혈흔데칼 프리팹 생성
        //Transform enemyblood2 = Instantiate(enemyBloodDecal, decalPos, decalRot) as Transform;
        //만약 혈흔 프리팹에 차일드 오브젝트를(혈흔) up 방향으로 미리 올려놨다면...
        Transform enemyblood2 = Instantiate(enemyBloodDecal, Decal, decalRot) as Transform;

        //혈흔데칼의 크기를 랜덤으로 설정
        enemyblood2.localScale = Vector3.one * scale;

        yield return null;
    }




    //// 포톤 추가
    ////자신을 파괴시킨 네트워크 베이스를 검색해서 스코어를 증가시켜주는 함수
    //IEnumerator SaveKillCount(int firePlayerId)
    //{

    //    //Base 태그로 지정된 모든 네트워크 베이스를 가져와 배열에 저장
    //    GameObject[] bases = GameObject.FindGameObjectsWithTag("Base");

    //    // 전체 네트워크 베이스를 검색하여 총알의 주인을 찿아줌...
    //    foreach (GameObject _base in bases)
    //    {

    //        var baseCtrl = _base.GetComponent<BaseCtrl>();

    //        //네트워크베이스의 playerId가 총알의 playerId와 동일한지 판단
    //        if (baseCtrl != null && baseCtrl.playerId == firePlayerId)
    //        {

    //            //동일한 베이스일 경우 스코어를 증가시켜줌
    //            baseCtrl.PlusKillCount();
    //            break;

    //        }

    //    }

    //    yield return null;

    //}











}
