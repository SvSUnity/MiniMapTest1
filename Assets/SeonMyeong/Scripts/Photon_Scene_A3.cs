using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Photon_Scene_A3 : MonoBehaviour
{
    Canvas  canvas;
    RectTransform   RT_Canvas;

    [SerializeField]GameObject  obj_Group_NickName;
    [SerializeField]Text[]  text_Player_List_Array;
    [SerializeField]Text    text_Player_Count;
    [SerializeField]Text    text_Player_NickName;
    [SerializeField]Image   image_MASTER;

    [SerializeField]Image   image_Button_START;
    [SerializeField]Image   image_Button_Y;
    [SerializeField]Image   image_Button_X;
    [SerializeField]Image   image_Button_BACK;

    float   f_Distance_Canvas_Short;

    int     i_Text_Player_List_Array_Size;
    [SerializeField]int     i_Player_Count;
    [SerializeField]int     i_Ready_Count;

    Font    font_Arial;
    
    float   deltaTime;

    float   f_Timer;
    string  str_NickName;

    Color   color_Transparent;

    /****************************************************************/
    /****************************************************************/

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
        str_NickName =
            SingleTon.INSTANCE.Get_NickName();
        
        if(SingleTon.INSTANCE.Get_Font_Arial() == null)
        {   SingleTon.INSTANCE.Set_Font_Arial(
                text_Player_NickName.font); }
        font_Arial =
            SingleTon.INSTANCE.Get_Font_Arial();
        if(font_Arial == null)
        {   font_Arial = text_Player_NickName.font; }

        color_Transparent = new Color(1.0f , 1.0f , 1.0f , 0.0f);
        image_MASTER.color = color_Transparent;

        image_Button_START.enabled = false;

        image_Button_Y.enabled = true;
        image_Button_X.enabled = false;

        image_Button_BACK.enabled = true;

        Set_Align_Image_Master();

        Set_Align_Text_Player_List_Array();
        Set_Align_Text_Player_Count();
        Set_Align_Text_Player_NickName();

        Set_Align_Image_Button_XY();
        Set_Align_Image_Button_Start();
        Set_Align_Image_Button_Back();

        deltaTime = 0.025f;

        f_Timer = 0.0f;

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
                inst_Scene_Current - 3;
            StartCoroutine(IE_Move_Scene(inst_Scene_Before));

        }/**if(str_NickName.Length < 1)**/

        if(str_NickName.Length > 0)
        {
            InvokeRepeating("Invoke_R_98",deltaTime,deltaTime);
            
        }/**if(str_NickName.Length > 0)**/

    }/**void    Invoke_O_99()**/

    void    Invoke_R_98()
    {
        if(!PhotonNetwork.inRoom)
        {
            f_Timer = f_Timer + deltaTime;

            if(f_Timer > 5.0f)
            {
                f_Timer = 0.0f;
                CancelInvoke();

                int inst_Scene_Current =
                    Get_Number_Scene_Current();
                int inst_Scene_Before =
                    inst_Scene_Current - 3;

                StartCoroutine(IE_Move_Scene(inst_Scene_Before));

            }/**if(f_Timer > 5.0f)**/
        }/**if(!PhotonNetwork.inRoom)**/

        if(PhotonNetwork.inRoom)
        {
            CancelInvoke("Invoke_R_98");

            if(PhotonNetwork.isMasterClient)
            {

            }/**if(PhotonNetwork.isMasterClient)**/

            if(!PhotonNetwork.isMasterClient)
            {

            }/**if(!PhotonNetwork.isMasterClient)**/

            PhotonNetwork.isMessageQueueRunning = false;
            PhotonNetwork.isMessageQueueRunning = true;
            
            Invoke("Invoke_O_97" , deltaTime);

        }/**if(PhotonNetwork.inRoom)**/

    }/**void    Invoke_R_98()**/

    void    Invoke_O_97()
    {
        CancelInvoke("Invoke_O_97");

        text_Player_NickName.text =
            "@ "+PhotonNetwork.player.NickName+" ";
        
        image_Button_X.raycastTarget = true;
        image_Button_Y.raycastTarget = true;
        
        if(PhotonNetwork.isMasterClient)
        {
            image_MASTER.color = Color.white;
        }
        //PhotonNetwork.isMessageQueueRunning = true;


        photonView_This.RPC(
            "RPC_Reset_Button_XY" , PhotonTargets.All , null);
        photonView_This.RPC(
            "PRC_FindOut_Player_Count" , PhotonTargets.All , null);
        photonView_This.RPC(
            "RPC_Reset_Ready_Count" , PhotonTargets.All , null);
        photonView_This.RPC(
            "RPC_Text_Player_Count" , PhotonTargets.All , null);
        photonView_This.RPC(
            "RPC_Button_Start_Enable_for_Master" , PhotonTargets.All , null);
        photonView_This.RPC(
            "RPC_Renovate_Text_Player_List_Array" , PhotonTargets.All , null);

    }/**void    Invoke_O_97()**/

    /****************************************************************/
    /****************************************************************/

    void    Move_To_Scene_One()
    {
        CancelInvoke();
        StopAllCoroutines();

        int inst_Scene_Current =
            Get_Number_Scene_Current();
        int inst_Scene_Before =
                inst_Scene_Current - 2;

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


    void    OnPhotonPlayerConnected(PhotonPlayer newPP)
    {
        if(PhotonNetwork.isMasterClient)
        {
            photonView_This.RPC(
                "RPC_Reset_Button_XY" , PhotonTargets.All , null);
            photonView_This.RPC(
                "PRC_FindOut_Player_Count" , PhotonTargets.All , null);
            photonView_This.RPC(
                "RPC_Reset_Ready_Count" , PhotonTargets.All , null);
            photonView_This.RPC(
                "RPC_Text_Player_Count" , PhotonTargets.All , null);
            photonView_This.RPC(
                "RPC_Button_Start_Enable_for_Master" , PhotonTargets.All , null);
            photonView_This.RPC(
                "RPC_Renovate_Text_Player_List_Array" , PhotonTargets.All , null);
            photonView_This.RPC(
                "RPC_Clear_Ready_Text_Player_List_Array" , PhotonTargets.All , null);


        }/**if(PhotonNetwork.isMasterClient)**/

    }/**void    OnPhotonPlayerConnected(PhotonPlayer newPP)**/

    void    OnPhotonPlayerDisconnected()
    {
        if(PhotonNetwork.isMasterClient)
        {
            photonView_This.RPC(
                "RPC_Reset_Button_XY" , PhotonTargets.All , null);
            photonView_This.RPC(
                "PRC_FindOut_Player_Count" , PhotonTargets.All , null);
            photonView_This.RPC(
                "RPC_Reset_Ready_Count" , PhotonTargets.All , null);
            photonView_This.RPC(
                "RPC_Text_Player_Count" , PhotonTargets.All , null);
            photonView_This.RPC(
                "RPC_Button_Start_Enable_for_Master" , PhotonTargets.All , null);
            photonView_This.RPC(
                "RPC_Renovate_Text_Player_List_Array" , PhotonTargets.All , null);
            photonView_This.RPC(
                "RPC_Clear_Ready_Text_Player_List_Array" , PhotonTargets.All , null);

        }/**if(PhotonNetwork.isMasterClient)**/

    }/**void    OnPhotonPlayerDisconnected()**/

    [PunRPC]
    void    RPC_Disable_Button_All()
    {
        image_Button_START.enabled = false;
        image_Button_Y.enabled = false;
        image_Button_X.enabled = false;
        image_Button_BACK.enabled = false;

        if(PhotonNetwork.isMasterClient)
        {PhotonNetwork.room.IsOpen = false;}

    }/**void    RPC_Disable_Button_All()**/

    [PunRPC]
    void    RPC_Move_Scene_Next()
    {
        StartCoroutine(IE_Load_Scene_NEXT());

    }/**void    RPC_Move_Scene_Next()**/

    [PunRPC]
    void    RPC_Reset_Button_XY()
    {
        image_Button_Y.enabled = true;
        image_Button_X.enabled = false;

    }/**void    RPC_Reset_XY_Button()**/

    [PunRPC]
    void    PRC_FindOut_Player_Count()
    {
        i_Player_Count =
            PhotonNetwork.room.PlayerCount;
    }/**void    PRC_FindOut_PlayerCount()**/

    [PunRPC]
    void    RPC_Reset_Ready_Count()
    {
        i_Ready_Count = 0;

    }/**void    RPC_Reset_ReadyCount()**/

    [PunRPC]
    void    RPC_Add_Ready_Count()
    {
        i_Ready_Count = i_Ready_Count + 1;

    }/**void    RPC_Add_Ready_Count()**/

    [PunRPC]
    void    RPC_Sub_Ready_Count()
    {
        i_Ready_Count = i_Ready_Count - 1;
        if(i_Ready_Count < 0){i_Ready_Count = 0;}

    }/**void    RPC_Sub_Ready_Count()**/

    [PunRPC]
    void    RPC_Text_Player_Count()
    {
        int inst_T = i_Player_Count;
        int inst_Y = i_Ready_Count;

        text_Player_Count.text =
        inst_Y.ToString("00")+" / "+inst_T.ToString("00");

    }/**void    RPC_Text_Player_Count()**/

    [PunRPC]
    void    RPC_Button_Start_Enable_for_Master()
    {
        bool b_inst_IsMaster =
            PhotonNetwork.isMasterClient;

        if(b_inst_IsMaster)
        {
            int inst_Total = PhotonNetwork.room.PlayerCount;
            int inst_Ready = i_Ready_Count;

            if(inst_Total == inst_Ready)
            {
                image_Button_START.enabled = true;
            }
            else
            {
                image_Button_START.enabled = false;
            }

        }/**if(b_inst_IsMaster)**/

        if(!b_inst_IsMaster)
        {
            image_Button_START.enabled = false;

        }/**if(!b_inst_IsMaster)**/

    }/**void    RPC_Button_Start_Enable_for_Master()**/


    [PunRPC]
    void    RPC_Renovate_Text_Player_List_Array()
    {
        int inst_PlayerCount =
            PhotonNetwork.room.PlayerCount;
        for(int iX = 0 ; iX < i_Text_Player_List_Array_Size ; iX++)
        {
            text_Player_List_Array[iX].text = string.Empty;

            if(iX < inst_PlayerCount)
            {
                text_Player_List_Array[iX].text =
                    PhotonNetwork.playerList[iX].NickName;
            }/**if(iX < inst_PlayerCount)**/

        }/**for(int iX = 0 ; iX < i_Text_Player_List_Array_Size ; iX++)**/

    }/**void    RPC_Renovate_Text_Player_List_Array()**/

    [PunRPC]

    void    RPC_Judge_Ready_Text_Player_List_Array(string NICKNAME , bool READY)
    {
        for(int iX = 0 ; iX < i_Text_Player_List_Array_Size ; iX++)
        {
            if(text_Player_List_Array[iX].text == NICKNAME)
            {
                if(READY)
                {
                    text_Player_List_Array[iX].color = Color.green;
                }

                if(!READY)
                {
                    text_Player_List_Array[iX].color = Color.white;
                }

                break;

            }/**if(text_Player_List_Array[iX].text == NICKNAME)**/

        }/**for(int iX = 0 ; iX < i_Text_Player_List_Array_Size ; iX++)**/

    }/**void    RPC_Judge_Ready_Text_Player_List_Array(string NICKNAME , bool READY)**/

    [PunRPC]
    void    RPC_Clear_Ready_Text_Player_List_Array()
    {
        for(int iX = 0 ; iX < i_Text_Player_List_Array_Size ; iX++)
        {
            text_Player_List_Array[iX].color = Color.white;

        }/**for(int iX = 0 ; iX < i_Text_Player_List_Array_Size ; iX++)**/
    }/**void    RPC_Clear_Ready_Text_Player_List_Array()**/

    /****************************************************************/
    /****************************************************************/

    void    Set_Align_Image_Master()
    {
        float f_Inst_S = RT_Canvas.sizeDelta.x;
        if(f_Inst_S > RT_Canvas.sizeDelta.y)
        {f_Inst_S = RT_Canvas.sizeDelta.y;}

        Vector2 v2_Inst_Size = Vector2.zero;
        v2_Inst_Size.y = 0.1f * f_Inst_S;
        v2_Inst_Size.x = v2_Inst_Size.y * 6.0f;

        image_MASTER.rectTransform.sizeDelta = v2_Inst_Size;

        Vector3 v3_Inst_Position_Image_Master =
            Get_XYZ_from_XY(RT_Canvas.sizeDelta);
        
        v3_Inst_Position_Image_Master.x =
            v3_Inst_Position_Image_Master.x-(v2_Inst_Size.x * 0.5f);
        v3_Inst_Position_Image_Master.y =
            v3_Inst_Position_Image_Master.y-(v2_Inst_Size.y * 0.5f);
        
        image_MASTER.rectTransform.position = v3_Inst_Position_Image_Master;

    }/**void    Set_Align_Image_Master()**/

    void    Set_Align_Text_Player_List_Array()
    {
        float   f_Inst_S = RT_Canvas.sizeDelta.x;
        if(f_Inst_S > RT_Canvas.sizeDelta.y)
        {f_Inst_S = RT_Canvas.sizeDelta.y;}

        Vector2 v2_Inst_Size = (f_Inst_S * 0.1f) *
            (Vector2.one + (Vector2.right * 3.0f));

        Vector3 v3_Inst_Move_R =
            Vector3.right * v2_Inst_Size.x;
        Vector3 v3_Inst_Move_D =
            Vector3.down * v2_Inst_Size.y * 0.5f;
        
        int inst_Font_Size =
            (int)(v2_Inst_Size.y * 0.4f);        

        Vector3 v3_Inst_Position_A =
            Vector3.up * RT_Canvas.sizeDelta.y;
        Vector3 v3_Inst_Position_B =
            v3_Inst_Position_A + (Vector3.down * f_Inst_S * 0.2f);
        Vector3 v3_Inst_Position_00 =
            v3_Inst_Position_B +
                (Vector3.right * v2_Inst_Size.x * 0.5f) +
                    (Vector3.down * v2_Inst_Size.y * 0.5f);
        
        i_Text_Player_List_Array_Size = 24;
        text_Player_List_Array =
            new Text[i_Text_Player_List_Array_Size];

        for(int iK = 0 ; iK < i_Text_Player_List_Array_Size ; iK++)
        {
            int     inst_X = iK%2;
            int     inst_Y = iK/2;
            float   f_Inst_X = (float)inst_X;
            float   f_Inst_Y = (float)inst_Y;

            Vector3 v3_Inst_Position_for_Insert = v3_Inst_Position_00 +
                (v3_Inst_Move_R * f_Inst_X) + (v3_Inst_Move_D * f_Inst_Y);

            string  str_Inst = "Player_"+iK.ToString("00");
            GameObject  obj_Inst = new GameObject(str_Inst);

            RectTransform   RT_Inst = obj_Inst.AddComponent<RectTransform>();
            Text    text_Inst = obj_Inst.AddComponent<Text>();
            text_Inst.alignment = TextAnchor.MiddleCenter;
            text_Inst.raycastTarget = false;
            text_Inst.maskable = false;
            text_Inst.supportRichText = true;
            text_Inst.lineSpacing = 0.0f;
            text_Inst.fontSize = inst_Font_Size;
            text_Inst.color = Color.white;
            text_Inst.text = string.Empty;
            text_Inst.font = font_Arial;
            text_Inst.fontStyle = FontStyle.Bold;

            text_Player_List_Array[iK] = text_Inst;

            obj_Inst.transform.SetParent(obj_Group_NickName.transform);

            obj_Inst.AddComponent<Outline>();

            RT_Inst.sizeDelta = v2_Inst_Size;
            RT_Inst.position = v3_Inst_Position_for_Insert;

        }/**for(int iK = 0 ; iK < inst_Text_Array_Size ; iK++)**/

    }/**void    Set_Align_Text_Player_List_Array()**/
    void    Set_Align_Text_Player_Count()
    {
        float f_Inst_S = RT_Canvas.sizeDelta.x;
        if(f_Inst_S > RT_Canvas.sizeDelta.y)
        {f_Inst_S = RT_Canvas.sizeDelta.y;}

        Vector2 v2_Inst_Size = (Vector2.one+ Vector2.right) * f_Inst_S * 0.1f;

        Vector2 v2_Inst_Position_A =
            (Vector2.down * f_Inst_S * 0.1f)+
                RT_Canvas.sizeDelta;
        Vector2 v2_Inst_Position_B =
            v2_Inst_Position_A - (v2_Inst_Size * 0.5f);

        text_Player_Count.fontSize = (int)(v2_Inst_Size.y * 0.5f);
        text_Player_Count.rectTransform.sizeDelta = v2_Inst_Size;
        text_Player_Count.rectTransform.position =
            Get_XYZ_from_XY(v2_Inst_Position_B);

    }/**void    Set_Align_Text_Player_Count()**/

    void    Set_Align_Text_Player_NickName()
    {
        float f_Inst_S = RT_Canvas.sizeDelta.x;
        if(f_Inst_S > RT_Canvas.sizeDelta.y)
        {f_Inst_S = RT_Canvas.sizeDelta.y;}

        Vector2 v2_Inst_Size = f_Inst_S * 0.1f *
            (Vector2.one + (Vector2.right * 5.0f) );
        Vector2 v2_Inst_Position_A =
            (Vector2.down * f_Inst_S * 0.2f) + RT_Canvas.sizeDelta;
        Vector2 v2_Inst_Position_B =
            v2_Inst_Position_A - (v2_Inst_Size * 0.5f);
        
        text_Player_NickName.fontSize = (int)(v2_Inst_Size.y * 0.5f);
        text_Player_NickName.rectTransform.sizeDelta = v2_Inst_Size;
        text_Player_NickName.rectTransform.position =
            Get_XYZ_from_XY(v2_Inst_Position_B);
        
        text_Player_NickName.text = string.Empty;

    }/**void    Set_Align_Text_Player_NickName()**/

    void    Set_Align_Image_Button_XY()
    {
        float f_Inst_S = RT_Canvas.sizeDelta.x;
        if(f_Inst_S > RT_Canvas.sizeDelta.y)
        {f_Inst_S = RT_Canvas.sizeDelta.y;}

        Vector2 v2_Inst_Size =
            Vector2.one * f_Inst_S * 0.1f;
        Vector3 v3_Inst_Position_A =
            Vector3.up * RT_Canvas.sizeDelta.y;
        Vector3 v3_Inst_Position_B =
            v3_Inst_Position_A + 
                (Vector3.right * v2_Inst_Size.x * 0.5f)+
                    (Vector3.down * v2_Inst_Size.y * 0.5f);
        
        Vector3 v3_Inst_Position_Y = v3_Inst_Position_B;
        Vector3 v3_Inst_Position_X =
            v3_Inst_Position_Y + (Vector3.down * v2_Inst_Size.y);

        image_Button_Y.rectTransform.sizeDelta = v2_Inst_Size;
        image_Button_X.rectTransform.sizeDelta = v2_Inst_Size;

        image_Button_Y.rectTransform.position = v3_Inst_Position_Y;
        image_Button_X.rectTransform.position = v3_Inst_Position_X;
        




    }/**void    Set_Align_Image_Button_XY()**/

    void    Set_Align_Image_Button_Start()
    {
        float f_Inst_S = RT_Canvas.sizeDelta.x;
        if(f_Inst_S > RT_Canvas.sizeDelta.y)
        {f_Inst_S = RT_Canvas.sizeDelta.y;}

        Vector2 v2_Inst_Size =
            Vector2.one * f_Inst_S * 0.4f;
        
        Vector3 v3_Inst_Position_A =
            Get_XYZ_from_XY(RT_Canvas.sizeDelta) +
                (Vector3.down * f_Inst_S * 0.3f);
        Vector3 v3_Inst_Position_B =
            v3_Inst_Position_A - Get_XYZ_from_XY(v2_Inst_Size * 0.5f);
        
        image_Button_START.rectTransform.sizeDelta = v2_Inst_Size;
        image_Button_START.rectTransform.position = v3_Inst_Position_B;

    }/**void    Set_Align_Image_Button_Start()**/

    void    Set_Align_Image_Button_Back()
    {
        float f_Inst_S = RT_Canvas.sizeDelta.x;
        if(f_Inst_S > RT_Canvas.sizeDelta.y)
        {f_Inst_S = RT_Canvas.sizeDelta.y;}

        Vector2 v2_Inst_Size =
            Vector2.one * f_Inst_S * 0.1f;
        
        Vector3 v3_Inst_Position_A =
            (Vector3.right * RT_Canvas.sizeDelta.x);
        Vector3 v3_Inst_Position_B =
            v3_Inst_Position_A +
                (Vector3.left * v2_Inst_Size.x * 0.5f) +
                    (Vector3.up * v2_Inst_Size.y * 0.5f);
        image_Button_BACK.rectTransform.sizeDelta = v2_Inst_Size;
        image_Button_BACK.rectTransform.position = v3_Inst_Position_B;

    }/**void    Set_Align_Image_Button_Back()**/


    /****************************************************************/
    /****************************************************************/

    void    OnMasterClientSwitched()
    {
        if(PhotonNetwork.isMasterClient)
        {   
            image_MASTER.color = Color.white;
        }
        if(!PhotonNetwork.isMasterClient)
        {
            image_MASTER.color = color_Transparent;
        }
    }/**void    OnMasterClientSwitched()**/

    /****************************************************************/
    /****************************************************************/

    

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

    IEnumerator IE_Load_Scene_NEXT()
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
        AsyncOperation AO_To_Next =
            SceneManager.LoadSceneAsync(inst_Scene_Next);
        AO_To_Next.allowSceneActivation = false;

        bool    b_Inst = false;

        while(!b_Inst)
        {
            yield return new WaitForSeconds(deltaTime);

            int inst_Progress =
                (int)(AO_To_Next.progress * 100.0f);
            if(inst_Progress > 87)
            {b_Inst = true;}

        }/**while(!b_Inst)**/

        StopCoroutine(IE_Load_Scene_NEXT()  );

        AO_To_Next.allowSceneActivation = true;

    }/**IEnumerator IE_Load_Scene_NEXT()**/

    IEnumerator IE_Move_Scene(int INSERT)
    {
        yield return null;
        if(deltaTime == 0.0f)
        {deltaTime = 0.05f;}

        int inst_Scene_Count =
            SceneManager.sceneCountInBuildSettings;
        INSERT = Get_Number_Q(
            INSERT , inst_Scene_Count);
        AsyncOperation AO_To_Next =
            SceneManager.LoadSceneAsync(INSERT);
        //asyncOperation.allowSceneActivation = false;

        bool    b_Ready_to_Move = false;

        yield return new WaitForSeconds(deltaTime);

        while(!b_Ready_to_Move)
        {
            float f_Progress =
                AO_To_Next.progress * 100.0f;
            int inst_Progress = (int)f_Progress;

            yield return new WaitForSeconds(deltaTime);

            if(inst_Progress > 87)
            {   b_Ready_to_Move = true; }

        }/**while(!asyncOperation.isDone)**/

        //StopCoroutine(IE_Move_Scene()  );

        AO_To_Next.allowSceneActivation = true;

    }/**IEnumerator IE_Move_Scene(int INSERT)**/

    /****************************************************************/
    /****************************************************************/

    //void OnPhotonSerializeView()
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        print("OnPhotonSerializeView");

    }/**void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)**/

    public  void    BUTTON_Y_DOWN()
    {
        image_Button_Y.enabled = false;
        image_Button_X.enabled = true;

        image_Button_BACK.enabled = false;

        photonView_This.RPC(
            "RPC_Add_Ready_Count" , PhotonTargets.All , null);
        photonView_This.RPC(
            "RPC_Text_Player_Count" , PhotonTargets.All , null);
        photonView_This.RPC(
            "RPC_Button_Start_Enable_for_Master" , PhotonTargets.All , null);
        photonView_This.RPC(
            "RPC_Judge_Ready_Text_Player_List_Array" ,
                PhotonTargets.All , PhotonNetwork.player.NickName, true);
        
    }/**public  void    BUTTON_Y_DOWN()**/

    public  void    BUTTON_X_DOWN()
    {
        image_Button_X.enabled = false;
        image_Button_Y.enabled = true;

        image_Button_BACK.enabled = true;

        photonView_This.RPC(
            "RPC_Sub_Ready_Count" , PhotonTargets.All , null);
        photonView_This.RPC(
            "RPC_Text_Player_Count" , PhotonTargets.All , null);
        photonView_This.RPC(
            "RPC_Button_Start_Enable_for_Master" , PhotonTargets.All , null);
        photonView_This.RPC(
            "RPC_Judge_Ready_Text_Player_List_Array" ,
                PhotonTargets.All , PhotonNetwork.player.NickName, false);
        
    }/**public  void    BUTTON_X_DOWN()**/

    public  void    BUTTON_START_DOWN()
    {
        print("BUTTON_START_DOWN");

        photonView_This.RPC(
            "RPC_Disable_Button_All",PhotonTargets.All,null);
        photonView_This.RPC(
            "RPC_Move_Scene_Next",PhotonTargets.All,null);
        

    }/**public  void    BUTTON_START_DOWN()**/

    public  void    BUTTON_BACK_DOWN()
    {
        print("BUTTON_BACK_DOWN");

        if(PhotonNetwork.inRoom)
        {   print("LeaveRoom");
            PhotonNetwork.LeaveRoom();}

        CancelInvoke();
        StopAllCoroutines();

        int inst_Scene_Current =
            Get_Number_Scene_Current();
        int inst_Scene_Before =
                inst_Scene_Current - 2;

        StartCoroutine(IE_Move_Scene(inst_Scene_Before));

    }/**public  void    BUTTON_BACK_DOWN()**/

    [ContextMenu("PREVIEW")]

    void    CM_PREVIEW()
    {
        if(canvas == null)
        {canvas = GameObject.Find(
            "Canvas").GetComponent<Canvas>();}
        if(RT_Canvas == null)
        {RT_Canvas = canvas.GetComponent<RectTransform>();}
        
        Set_Align_Image_Master();
        
        Set_Align_Text_Player_Count();
        Set_Align_Text_Player_NickName();

        Set_Align_Image_Button_XY();
        Set_Align_Image_Button_Start();
        Set_Align_Image_Button_Back();

        image_MASTER.color = Color.white;

        image_Button_START.enabled = true;

        image_Button_Y.enabled = true;
        image_Button_X.enabled = true;

        image_Button_BACK.enabled = true;

        text_Player_NickName.text = "@ NickName";

        //Set_Align_Text_Player_List_Array();

    }/**void    CM_PREVIEW()**/


}/**public class Photon_Scene_A3 : MonoBehaviour**/