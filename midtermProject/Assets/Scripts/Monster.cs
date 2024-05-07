using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public float atbTime;
    private float nowTime;

    public Slider atbSlider;

    private void Start()
    {
        nowTime = 0f;
    }

    private void Update()
    {
        nowTime += Time.deltaTime;
        atbSlider.value = nowTime / atbTime;

        if (nowTime >= atbTime)
        {
            Debug.Log("Monster's Attack!");
            nowTime = 0f;
        }
    }
}
