using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    Vector3 startPos;

    public float waveSpeed;
    public float wavePower;

    private void Awake()
    {
        startPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = gameObject.transform.position;

        float theta = startPos.x + Time.time;
        float sin = (wavePower) * Mathf.Sin(theta);
        float cos = (waveSpeed) * Mathf.Cos(theta);

        pos.x = startPos.x + cos;
        pos.z = startPos.z + sin;
        gameObject.transform.position = pos;



    }
}
