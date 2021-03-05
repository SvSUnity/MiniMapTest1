using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Photon_Scene_A1 : MonoBehaviour
{
    string  str_NickName;

    //string  str_GameVersion;

    float   deltaTime;

    /****************************************************************/
    /****************************************************************/


    void    Start()
    {
        //str_GameVersion = "0.1.0";

        str_NickName = SingleTon.INSTANCE.Get_NickName();
        
        deltaTime = 0.05f;

        Invoke("Invoke_O_99" , deltaTime);

    }/**void    Start()**/

    /****************************************************************/
    /****************************************************************/

    void    Invoke_O_99()
    {
        CancelInvoke("Invoke_O_99");

        if(str_NickName.Length < 1)
        {
            int inst_Scene_Current =
                Get_Number_Scene_Current();
            int inst_Scene_Before =
                inst_Scene_Current - 1;
            
            StartCoroutine(IE_Move_Scene(inst_Scene_Before));

        }/**if(str_NickName.Length < 1)**/

        if(str_NickName.Length > 0)
        {
            Invoke("Invoke_O_98" , deltaTime);

        }/**if(str_NickName.Length > 0)**/

    }/**void    Invoke_O_99()**/

    void    Invoke_O_98()
    {
        CancelInvoke("Invoke_O_98");

        if(!PhotonNetwork.connected)
        {
            PhotonNetwork.playerName = str_NickName;
            PhotonNetwork.player.NickName = str_NickName;
            PhotonNetwork.gameVersion =
                SingleTon.INSTANCE.Get_GameVersion();
            PhotonNetwork.ConnectUsingSettings(
                SingleTon.INSTANCE.Get_GameVersion());

        }/**if(!PhotonNetwork.connected)**/

        InvokeRepeating(
            "Invoke_R_97" , deltaTime , deltaTime);

    }/**void    Invoke_O_98()**/

    void    Invoke_R_97()
    {
        if(PhotonNetwork.connected)
        {
            CancelInvoke("Invoke_R_97");
            int     i_Ping = PhotonNetwork.GetPing();
            float   f_Ping_mm = (float)i_Ping;
            float   f_Ping_mm_X = f_Ping_mm * 0.01f;

            Invoke("Invoke_O_96" , f_Ping_mm_X);

        }/**if(PhotonNetwork.connected)**/

    }/**void    Invoke_R_97()**/

    void    Invoke_O_96()
    {
        CancelInvoke("Invoke_O_96");

        if(!PhotonNetwork.insideLobby)
        {
            TypedLobby typedLobby = new TypedLobby(
                "Lobby_00" , LobbyType.Default);

            print("JoinLobby");
            PhotonNetwork.JoinLobby(typedLobby);

        }/**if(!PhotonNetwork.insideLobby)**/

        InvokeRepeating("Invoke_R_95", deltaTime , deltaTime);

    }/**void    Invoke_O_96()**/

    void    Invoke_R_95()
    {
        if(PhotonNetwork.inRoom)
        {
            CancelInvoke("Invoke_R_95");
            print("LeaveRoom");
            PhotonNetwork.LeaveRoom();
            Invoke("Invoke_O_99" , deltaTime);

        }/**if(PhotonNetwork.inRoom)**/

        if(PhotonNetwork.insideLobby)
        {
            CancelInvoke("Invoke_R_95");

            Invoke("Invoke_O_94" , deltaTime);

        }/**if(PhotonNetwork.insideLobby)**/

    }/**void    Invoke_R_95()**/

    void    Invoke_O_94()
    {
        CancelInvoke("Invoke_O_94");

        StartCoroutine(IE_Move_Next_Scene() );

    }/**void    Invoke_O_94()**/



    /****************************************************************/
    /****************************************************************/

    int Get_Scene_BuildSetting_Count()
    {return UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;}

    int Get_Number_Scene_Current()
    {return UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;}
    IEnumerator IE_Move_Scene(int INSERT)
    {
        yield return null;
        if(deltaTime == 0.0f)
        {deltaTime = 0.05f;}

        int inst_Scene_Count =
            SceneManager.sceneCountInBuildSettings;
        INSERT = Get_Number_Q(
            INSERT , inst_Scene_Count);
        AsyncOperation  asyncOperation =
            SceneManager.LoadSceneAsync(INSERT);
        asyncOperation.allowSceneActivation = false;

        bool    b_Ready_to_Move = false;

        yield return new WaitForSeconds(deltaTime);

        while(!b_Ready_to_Move)
        {
            float f_Progress =
                asyncOperation.progress * 100.0f;
            int inst_Progress = (int)f_Progress;

            yield return new WaitForSeconds(deltaTime);
            //yield return null;

            if(inst_Progress > 87)
            {   b_Ready_to_Move = true; }

        }/**while(!asyncOperation.isDone)**/

        //StopCoroutine(IE_Move_Scene()  );

        asyncOperation.allowSceneActivation = true;

    }/**IEnumerator IE_Move_Scene(int INSERT)**/

    IEnumerator IE_Move_Next_Scene()
    {
        yield return null;
        if(deltaTime == 0.0f)
        {deltaTime = 0.05f;}

        int inst_Scene_Count =
            SceneManager.sceneCountInBuildSettings;
        int inst_Scene_Now =
            SceneManager.GetActiveScene().buildIndex;
        int inst_Scene_Next = Get_Number_Q(
            inst_Scene_Now + 1 , inst_Scene_Count);

        AsyncOperation  asyncOperation =
            SceneManager.LoadSceneAsync(inst_Scene_Next);
        asyncOperation.allowSceneActivation = false;

        yield return new WaitForSeconds(deltaTime);

        bool    b_Ready_to_Move = false;

        while(!b_Ready_to_Move)
        {
            float f_Progress =
                asyncOperation.progress * 100.0f;
            int inst_Progress = (int)f_Progress;

            yield return new WaitForSeconds(deltaTime);
            //yield return null;

            if(inst_Progress > 89)
            {   b_Ready_to_Move = true; }

        }/**while(!asyncOperation.isDone)**/

        //StopCoroutine(IE_Move_Next_Scene()  );

        asyncOperation.allowSceneActivation = true;

    }/**IEnumerator IE_Move_Next_Scene()**/

    int     Get_Number_Q(int NUMBER , int SIZE)
    {
        if(SIZE < 1){    SIZE = 1;    }
        if(NUMBER < 0){NUMBER = NUMBER + SIZE;}
        if(NUMBER >= SIZE){NUMBER = NUMBER - SIZE;}

        if( (NUMBER < 0) || (NUMBER >= SIZE) )
        {    NUMBER = Get_Number_Q(NUMBER , SIZE);    }

        return    NUMBER;
    }/**int     Get_Number_Q(int NUMBER , int SIZE)**/

}/**public class Photon_Scene_A1 : MonoBehaviour**/