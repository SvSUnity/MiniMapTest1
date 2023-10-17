using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        //이오브젝트는 씬전환시에 사라지지않게함
        DontDestroyOnLoad(this.gameObject);
    }
}
