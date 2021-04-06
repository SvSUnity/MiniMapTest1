using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class csTurret : MonoBehaviour
{
    [HideInInspector]
    public bool isDie; // 터렛이 작동중인지 부셔졌는지 확인 유무

    public float dist1; // 적과의 거리를 위한 변수
    public float dist2;

    private GameObject[] Enemys; // 적을 찾기 위한 배열
    Transform EnemyTarget; //

    private Transform myTr; // 터렛 자신의 위치를 확인하는 변수

    public Transform TargetTr; // 회전의 중심축 값을 저장하는 변수

    bool shot; // 발사 변수

    private float enemyLookTime; // 적을 바라보는 회전 속도

    private Quaternion enemyLookRotation; // 회전 각도   


    public GameObject bullet; //총탄 프리팹을 위한 레퍼런스

    public Transform firePos; //총탄의 발사 시작 좌표 연결 변수 ( 적을 감지하는 위치 , 약간 위에 있음 )

    private Vector3 spawnPoint; // 총알의 생성 위치.

    private float bulletSpeed; //  //총알 발사 주기 ( 공격 속도 )

    private AudioSource source = null;  //AudioSource 컴포넌트 저장할 레퍼런스 

    

    public AudioClip fireSfx; //총탄의 발사 사운드 
    public AudioClip[] buildSfxList;//건설사운드 건설중, 건설완료


    Ray ray; //Ray 정보 저장 구조체 

    RaycastHit hitInfo; // Ray에 맞은 오브젝트 정보를 저장 할 구조체

    bool check; //Ray 센서를 위한 변수

   

   

    GameObject turret;

    // 포톤 추가/////////////////////////////
    //PhotonView 컴포넌트를 할당할 레퍼런스 
    public PhotonView pv = null;

    //위치 정보를 송수신할 때 사용할 변수 선언 및 초기값 설정 
    Quaternion currRot = Quaternion.identity;

    //플레이어의 Id를 저장하는 변수
    public int playerId = -1;
    //몬스터의 파괴 스코어를 저장하는 변수 
    public int killCount = 0;
    //로컬  플레이어 연결 레퍼런스
    public PlayerCtrl localPlayer;

    
    //////////////////////////////////////////////////////////

        


    private void Awake()
    {
        // Resource 라는 폴더는 특이해서 폴더 안에 Load와 하위 폴더를 정해서 리소스를 동적으로 가져올수 있다.


        bullet = (GameObject)Resources.Load("Turret/MissileObject", typeof(GameObject)); // 이 스크립트의 불렛은 리소스 폴더 안에 베이스 폴더 안에 불렛이라는 게임오브젝트 타입을 가져와 할당한다.
      
        // bullet = (GameObject)Resources.Load("Base/Bullet"); // 타입을 지정해주지 않으면 중복된 이름 중 프로젝트 영역에 있는 파일 가장 위가 선택되므로 하지 말것.
        // test = Resources.Load("Base/Bullet", typeof(Texture)) as Texture; // 형 변환을 이런 방법으로도 가능하다.

        // 테스트 후 해당 변수들을 프라이빗으로 바꾸자

        myTr = transform.Find("turret_1").Find("Base").Find("Head");

        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        


        turret = transform.Find("turret_1").gameObject;




        // 포톤 추가/////////////////////////////////////////

        /*
         * (참고) 우린 수업시간에 Head에 컴포넌트 작업을 하였다. 따라서
         * PhotonView 를 이 오브젝트에 추가 하였다. 물론 최상위 Base 오브젝트에
         * 추가하고 이 스크립트를 연결 하여도 상관없지만 RPC 호출을 위하여 
         * 같은 차원상에 PhotonView 가 필요하기 때문에 Head 에 추가한거다.
         * 문제는 네트워크 유저가 나갈때 PhotonView 컴포넌트가 들어가있는
         * 게임오브젝트가 Destroy 되는데 Head에 PhotonView 가 들어가 있으므로
         * 단지, Head 만 사라지고 Base 게임오브젝트는 남는다. 이 문제를 해결하기
         * 위하여 우린 Base 게임오브젝트에 단순히 PhotonView 만 추가하면 된다.
         * 하지만 가장 최선은 수업을 진행하다보니 이렇게 된거지 Base 게임오브젝트
         * 부터 스크립트 작업을 시작하는거다...샘이 항상 말했듯이 
         * 빈 게임오브젝트 => 하위로 모델링 차일드 => 부모 게임오브젝트 부터 스크립트 작업
         * 이 순선의 중요성이다. 그냥 뭘하던 빈 게임오브젝트 부터!!!
         */

        //PhotonView 컴포넌트 할당 
        pv = GetComponent<PhotonView>();
        //PhotonView Observed Components 속성에 BaseCtrl(현재) 스크립트 Component를 연결
        pv.ObservedComponents[0] = this;
        //데이타 전송 타입을 설정
        pv.synchronization = ViewSynchronization.UnreliableOnChange;

        //PhotonView의 ownerId를 playerId에 저장
        //유저 ownerId 부여(숫자 1부터~)
        playerId = pv.ownerId;

        // 원격 플래이어의 회전 값을 처리할 변수의 초기값 설정 
        currRot = myTr.rotation;




        //아바타에선 소리가 안나도록
        if (!pv.isMine)
            source.mute = true;

    }


    IEnumerator Start()
    {
        turret.SetActive(false);
        source.loop = true;
        SoundManager.Instance.PlayEffect(buildSfxList[0], this.gameObject);
        // 5초뒤 건설 완료
        yield return new WaitForSeconds(5.0f);
        source.Stop();
        source.loop = false;
        SoundManager.Instance.PlayEffect(buildSfxList[1], this.gameObject);

        this.gameObject.tag = "Build";

        turret.SetActive(true);

        StartBase();
    }



    // 스크립트 진행 시 몬스터가 없을 수 있으므로 
    public void StartBase()
    {
        //StartCoroutine(this.TargetSetting());

        //StartCoroutine(this.ShotSetting());


        if (pv.isMine)
        {
            StartCoroutine(this.TargetSetting());

            StartCoroutine(this.ShotSetting());

          

        }

    }

    IEnumerator TargetSetting()
    {
        while (!isDie)
        {

            yield return new WaitForSeconds(0.2f);  // 0.2초 대기 후 

            Enemys = GameObject.FindGameObjectsWithTag("EnemyBody"); // 

            if (Enemys.Length > 0)
            {                

                Transform EnemyTargets = Enemys[0].transform;
                float dist = (EnemyTargets.position - myTr.position).sqrMagnitude;

                foreach (GameObject _Enemy in Enemys)
                {
                    if ((_Enemy.transform.position - myTr.position).sqrMagnitude < dist)
                    {
                        EnemyTargets = _Enemy.transform;
                        dist = (EnemyTargets.position - myTr.position).sqrMagnitude;
                    }
                }

                EnemyTarget = EnemyTargets;
            }
        }
    }


    IEnumerator ShotSetting()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.2f);

            if (Enemys.Length > 0)
            {

                // dist1 = (EnemyTarget.position - myTr.position).sqrMagnitude;
                dist2 = Vector3.Distance(myTr.position, EnemyTarget.position);

                // 체크 후 발사 지정 ( 코루틴을 써서 처리가 더 효율적 )

                if (dist2 < 20.0f)
                {
                    shot = true;
                }
                else
                {
                    shot = false;
                }
            }
            else // 없으면 적이 없어도 계속 공격함.
            {               
                    shot = false;                
            }

        }
    }


    [PunRPC]
    //터렛 발사
    private void ShotStart()
    {
        //잠시 기다리는 로직처리를 위해 코루틴 함수로 호출
        StartCoroutine(this.FireStart());
    }

    // 총탄 발사 코루틴 함수
    IEnumerator FireStart()
    {


        float tempPointX = firePos.transform.position.x;
        float tempPointZ = firePos.transform.position.z;

        spawnPoint = new Vector3(tempPointX, 1f, tempPointZ);




        ////Debug.Log("Fire");
        ////Bullet 프리팹을 동적 생성
        //Instantiate(bullet, firePos.position, firePos.rotation);

        // 포톤 추가///////////////////////////////

        //Debug.Log("Fire");
        //Bullet 프리팹을 동적 생성
        csMissile obj = Instantiate(bullet, spawnPoint, firePos.rotation).GetComponent<csMissile>();
        // 동적 생성한 총알에 유저 ownerId 부여(숫자 1부터~)
        obj.playerId = pv.ownerId;

        ////////////////////////////////////////////







        //총탄 사운드 발생 
        //source.PlayOneShot(fireSfx, fireSfx.length + 0.2f); // 0.2초를 추가해줘야 음악 소리가 끝까지 날수 있다.

        ////MuzzleFlash 스케일을 불규칙하게 하자  ( 아직 비활성화라 보이지 않음 )
        //float scale = Random.Range(1.0f, 2.5f); // 1~ 2.5배 크기로 랜덤값을 주고
        //muzzleFlash.transform.localScale = Vector3.one * scale; // 로컬 스케일을 변경.

        ////MuzzleFlash를 Z축으로 불규칙하게 회전시키자 
        //Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0, 360)); // 0~ 360도 z축 회전을 시키고
        //muzzleFlash.transform.localRotation = rot; // 로컬 로테이트 변경

        //muzzleFlash.SetActive(true); // 이제야 보이게 활성화 한다.

        //랜덤 시간 동안 Delay한 다음 MeshRenderer를 비활성화
        yield return new WaitForSeconds(Random.Range(0.05f, 0.2f)); // 0.05초 ~ 0.2초 랜덤하게 유지되다가 

        //muzzleFlash.SetActive(false); // 다시 비활성화 한다.

    }


    [ContextMenu("FireStart")]
    void Fire()
    {
        shot = true;
    }




    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {

        ray.origin = firePos.position; // 레이의 시작점은 파이어포스의 위치.

        ray.direction = firePos.TransformDirection(Vector3.forward);  // firePos local space(앞 방향)를 world space로 변환 [ 로컬 좌표를 월드 좌표로 변경 ]

        Debug.DrawRay(ray.origin, ray.direction * 20.0f, Color.green); //Scene 뷰에만 시각적으로 표현함


        //위에서 미리 생성한 ray를 인자로 전달, out(메서드 안에서 메서드 밖으로 데이타를 전달 할때 사용)hit, ray 거리
        if (Physics.Raycast(ray, out hitInfo, 30.0f))
        {
            if (shot && hitInfo.collider.tag == "Enemy" && pv.isMine) // 포톤으로 && pv.isMine 추가.
            {
                //발사를 위한 변수 true
                check = true;
            }

        }
        else
        {
            //기본 거리체크 레이저 생성
           // rayLine.SetPosition(0, new Vector3(0, 0, 20.0f));

            //타겟에 레이저 Dot 초기화 
           // rayDot.localPosition = Vector3.zero;

        }




        if (pv.isMine)
        {

            if (!shot)
            {

                // myTr.RotateAround(myTr.position, Vector3.up, Time.deltaTime * 55.0f); /// 이게 회전임.
                //transform.RotateAroundLocal(Vector3.up, Time.deltaTime * 55.0f);

                //발사를 위한 변수 false
                check = false;
            }
            else
            {
               // 적을 봐라봄
                if (shot)
                {
                    if (Time.time > enemyLookTime && EnemyTarget !=null)
                    {

                        enemyLookRotation = Quaternion.LookRotation(-(EnemyTarget.forward)); // - 해줘야 바라봄  
                        enemyLookRotation = Quaternion.LookRotation(EnemyTarget.position - myTr.position); // - 해줘야 바라봄  


                        myTr.rotation = Quaternion.Lerp(TargetTr.rotation, enemyLookRotation, Time.deltaTime * 15.0f);
                        enemyLookTime = Time.time + 0.01f;
                    }
                }
            }



            //만약 발사가 true 이면....
            if (shot && check)
            {
                if (Time.time > bulletSpeed)
                {
                    // 포톤 추가//////////////////////////////

                    //일정 주기로 발사
                    //(포톤 추가)자신의 플레이어일 경우는 로컬함수를 호출하여 총을 발포
                    //일정 주기로 발사
                    ShotStart();

                    //(포톤 추가)원격 네트워크 플레이어의 자신의 아바타 플레이어에는 RPC로 원격으로 FireStart 함수를 호출 
                    pv.RPC("ShotStart", PhotonTargets.Others, null);
                    SoundManager.Instance.PlayEffect(fireSfx, this.gameObject);
                    //(포톤 추가)모든 네트웍 유저에게 RPC 데이타를 전송하여 RPC 함수를 호출, 로컬 플레이어는 로컬 Fire 함수를 바로 호출 
                    //pv.RPC("ShotStart", PhotonTargets.All, null);

                    ///////////////////////////////////////////////
                    ///



                    bulletSpeed = Time.time + 1.365f;






                }
            }
        }


        // 포톤 추가
        //원격 플레이어일 때 수행
        else
        {
            //원격 베이스의 아바타를 수신받은 각도만큼 부드럽게 회전시키자
            myTr.rotation = Quaternion.Slerp(myTr.rotation, currRot, Time.deltaTime * 3.0f);
        }

    }




    // 포톤 추가/////////////////////////////////////////////////

    //네트워크 플레이어의 스코어 증가 및 HUD 설정 함수
    //public void PlusKillCount()
    //{
    //    //Enemy 파괴 스코어 증가
    //    ++killCount;
    //    //HUD Text UI 항목에 스코어 표시
    //    localPlayer.txtKillCount.text = killCount.ToString();

    //    /* 포톤 클라우드에서 제공하는 플레이어의 점수 관련 메서드
    //     * 
    //     * PhotonPlayer.AddScore ( int score )      점수를 누적
    //     * PhotonPlayer.SetScore( int totScore )    해당 점수로 셋팅
    //     * PhotonPlayer.GetScore()                  현재 점수를 조회
    // */

    //    //스코어를 증가시킨 베이스가 자신인 경우에만 저장
    //    if (pv.isMine)
    //    {
    //        /* PhotonNetwork.player는 로컬 플레이어 즉 자신을 의미한다.
    //           즉 다음 로직은 자기 자신의 스코어에 1점을 증가시킨다. 이 정보는 동일 룸에
    //           입장해있는 다른 네트워크 플레이어와 실시간으로 공유된다.*/
    //        PhotonNetwork.player.AddScore(1);
    //    }
    //}


    /*
     * PhotonView 컴포넌트의 Observe 속성이 스크립트 컴포넌트로 지정되면 PhotonView
     * 컴포넌트는 데이터를 송수신할 때, 해당 스크립트의 OnPhotonSerializeView 콜백 함수를 호출한다. 
     */
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //로컬 플레이어의 위치 정보를 송신
        if (stream.isWriting)
        {
            //박싱
            stream.SendNext(myTr.rotation);
        }
        //원격 플레이어의 위치 정보를 수신
        else
        {
            //언박싱
            currRot = (Quaternion)stream.ReceiveNext();
        }

    }
    ////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////



    //[ContextMenu("Destroy")]
    //void Boom()
    //{
    //    // 자신과 네트워크상의 아바타들까지 모두 소멸
    //    PhotonNetwork.Destroy(gameObject);
    //}




}
