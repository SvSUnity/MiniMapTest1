using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[System.Serializable]
public class Craft
{
    public string craftName; // 건설될 건축물 이름
    public GameObject realPrefab; // 실제로 건설될
    public GameObject previewPrefab; // 초록색으로 미리 보여줄
}


public class CraftManual : MonoBehaviour
{
    
    private bool isActivated = false; // 건물생성 창이 열려있는지 확인하는 변수. ( 최초 창은 꺼져있으므로 false )

    [SerializeField]
    private GameObject go_BaseUI; // 열릴 윈도우 창

    public GameObject buildUI; // 모바일 버전 빌드 ui;




    [SerializeField]
    public Craft[]build; //  탭과 연결될? 쫌더 지켜봄


    public Craft[] craft;


    private GameObject buildPreview; // 미리보기 프리팹을 담을 변수
    private GameObject buildPrefab; // 미리보기 확인 후 실제 지어질 프리팹.

    [SerializeField] private LayerMask whatIsGround;


    public float previewRotation;

    public Text test;



    private bool isPreviewActivated = false; // 프리뷰를 보고 있는 상태인지를 확인

   // [ 프리뷰 이동을 위해]

    private RaycastHit hitInfo; // 레이캐스트가 맞은 대상의 정보
    [SerializeField]
    private LayerMask layerMask; // 
    [SerializeField]
    private float range;

    Ray ray; // 레이 정보 저장 구조체

    public GameObject tab1;
    public GameObject tab2;

    Inventory myinven;







    bool isPickedUp = false;



    int pointerID = 0;

    RequireCheck reqCheck;





    public int GetCraftTouchIndex()
    {
        // 터치 중인 손가락 수만큼 돌면서 craft중인 손가락인덱스를 가져옴
        for (int fingerID = 0; fingerID < Input.touchCount; ++fingerID)
        {
            if (EventSystem.current.IsPointerOverGameObject(0) == false)
            {
                return fingerID;
            }
        }

        return 0;
    }













    void Awake()
    {
        myinven = GameObject.FindObjectOfType<Inventory>();
        reqCheck = GetComponent<RequireCheck>();
    }

    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR

        //ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 카메라의 시점으로 마우스 포인터를 바라보는 방향           

        //if (Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated) // 프리뷰를 보고있지 않고, 탭 키를 누르면        
        //{
        //    BuildWindow();
        //}

        //if (isPreviewActivated)
        //{
        //    PreviewPositionUpdate();
        //}


        //if (Input.GetButtonDown("Fire1"))
        //{
        //    Building();
        //}



        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Cancel();
        //}
#endif

#if UNITY_ANDROID


        pointerID = 0;


        // 터치시

        if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(0) == false && (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved))
        {
           // int craftFingerID = GetCraftTouchIndex();

            //ray = Camera.main.ScreenPointToRay(Input.mousePosition);
             ray = Camera.main.ScreenPointToRay(Input.touches[0].position); // 카메라의 시점으로 마우스 포인터를 바라보는 방향   

            PreviewPositionUpdate();




            //switch (Input.GetTouch(craftFingerID).phase)
            //{
            //    case TouchPhase.Moved:
            //    case TouchPhase.Began:
            //        if (isPreviewActivated)
            //        {
            //            PreviewPositionUpdate();
            //        }
            //        break;

            //    case TouchPhase.Ended:
            //        break;
            //    default:
            //        break;
            //}



        }




        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //{
        //    if (EventSystem.current.IsPointerOverGameObject(0) == false)
        //    {
        //        ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 카메라의 시점으로 마우스 포인터를 바라보는 방향    

        //        if (isPreviewActivated)
        //        {
        //            PreviewPositionUpdate();
        //        }


        //        //if (isPreviewActivated)
        //        //{
        //        //    buildUI.SetActive(true);
        //        //}
        //        //else
        //        //{
        //        //    buildUI.SetActive(false);

        //        //}




        //    }
        //}


        if (isPreviewActivated)
        {
            buildUI.SetActive(true);
        }
        else
        {
            buildUI.SetActive(false);

        }
        //ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 카메라의 시점으로 마우스 포인터를 바라보는 방향    

        //if (isPreviewActivated && buildUI.activeSelf == false)
        //{
        //    PreviewPositionUpdate();
        //}






        //if (isPreviewActivated)
        //{
        //    buildUI.SetActive(true);
        //}
        //else
        //{
        //    buildUI.SetActive(false);

        //}





#endif

    }


    private void BuildWindow()
    {
        if ( !isActivated)
        {
            OpenBuildWindow();
        }
        else
        {
           Cancel();           
        }
    }

    private void OpenBuildWindow()
    {
        isActivated = true; // 이젠 창이 열려있다고 인식
        go_BaseUI.SetActive(true); 
    }

    private void CloseBuildWindow()
    {
        isActivated = false;
        go_BaseUI.SetActive(false);
    }


   


    public void SlotClick(int slotNum)
    {
       
        buildPreview = Instantiate(build[slotNum].previewPrefab, Vector3.zero, Quaternion.Euler(0, previewRotation, 0));

        buildPrefab = build[slotNum].realPrefab; 

        isPreviewActivated = true;
        go_BaseUI.SetActive(false);

        isPickedUp = true;

    }

    public void CraftSlotClick(int slotNum)
    {

        buildPreview = Instantiate(craft[slotNum].previewPrefab, Vector3.zero, Quaternion.identity);

        buildPrefab = craft[slotNum].realPrefab;

        isPreviewActivated = true;
        go_BaseUI.SetActive(false);

        isPickedUp = true;

    }





    public void Tab1Click()
    {
        if ( tab1.activeSelf == true )
        {
            tab1.SetActive(false);
        }
        else
        {
            tab1.SetActive(true);
            tab2.SetActive(false);
        }
       
    }

    public void Tab2Click()
    {
        if (tab2.activeSelf == true)
        {
            tab2.SetActive(false);
        }
        else
        {
            tab1.SetActive(false);
            tab2.SetActive(true);
        }
      
    }

    private void Cancel() // esc를 눌러 프리뷰를 보는걸 취소할때
    {
        if(isPreviewActivated)
        {
            Destroy(buildPreview);
        }

        isActivated = false;
        isPreviewActivated = false;

        buildPreview = null;

        buildPrefab = null;


        go_BaseUI.SetActive(false);

    }



    void PreviewPositionUpdate()
    {
                  


        if ( Physics.Raycast(ray , out hitInfo , range,whatIsGround))
        {
           // if  (hitInfo.collider.tag != null)
            //if   (hitInfo.collider.tag == "Ground" || )
             //  (hitInfo.collider.tag == "Ground" || hitInfo.collider.tag == "Player" || hitInfo.collider.tag == "CubeTop" || hitInfo.collider.tag == "555")
            //{
               // Vector3 PreviewPos = hitInfo.point;

                Vector3 PreviewPos = new Vector3((int)hitInfo.point.x, (int)hitInfo.point.y, (int)hitInfo.point.z);

                buildPreview.SetActive(true);


                buildPreview.transform.position = PreviewPos;

            //}






            //if (Input.GetButtonDown("Fire2"))
            //{
            //    previewRotation += 90f;
            //}
            //buildPreview.transform.rotation = Quaternion.Euler(0f, previewRotation, 0f);



        }
        else
        {
            buildPreview.SetActive(false);


        }
    }



    void Building()
    {
        if ( isPreviewActivated && buildPreview.GetComponent<PreviewObject>().isBuildable())
        {
            //건설필요재료 정보
            Requirement req = buildPreview.GetComponent<Requirement>();

            if (!reqCheck.Check(req))
                return;


            if (buildPreview.tag == "Wall")
            {
                Vector3 RatioBuild = new Vector3((int)buildPreview.transform.position.x, (int)buildPreview.transform.position.y,(int)buildPreview.transform.position.z);

                PhotonNetwork.Instantiate("Wall", RatioBuild, Quaternion.Euler(0, previewRotation, 0), 0, null);
               
              //  PhotonNetwork.Instantiate("CubeTop", buildPreview.transform.position, Quaternion.identity, 0, null);

            }

            if (buildPreview.tag == "RockWall")
            {
                PhotonNetwork.Instantiate("RockWall", buildPreview.transform.position, Quaternion.identity, 0, null);
                               
            }

            if (buildPreview.tag == "TopPre")
            {

                Vector3 RatioBuild = new Vector3((int)buildPreview.transform.position.x, (int)buildPreview.transform.position.y, (int)buildPreview.transform.position.z);
                PhotonNetwork.Instantiate("Top", RatioBuild, Quaternion.Euler(0, previewRotation, 0), 0, null);

            }

            Destroy(buildPreview);
            isActivated = false;
            isPreviewActivated = false;
            buildPreview = null;
            buildPrefab = null;
        }



    }





    public void BuildButtonClick()
    {
        if (!isPreviewActivated)
        {
            BuildWindow();
        }
    }





    public void BuildBtn()
    {
        Building();
    }

    public void RotaBtn()
    {



        previewRotation += 90f;     
        buildPreview.transform.rotation = Quaternion.Euler(0f, previewRotation, 0f);

        

        PreviewPositionUpdate();



    }

    public void CancelBtn()
    {
        Cancel();
    }
    //private void OnMouseUp()
    //{
    //    test.text = "집 가고 싶다";
    //}



    void Test()
    {

        if ( Input.touchCount == 0)
        {

        }

        if (Physics.Raycast(ray, out hitInfo, range, whatIsGround))
        {
            // if  (hitInfo.collider.tag != null)
            //if   (hitInfo.collider.tag == "Ground" || )
            //  (hitInfo.collider.tag == "Ground" || hitInfo.collider.tag == "Player" || hitInfo.collider.tag == "CubeTop" || hitInfo.collider.tag == "555")
            //{
            // Vector3 PreviewPos = hitInfo.point;

            Vector3 PreviewPos = new Vector3((int)hitInfo.point.x, (int)hitInfo.point.y, (int)hitInfo.point.z);

            buildPreview.SetActive(true);
            buildPreview.transform.position = PreviewPos;

            //}

            if (Input.GetButtonDown("Fire2"))
            {
                previewRotation += 90f;
            }

            buildPreview.transform.rotation = Quaternion.Euler(0f, previewRotation, 0f);
            


        }
        else
        {
            buildPreview.SetActive(false);


        }
    }









}
