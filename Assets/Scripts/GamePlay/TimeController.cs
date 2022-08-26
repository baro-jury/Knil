using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public static TimeController instance;

    [SerializeField]
    private Slider slider;
    public static float time;
    private float timeBurn = 1f;
    private bool isFreezed;

    void _MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Awake()
    {
        _MakeInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFreezed)
        {
            slider.value = time;
        }
        else
        {
            if (time > 0)
            {
                time -= timeBurn * Time.deltaTime;
                slider.value = time;
            }
            else
            {
                GameplayController.instance._GameOver();
            }
        }
    }

    public void _SetTimeForSlider(bool isBoosted)
    {
        if (isBoosted)
        {
            time += 10;
        }
        slider.minValue = 0f;
        slider.maxValue = time;
        slider.value = slider.maxValue;
    }

    public void _FreezeTime(bool freeze)
    {
        isFreezed = freeze;
    }
}
