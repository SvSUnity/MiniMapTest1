using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Photon_Scene_A2 : MonoBehaviour
{
    [SerializeField]GameObject  obj_Prefab_Text_Room_Button;
    [SerializeField]Text_Room_Button[]    cs_Text_Room_Button_Array;

    [SerializeField]Text    text_BUTTON_REFRESH;
    [SerializeField]Text    text_BUTTON_CREATE;
    [SerializeField]Text    text_BUTTON_EXIT;

    Vector3[] v3_Position_Text_Array;

    Canvas  canvas;
    RectTransform   RT_Canvas;
    [SerializeField]RectTransform   RT_BUTTON_REFRESH;
    [SerializeField]RectTransform   RT_BUTTON_CREATE;
    [SerializeField]RectTransform   RT_BUTTON_EXIT;

    [SerializeField]Text    text_NickName;

    RoomInfo[]  RI_Array;
    Vector3 v3_Canvas_Size;
    Vector2 v2_Canvas_Size;
    Vector2 v2_Text_Room_Size;
    float   deltaTime;

    float   f_Distance_Canvas_Axis_Shorter;

    int i_Text_Room_Button_Size;

    int i_Room_Count_A;
    int i_Room_Count_B;
    int i_Room_Count_Result;

    int i_Number_Room;

    bool    b_INPUT_BUTTON_REFRESH;
    bool    b_INPUT_BUTTON_CREATE;
    bool    b_INPUT_BUTTON_EXIT;

    string  str_NickName;

    /****************************************************************/
    /****************************************************************/

    void    Awake()
    {

        if(obj_Prefab_Text_Room_Button == null)
        {obj_Prefab_Text_Room_Button =
            (GameObject)Resources.Load(
                "Text_Room_Button" , typeof(GameObject));}

        if(canvas == null)
        {canvas = GameObject.Find(
            "Canvas").GetComponent<Canvas>();}
        if(RT_Canvas == null)
        {RT_Canvas = canvas.GetComponent<RectTransform>();}

        if(RT_BUTTON_REFRESH == null)
        {RT_BUTTON_REFRESH =
            text_BUTTON_REFRESH.GetComponent<RectTransform>();}

        if(RT_BUTTON_CREATE == null)
        {RT_BUTTON_CREATE =
            text_BUTTON_CREATE.GetComponent<RectTransform>();}
        
        if(RT_BUTTON_EXIT == null)
        {RT_BUTTON_EXIT =
            text_BUTTON_EXIT.GetComponent<RectTransform>();}
        


    }/**void    Awake()**/

    void    Start()
    {
        //str_GameVersion = "0.1.0";

        deltaTime = 0.05f;

        SingleTon.INSTANCE.Set_RoomName(string.Empty);

        str_NickName = PhotonNetwork.player.NickName;

        v2_Canvas_Size = RT_Canvas.sizeDelta;
        v3_Canvas_Size = Get_XYZ_from_XY(v2_Canvas_Size);

        float f_Inst_S = RT_Canvas.sizeDelta.x;
        if(f_Inst_S > RT_Canvas.sizeDelta.y)
        {f_Inst_S = RT_Canvas.sizeDelta.y;}

        f_Distance_Canvas_Axis_Shorter = f_Inst_S;


        Vector2   v2_Inst_Text_NickName_Size = f_Inst_S *
            (Vector2.one + (Vector2.right * 4.0f) ) * 0.125f;
        if(str_NickName.Length < 1)
        {text_NickName.text = "@ NickName";}
        if(str_NickName.Length > 0)
        {text_NickName.text = "@ "+str_NickName+" ";}
        text_NickName.fontSize =
            (int)(v2_Inst_Text_NickName_Size.y * 0.5f);
        text_NickName.rectTransform.sizeDelta = v2_Inst_Text_NickName_Size;
        text_NickName.rectTransform.position =
            v3_Canvas_Size - Get_XYZ_from_XY(v2_Inst_Text_NickName_Size * 0.5f);
        
        v2_Text_Room_Size.x =
            f_Distance_Canvas_Axis_Shorter * 0.5f;
        v2_Text_Room_Size.y =
            v2_Text_Room_Size.x * 0.25f;

        Vector3 v3_Inst_Position_Refresh =
            (Vector3.right * v2_Text_Room_Size.x * 0.5f) +
                (Vector3.down * v2_Text_Room_Size.y * 0.5f) +
                    (Vector3.up * RT_Canvas.sizeDelta.y);
        Vector3 v3_Inst_Position_Create =
            v3_Inst_Position_Refresh + (Vector3.down * v2_Text_Room_Size.y);

        Vector3 v3_Inst_Position_Exit =
            (Vector3.left * v2_Text_Room_Size.x * 0.5f) +
                (Vector3.up * v2_Text_Room_Size.y * 0.5f) +
                    (Vector3.right * RT_Canvas.sizeDelta.x);

        RT_BUTTON_REFRESH.sizeDelta = v2_Text_Room_Size;
        RT_BUTTON_CREATE.sizeDelta = v2_Text_Room_Size;
        RT_BUTTON_EXIT.sizeDelta = v2_Text_Room_Size;

        RT_BUTTON_REFRESH.position = v3_Inst_Position_Refresh;
        RT_BUTTON_CREATE.position = v3_Inst_Position_Create;
        RT_BUTTON_EXIT.position = v3_Inst_Position_Exit;

        text_BUTTON_REFRESH.raycastTarget = false;
        text_BUTTON_CREATE.raycastTarget = false;
        text_BUTTON_EXIT.raycastTarget = false;

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
                inst_Scene_Current - 2;
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

        //PhotonNetwork.isMessageQueueRunning = true;
        float f_Inst_S = RT_Canvas.sizeDelta.x;
        if(f_Inst_S > RT_Canvas.sizeDelta.y)
        {f_Inst_S = RT_Canvas.sizeDelta.y;}

        float   f_Inst_Font_Size = f_Inst_S;
        f_Inst_Font_Size = f_Inst_Font_Size * 0.25f;
        f_Inst_Font_Size = f_Inst_Font_Size * 0.25f;

        int     inst_Font_Size = (int)f_Inst_Font_Size;
        
        text_BUTTON_REFRESH.fontSize = inst_Font_Size;
        text_BUTTON_CREATE.fontSize = inst_Font_Size;
        text_BUTTON_EXIT.fontSize = inst_Font_Size;



        PhotonNetwork.sendRate = 20;
        PhotonNetwork.sendRateOnSerialize = 20;

        Vector3 v3_Inst_Position = 0.5f *
            Get_XYZ_from_XY(v2_Text_Room_Size);
        float   f_Inst_Height = v2_Text_Room_Size.y;
        i_Text_Room_Button_Size = 4;
        float   f_Inst_Size = (float)(i_Text_Room_Button_Size - 1);
        v3_Inst_Position = v3_Inst_Position +
            (Vector3.up * f_Inst_Height * f_Inst_Size);
        
        v3_Position_Text_Array =
            new Vector3[i_Text_Room_Button_Size];
        cs_Text_Room_Button_Array =
            new Text_Room_Button[i_Text_Room_Button_Size];

        for(int iX = 0 ; iX < i_Text_Room_Button_Size ; iX++)
        {
            float   f_iX = (float)iX;
            v3_Position_Text_Array[iX] =
                v3_Inst_Position + (Vector3.down * f_iX * f_Inst_Height);
            GameObject  obj_Inst = (GameObject)Instantiate(
                    obj_Prefab_Text_Room_Button , RT_Canvas);
            obj_Inst.transform.position = v3_Position_Text_Array[iX];
            obj_Inst.name = "Text_Room_Button_"+iX.ToString("00");
            cs_Text_Room_Button_Array[iX] =
                obj_Inst.GetComponent<Text_Room_Button>();
            cs_Text_Room_Button_Array[iX].Set_Photon_Scene_A2(this);
            cs_Text_Room_Button_Array[iX].Set_RectTransform_Canvas(RT_Canvas);
            cs_Text_Room_Button_Array[iX].Set_Canvas(canvas);
            cs_Text_Room_Button_Array[iX].Initialize(v2_Text_Room_Size);

        }/**for(int iX = 0 ; iX < i_Text_Room_Button_Size ; iX++)**/

        InvokeRepeating("Invoke_R_97" , deltaTime , deltaTime);

    }/**void    Invoke_O_98()**/

    void    Invoke_R_97()
    {
        int inst_Count = 0;
        
        for(int iX = 0 ; iX < i_Text_Room_Button_Size ; iX++)
        {
            if(cs_Text_Room_Button_Array[iX].Get_Check_Initialize_Finished())
            {
                inst_Count = inst_Count + 1;
                cs_Text_Room_Button_Array[iX].Set_Text_String(string.Empty);
                //cs_Text_Room_Button_Array[iX].Set_RaycastTarget(true);
            }
        }/**for(int iX = 0 ; iX < i_Text_Room_Button_Size ; iX++)**/

        if(inst_Count > i_Text_Room_Button_Size - 1)
        {
            CancelInvoke("Invoke_R_97");

            Invoke("Invoke_O_96" , deltaTime);

        }/**if(inst_Count > i_Text_Room_Button_Size - 1)**/

    }/**void    Invoke_R_97()**/

    void    Invoke_O_96()
    {
        CancelInvoke("Invoke_O_96");

        i_Number_Room = 0;

        RI_Array = PhotonNetwork.GetRoomList();
        i_Room_Count_A = RI_Array.Length;
        i_Room_Count_B = PhotonNetwork.countOfRooms;

        i_Room_Count_Result =
            Get_Smaller(i_Room_Count_A, i_Room_Count_B);
        
        i_Number_Room = Get_Number_Q(
            i_Number_Room, i_Room_Count_Result);
        
        if(i_Room_Count_Result < i_Text_Room_Button_Size + 1)
        {
            for(int iX = 0 ; iX < i_Room_Count_Result ; iX++)
            {
                int inst_Problem_Count = 0;
                if(!RI_Array[iX].IsOpen)
                {inst_Problem_Count = inst_Problem_Count+1;}
                if(!RI_Array[iX].IsVisible)
                {inst_Problem_Count = inst_Problem_Count+1;}
                if(RI_Array[iX].PlayerCount < 1)
                {inst_Problem_Count = inst_Problem_Count+1;}
                if(RI_Array[iX].MaxPlayers < 2)
                {inst_Problem_Count = inst_Problem_Count+1;}
                if(RI_Array[iX].PlayerCount >= RI_Array[iX].MaxPlayers)
                {inst_Problem_Count = inst_Problem_Count+1;}

                cs_Text_Room_Button_Array[iX].Set_Text_String(RI_Array[iX].Name);

                if(inst_Problem_Count > 0)
                {   cs_Text_Room_Button_Array[iX].Set_RaycastTarget(false);
                    cs_Text_Room_Button_Array[iX].Set_Color_Text(Color.red);  }
                if(inst_Problem_Count < 1)
                {   cs_Text_Room_Button_Array[iX].Set_RaycastTarget(true);
                    cs_Text_Room_Button_Array[iX].Set_Color_Text(Color.white);  }

            }/**for(int iX = 0 ; iX < i_Room_Count_Result ; iX++)**/
        }/**if(i_Room_Count_Result < i_Text_Room_Button_Size + 1)**/

        if(i_Room_Count_Result > i_Text_Room_Button_Size)
        {
            for(int iX = 0 ; iX < i_Text_Room_Button_Size ; iX++)
            {
                int inst_Problem_Count = 0;
                if(!RI_Array[iX].IsOpen)
                {inst_Problem_Count = inst_Problem_Count+1;}
                if(!RI_Array[iX].IsVisible)
                {inst_Problem_Count = inst_Problem_Count+1;}
                if(RI_Array[iX].PlayerCount < 1)
                {inst_Problem_Count = inst_Problem_Count+1;}
                if(RI_Array[iX].MaxPlayers < 2)
                {inst_Problem_Count = inst_Problem_Count+1;}
                if(RI_Array[iX].PlayerCount >= RI_Array[iX].MaxPlayers)
                {inst_Problem_Count = inst_Problem_Count+1;}

                cs_Text_Room_Button_Array[iX].Set_Text_String(RI_Array[iX].Name);

                if(inst_Problem_Count > 0)
                {   cs_Text_Room_Button_Array[iX].Set_RaycastTarget(false);
                    cs_Text_Room_Button_Array[iX].Set_Color_Text(Color.red);  }
                if(inst_Problem_Count < 1)
                {   cs_Text_Room_Button_Array[iX].Set_RaycastTarget(true);
                    cs_Text_Room_Button_Array[iX].Set_Color_Text(Color.white);  }

            }/**for(int iX = 0 ; iX < 4 ; iX++)**/
        }/**if(i_Room_Count_Result > i_Text_Room_Button_Size)**/

        Invoke("Invoke_O_95" , deltaTime);

    }/**void    Invoke_O_96()**/

    ///<Summary>
    ///Initialized Finished :: 초기화 완료
    ///</Summary>
    void    Invoke_O_95()
    {
        CancelInvoke("Invoke_O_95");

        text_BUTTON_REFRESH.raycastTarget = true;
        text_BUTTON_CREATE.raycastTarget = true;
        text_BUTTON_EXIT.raycastTarget = true;

        CancelInvoke();

    }/**void    Invoke_O_95()**/



    ///<Summary>
    ///REFRESH ROOM LIST :: 룸 리스트 받아오기
    ///</Summary>
    void    Invoke_O_89()
    {
        CancelInvoke();

        text_BUTTON_REFRESH.text = "WAITING";
        text_BUTTON_REFRESH.raycastTarget = false;
        text_BUTTON_CREATE.raycastTarget = false;
        text_BUTTON_EXIT.raycastTarget = false;

        Invoke("Invoke_O_88" , deltaTime);

    }/**void    Invoke_O_89()**/

    void    Invoke_O_88()
    {
        CancelInvoke("Invoke_O_88");

        for(int iX = 0 ; iX < i_Text_Room_Button_Size ; iX++)
        {
            cs_Text_Room_Button_Array[iX].Set_RaycastTarget(false);
            cs_Text_Room_Button_Array[iX].Set_Text_String(string.Empty);

        }/**for(int iX = 0 ; iX < i_Text_Room_Button_Size ; iX++)**/

        Invoke("Invoke_O_87" , deltaTime);

    }/**void    Invoke_O_88()**/

    void    Invoke_O_87()
    {
        CancelInvoke("Invoke_O_87");

        RI_Array = PhotonNetwork.GetRoomList();
        i_Room_Count_A = RI_Array.Length;
        i_Room_Count_B = PhotonNetwork.countOfRooms;
        i_Room_Count_Result =
            Get_Smaller(i_Room_Count_A , i_Room_Count_B);
        
        if(i_Room_Count_Result < i_Text_Room_Button_Size + 1)
        {
            i_Number_Room = 0;

            for(int iX = 0 ; iX < i_Room_Count_Result ; iX++)
            {
                int inst_Problem_Count = 0;
                if(!RI_Array[iX].IsOpen)
                {inst_Problem_Count = inst_Problem_Count+1;}
                if(!RI_Array[iX].IsVisible)
                {inst_Problem_Count = inst_Problem_Count+1;}
                if(RI_Array[iX].PlayerCount < 1)
                {inst_Problem_Count = inst_Problem_Count+1;}
                if(RI_Array[iX].MaxPlayers < 2)
                {inst_Problem_Count = inst_Problem_Count+1;}
                if(RI_Array[iX].PlayerCount >= RI_Array[iX].MaxPlayers)
                {inst_Problem_Count = inst_Problem_Count+1;}

                cs_Text_Room_Button_Array[iX].Set_Text_String(RI_Array[iX].Name);

                if(inst_Problem_Count > 0)
                {   cs_Text_Room_Button_Array[iX].Set_RaycastTarget(false);
                    cs_Text_Room_Button_Array[iX].Set_Color_Text(Color.red);  }
                if(inst_Problem_Count < 1)
                {   cs_Text_Room_Button_Array[iX].Set_RaycastTarget(true);
                    cs_Text_Room_Button_Array[iX].Set_Color_Text(Color.white);  }

            }/**for(int iX = 0 ; iX < i_Room_Count_Result ; iX++)**/

        }/**if(i_Room_Count_Result < i_Text_Room_Button_Size + 1)**/


        if(i_Room_Count_Result > i_Text_Room_Button_Size)
        {
            i_Number_Room = Get_Number_Q(i_Number_Room +
                i_Text_Room_Button_Size , i_Room_Count_Result);
            
            for(int iX = 0 ; iX < i_Text_Room_Button_Size ; iX++)
            {
                int inst_X = Get_Number_Q(
                    i_Number_Room + iX , i_Room_Count_Result);

                int inst_Problem_Count = 0;
                if(!RI_Array[inst_X].IsOpen)
                {inst_Problem_Count = inst_Problem_Count+1;}
                if(!RI_Array[inst_X].IsVisible)
                {inst_Problem_Count = inst_Problem_Count+1;}
                if(RI_Array[inst_X].PlayerCount < 1)
                {inst_Problem_Count = inst_Problem_Count+1;}
                if(RI_Array[inst_X].MaxPlayers < 2)
                {inst_Problem_Count = inst_Problem_Count+1;}
                if(RI_Array[inst_X].PlayerCount >= RI_Array[iX].MaxPlayers)
                {inst_Problem_Count = inst_Problem_Count+1;}

                cs_Text_Room_Button_Array[iX].Set_Text_String(RI_Array[inst_X].Name);

                if(inst_Problem_Count > 0)
                {   cs_Text_Room_Button_Array[iX].Set_RaycastTarget(false);
                    cs_Text_Room_Button_Array[iX].Set_Color_Text(Color.red);  }
                if(inst_Problem_Count < 1)
                {   cs_Text_Room_Button_Array[iX].Set_RaycastTarget(true);
                    cs_Text_Room_Button_Array[iX].Set_Color_Text(Color.white);  }

            }/**for(int iX = 0 ; iX < i_Text_Room_Button_Size ; iX++)**/

        }/**if(i_Room_Count_Result > i_Text_Room_Button_Size)**/

        Invoke("Invoke_O_86" , deltaTime);

    }/**void    Invoke_O_87()**/

    void    Invoke_O_86()
    {
        CancelInvoke("Invoke_O_86");

        text_BUTTON_REFRESH.text = "REFRESH";
        text_BUTTON_REFRESH.raycastTarget = true;
        text_BUTTON_CREATE.raycastTarget = true;
        text_BUTTON_EXIT.raycastTarget = true;

    }/**void    Invoke_O_86()**/



    ///<Summary>
    ///CREATE ROOM :: 룸 생성
    ///</Summary>
    void    Invoke_O_79()
    {
        CancelInvoke();

        text_BUTTON_REFRESH.raycastTarget = false;
        text_BUTTON_CREATE.raycastTarget = false;
        text_BUTTON_EXIT.raycastTarget = false;

        for(int iX = 0 ; iX < i_Text_Room_Button_Size ; iX++)
        {cs_Text_Room_Button_Array[iX].Set_RaycastTarget(false);}

        Invoke("Invoke_O_78" , deltaTime);

    }/**void    Invoke_O_79()**/

    void    Invoke_O_78()
    {
        CancelInvoke("Invoke_O_78");

        int inst_00 = PhotonNetwork.countOfPlayersOnMaster;
        int inst_01 = PhotonNetwork.countOfRooms;
        int inst_R0 = Random.Range(0,10000);

        inst_00 = Get_Number_Q(inst_00 , 100);
        inst_01 = Get_Number_Q(inst_01 , 100);
        inst_R0 = Get_Number_Q(inst_R0 , 10000);

        string  str_Inst_RoomName = "Room_" +
            inst_00.ToString("00") + inst_01.ToString("00")+
                inst_R0.ToString("0000") + "_" + str_NickName;
        SingleTon.INSTANCE.Set_RoomName(str_Inst_RoomName);

        Invoke("Invoke_O_77" , deltaTime);

    }/**void    Invoke_O_78()**/

    void    Invoke_O_77()
    {
        CancelInvoke("Invoke_O_77");

        StartCoroutine(IE_Move_Scene_NEXT_as_Room_Creator());

    }/**void    Invoke_O_77()**/




    ///<Summary>
    ///JOINING ROOM :: 룸 입장
    ///</Summary>
    void    Invoke_O_69()
    {
        CancelInvoke();

        text_BUTTON_REFRESH.raycastTarget = false;
        text_BUTTON_CREATE.raycastTarget = false;
        text_BUTTON_EXIT.raycastTarget = false;

        for(int iX = 0 ; iX < i_Text_Room_Button_Size ; iX++)
        {cs_Text_Room_Button_Array[iX].Set_RaycastTarget(false);}

        Invoke("Invoke_O_68" , deltaTime);

    }/**void    Invoke_O_69()**/

    void    Invoke_O_68()
    {
        CancelInvoke("Invoke_O_68");

        StartCoroutine(IE_Move_Scene_NEXT_as_Room_Joinner());
        
    }/**void    Invoke_O_68()**/

    /****************************************************************/
    /****************************************************************/

    /****************************************************************/
    /****************************************************************/



    Vector3 Get_XYZ_from_XY(Vector2 INSERT)
    {return  new Vector3(INSERT.x , INSERT.y);}
    Vector2 Get_XY_from_XYZ(Vector3 INSERT)
    {return  new Vector2(INSERT.x , INSERT.y);}

    int    Get_Bigger(int A, int B)
    {
        int  i_Return = A;if(i_Return < B)
        {i_Return = B;}return i_Return;
    }/**int    Get_Bigger(int A, int B)**/

    int    Get_Smaller(int A, int B)
    {
        int  i_Return = A;if(i_Return > B)
        {i_Return = B;}return  i_Return;
    }/**int    Get_Smaller(int A, int B)**/

    int     Get_Number_Q(int NUMBER , int SIZE)
    {
        if(SIZE < 1){    SIZE = 1;    }
        if(NUMBER < 0){NUMBER = NUMBER + SIZE;}
        if(NUMBER >= SIZE){NUMBER = NUMBER - SIZE;}

        if( (NUMBER < 0) || (NUMBER >= SIZE) )
        {    NUMBER = Get_Number_Q(NUMBER , SIZE);    }

        return    NUMBER;
    }/**int     Get_Number_Q(int NUMBER , int SIZE)**/

    void    Reverse_Array<Type_T>(Type_T[] Array)
    {
        int inst_Array_Size = Array.Length;
        int inst_Size_M = inst_Array_Size - 1;
        int inst_Half = inst_Array_Size / 2;

        if(inst_Array_Size > 1)
        {
            for(int iX = 0 ; iX < inst_Half ; iX++)
            {
                Type_T  tt_Inst = Array[iX];
                Array[iX] = Array[inst_Size_M-iX];
                Array[inst_Size_M - iX] = tt_Inst;

            }/**for(int iX = 0 ; iX < inst_Half ; iX++)**/
        }/**if(inst_Array_Size > 1)**/

    }/**void    Reverse_Array<Type_T>(Type_T[] Array)**/

    int Get_Scene_BuildSetting_Count()
    {return UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;}
    int Get_Number_Scene_Current()
    {return UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;}

    IEnumerator IE_Move_Scene_NEXT_as_Room_Joinner()
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

        AsyncOperation  asyncOperation =
            SceneManager.LoadSceneAsync(inst_Scene_Next);
        asyncOperation.allowSceneActivation = false;

        text_BUTTON_CREATE.text = "LOADING : 00"+"%";

        bool    b_Ready_to_Move = false;

        while(!b_Ready_to_Move)
        {
            yield return new WaitForSeconds(deltaTime);

            float f_Progress =
                asyncOperation.progress * 100.0f;
            int inst_Progress = (int)f_Progress;

            text_BUTTON_CREATE.text =
                "LOADING : "+ inst_Progress.ToString("00")+"%";

            if(inst_Progress > 88)
            {
                b_Ready_to_Move = true;
            }/**if(inst_Progress > 89)**/
        }/**while(!b_Ready_to_Move)**/

        text_BUTTON_CREATE.text = "GOOD";
        yield return new WaitForSeconds(1.0f);

        PhotonNetwork.JoinRoom(SingleTon.INSTANCE.Get_RoomName());

        asyncOperation.allowSceneActivation = true;

    }/**IEnumerator IE_Move_Scene_NEXT_as_Room_Joinner()**/

    IEnumerator IE_Move_Scene_NEXT_as_Room_Creator()
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

        AsyncOperation  asyncOperation =
            SceneManager.LoadSceneAsync(inst_Scene_Next);
        asyncOperation.allowSceneActivation = false;

        text_BUTTON_CREATE.text = "CREATE : 00"+"%";

        bool    b_Ready_to_Move = false;

        while(!b_Ready_to_Move)
        {
            yield return new WaitForSeconds(deltaTime);

            float f_Progress =
                asyncOperation.progress * 100.0f;
            int inst_Progress = (int)f_Progress;

            text_BUTTON_CREATE.text =
                "CREATE : "+ inst_Progress.ToString("00")+"%";

            if(inst_Progress > 88)
            {
                b_Ready_to_Move = true;

            }/**if(inst_Progress > 89)**/

        }/**while(!b_Ready_to_Move)**/

        text_BUTTON_CREATE.text = "GOOD";
        yield return new WaitForSeconds(1.0f);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 24;
        //roomOptions.CleanupCacheOnLeave = true;
        roomOptions.CleanupCacheOnLeave = true;

        PhotonNetwork.CreateRoom(
            SingleTon.INSTANCE.Get_RoomName(),
                roomOptions , TypedLobby.Default);

        asyncOperation.allowSceneActivation = true;

    }/**IEnumerator IE_Move_Scene_NEXT_as_Room_Creator()**/


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

            if(inst_Progress > 89)
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

    /****************************************************************/
    /****************************************************************/
    void    OnPhotonPlayerDisconnected(PhotonPlayer PP)
    {
        print("AAVV");
    }

    public  void    BUTTON_ROOM_DOWN()
    {
        print("BUTTON_ROOM_DOWN");
        Invoke("Invoke_O_69" , deltaTime);

    }/**public  void    BUTTON_ROOM_DOWN()**/

    public  void    BUTTON_REFRESH_DOWN()
    {
        print("BUTTON_REFRESH_DOWN");
        Invoke("Invoke_O_89" , deltaTime);

    }/**public  void    BUTTON_REFRESH_DOWN()**/

    public  void    BUTTON_CREATE_DOWN()
    {
        print("BUTTON_CREATE_DOWN");
        Invoke("Invoke_O_79" , deltaTime);
        
    }/**public  void    BUTTON_CREATE_DOWN()**/

    public  void    BUTTON_EXIT_DOWN()
    {
        print("BUTTON_EXIT_DOWN");

        CancelInvoke();
        StopAllCoroutines();

        StartCoroutine(IE_Photon_Leave_Lobby() );
        
    }/**public  void    BUTTON_EXIT_DOWN()**/


    private IEnumerator IE_Photon_Leave_Lobby()
    {
        print("IE_Photon_Leave_Lobby");
        yield return null;
        PhotonNetwork.LeaveLobby();
        yield return new WaitForSeconds(deltaTime);

        while(PhotonNetwork.insideLobby)
        {
            yield return new WaitForSeconds(deltaTime);
        }

        if(!PhotonNetwork.insideLobby)
        {
            StopCoroutine(IE_Photon_Leave_Lobby());
        }

        StartCoroutine(IE_Photon_DisConnect());
    }/**private IEnumerator IE_Photon_Leave_Lobby()**/

    private IEnumerator IE_Photon_DisConnect()
    {
        print("IE_Photon_DisConnect");
        yield return null;
        if(PhotonNetwork.connected)
        {PhotonNetwork.Disconnect();}
        yield return new WaitForSeconds(deltaTime);
        print("QUIT");
        Application.Quit();
    }/**private IEnumerator IE_Photon_DisConnect()**/



}/**public class Photon_Scene_A2 : MonoBehaviour**/