using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csTestPhoton : MonoBehaviour {

    //App의 버전 정보 (번들 버전과 일치 시키자...)
    public string version = "Ver 0.1.0";


    public PhotonLogLevel LogLevel = PhotonLogLevel.ErrorsOnly;

    //플레어의 생성 위치 저장 레퍼런스
    public Transform playerPos;

    RadarMap radamap;
    smoothFollowCam cam;


    //App 인증 및 로비연결
    void Awake()
    {

        if (!PhotonNetwork.connected)
        {
            PhotonNetwork.ConnectUsingSettings(version);

            PhotonNetwork.logLevel = LogLevel;

            PhotonNetwork.playerName = "GUEST " + Random.Range(1, 9999);

            radamap = GameObject.FindGameObjectWithTag("Minimap").GetComponent<RadarMap>();
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<smoothFollowCam>();
        }

    }

    void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby !!!");
        PhotonNetwork.JoinRandomRoom(); // (UI 버전에서는 주석 처리)
    }



    void OnPhotonRandomJoinFailed()
    {
        //랜텀 매치 메이킹이 실패한 후 Console 뷰에 나타나는 메시지 설정
        Debug.Log("No Rooms !!!");

        bool isSucces = PhotonNetwork.CreateRoom("MyRoom");

        Debug.Log("[정보] 게임 방 생성 완료 : " + isSucces);
    }


    void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        //오류 코드( ErrorCode Class )
        Debug.Log(codeAndMsg[0].ToString());
        //오류 메시지
        Debug.Log(codeAndMsg[1].ToString());
        // 여기선 디버그로 표현했지만 릴리즈 버전에선 사용자에게 메시지 전달
        Debug.Log("Create Room Failed = " + codeAndMsg[1]);
    }

    // 룸에 입장하면 호출되는 콜백 함수 
    // PhotonNetwork.CreateRoom 함수로 룸을 생성한 후 입장하거나, PhotonNetwork.JoinRandomRoom, PhotonNetwork.JoinRoom 함수를 통해 입장해도 호출 된다.
    void OnJoinedRoom()
    {
        Debug.Log("Enter Room");

        CreatePlayer();
    }

    //플레이어를 생성하는 함수
    void CreatePlayer()
    {
        GameObject go = PhotonNetwork.Instantiate("MainPlayer", playerPos.position, playerPos.rotation, 0);
        radamap.SetPlayerPos(go);
    }

}
// 참고 https://doc-api.photonengine.com/ko-kr/pun/current/class_room_options.html
