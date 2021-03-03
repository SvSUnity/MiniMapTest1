using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{

    public Slider slider;

    public float maxLoading;
    public float currentLoading;

    public void SetLoading(float time)
    {
        slider.value = time;
    }

    public void SetMaxHealth(float time)
    {
        slider.maxValue = time;
        slider.value = time;
    }




    // Start is called before the first frame update
    void Start()
    {
        currentLoading = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentLoading += 1 * Time.deltaTime;
        SetLoading(currentLoading);

        if ( slider.maxValue <= currentLoading)
        {
            Destroy(this.gameObject);
        }

    }
}
