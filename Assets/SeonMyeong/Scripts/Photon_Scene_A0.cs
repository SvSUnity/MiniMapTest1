using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Photon_Scene_A0 : MonoBehaviour
{
    [SerializeField]InputField  IF_NickName;
    [SerializeField]Image       image_BUTTON_OK;

    Canvas  canvas;
    RectTransform   RT_Canvas;
    RectTransform   RT_IF;

    Vector3 v3_Canvas_Size;
    Vector2 v2_Canvas_Size;
    Vector2 v2_IF_Size;

    float   deltaTime;
    float   f_Distance_Axis_Canvas_Short;

    int     i_Limit_NickName_Length;

    bool    b_INPUT_BUTTON_OK;

    /****************************************************************/
    /****************************************************************/

    void    Awake()
    {
        if(canvas == null)
        {
            canvas = GameObject.Find(
                "Canvas").GetComponent<Canvas>();
        }/**if(canvas == null)**/

        if(RT_Canvas == null)
        {
            RT_Canvas =
                canvas.GetComponent<RectTransform>();
        }/**if(RT_Canvas == null)**/

        if(IF_NickName == null)
        {
            IF_NickName = GameObject.Find(
                "InputField").GetComponent<InputField>();
        }/**if(IF_NickName == null)**/

        if(RT_IF == null)
        {
            RT_IF = IF_NickName.GetComponent<RectTransform>();

        }/**if(RT_IF == null)**/

        if(image_BUTTON_OK == null)
        {
            image_BUTTON_OK = GameObject.Find(
                "Image_BUTTON_OK").GetComponent<Image>();

        }/**if(image_BUTTON_OK == null)**/

    }/**void    Awake()**/

    /****************************************************************/
    /****************************************************************/

    void    Start()
    {

        deltaTime = 0.05f;

        i_Limit_NickName_Length = 12;

        v2_Canvas_Size = RT_Canvas.sizeDelta;
        v3_Canvas_Size = Get_XYZ_from_XY(v2_Canvas_Size);

        f_Distance_Axis_Canvas_Short = v2_Canvas_Size.x;

        if(f_Distance_Axis_Canvas_Short > v2_Canvas_Size.y)
        {f_Distance_Axis_Canvas_Short = v2_Canvas_Size.y;}

        v2_IF_Size.x = f_Distance_Axis_Canvas_Short * 1.0f;
        v2_IF_Size.y = v2_IF_Size.x * 0.25f;

        RT_IF.sizeDelta = v2_IF_Size;


        IF_NickName.textComponent.fontSize = (int)(v2_IF_Size.y * 0.25f);
        IF_NickName.placeholder.GetComponent<Text>().fontSize = (int)(v2_IF_Size.y * 0.25f);

        RT_IF.transform.position = v3_Canvas_Size * 0.5f;

        image_BUTTON_OK.rectTransform.position =
            (v3_Canvas_Size * 0.5f) + (Vector3.down * v2_IF_Size.y);
        image_BUTTON_OK.rectTransform.sizeDelta =
            (Vector2.up * v2_IF_Size.y) + (Vector2.right * v2_IF_Size.y * 4.0f);
        
        image_BUTTON_OK.gameObject.SetActive(false);
        IF_NickName.gameObject.SetActive(false);

        if(SingleTon.INSTANCE.Get_Font_Arial() == null)
        {   SingleTon.INSTANCE.Set_Font_Arial(
                IF_NickName.placeholder.GetComponent<Text>().font); }

        SingleTon.INSTANCE.Set_Canvas_Size(v2_Canvas_Size);
        SingleTon.INSTANCE.Set_Distance_Canvas_Shorter(f_Distance_Axis_Canvas_Short);
        
        SingleTon.INSTANCE.Set_GameVersion("0.1.0");

        Invoke("Invoke_O_99" , 0.25f);

    }/**void    Start()**/

    /****************************************************************/
    /****************************************************************/


    void    Invoke_O_99()
    {
        CancelInvoke("Invoke_O_99");

        int inst_NickName_Length =
            SingleTon.INSTANCE.Get_NickName().Length;

        if(inst_NickName_Length < 1)
        {
            IF_NickName.gameObject.SetActive(true);
            image_BUTTON_OK.gameObject.SetActive(true);
            InvokeRepeating("Invoke_R_98" , deltaTime , deltaTime);

        }/**if(inst_NickName_Length < 1)**/

        if(inst_NickName_Length > 0)
        {
            Invoke("Invoke_O_97" , deltaTime);

        }/**if(inst_NickName_Length > 0)**/

    }/**void    Invoke_O_99()**/

    void    Invoke_R_98()
    {
        string  str_Inst = IF_NickName.text;

        if(str_Inst.Contains(" "))
        {
            str_Inst = string.Empty;
            IF_NickName.text = str_Inst;
            SingleTon.INSTANCE.Set_NickName(str_Inst);

        }/**if(str_Inst.Contains(" "))**/

        if(str_Inst.Length > i_Limit_NickName_Length)
        {
            str_Inst = str_Inst.Remove(
                i_Limit_NickName_Length , str_Inst.Length - i_Limit_NickName_Length);
            IF_NickName.text = str_Inst;
            SingleTon.INSTANCE.Set_NickName(str_Inst);

        }/**if(str_Inst.Length > i_Limit_NickName_Length)**/

        if(str_Inst.Length > 0)
        {
            if(b_INPUT_BUTTON_OK == true)
            {
                CancelInvoke("Invoke_R_98");

                SingleTon.INSTANCE.Set_NickName(str_Inst);
                print("NickName : "+SingleTon.INSTANCE.Get_NickName().ToString());

                IF_NickName.gameObject.SetActive(false);
                image_BUTTON_OK.gameObject.SetActive(false);

                Invoke("Invoke_O_97" , deltaTime);

            }/**if(b_INPUT_BUTTON_OK == true)**/
        }/**if(str_Inst.Length > 0)**/

    }/**void    Invoke_R_98()**/

    void    Invoke_O_97()
    {
        CancelInvoke("Invoke_O_97");
        IF_NickName.text = " ";
        IF_NickName.gameObject.SetActive(true);

        StartCoroutine(IE_Move_Next_Scene() );

    }/**void    Invoke_O_97()**/

    IEnumerator IE_Move_Next_Scene()
    {
        yield return null;

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

            IF_NickName.text = "LOADING : " +
                inst_Progress.ToString("00")+" %";

            yield return new WaitForSeconds(deltaTime);
            //yield return null;
            //print(inst_Progress.ToString("00")+"%");

            if(inst_Progress > 89)
            {   b_Ready_to_Move = true; }

        }/**while(!asyncOperation.isDone)**/

        //StopCoroutine(IE_Move_Next_Scene()  );

        asyncOperation.allowSceneActivation = true;

    }/**IEnumerator IE_Move_Next_Scene()**/

    /****************************************************************/
    /****************************************************************/

    int     Get_Number_Q(int NUMBER , int SIZE)
    {
        if(SIZE < 1){    SIZE = 1;    }
        if(NUMBER < 0){NUMBER = NUMBER + SIZE;}
        if(NUMBER >= SIZE){NUMBER = NUMBER - SIZE;}

        if( (NUMBER < 0) || (NUMBER >= SIZE) )
        {    NUMBER = Get_Number_Q(NUMBER , SIZE);    }

        return    NUMBER;
    }/**int     Get_Number_Q(int NUMBER , int SIZE)**/

    Vector3 Get_XYZ_from_XY(Vector2 INSERT)
    {return  new Vector3(INSERT.x , INSERT.y);}
    Vector2 Get_XY_from_XYZ(Vector3 INSERT)
    {return  new Vector2(INSERT.x , INSERT.y);}

    /****************************************************************/
    /****************************************************************/

    public  void    BUTTON_OK_DOWN()
    {
        b_INPUT_BUTTON_OK = true;
        //print("INPUT_BUTTON_OK : "+b_INPUT_BUTTON_OK);

    }

    public  void    BUTTON_OK_UP()
    {
        b_INPUT_BUTTON_OK = false;
        //print("INPUT_BUTTON_OK : "+b_INPUT_BUTTON_OK);

    }

}/**public class Photon_Scene_A0 : MonoBehaviour**/