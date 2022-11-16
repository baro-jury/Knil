using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TimeDemo : MonoBehaviour
{
    public static TimeDemo instance;

    [SerializeField]
    private AudioClip timeWarning;
    public Transform iconClock;
    [SerializeField]
    private Text timeText, timeTextComplete;

    public static float time;
    //private float timeBurn = 1f;
    private float timestampFor1Star;
    private float timestampFor2Star;
    private float timestampFor3Star;
    private bool isFreezed, muchTimeLeft;

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
        muchTimeLeft = true;

        time = BoardDemo.levelData.Time[0];
        timestampFor1Star = BoardDemo.levelData.Time[1];
        timestampFor2Star = BoardDemo.levelData.Time[2];
        timestampFor3Star = BoardDemo.levelData.Time[3];
    }

    void Update()
    {
        if (!isFreezed)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;

                if (muchTimeLeft && time <= 15)
                {
                    muchTimeLeft = false;
                    PlayerPrefsDemo.instance.audioSource.PlayOneShot(timeWarning);

                    iconClock.DOScale(new Vector3(1.2f, 1.2f, 1), 0.5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
                    iconClock.DORotate(new Vector3(0, 0, -20), 0.5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
                    iconClock.GetComponent<Image>().DOColor(Color.red, 0.5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
                }
            }
            else
            {
                muchTimeLeft = true;
                foreach (var tile in BoardDemo.buttonList)
                {
                    DOTween.Kill(tile);
                    tile.localScale = Vector3.one;
                }
                LineDemo.instance._EraseLine();
                time = 0;
                GameplayDemo.instance._TimeOut();
            }
            _DisplayTime(time);
        }
    }

    void _DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        timeTextComplete.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void _FreezeTime(bool freeze)
    {
        isFreezed = freeze;
    }
}
