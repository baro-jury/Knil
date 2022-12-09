using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class TimeDemo : MonoBehaviour
{
    public static TimeDemo instance;
    public float time;
    public bool muchTimeLeft;
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
        _ChangeTime(oldTime);
    }

    void Update()
    {
        if (!isFreezed)
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
            else
            {
                _SetTime(0, 0);
                muchTimeLeft = true;
                PlayerPrefsDemo.instance.timeWarningSource.Stop();
                foreach (var tile in BoardDemo.buttonList)
                {
                    DOTween.Kill(tile);
                    tile.localScale = Vector3.one;
                }
                LineDemo.instance._EraseLine();
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
        if(minutes >= 0 && seconds >= 0)
        {
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            timeTextComplete.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void _FreezeTime(bool freeze)
    {
        isFreezed = freeze;
    }

    public void _TweenTimeWarn()
    {
        iconClock.DOScale(new Vector3(1.2f, 1.2f, 1), 0.5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
        iconClock.DORotate(new Vector3(0, 0, -20), 0.5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
        iconClock.GetComponent<Image>().DOColor(Color.red, 0.5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }

    public void _ResetIconState()
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
