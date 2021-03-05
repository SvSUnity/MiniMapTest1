using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleTon
{
    private static SingleTon   singleTon = null;

    public static SingleTon   INSTANCE
    {
        get
        {
            if(singleTon == null)
            {singleTon = new SingleTon();}
            return  singleTon;
        }
    }/**public static SingleTon   INSTANCE**/

    /****************************************************************/
    /****************************************************************/

    Font    font_Arial;
    string  str_GameVersion;
    string  str_NickName;
    string  str_RoomName;
    Vector2 v2_Canvas_Size;
    float   f_Distance_Canvas_Shorter;

    /****************************************************************/
    /****************************************************************/

    public  void    Set_Font_Arial(Font font)
    {font_Arial = font;}
    public  Font    Get_Font_Arial()
    {return font_Arial;}
    public  void    Set_GameVersion(string GameVersion)
    {str_GameVersion = GameVersion;}
    public  string  Get_GameVersion()
    {return str_GameVersion;}
    public  void    Set_NickName(string NickName)
    {str_NickName = NickName;}
    public  string  Get_NickName()
    {return str_NickName;}

    public  void    Set_RoomName(string RoomName)
    {str_RoomName = RoomName;}
    public  string  Get_RoomName()
    {return str_RoomName;}

    public  void    Set_Canvas_Size(Vector2 INSERT)
    {v2_Canvas_Size = INSERT;}
    public  Vector2 Get_Canvas_Size()
    {return v2_Canvas_Size;}

    public  void    Set_Distance_Canvas_Shorter(float INSERT)
    {f_Distance_Canvas_Shorter = INSERT;}
    public  float   Get_Distance_Canvas_Shorter()
    {return f_Distance_Canvas_Shorter;}

    public  SingleTon()
    {
        font_Arial = null;
        
        str_GameVersion = string.Empty;
        str_NickName = string.Empty;
        str_RoomName = string.Empty;

        v2_Canvas_Size = Vector2.zero;

        f_Distance_Canvas_Shorter = 0.0f;

    }/**public  SingleTon()**/

}/**public class SingleTon**/