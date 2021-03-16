using UnityEngine;
using UnityEngine.UI;

// 알림 뷰의 표시 옵션을 지정하기 위한 클래스
public class AlertViewOptions
{
	// 취소 버튼의 타이틀
	public string cancelButtonTitle;
	// 취소 버튼 눌렀을 때 실행되는 대리자
	public System.Action cancelButtonDelegate;
	// OK 버튼의 타이틀
	public string okButtonTitle;
	// OK 버튼을 눌렀을 때 실행되는 대리자
	public System.Action okButtonDelegate;
}

public class AlertViewController : MonoBehaviour
{
	// 타이틀을 표시
	public Text titleLabel;
	// 메세지를 표시
	public Text messageLabel;
	// 취소버튼
	public Button cancelButton;
	// OK 버튼
	public Button okButton;
	// 취소 버튼의 타이틀
	public Text cancelButtonLabel;
	// OK 버튼의 타이틀
	public Text okButtonLabel;

	// 이지트윈
	public EasyTween easyTween;

	private static GameObject prefab;
	// 취소 버튼 클릭시 실행되는 대리자 저장
	private System.Action cancelButtonDelegate;
	// OK 버튼 클릭시 실행되는 대리자 저장
	private System.Action okButtonDelegate;

	// 알림 뷰를 표시하는 static 메서드
	public static AlertViewController Show(
		string title, string message, AlertViewOptions options = null)
	{
		if (prefab == null)
		{
			// 프리팹을 읽어 들인다
			// animation
			prefab = Resources.Load("AlertView") as GameObject;

			// basic
			// prefab = Resources.Load ("AlertViewBasic") as GameObject;
		}

		GameObject go = Instantiate(prefab) as GameObject;
		AlertViewController alertView = go.GetComponent<AlertViewController>();
		alertView.UpdateContent(title, message, options);


		if (alertView.easyTween != null)
		{
			alertView.easyTween.OpenCloseObjectAnimation();
		}

		return alertView;
	}

	// 알림뷰의 내용을 갱신하는 메서드
	public void UpdateContent(
		string title, string message, AlertViewOptions options = null)
	{
		// 타이틑과 메세지를 설정
		titleLabel.text = title;
		messageLabel.text = message;

		if (options != null)
		{
			// 표시 옵션이 있을 때 옵션의 내용에 맞춰 버튼을 표시하거나 표시하지 않는다
			cancelButton.transform.gameObject.SetActive(
				options.cancelButtonTitle != null || options.okButtonTitle != null);

			cancelButton.gameObject.SetActive(options.cancelButtonTitle != null);
			cancelButtonLabel.text = options.cancelButtonTitle ?? "";
			cancelButtonDelegate = options.cancelButtonDelegate;

			okButton.gameObject.SetActive(options.okButtonTitle != null);
			okButtonLabel.text = options.okButtonTitle ?? "";
			okButtonDelegate = options.okButtonDelegate;
		}
		else
		{
			// 표시 옵션이 지정돼 있지 않을 때 기본 버튼을 표시한다.
			cancelButton.gameObject.SetActive(false);
			okButton.gameObject.SetActive(true);
			okButtonLabel.text = "OK";
		}
	}

	// 알림창을 닫는 메서드 (Hierarchy 객체 사라짐)
	public void Dismiss()
	{
		gameObject.SetActive(false);
		//Destroy(gameObject);              // 주석풀면 Hierarchy 객체 사라짐 (gameObject.SetActive(false);지우기)
	}

	// 취소 버튼을 눌렀을 때 호출되는 메서드
	public void OnPressCancelButton()
	{
		if (cancelButtonDelegate != null)
		{
			cancelButtonDelegate.Invoke();
			Debug.Log("취소");
		}

		if (easyTween != null)
		{
			easyTween.OpenCloseObjectAnimation();
			Invoke("Dismiss", 2f);
			Debug.Log("취소");
		}
		else
		{
			// Basic
			Dismiss();
		}

	}

	// OK 버튼을 눌렀을 때 호출되는 메서드
	public void OnPressOKButton()
	{
		if (okButtonDelegate != null)
		{
			okButtonDelegate.Invoke();
			Debug.Log("바바이~");
		}
		// Dismiss();
		if (easyTween != null)
		{
			easyTween.OpenCloseObjectAnimation();
			Invoke("Dismiss", 2f);
			Debug.Log("바바이~");
		}
		else
		{
			// Basic
			Dismiss();
		}
	}

	public void OnApplicationQuit()
	{

		Debug.Log("게임종료");



//#if unity_editor
//		unityeditor.editorapplication.isplaying = false; // play모드를 false
//#elif unity_webplayer
//	    application.openurl("http://google.com"); // 구글웹으로 전환
//#else
//		application.quit(); // 어플리케이션 종료
//	}
//#endif
	}
}
