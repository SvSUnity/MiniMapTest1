using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StageManager : MonoBehaviour
{
    // 포톤 추가////////////////////////////////////////////////
   
    
    
    PhotonView pv; //RPC 호출을 위한 PhotonView 연결 레퍼런스
   private Transform[] playerPos; //플레어의 생성 위치 저장 레퍼런스

    public csTurret Turret; // 베이스 스타트를 위한 변수


    //스폰 장소 
    private Transform[] EnemySpawnPoints;



    //게임 끝
    private bool gameEnd;

    // 스테이지 Enemy들을 위한 레퍼런스
    private GameObject[] Enemys;




    public GameObject worldMap;

    private GameObject[] propTrees;
    private GameObject[] propGrass1;
    private GameObject[] propGrass2;
    private GameObject[] propGrass3;

    private GameObject[] rockItems;
    private GameObject[] woodItems;


    float randomX;
    float randomZ;
    Vector3 randomPoint;


    /// <summary>
    /// / 낮밤
    /// </summary>



    public bool day; // 낮밤 구분 변수, true 가 낮, false 는 밤;

    public float halfDay; // 낮, 밤의 시간
    public float currentTime; // 현재 시간.

    public Image dayIMG;
    public Image nightIMG;
    public Text dayTxt;

    public Image dayIcon;
    public Image nightICon;



    int suvDay;

    // 로비 이식전 테스트용

    bool test = false;


    void Awake()
    {
        pv = GetComponent<PhotonView>(); //PhotonView 컴포넌트를 레퍼런스에 할당

        //playerPos = GameObject.Find("PlayerSpawnPoint").GetComponentsInChildren<Transform>();
        //// 씬 전환이 완벽히 끝나고 나서 플레이어가 생성되어야 네트워크 통신이 원활 ( 만들어 지는 와중에 통신이 되면 안되기 때문 )

       //StartCoroutine(this.CreatePlayer()); //플레이어를 생성하는 함수 호출

        PhotonNetwork.isMessageQueueRunning = true; //포톤 클라우드로부터 네트워크 메시지 수신을 다시 연결 ( 포톤 로드씬을 이용해도 되지만 일단 아는것부터 )        



        Debug.Log(123);


        //스폰 위치 얻기
        EnemySpawnPoints = GameObject.Find("EnemySpawnPoint").GetComponentsInChildren<Transform>();
            

        // 포톤 추가
        if (PhotonNetwork.connected && PhotonNetwork.isMasterClient)
        {
            // 몬스터 스폰 코루틴 호출
           // StartCoroutine(this.CreateEnemy());

            //StartCoroutine(this.CreateTree());

            StartCoroutine(this.CreateItem());
            
        }
      else
        {
            test = true;
        }

        

        day = true;



    }



    // Start is called before the first frame update
    IEnumerator Start()
    {

       


   
        yield return new WaitForSeconds(1.0f);

 


        suvDay = 1;

    }

    public void AddObject(GameObject obj)
    {
        Instantiate(obj, this.transform);
    }



    //IEnumerator SetConnectPlayerScore()
    //{
    //    /*
    //     * PhotonNetwork.playerList 속성은 같은 룸에 입장한 모든 플레이어의 정보를 반환한다. 따라서
    //     * 차후, 이 속성은 현재 입장한 플레이어 목록을 UI로 표시할 때 유용하게 활용할 수 있다.
    //     */

    //    // 현재 입장한 룸에 접속한 모든 네트워크 플레이어의 정보를 저장 
    //    PhotonPlayer[] players = PhotonNetwork.playerList;

    //    // 전체 네트워크 플레이어의 정보를 출력
    //    foreach (PhotonPlayer _player in players)
    //    {
    //        Debug.Log("[" + _player.ID + "]" + _player.NickName + " " + _player.GetScore() + " Kill");
    //    }

    //    //모든 Player 프리팹을 배열에 저장
    //    GameObject[] net_Player = GameObject.FindGameObjectsWithTag("Player");

    //    //Debug.Log(players.Length);

    //    // 동일 룸에 입장해있는 모든 네트워크 플레이어의 케릭터에 HUD 스코어 표시
    //    foreach (GameObject _player in net_Player)
    //    {

    //        //각 베이스(플레이어)별 스코어를 조회
    //        int currKillCount = _player.GetComponent<PhotonView>().owner.GetScore();

    //        //해당 베이스의 주인인 플레이어의 UI에 스코어 표시
    //       // _player.GetComponent<PlayerCtrl>().txtKillCount.text = currKillCount.ToString();

    //    }

    //    yield return null;
    //}


    //// 몬스터 생성 코루틴 함수
    //IEnumerator CreateEnemy()
    //{
    //    //게임중 일정 시간마다 계속 호출됨 
    //    while (!gameEnd)
    //    {
    //        //리스폰 타임 5초
    //        yield return new WaitForSeconds(5.0f);

    //        // 스테이지 총 몬스터 객수 제한을 위하여 찾자~
    //        Enemys = GameObject.FindGameObjectsWithTag("Enemy");

    //        // 스테이지 총 몬스터 객수 제한
    //        if (Enemys.Length < 20)
    //        {
    //            //루트 생성위치는 필요하지 않다.그래서 1 번째 인덱스부터 돌리자
    //            for (int i = 1; i < EnemySpawnPoints.Length; i++)
    //            {
    //                // (포톤 추가)
    //                // 네트워크 플레이어를 Scene 에 귀속하여 생성
    //                PhotonNetwork.InstantiateSceneObject("Enemy", EnemySpawnPoints[i].localPosition, EnemySpawnPoints[i].localRotation, 0, null);
    //            }
    //        }
    //    }
    //}





    // 포톤 추가
    // 플레이어를 생성하는 함수
    //IEnumerator CreatePlayer()
    //{
    //    // 지금은 테스트를 위하여 플레이어 스폰 포인트가 2개이다 따라서 차후 접속 인원수에 맞게 스폰 포인트와
    //    // 총 접속인원의 수를 제한


    //    //현재 입장한 룸 정보를 받아옴(레퍼런스 연결)
    //    Room currRoom = PhotonNetwork.room;

    //    // 테스트를 위한 object 배열
    //    object[] ex = new object[3];
    //    ex[0] = 3;
    //    ex[1] = 4;
    //    ex[2] = 5;

    //    //float pos = Random.Range(-100.0f, 100.0f);
    //    //포톤네트워크를 이용한 동적 네트워크 객체는 다음과 같이 Resources 폴더 안에 애셋의 이름을 인자로 전달 해야한다. 
    //    //PhotonNetwork.Instantiate( "MainPlayer", new Vector3(pos, 20.0f, pos), Quaternion.identity, 0 );
    //    GameObject player = PhotonNetwork.Instantiate("MainPlayer", playerPos[currRoom.PlayerCount].position, playerPos[currRoom.PlayerCount].rotation, 0, ex);

    //    //// 기존 이름으로 변경해야 드럼통 폭파 가능(DestructionRay 스크립트 참조)
    //    player.name = "Player";

    //    //PhotonNetwork.InstantiateSceneObject(string prefabName, Vector3 position, Quaternion rotation, byte group, object[] data);
    //    //이 함수도 PhotonNetwork.Instantiate와 마찬가지로 네트워크 상에 프리팹을 동시에 생성시키지만, Master Client 만 생성 및 삭제 가능.
    //    //생성된 프리팹 오브젝트의 PhotonView 컴포넌트의 Owner는 Scene이 된다.

    //    yield return null;
    //}




    // 포톤 추가
    // 룸 나가기 버튼 클릭 이벤트에 연결될 함수
    public void OnClickExitRoom()
    {

        // 로그 메시지에 출력할 문자열 생성
        string msg = "\n\t<color=#ff0000>["
                    + PhotonNetwork.player.NickName
                    + "] Disconnected</color>";

        //RPC 함수 호출
        pv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);

        //현재 룸을 빠져나가며 생성한 모든 네트워크 객체를 삭제
        PhotonNetwork.LeaveRoom();

        //(!) 서버에 통보한 후 룸에서 나가려는 클라이언트가 생성한 모든 네트워크 객체및 RPC를 제거하는 과정 진행(포톤 서버에서 진행)
    }

    // 포톤 추가
    //룸에서 접속 종료됐을 때 호출되는 콜백 함수 ( (!) 과정 후 포톤이 호출 )
    public void OnLeftRoom()
    {
        // 로비로 이동
        SceneManager.LoadScene("Lobby");
    }

    /////////////////////////////////////////////////////////////////////////////
    ///


    //// 몬스터 생성 코루틴 함수
    //IEnumerator CreateEnemy()
    //{
    //    //게임중 일정 시간마다 계속 호출됨 
    //    while (!gameEnd)
    //    {
    //        //리스폰 타임 5초
    //        yield return new WaitForSeconds(5.0f);

    //        // 스테이지 총 몬스터 객수 제한을 위하여 찾자~
    //        Enemys = GameObject.FindGameObjectsWithTag("Enemy");

    //        // 스테이지 총 몬스터 객수 제한
    //        if (Enemys.Length < 10 && day == false)
    //        {
    //            for (int j = Enemys.Length; j < 10; j++)
    //            {
    //                //루트 생성위치는 필요하지 않다.그래서 1 번째 인덱스부터 돌리자
    //                for (int i = 1; i < EnemySpawnPoints.Length; i++)
    //                {
    //                    // (포톤 추가)
    //                    // 네트워크 플레이어를 Scene 에 귀속하여 생성
    //                    PhotonNetwork.InstantiateSceneObject("Enemy", EnemySpawnPoints[i].localPosition, EnemySpawnPoints[i].localRotation, 0, null);
    //                }
    //            }


    //        }
    //    }




    //}



    //IEnumerator CreateTree()
    //{
    //게임중 일정 시간마다 계속 호출됨 
    //while (!gameEnd)
    //{
    //    //리스폰 타임0.5초
    //    yield return new WaitForSeconds(0.5f);

    //    // 스테이지 총 몬스터 객수 제한을 위하여 찾자~
    //    propTrees = GameObject.FindGameObjectsWithTag("PropTree");

    //    // 스테이지 총 몬스터 객수 제한
    //    if (propTrees.Length < 100)
    //    {
    //        //루트 생성위치는 필요하지 않다.그래서 1 번째 인덱스부터 돌리자
    //        for (int i = propTrees.Length; i < 100; i++)
    //        {
    //           // randomX = Random.Range(-(worldMap.transform.localScale.x / 2), (worldMap.transform.localScale.x / 2));
    //           // 큐브가 아니라 플랜으로 하면 /2가 아니라 *5 처리 하면 사이즈가 맞음.

    //            randomX = Random.Range(-(worldMap.transform.localScale.x *5), (worldMap.transform.localScale.x *5));
    //            randomZ = Random.Range(-(worldMap.transform.localScale.z *5), (worldMap.transform.localScale.z *5));
    //            randomPoint = new Vector3(randomX, 0.5f, randomZ);

    //            // (포톤 추가)
    //            // 네트워크 플레이어를 Scene 에 귀속하여 생성
    //            PhotonNetwork.InstantiateSceneObject("PropTree", randomPoint, Quaternion.identity, 0, null);
    //        }
    //    }




    //    propGrass1 = GameObject.FindGameObjectsWithTag("Grass1");            
    //    if (propGrass1.Length < 30)
    //    {                
    //        for (int i = propGrass1.Length; i < 30; i++)
    //        {
    //            randomX = Random.Range(-(worldMap.transform.localScale.x * 5), (worldMap.transform.localScale.x * 5));
    //            randomZ = Random.Range(-(worldMap.transform.localScale.z * 5), (worldMap.transform.localScale.z * 5));
    //            randomPoint = new Vector3(randomX, 0.5f, randomZ);

    //            // 네트워크 플레이어를 Scene 에 귀속하여 생성
    //            PhotonNetwork.InstantiateSceneObject("Grass1", randomPoint, Quaternion.identity, 0, null);
    //        }
    //    }

    //    propGrass2 = GameObject.FindGameObjectsWithTag("Grass2");
    //    if (propGrass2.Length < 30)
    //    {
    //        for (int i = propGrass2.Length; i < 30; i++)
    //        {
    //            randomX = Random.Range(-(worldMap.transform.localScale.x * 5), (worldMap.transform.localScale.x * 5));
    //            randomZ = Random.Range(-(worldMap.transform.localScale.z * 5), (worldMap.transform.localScale.z * 5));
    //            randomPoint = new Vector3(randomX, 0.5f, randomZ);

    //            // 네트워크 플레이어를 Scene 에 귀속하여 생성
    //            PhotonNetwork.InstantiateSceneObject("Grass2", randomPoint, Quaternion.identity, 0, null);
    //        }
    //    }

    //    propGrass3 = GameObject.FindGameObjectsWithTag("Grass3");
    //    if (propGrass3.Length < 30)
    //    {
    //        for (int i = propGrass3.Length; i < 30; i++)
    //        {
    //            randomX = Random.Range(-(worldMap.transform.localScale.x * 5), (worldMap.transform.localScale.x * 5));
    //            randomZ = Random.Range(-(worldMap.transform.localScale.z * 5), (worldMap.transform.localScale.z * 5));
    //            randomPoint = new Vector3(randomX, 0.5f, randomZ);

    //            // 네트워크 플레이어를 Scene 에 귀속하여 생성
    //            PhotonNetwork.InstantiateSceneObject("Grass3", randomPoint, Quaternion.identity, 0, null);
    //        }
    //    }


    //}
    //}




    IEnumerator CreateItem()
    {
        //게임중 일정 시간마다 계속 호출됨 
        while (!gameEnd)
        {
            //리스폰 타임 5초
            yield return new WaitForSeconds(0.5f);

            // 스테이지 총 몬스터 객수 제한을 위하여 찾자~
            rockItems = GameObject.FindGameObjectsWithTag("Item");

            // 스테이지 총 몬스터 객수 제한
            if (rockItems.Length < 20)
            {
                randomX = Random.Range(-(worldMap.transform.localScale.x * 4.8f), (worldMap.transform.localScale.x * 4.8f));
                randomZ = Random.Range(-(worldMap.transform.localScale.z * 4.8f), (worldMap.transform.localScale.z * 4.8f));
                randomPoint = new Vector3(randomX, 1f, randomZ);
                PhotonNetwork.InstantiateSceneObject("ItemRock", randomPoint, Quaternion.identity, 0, null);

                randomX = Random.Range(-(worldMap.transform.localScale.x * 4.8f), (worldMap.transform.localScale.x * 4.8f));
                randomZ = Random.Range(-(worldMap.transform.localScale.z * 4.8f), (worldMap.transform.localScale.z * 4.8f));
                randomPoint = new Vector3(randomX, 1f, randomZ);
                PhotonNetwork.InstantiateSceneObject("ItemWood", randomPoint, Quaternion.identity, 0, null);


                // *5 로 하니 완전 맵끝에 생성될때 부자연스러움
            }
                      



        }




    }


    private void Update()
    {
        if (test == true)
        {
            if (PhotonNetwork.connected && PhotonNetwork.isMasterClient)
            {
                // 몬스터 스폰 코루틴 호출
               // StartCoroutine(this.CreateEnemy());

                //StartCoroutine(this.CreateTree());

                StartCoroutine(this.CreateItem());

                test = false;
            }
        }



        currentTime += Time.deltaTime;
        if ( currentTime >= halfDay)
        {
            day = !day;
            currentTime = 0;


            if ( day == true)
            {

                suvDay++;

                dayTxt.text = suvDay.ToString() + " Day";
                dayIMG.fillAmount = 1;
                nightIMG.fillAmount = 1;

            }

        }


        //if ( day == true)
        //{
        //    dayIMG.fillAmount = 1 - (currentTime / halfDay) ;
        //    dayIcon.gameObject.SetActive(true);
        //    nightICon.gameObject.SetActive(false);


        //}
        //else if (day == false)
        //{
        //    nightIMG.fillAmount = 1 - (currentTime / halfDay);
        //    dayIcon.gameObject.SetActive(false);
        //    nightICon.gameObject.SetActive(true);

        //}

        ////RPC 함수 호출
        //pv.RPC("DayTemp", PhotonTargets.AllBuffered);

        if (PhotonNetwork.isMasterClient)
        {
            
        }


        DayTemp();

    }


    [PunRPC]
    public void DayTemp()
    {




            dayIMG.fillAmount = 1 - (currentTime / halfDay);
            dayIcon.gameObject.SetActive(day);
            nightICon.gameObject.SetActive(!day);
    }
}
