using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Photon_Scene_B4 : MonoBehaviour
{
    Canvas canvas;
    RectTransform RT_Canvas;

    [SerializeField] Image image_BackGround_Black;
    [SerializeField] Image image_Loading_BAR;

    Vector3 v3_Canvas_Size;
    Vector2 v2_Canvas_Size;

    float deltaTime;

    int i_Loaded_Count;


    AsyncOperation AO_Load_Scene_Next;
    string str_NickName;
    [SerializeField] PhotonView photonView_This;

    /****************************************************************/
    /****************************************************************/

    void Awake()
    {
        if (canvas == null)
        {
            canvas = GameObject.Find(
               "Canvas").GetComponent<Canvas>();
        }
        if (RT_Canvas == null)
        { RT_Canvas = canvas.GetComponent<RectTransform>(); }

        if (photonView_This == null)
        { photonView_This = this.gameObject.GetComponent<PhotonView>(); }
        if (photonView_This.ObservedComponents.Count < 1)
        { photonView_This.ObservedComponents.Add(this); }
        if (photonView_This.ObservedComponents[0] != this)
        { photonView_This.ObservedComponents[0] = this; }

    }/**void    Awake()**/

    /****************************************************************/
    /****************************************************************/

    private void Start()
    {
        deltaTime = 0.05f;

        v2_Canvas_Size = RT_Canvas.sizeDelta;
        v3_Canvas_Size = Get_XYZ_from_XY(v2_Canvas_Size);

        Align_Image_BackGround_Black();
        Align_Image_Loading_BAR();

        Invoke("Invoke_O_99", deltaTime);

    }/**private void Start()**/


    void Align_Image_BackGround_Black()
    {
        image_BackGround_Black.rectTransform.sizeDelta = v2_Canvas_Size;
        image_BackGround_Black.rectTransform.position = v3_Canvas_Size * 0.5f;

    }/**void Align_Image_BackGround_Black()**/

    void Align_Image_Loading_BAR()
    {
        Vector2 v2_Inst_Size = Vector2.right * v3_Canvas_Size.x;
        v2_Inst_Size.y = v3_Canvas_Size.x * 0.2f;

        Vector3 v3_Inst_Position_A =
            Get_XYZ_from_XY(v2_Inst_Size * 0.5f);
        image_Loading_BAR.rectTransform.sizeDelta = v2_Inst_Size;
        image_Loading_BAR.rectTransform.position = v3_Inst_Position_A;

    }/**void Align_Image_Loading_BAR()**/

    /****************************************************************/
    /****************************************************************/
    ///<summary>
    ///문제가 있는지 일단 검출하고 , 문제가 있으면 뒤로 강제로 돌려보낸다.
    ///</summary>
    void Invoke_O_99()
    {
        ///안전장치로서 일단 CancelInvoke 넣어두었다.
        CancelInvoke("Invoke_O_99");

        int inst_Error_Count = 0;

        if (!PhotonNetwork.connected)
        { inst_Error_Count++; }
        if (!PhotonNetwork.inRoom)
        { inst_Error_Count++; }

        if (inst_Error_Count > 0)
        {
            print("문제가 있으니 씬 강제이동 집행.");
            int inst_To_Move = Get_Number_Scene_Current()-3;
            StartCoroutine(IE_Move_To_Scene_Enforce(inst_To_Move));

        }/**if (inst_Error_Count > 0)**/

        if (inst_Error_Count < 1)
        {
            Invoke("Invoke_O_98", 0.5f);

        }/**if (inst_Error_Count < 1)**/

    }/**void Invoke_O_99()**/

    void Invoke_O_98()
    {
        CancelInvoke("Invoke_O_98");

        if (PhotonNetwork.player.IsMasterClient)
        {
            photonView_This.RPC(
                "RPC_Order_Load_Scene_Next_And_Send_Signal",
                    PhotonTargets.All , null);

        }/**if (PhotonNetwork.player.IsMasterClient)**/

    }/**void Invoke_O_98()**/



    void Invoke_R_89()
    {
        if (PhotonNetwork.player.IsMasterClient)
        {
            int inst_Player_Count_M =
                PhotonNetwork.room.PlayerCount - 1;
            int inst_Ready_Count = i_Loaded_Count;

            print("Ready Count : " + inst_Ready_Count);

            if (inst_Ready_Count > inst_Player_Count_M)
            {
                CancelInvoke("Invoke_R_89");
                photonView_This.RPC(
                    "RPC_Order_Allow_Move_Scene_Next",
                        PhotonTargets.AllBufferedViaServer, null);
                        
            }/**if (inst_Ready_Count > inst_Player_Count_M)**/

        }/**if (PhotonNetwork.player.IsMasterClient)**/

    }/**void Invoke_R_89()**/


    /****************************************************************/
    /****************************************************************/

    Vector3 Get_XYZ_from_XY(Vector2 INSERT)
    { return new Vector3(INSERT.x, INSERT.y); }
    Vector2 Get_XY_from_XYZ(Vector3 INSERT)
    { return new Vector2(INSERT.x, INSERT.y); }

    int Get_Number_Q(int NUMBER, int SIZE)
    {
        if (SIZE < 1) { SIZE = 1; }
        if (NUMBER < 0) { NUMBER = NUMBER + SIZE; }
        if (NUMBER >= SIZE) { NUMBER = NUMBER - SIZE; }

        if ((NUMBER < 0) || (NUMBER >= SIZE))
        { NUMBER = Get_Number_Q(NUMBER, SIZE); }

        return NUMBER;
    }/**int     Get_Number_Q(int NUMBER , int SIZE)**/


    int Get_Scene_BuildSetting_Count()
    { return UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; }
    int Get_Number_Scene_Current()
    { return UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex; }

    /****************************************************************/
    /****************************************************************/

    ///<summary>
    ///씬 이동 강제 집행한다. 안전장치로서 존재한다.
    ///</summary>
    IEnumerator IE_Move_To_Scene_Enforce(int SceneNumber)
    {
        yield return null;

        int inst_Count =
            Get_Scene_BuildSetting_Count();
        SceneNumber =
            Get_Number_Q(SceneNumber, inst_Count);
        AsyncOperation  asyncOperation =
            SceneManager.LoadSceneAsync(SceneNumber);
        asyncOperation.allowSceneActivation = false;

        bool b_Ready = false;

        while (!b_Ready)
        {
            yield return null;

            float f_Inst_Progress = asyncOperation.progress;
            image_Loading_BAR.fillAmount = f_Inst_Progress;
            if (f_Inst_Progress > 0.87f)
            { b_Ready = true; }

        }/**while (!b_Ready)**/

        yield return new WaitForSeconds(deltaTime);
        image_Loading_BAR.fillAmount = 1.0f;
        yield return new WaitForSeconds(deltaTime);

        asyncOperation.allowSceneActivation = true;

    }/**IEnumerator IE_Move_To_Scene_Enforce(int SceneNumber)**/


    ///<summary>
    ///다음 씬으로 이동할 준비를 한다. 준비 완료시 모두에게 카운트를 보내준다.
    ///</summary>
    IEnumerator IE_Load_Scene_Next_And_Send_Signal()
    {
        yield return null;

        int inst_Count =
            Get_Scene_BuildSetting_Count();
        int inst_N = Get_Number_Scene_Current() + 1;
        inst_N = Get_Number_Q(inst_N, inst_Count);

        AO_Load_Scene_Next =
            SceneManager.LoadSceneAsync(inst_N);
        AO_Load_Scene_Next.allowSceneActivation = false;

        bool b_Ready = false;

        while (!b_Ready)
        {
            yield return new WaitForSeconds(deltaTime);

            float f_Inst_Progress = AO_Load_Scene_Next.progress;

            image_Loading_BAR.fillAmount = f_Inst_Progress;

            if (f_Inst_Progress > 0.86f)
            {
                b_Ready = true;
            }

        }/**while (!b_Ready)**/

        yield return new WaitForSeconds(deltaTime);
        image_Loading_BAR.fillAmount = 1.0f;
        yield return new WaitForSeconds(deltaTime);

        ///로딩 완료했다는 카운트를 보내주는 역할만 하므로 Via 를 사용할 필요가 없다.
        photonView_This.RPC(
            "RPC_Add_Loaded_Count_To_All",
                PhotonTargets.All, null);

    }/**IEnumerator IE_Load_Scene_Next_And_Send_Signal()**/

    /****************************************************************/
    /****************************************************************/

    [PunRPC]
    void RPC_Add_Loaded_Count_To_All()
    {
        i_Loaded_Count++;

    }/**void RPC_Add_Loaded_Count_To_All()**/

    [PunRPC]
    void RPC_Order_Allow_Move_Scene_Next()
    {
        CancelInvoke();
        StopAllCoroutines();

        AO_Load_Scene_Next.allowSceneActivation = true;

    }/**void RPC_Order_Allow_Move_Scene_Next()**/

    [PunRPC]

    void RPC_Order_Load_Scene_Next_And_Send_Signal()
    {
        CancelInvoke();

        StartCoroutine(IE_Load_Scene_Next_And_Send_Signal());
        InvokeRepeating("Invoke_R_89", deltaTime, deltaTime);

    }/**void RPC_Order_Load_Scene_Next_And_Send_Signal()**/







}/**public class Photon_Scene_B4 : MonoBehaviour**/