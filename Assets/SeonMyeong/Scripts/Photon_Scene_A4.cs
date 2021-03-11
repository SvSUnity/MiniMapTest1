using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Photon_Scene_A4 : MonoBehaviour
{
    Canvas  canvas;
    RectTransform   RT_Canvas;

    [SerializeField]Image   image_BackGround_Black;
    [SerializeField]Image   image_Loading_BAR;

    Vector3 v3_Canvas_Size;
    Vector2 v2_Canvas_Size;

    float   deltaTime;

    [SerializeField]int     i_Player_Count;
    [SerializeField]int     i_Loaded_Count;


    AsyncOperation  AO_Load_Scene_Next;
    string  str_NickName;
    [SerializeField]PhotonView  photonView_This;

    /****************************************************************/
    /****************************************************************/



    void    Awake()
    {
        if(canvas == null)
        {canvas = GameObject.Find(
            "Canvas").GetComponent<Canvas>();}
        if(RT_Canvas == null)
        {RT_Canvas = canvas.GetComponent<RectTransform>();}

        if(photonView_This == null)
        {photonView_This = this.gameObject.GetComponent<PhotonView>();}
        if(photonView_This.ObservedComponents.Count < 1)
        {photonView_This.ObservedComponents.Add(this);}
        if(photonView_This.ObservedComponents[0] != this)
        {photonView_This.ObservedComponents[0] = this;}

    }/**void    Awake()**/

    void    Start()
    {
        v2_Canvas_Size = RT_Canvas.sizeDelta;
        v3_Canvas_Size = Get_XYZ_from_XY(v2_Canvas_Size);

        image_BackGround_Black.rectTransform.position =
            v3_Canvas_Size * 0.5f;
        image_BackGround_Black.rectTransform.sizeDelta =
            v2_Canvas_Size;
        
        Set_Align_Image_Loading_BAR();

        deltaTime = 0.05f;

        str_NickName =
            SingleTon.INSTANCE.Get_NickName();

        Invoke("Invoke_O_99" , deltaTime);
        
    }/**void    Start()**/


    void    Set_Align_Image_Loading_BAR()
    {
        float   f_Inst_W = v2_Canvas_Size.x;
        Vector2 v2_Inst_Size =
            (Vector2.right * f_Inst_W) +
                (Vector2.up * f_Inst_W * 0.25f);
        Vector3 v3_Inst_Position_K =
            Get_XYZ_from_XY(v2_Inst_Size * 0.5f);
        image_Loading_BAR.rectTransform.sizeDelta = v2_Inst_Size;
        image_Loading_BAR.rectTransform.position = v3_Inst_Position_K;
        image_Loading_BAR.fillAmount = 0.0f;


    }/**void    Set_Align_Image_Loading_BAR()**/


    /****************************************************************/
    /****************************************************************/

    ///<Summary>
    /// 포톤에 연결되어있으며, 닉네임이 존재하는지 확인.
    ///</Summary>
    void    Invoke_O_99()
    {
        CancelInvoke("Invoke_O_99");

        if( (str_NickName.Length < 1 ) || (!PhotonNetwork.connected) )
        {
            CancelInvoke();
            StopAllCoroutines();

            int inst_Scene_Current =
                Get_Number_Scene_Current();
            int inst_Scene_Before =
                inst_Scene_Current - 4;

            StartCoroutine(IE_Move_Scene(inst_Scene_Before));

        }/**if( (str_NickName.Length < 1 ) || (!PhotonNetwork.connected) )**/

        if( (str_NickName.Length > 0 ) && (PhotonNetwork.connected) )
        {
            Invoke("Invoke_O_98" , deltaTime);

        }/**if( (str_NickName.Length > 0 ) && (PhotonNetwork.connected) )**/

    }/**void    Invoke_O_99()**/

    ///<Summary>
    /// 포톤연결 및 닉네임 존재함을 확인 완료하였다.
    ///</Summary>
    void    Invoke_O_98()
    {
        CancelInvoke("Invoke_O_98");
        
        StartCoroutine(IE_Load_Scene_NEXT_and_Notify());

    }/**void    Invoke_O_98()**/



    /****************************************************************/
    /****************************************************************/


    void    Move_To_Scene_One()
    {
        CancelInvoke();
        StopAllCoroutines();

        int inst_Scene_Current =
            Get_Number_Scene_Current();
        int inst_Scene_Before =
                inst_Scene_Current - 3;

        StartCoroutine(IE_Move_Scene(inst_Scene_Before));

    }/**void    Move_To_Scene_One()**/
    void    OnPhotonCreateRoomFailed()
    {
        Move_To_Scene_One();

    }/**void    OnPhotonCreateRoomFailed()**/

    void    OnPhotonJoinRoomFailed()
    {
        Move_To_Scene_One();

    }/**void    OnPhotonJoinRoomFailed()**/
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        print("OnPhotonSerializeView");

    }/**void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)**/

    /****************************************************************/
    /****************************************************************/

    Vector3 Get_XYZ_from_XY(Vector2 INSERT)
    {return  new Vector3(INSERT.x , INSERT.y);}
    Vector2 Get_XY_from_XYZ(Vector3 INSERT)
    {return  new Vector2(INSERT.x , INSERT.y);}

    int     Get_Number_Q(int NUMBER , int SIZE)
    {
        if(SIZE < 1){    SIZE = 1;    }
        if(NUMBER < 0){NUMBER = NUMBER + SIZE;}
        if(NUMBER >= SIZE){NUMBER = NUMBER - SIZE;}

        if( (NUMBER < 0) || (NUMBER >= SIZE) )
        {    NUMBER = Get_Number_Q(NUMBER , SIZE);    }

        return    NUMBER;
    }/**int     Get_Number_Q(int NUMBER , int SIZE)**/


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
        AsyncOperation asyncOperation =
            SceneManager.LoadSceneAsync(INSERT);
        //asyncOperation.allowSceneActivation = false;

        bool    b_Ready_to_Move = false;

        yield return new WaitForSeconds(deltaTime);

        while(!b_Ready_to_Move)
        {
            float f_Progress =
                asyncOperation.progress * 100.0f;
            int inst_Progress = (int)f_Progress;

            yield return new WaitForSeconds(deltaTime);

            if(inst_Progress > 87)
            {   b_Ready_to_Move = true; }

        }/**while(!asyncOperation.isDone)**/

        //StopCoroutine(IE_Move_Scene()  );

        asyncOperation.allowSceneActivation = true;

    }/**IEnumerator IE_Move_Scene(int INSERT)**/



    IEnumerator IE_Load_Scene_NEXT_and_Notify()
    {
        yield return null;
        if(deltaTime == 0.0f)
        {deltaTime = 0.05f;}

        int inst_Scene_Count =
            Get_Scene_BuildSetting_Count();
        int inst_Scene_Now =
            Get_Number_Scene_Current();
        int inst_Scene_Next = Get_Number_Q(
            inst_Scene_Now + 1 , inst_Scene_Count);
        AO_Load_Scene_Next =
            SceneManager.LoadSceneAsync(inst_Scene_Next);
        AO_Load_Scene_Next.allowSceneActivation = false;

        bool    b_Inst = false;

        while(!b_Inst)
        {
            yield return new WaitForSeconds(deltaTime);

            float   f_inst_Progress =
                AO_Load_Scene_Next.progress;
            int     inst_Progress =
                (int)(f_inst_Progress * 100.0f);
            image_Loading_BAR.fillAmount = f_inst_Progress;
            if(inst_Progress > 87)
            {b_Inst = true;}

        }/**while(!b_Inst)**/

        StopCoroutine(IE_Load_Scene_NEXT_and_Notify()  );

        StartCoroutine(IE_RPC_Add_Loaded_Count_To_EveryOne()  );

    }/**IEnumerator IE_Load_Scene_NEXT_and_Notify()**/

    IEnumerator IE_RPC_Add_Loaded_Count_To_EveryOne()
    {
        image_Loading_BAR.fillAmount = 1.0f;
        yield return new WaitForSeconds(0.5f);
        photonView_This.RPC(
            "RPC_Add_Loaded_Count" , PhotonTargets.All , null);

        StopCoroutine(IE_RPC_Add_Loaded_Count_To_EveryOne()  );

        InvokeRepeating("Invoke_R_Waiting_Another_Player",deltaTime , deltaTime);

    }/**IEnumerator IE_RPC_Add_Loaded_Count_To_EveryOne()**/

    /****************************************************************/
    /****************************************************************/

    void    Invoke_R_Waiting_Another_Player()
    {
        bool    b_Inst_IsMaster =
            PhotonNetwork.isMasterClient;

        if(b_Inst_IsMaster)
        {
            i_Player_Count =
                PhotonNetwork.room.PlayerCount;

            if(i_Loaded_Count > i_Player_Count - 1)
            {
                CancelInvoke("Invoke_R_Waiting_Another_Player");

                //print("Time : "+PhotonNetwork.);

                //int inst_Ping =
                //    PhotonNetwork.GetPing();
                //    print("Ping : "+inst_Ping.ToString("000")+"ms");
                //float   f_Ping = (float)inst_Ping;
                //f_Ping = f_Ping * 0.01f;
                Invoke("Invoke_O_RPC_Load_Scene_Next",0.01f);

            }/**if(i_Loaded_Count > i_Player_Count - 1)**/

        }/**if(b_Inst_IsMaster)**/

    }/**void    Invoke_R_Waiting_Another_Player()**/


    void    Invoke_O_RPC_Load_Scene_Next()
    {
        CancelInvoke("Invoke_O_RPC_Load_Scene_Next");

        photonView_This.RPC(
                    "RPC_Load_Scene_Next" , PhotonTargets.AllViaServer , null);
        
    }/**void    Invoke_O_RPC_Load_Scene_Next()**/

    /****************************************************************/
    /****************************************************************/

    [PunRPC]
    void    RPC_Add_Loaded_Count()
    {
        i_Loaded_Count = i_Loaded_Count +1;

    }/**void    RPC_Add_Loaded_Count()**/

    [PunRPC]
    void    RPC_Load_Scene_Next()
    {
        CancelInvoke();
        StopAllCoroutines();

        AO_Load_Scene_Next.allowSceneActivation = true;

    }/**void    RPC_Load_Scene_Next()**/

}/**public class Photon_Scene_A4 : MonoBehaviour**/