using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class TimeDemo : MonoBehaviour
{
    public static TimeDemo instance;
    public static bool muchTimeLeft, playingGame;
    public float time;
    public Transform iconClock;

    private float oldTime;
    private bool isFreezed = false;
    [SerializeField]
    private Text timeText, timeTextComplete;

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
        playingGame = false;
        _SetTime(BoardDemo.levelData.Time[0], 0);
        _ChangeTime(oldTime);
    }

    void Update()
    {
        if (playingGame && !isFreezed)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                oldTime = time;
                if (muchTimeLeft && time <= 15)
                {
                    muchTimeLeft = false;
                    PlayerPrefsDemo.instance.timeWarningSource.Play();
                    _TweenTimeWarn();
                }
            }
            else if (!muchTimeLeft && time <= 0)
            {
                _SetTime(0, 0);
                muchTimeLeft = true;
                GameplayDemo.instance._ResetTileState();
                //_ResetClockState();
                LineController.instance._EraseLine();
                GameplayDemo.instance._TimeOut();
            }
            _DisplayTime(oldTime);
        }
    }

    public void _SetTime(float t, int plus)
    {
        if (plus == 1)
        {
            time += t;
            oldTime += t;
        }
        else if (plus == 0)
        {
            time = t;
            oldTime = t;
        }
        else if (plus == -1)
        {
            time -= t;
            oldTime -= t;
        }
    }

    void _DisplayTime(float realTime)
    {
        if (oldTime >= realTime)
        {
            oldTime = realTime;
            _ChangeTime(oldTime);
        }
    }

    void _ChangeTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        if (minutes >= 0 && seconds >= 0)
        {
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            timeTextComplete.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void _FreezeTime(bool freeze)
    {
        isFreezed = freeze;
    }

    public void _TweenExtraTime()
    {
        iconClock.GetComponent<Image>().DOColor(Color.yellow, 0.5f).SetEase(Ease.InOutQuad).SetUpdate(true);
        iconClock.DOScale(new Vector3(1.2f, 1.2f, 1), 0.75f).SetEase(Ease.OutBounce).SetUpdate(true)
            .OnComplete(delegate
            {
                iconClock.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutQuad).SetUpdate(true).SetDelay(0.75f);
                iconClock.GetComponent<Image>().DOColor(Color.white, 0.5f).SetEase(Ease.InOutQuad).SetUpdate(true).SetDelay(0.75f);
            });
    }

    public void _TweenTimeWarn()
    {
        iconClock.DOScale(new Vector3(1.2f, 1.2f, 1), 0.5f).SetEase(Ease.InOutQuad).SetUpdate(true).SetLoops(-1, LoopType.Yoyo);
        iconClock.DORotate(new Vector3(0, 0, -20), 0.5f).SetEase(Ease.InOutQuad).SetUpdate(true).SetLoops(-1, LoopType.Yoyo);
        iconClock.GetComponent<Image>().DOColor(Color.red, 0.5f).SetEase(Ease.InOutQuad).SetUpdate(true).SetLoops(-1, LoopType.Yoyo);
    }

    public void _ResetClockState()
    {
        DOTween.Kill(iconClock);
        DOTween.Kill(iconClock.GetComponent<Image>());
        iconClock.localScale = Vector3.one;
        iconClock.localEulerAngles = Vector3.zero;
        iconClock.GetComponent<Image>().color = Color.white;
    }

    public void _TestTimeOut()
    {
        time = 3;
    }
}
