using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public static TimeController instance;
    public static bool isSaved;

    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Text timeText, timeTextComplete;

    public static float time;
    //private float timeBurn = 1f;
    private float timestampFor1Star;
    private float timestampFor2Star;
    private float timestampFor3Star;
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

    void Start()
    {
        isSaved = false;

        time = BoardController.levelData.time[0];
        timestampFor1Star = BoardController.levelData.time[1];
        timestampFor2Star = BoardController.levelData.time[2];
        timestampFor3Star = BoardController.levelData.time[3];
        instance._SetTimeForSlider(false);

        slider.transform.GetChild(3).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            timestampFor1Star / time * slider.GetComponent<RectTransform>().rect.size.x, 0);
        slider.transform.GetChild(4).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            timestampFor2Star / time * slider.GetComponent<RectTransform>().rect.size.x, 0);
        slider.transform.GetChild(5).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            timestampFor3Star / time * slider.GetComponent<RectTransform>().rect.size.x, 0);
    }

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
                time -= Time.deltaTime;
                slider.value = time;

                if(timestampFor3Star <= time)
                {
                    slider.transform.GetChild(3).GetChild(0).gameObject.SetActive(true);
                    slider.transform.GetChild(4).GetChild(0).gameObject.SetActive(true);
                    slider.transform.GetChild(5).GetChild(0).gameObject.SetActive(true);
                }
                if (timestampFor2Star <= time && time < timestampFor3Star)
                {
                    slider.transform.GetChild(3).GetChild(0).gameObject.SetActive(true);
                    slider.transform.GetChild(4).GetChild(0).gameObject.SetActive(true);
                    slider.transform.GetChild(5).GetChild(0).gameObject.SetActive(false);
                }
                else if (timestampFor1Star <= time && time < timestampFor2Star)
                {
                    slider.transform.GetChild(3).GetChild(0).gameObject.SetActive(true);
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
                if (isSaved)
                {
                    GameplayController.instance._GameOver();
                }
                else
                {
                    GameplayController.instance._TakeTheLastChance();
                }
            }
            _DisplayTime(time);
        }
    }

    void _DisplayTime(float timeToDisplay)
    {
        //timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        timeTextComplete.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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

    public int _NumberOfStarsAchieved()
    {
        int count = 1;
        for (int i = 4; i <= 5; i++)
        {
            if (slider.transform.GetChild(i).GetChild(0).gameObject.activeSelf)
            {
                count++;
            }
        }
        return count;
    }
}