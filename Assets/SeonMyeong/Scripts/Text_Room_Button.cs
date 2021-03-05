using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text_Room_Button : MonoBehaviour
{

    Canvas  canvas;
    RectTransform   RT_Canvas;
    RectTransform   RT_This;

    Text    text_This;

    [SerializeField]Photon_Scene_A2 cs_Photon_Scene_A2;

    bool    b_Initialize_Finished;

    /****************************************************************/
    /****************************************************************/



    /****************************************************************/
    /****************************************************************/

    /****************************************************************/
    /****************************************************************/

    public  void    Initialize(Vector2 Size)
    {
        int inst_Count = 0 ;

        if(canvas == null)
        {canvas = GameObject.Find(
            "Canvas").GetComponent<Canvas>();}
        if(RT_Canvas == null)
        {RT_Canvas = canvas.GetComponent<RectTransform>();}

        if(cs_Photon_Scene_A2 == null)
        {cs_Photon_Scene_A2 = GameObject.Find(
            "Photon_Scene_A2").GetComponent<Photon_Scene_A2>();}
        if(text_This == null)
        {text_This = this.gameObject.GetComponent<Text>();}
        if(RT_This == null)
        {RT_This = this.gameObject.GetComponent<RectTransform>();}

        RT_This.sizeDelta = Size;

        float   f_Inst_Font_Size = Size.y * 0.20f;
        text_This.fontSize = (int)f_Inst_Font_Size;

        if(canvas != null){inst_Count++;}
        if(RT_Canvas != null){inst_Count++;}
        if(cs_Photon_Scene_A2 != null){inst_Count++;}
        if(text_This != null){inst_Count++;}
        if(RT_This != null){inst_Count++;}

        if(inst_Count>4){b_Initialize_Finished = true;}

        if(!b_Initialize_Finished){Initialize(Size);}

    }/**public  void    Initialize(Vector2 Size)**/

    /****************************************************************/
    /****************************************************************/

    public  void    Set_Photon_Scene_A2(Photon_Scene_A2 INSERT)
    {cs_Photon_Scene_A2 = INSERT;}
    public  Photon_Scene_A2 Get_Photon_Scene_A2()
    {return cs_Photon_Scene_A2;}

    public  void    Set_RectTransform_Canvas(RectTransform INSERT)
    {RT_Canvas = INSERT;}
    public  RectTransform   Get_RectTransform_Canvas()
    {return RT_Canvas;}

    public  void    Set_Canvas(Canvas INSERT)
    {canvas = INSERT;}
    public  Canvas  Get_Canvas()
    {return canvas;}

    /****************************************************************/
    /****************************************************************/

    public  bool    Get_Check_Initialize_Finished()
    {return b_Initialize_Finished;}
    public  void    Set_Text_String(string INSERT)
    {
        if(b_Initialize_Finished)
        {text_This.text = INSERT;}
    }/**public  void    Set_Text_String(string INSERT)**/

    public  void    Set_RaycastTarget(bool INSERT)
    {
        if(b_Initialize_Finished)
        {text_This.raycastTarget = INSERT;}
    }/**public  void    Set_RaycastTarget(bool INSERT)**/

    public  void    Set_Color_Text(Color color)
    {
        if(b_Initialize_Finished)
        {text_This.color = color;}
    }/**public  void    Set_Color_Text()**/

    /****************************************************************/
    /****************************************************************/
    public  void    BUTTON_DOWN()
    {
        if(b_Initialize_Finished)
        {
            print(text_This.text.ToString() );

            SingleTon.INSTANCE.Set_RoomName(text_This.text);

            cs_Photon_Scene_A2.BUTTON_ROOM_DOWN();

        }/**if(b_Initialize_Finished)**/
        
    }/**public  void    BUTTON_DOWN()**/



}/**public class Text_Room_Button : MonoBehaviour**/