using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//미니맵 각도계산에 참조하기위한 플레이어의 x,z
public class PlayerInfo
{

    Vector3 _playerVector;


    public Vector3 playerVector
    {
        get
        {
            return _playerVector;
        }
        set
        {
            _playerVector = value;
        }
    }

}
// 테스트는 NavMeshAgent 비활성
public class PlayerMoveCtrl : MonoBehaviour
{
    GameObject actionBtn;
    Text btnText;

    // CharacterController 컴포넌트를 위한 레퍼런스
    CharacterController controller;
    Transform myTr;
    Ray ray;
    RaycastHit hitInfo;
    // 중력 
    public float gravity = 20.0f;

    // 케릭터 이동속도
    public float movSpeed = 5.0f;
    // 케릭터 회전속도
    public float rotSpeed = 120.0f;
    //케릭터 점프 속도
    public float jumpSpeed = 10.0f;

    PhotonView pv;
    //위치 정보를 송수신할 때 사용할 변수 선언 및 초기값 설정 
    Vector3 currPos = Vector3.zero;
    Quaternion currRot = Quaternion.identity;


    // 케릭터 이동 방향
    public Vector3 moveDirection;
    public PlayerInfo playerInfo = new PlayerInfo();
    Inventory inven;

    public Sprite[] imgList;

    Transform PlayerBody;

    // 캐릭터 체력 구현 작업

    public int currHP;
    public int hp;

    public int maxLife = 100;

    public Image lifeBar;

    public Animator anim;

    int deadCount;

    void Awake()
    {
        // 레퍼런스 연결
        myTr = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();
        pv = GetComponent<PhotonView>();
        PlayerBody = transform.Find("Body");
        pv.ObservedComponents[0] = this;
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        currPos = myTr.position;
        currRot = PlayerBody.rotation;

        //액션버튼


        actionBtn = GameObject.FindGameObjectWithTag("actionBtn");
        //테스트용 버튼텍스트
        //이곳에서 NULL 발생합니다.




        //btnText = actionBtn.transform.Find("Text").GetComponent<Text>();

        inven = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        if (!pv.isMine)
        {
            this.tag = "TeamPlayer";
        }
        else
        {
            Camera.main.GetComponent<smoothFollowCam>().target = this.transform;
        }


        //lifeBar = GameObject.Find("HpBar").GetComponent<Image>();

        hp = maxLife;

        anim = GetComponent<Animator>();

    }

    void Update()
    {
        if (Inventory.inventoryActivated)
            return;
        //사용자 자신이 조작할때만 움직임, 다른유저의 조작에 간섭X
        if (pv.isMine)
        {

            float v = Input.GetAxis("Vertical") + UltimateJoystick.GetVerticalAxis("Test");
            float h = Input.GetAxis("Horizontal") + UltimateJoystick.GetHorizontalAxis("Test");

            


            SetMoveDriection(h, v);
            pv.RPC("SetMoveDriection", PhotonTargets.Others, h, v);

            if ((Mathf.Abs(playerInfo.playerVector.x) + Mathf.Abs(playerInfo.playerVector.z)) > 0)
            {
                float ang = Mathf.Atan2(playerInfo.playerVector.z, playerInfo.playerVector.x) * Mathf.Rad2Deg *-1f;
                PlayerBody.rotation = Quaternion.Euler(0, ang + 135f, 0);
            }

            if (controller.isGrounded)
            {

                // 만약 콜라이더가 땅에 있을 경우 
                //디바이스마다 일정한 회전 속도
                float amtRot = rotSpeed * Time.deltaTime;
                //인풋입력 키보드+조이스틱


                //오브젝트를 회전
                //transform.Rotate(Vector3.up * ang * amtRot);

                //Vector3 test = moveDirection.normalized;
                //Debug.Log(Mathf.Atan2(test.z, test.x) * Mathf.Rad2Deg );

                // transform.TransformDirection 함수는 인자로 전달된 벡터를 
                // 월드좌표계 기준으로 변환하여 변환된 벡터를 반환해 준다.
                //즉, 로컬좌표계 기준의 방향벡터를 > 월드좌표계 기준의 방향벡터로 변환

                moveDirection = transform.TransformDirection(moveDirection);
                
                ////키보드가 점프 입력일 경우
                //if (Input.GetButton("Jump"))
                //{
                //    // jumpSpeed 만큼 케릭을 이동
                //    moveDirection.y = jumpSpeed;
                //}
            }
            Quaternion rot = Quaternion.Euler(0, 45f, 0f);//실제 맵은 쿼터뷰로 45도각도로 기울어져있으므로 방향을 45도돌려서계산

            moveDirection.y -= gravity * Time.deltaTime;// 디바이스마다 일정 속도로 케릭에 중력 적용
            // CharacterController의 Move 함수에 방향과 크기의 벡터값을 적용(디바이스마다 일정)
            controller.Move(rot * moveDirection * Time.deltaTime);




            if (hp <= 0)
            {



                StartCoroutine(PlayerDie());
                
            }


        }
        //원격플레이어일때 수행
        else
        {
            myTr.position = Vector3.Lerp(myTr.position, currPos, Time.deltaTime * 3.0f);
            PlayerBody.rotation = Quaternion.Slerp(PlayerBody.rotation, currRot, Time.deltaTime * 3.0f);

            hp = currHP;            
        }


        // 스프라이트 방식으로 조절할때는 이렇게
        //Vector3 tempScale = lifeBar.transform.localScale;
        //tempScale.x = (float)hp / (float)maxLife;
        //lifeBar.transform.localScale = tempScale;

        lifeBar.fillAmount = (float)hp / (float)maxLife;


        //생명력 수치에 따라 Filled 이미지의 색상을 변경 
        if (lifeBar.fillAmount <= 0.4f)
        {
            lifeBar.color = Color.red;
        }
        else if (lifeBar.fillAmount <= 0.6f)
        {
            lifeBar.color = Color.yellow;
        }
        else if (lifeBar.fillAmount <= 0.9f)
        {
            lifeBar.color = Color.green;
        }



       


    }
    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        StageManager.instance.PlayerListAdd(this.gameObject);
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //로컬 플레이어의 위치 정보를 송신
        if (stream.isWriting)
        {
            //박싱
            stream.SendNext(myTr.position);
            stream.SendNext(PlayerBody.rotation);

            stream.SendNext(hp);
        }
        //원격 플레이어의 위치 정보를 수신
        else
        {
            //언박싱
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();

            currHP = (int)stream.ReceiveNext();
        }

    }

    [PunRPC]
    void SetMoveDriection(float h, float v)
    {
        moveDirection = new Vector3(h * movSpeed, 0, v * movSpeed);
        playerInfo.playerVector = moveDirection.normalized;

        if (h != 0 || v  != 0)
        {
            anim.SetBool("Move", true);
        }
        else
        {
            anim.SetBool("Move", false);
        }


    }
    public void btnSet(GameObject go)
    {
        Button btn = actionBtn.GetComponent<Button>();

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(delegate { OnClickButton(go); });


        if (go.tag == "Item")
        {
            ItemInfo itemInfo = go.GetComponent<ItemInfo>();

            // btnText.text = itemInfo.item.name + " 선택";

            if (itemInfo.item.name == "Tree")
            {
                actionBtn.GetComponent<Image>().sprite = imgList[2];
            }
            else if (itemInfo.item.name == "Rock")
            {
                actionBtn.GetComponent<Image>().sprite = imgList[1];
            }
        }
        else
        {
            // btnText.text = "땅누름";
            actionBtn.GetComponent<Image>().sprite = imgList[0];
        }
    }
        void OnClickButton(GameObject go)
    {
        if (Inventory.inventoryActivated)
            return;

        Button btn = actionBtn.GetComponent<Button>();

        if (go.tag == "Item")
        {
            if (!inven.IsInvenFull())
            {
                Item item = go.GetComponent<ItemInfo>().item;
                inven.AcquireItem(item);
                //소유권을 가져온후 오브젝트 삭제
                go.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player.ID);
                PhotonNetwork.Destroy(go);
                btn.onClick.RemoveAllListeners();
                actionBtn.GetComponent<Image>().sprite = imgList[0];
                //SelectObjectRay so = GameObject.FindGameObjectWithTag("selectObject").GetComponent<SelectObjectRay>();
                //so.SelectObjectDestroy();//오브젝트가 파괴됬음을 알림
            }
            else
                Debug.Log("인벤토리 꽉참");
        }

    }
    //void OnTriggerEnter(Collider col)
    //{
    //    //아이템 획득 테스트용
    //    if(col.tag =="Item" && pv.isMine)
    //    {
    //        Item item = col.GetComponent<ItemInfo>().item;
    //        inven.AcquireItem(item);
    //        Destroy(col.gameObject);
    //    }
    //}



    private void OnTriggerEnter(Collider other)
    {
        if ( pv.isMine)
        {
            if ( other.tag == "EnemyWeapon")
            {
                int monsterDMG = other.gameObject.GetComponent<EnemyAttackPoint>().power;

                hp -= monsterDMG;                
            }
        }



        if ( hp <= 0)
        {
            hp = 0;
        }


    }



    IEnumerator PlayerDie()
    {

        if (deadCount > 0)
        {
            yield break;
        }

        deadCount++;

        if ( pv.isMine)
        {            


            movSpeed = 0;
            anim.SetTrigger("Die");
            yield return new WaitForSeconds(5.0f);
            anim.SetTrigger("Heal");
            

            hp = maxLife;
            lifeBar.color = Color.green;
            movSpeed = 5;

            deadCount = 0;
        }
        else
        {           
            lifeBar.color = Color.green;
            anim.SetTrigger("Die");
            yield return new WaitForSeconds(5.0f);
            anim.SetTrigger("Heal");
            deadCount = 0;

        }
       

    }







}


/* 만약 CharacterController를 안쓴다면...

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerCtrl_1 : MonoBehaviour {

    // 이동 관련 변수
    private float ang = 0.0f;
    private float ver = 0.0f;
    
    // 자기 자신의 트렌스폼을 가리킴 
    private Transform myTr;
    //이동 속도 변수 
    public float moveSpeed = 10.0f;
    
    //회전 속도 변수
    public float rotSpeed = 100.0f;
         

    
    void Awake () {
        
        //자기 자신의 Transform 컴포넌트 할당
        myTr = GetComponent<Transform>();

    }
    
    void Update () {
        ang = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");
        
        //Debug.Log("ang = " + h.ToString());
        //Debug.Log("ver = " + v.ToString());
        
        //전후좌우 이동 방향 벡터 계산
        Vector3 moveDir = (Vector3.forward * ver) + (Vector3.right * ang);
        
        //Translate(이동 방향 * Time.deltaTime * 변위값 * 속도, 기준좌표)
        tr.Translate(moveDir * Time.deltaTime * moveSpeed, Space.Self);
        
        //Vector3.up 축을 기준으로 rotSpeed만큼의 속도로 회전 (마우스를 이용한 회전)
        //자동 게임에선 Input.GetAxis("Mouse X") 이거 대신 적 케릭터 위치값 넣으면 된다.
        tr.Rotate(Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis("Mouse X"));
        
    }
    

 
}
 */
