using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [Header("LoginPanel")]
    public InputField IDInputField;
    public InputField PassInputField;
    [Header("CreateAccountPanel")]
    public InputField New_IDInputField;
    public InputField New_PassInputField;
    public GameObject CreateAccountPane10bj;
    public GameObject loginFailed;

    string LoginUrl;
    string CreateUrl;

    // Start is called before the first frame update
    void Start()
    {
        LoginUrl = "http://gusdka3.dothome.co.kr/Login.php"; 
        CreateUrl = "http://gusdka3.dothome.co.kr/CreateAccount.php";
        SoundManager.Instance.PlayBGM(0);
    }

    public void LoginBtn()
    {
        StartCoroutine(LoginCo());
    }

    IEnumerator LoginCo()
    {
        Debug.Log(IDInputField.text);
        Debug.Log(PassInputField.text);

        WWWForm form = new WWWForm();
        form.AddField("Input_user", IDInputField.text);
        form.AddField("Input_pass", PassInputField.text);

        // 예)
        // form.AddField("Input_Position", "(0,0,0)");
        // form.AddField("Input_ITem", "검입니다. !");

        WWW webRequest = new WWW(LoginUrl, form);
        yield return webRequest;

        Debug.Log(webRequest.text);
        if(webRequest.text == "success")
        {
            SceneManager.LoadScene("Scene_A0");
        }
        else
        {
            loginFailed.SetActive(true);
        }
    }

    public void OpenCreateAccountBtn()
    {
        CreateAccountPane10bj.SetActive(true);
    }

    public void CreateAccountBtn()
    {
        StartCoroutine(CreateCo());
    }

    IEnumerator CreateCo()
    {
        WWWForm form = new WWWForm();
        form.AddField("Input_user", New_IDInputField.text);
        form.AddField("Input_pass", New_PassInputField.text);
        form.AddField("Input_Info", "안녕하세요 뉴비입니다.");


        WWW webRequest = new WWW(CreateUrl, form);
        yield return webRequest;

        Debug.Log(webRequest.text);

        CreateAccountPane10bj.SetActive(false);
        yield return null;
    }         

}