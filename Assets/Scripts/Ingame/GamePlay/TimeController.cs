using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeController : MonoBehaviour
{
    public static TimeController instance;

    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TextMeshProUGUI timeText;

    public static float time;
    private float timeBurn = 1f;
    private bool isFreezed, timerIsRunning;

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

    void Start()
    {
        timerIsRunning = true;
    }

    void Update()
    {
        if (isFreezed)
        {
            slider.value = time;
        }
        else
        {
            if (timerIsRunning)
            {
                if (time > 0)
                {
                    time -= timeBurn * Time.deltaTime;
                    slider.value = time;

                    if (slider.maxValue / 2 <= time && time < 3 * slider.maxValue / 4)
                    {
                        slider.transform.GetChild(5).transform.GetChild(0).gameObject.SetActive(false);
                    }else if (slider.maxValue / 4 <= time && time < slider.maxValue / 2)
                    {
                        slider.transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(false);
                    }
                    else if(time < slider.maxValue / 4)
                    {
                        slider.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
                else
                {
                    time = 0;
                    GameplayController.instance._GameOver();
                    timerIsRunning = false;
                }
                _DisplayTime(time);
            }
        }
    }

    void _DisplayTime(float timeToDisplay)
    {
        //timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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
