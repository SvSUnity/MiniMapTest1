using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PreviewObject : MonoBehaviour 
{

    public List<Collider> colliderList = new List<Collider>(); // 충돌한 콜라이더들을 담을 리스트. ( 하나라도 담긴다면 건설 불가능 하게 )

    [SerializeField]
    private int layerGround; // 충돌해도 괜찮은 지상 Ground 레이어를 별도로 설정
    private const int IGNORE_RAYCAST_LAYER = 2; // 이그노어 레이캐스트 레이어

    [SerializeField]
    private Material greenMaterial;
    [SerializeField]
    private Material redMaterial;

    GameObject previewUI;

    void Awake()
    {

    }



    // Update is called once per frame
    void Update()
    {
        ChangeColor();


        //if(Input.GetMouseButtonDown(0))
        //{
        //    previewUI.SetActive(false);
        //}
        //else if(Input.GetMouseButtonUp(0))
        //{
        //    previewUI.SetActive(true);
        //}
    }


    private void OnTriggerEnter(Collider other)
    {
        if ( other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        //if (other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {
            colliderList.Add(other);
        }
       
    }



    private void OnTriggerExit(Collider other)
    {
         if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)

       // if (other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {
            colliderList.Remove(other);
        } 
       
    }


    void ChangeColor()
    {
        if ( colliderList.Count > 0) // 땅이나 무시 레이어가 아닌 충돌체가 0보다 크면
        {
            SetColor(redMaterial);
        }
        else
        {
            SetColor(greenMaterial);
        }

    }


    private void SetColor (Material mat)
    {
        //foreach(Transform Child in this.transform) // 프리뷰 자기 자신과 자식계층으로 포함된 오브젝트들의 트랜스폼들까지 반복.( 바로 밑의 자식만 )
        foreach (Renderer Child in GetComponentsInChildren<Renderer>()) // 자식의 자식 계층까지 모두 포함.

        {


            var newMaterials = new Material[Child.GetComponent<Renderer>().materials.Length]; // 각 Child의 배열의 크기는 메쉬 렌더러 안에 있는 Materials의 길이 (size) 


            for (int i = 0; i < newMaterials.Length; i++) // Materials의 길이 만큼 반복.
            {
                newMaterials[i] = mat; // 각 메테리얼을 SetColor에서 받은 메테리얼로 변경.
            }

            Child.GetComponent<Renderer>().materials = newMaterials;   // 그렇게 바뀐 메테리얼을 Child 메테리얼에 저장


            //var newMaterials = new Material[Child.GetComponent<Renderer>().materials.Length]; // 각 Child의 배열의 크기는 메쉬 렌더러 안에 있는 Materials의 길이 (size) 


            //for ( int i = 0; i < newMaterials.Length; i++) // Materials의 길이 만큼 반복.
            //{
            //    newMaterials[i] = mat; // 각 메테리얼을 SetColor에서 받은 메테리얼로 변경.
            //}

            //Child.GetComponent<Renderer>().materials = newMaterials;   // 그렇게 바뀐 메테리얼을 Child 메테리얼에 저장
        }
    }


    public bool isBuildable()
    {
                 
        return colliderList.Count == 0 ; // 콜라이더 리스트에 아무것도 없을때만 true 값 반환 ( 건설 가능 )
    }

    









}
