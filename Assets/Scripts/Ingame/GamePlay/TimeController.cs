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
    public static float timestampFor1Star;
    public static float timestampFor2Star;
    public static float timestampFor3Star;
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

        time = BoardController.levelData.time[0];
        timestampFor1Star = BoardController.levelData.time[1];
        timestampFor2Star = BoardController.levelData.time[2];
        timestampFor3Star = BoardController.levelData.time[3];
        instance._SetTimeForSlider(false);

        slider.transform.GetChild(3).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            timestampFor1Star / time * slider.gameObject.GetComponent<RectTransform>().sizeDelta.x, 0);
        slider.transform.GetChild(4).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            timestampFor2Star / time * slider.gameObject.GetComponent<RectTransform>().sizeDelta.x, 0);
        slider.transform.GetChild(5).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            timestampFor3Star / time * slider.gameObject.GetComponent<RectTransform>().sizeDelta.x, 0);
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

                    //if (slider.maxValue / 2 <= time && time < 3 * slider.maxValue / 4)
                    //{
                    //    slider.transform.GetChild(5).transform.GetChild(0).gameObject.SetActive(false);
                    //}
                    //else if (slider.maxValue / 4 <= time && time < slider.maxValue / 2)
                    //{
                    //    slider.transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(false);
                    //}
                    //else if (time < slider.maxValue / 4)
                    //{
                    //    slider.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(false);
                    //}

                    if (timestampFor2Star <= time && time < timestampFor3Star)
                    {
                        slider.transform.GetChild(5).GetChild(0).gameObject.SetActive(false);
                    }
                    else if (timestampFor1Star <= time && time < timestampFor2Star)
                    {
                        slider.transform.GetChild(4).GetChild(0).gameObject.SetActive(false);
                    }
                    else if (time < timestampFor1Star)
                    {
                        slider.transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
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
