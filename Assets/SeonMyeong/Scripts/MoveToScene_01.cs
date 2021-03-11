using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveToScene_01 : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] RectTransform RT_Canvas;

    [SerializeField] Image image_Button_Move_To_Scene_01;

    [SerializeField] StageManager2 cs_StageManager2;


    Vector3 v3_Canvas_Size;
    Vector2 v2_Canvas_Size;

    /****************************************************************/
    /****************************************************************/

    private void Awake()
    {
        if (canvas == null)
        { canvas = GameObject.Find("Canvas").GetComponent<Canvas>(); }
        if (RT_Canvas == null)
        { RT_Canvas = canvas.GetComponent<RectTransform>(); }
        
    }/**private void Awake()**/

    /****************************************************************/
    /****************************************************************/

    private void Start()
    {
        v2_Canvas_Size = RT_Canvas.sizeDelta;
        v3_Canvas_Size = Get_XYZ_from_XY(v2_Canvas_Size);

        Align_image_Button_Move_To_Scene_01();

        image_Button_Move_To_Scene_01.raycastTarget = true;

    }/**private void Start()**/

    void    Align_image_Button_Move_To_Scene_01()
    {
        

    }/**void    Align_image_Button_Move_To_Scene_01()**/


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

    IEnumerator IE_Move_To_Scene_01()
    {
        yield return null;

        int inst_Scene_Count =
            Get_Scene_BuildSetting_Count();
        int inst_Scene_01 =
            Get_Number_Q(1, inst_Scene_Count);
        AsyncOperation asyncOperation =
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(inst_Scene_01);
        asyncOperation.allowSceneActivation = false;

        bool b_Ready = false;

        while (!b_Ready)
        {
            yield return new WaitForSeconds(0.05f);

            float f_Progress =
                asyncOperation.progress;
            if (f_Progress > 0.87f)
            { b_Ready = true; }

        }/**while (!b_Ready)**/
        yield return new WaitForSeconds(0.5f);
        cs_StageManager2.enabled = false;
        yield return new WaitForSeconds(0.5f);
        PhotonNetwork.LeaveRoom();
        asyncOperation.allowSceneActivation = true;

    }/**IEnumerator IE_Move_To_Scene_01()**/

    /****************************************************************/
    /****************************************************************/

    public void Button_Move_To_Scene_01_DOWN()
    {
        print("Button_Move_To_Scene_01_DOWN");
        image_Button_Move_To_Scene_01.raycastTarget = false;
        image_Button_Move_To_Scene_01.enabled = false;
        StartCoroutine(IE_Move_To_Scene_01());

    }/**public void Button_Move_To_Scene_01_DOWN()**/


}/**public class MoveToScene_01 : MonoBehaviour**/