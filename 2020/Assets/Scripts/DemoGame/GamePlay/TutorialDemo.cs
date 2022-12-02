using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDemo : MonoBehaviour
{
    public static TutorialDemo instance;
    public static int order = 0;
    public static ((int, int), (int, int))[] coupleIndex =
    {
        ((0, 1), (1, 1)),
        ((0, 0), (1, 2)),
        ((1, 0), (3, 0)),
        ((1, 3), (0, 4)),
        ((2, 0), (2, 3))
    };

    public GameObject finger, tutorialPanel, connectFailTutorial;
    [SerializeField]
    private GameObject timer, bottom;

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

    public void _ChangeObjectState()
    {
        timer.SetActive(false);
        finger.SetActive(true);
        _TutTweening();
        _TweenFinger();
    }

    void _TutTweening()
    {
        tutorialPanel.transform.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        tutorialPanel.SetActive(true);
        if (order < 3)
        {
            tutorialPanel.transform.GetChild(0).gameObject.SetActive(true);
            tutorialPanel.transform.GetChild(0).GetComponent<RectTransform>().DOScale(Vector3.one, .4f).SetEase(Ease.InOutQuad).SetUpdate(true);
        }
        tutorialPanel.transform.GetComponent<Image>().DOFade(0.5f, .4f).SetEase(Ease.InOutQuad).SetUpdate(true)
            .OnComplete(delegate { BoardDemo.instance._EnableTile(true); });
    }

    void _TweenFinger()
    {
        finger.transform.localPosition = new Vector3(0, -(0 - bottom.transform.localPosition.y) * 15 / 28, 0);
        Vector3 temp = finger.transform.position;

        Sequence mySequence = DOTween.Sequence();
        //mySequence.Append(finger.transform.DOScale(Vector3.one, 0.5f).SetUpdate(true));
        //mySequence.Append(finger.transform.DOMove(_FindTransform(BoardDemo.buttonListWithoutBlocker, (0, 1)).position, 0.5f).SetUpdate(true));
        //mySequence.Append(finger.transform.DOScale(new Vector3(0.6f, 0.6f, 1), 0.5f).SetUpdate(true));
        //mySequence.Append(finger.transform.DOScale(Vector3.one, 0.5f).SetUpdate(true));
        //mySequence.Append(finger.transform.DOMove(_FindTransform(BoardDemo.buttonListWithoutBlocker, (1, 1)).position, 0.5f).SetUpdate(true));
        //mySequence.Append(finger.transform.DOScale(new Vector3(0.6f, 0.6f, 1), 0.5f).SetUpdate(true));
        //mySequence.Append(finger.transform.DOScale(Vector3.one, 0.5f).SetUpdate(true));
        //mySequence.Append(finger.transform.DOMove(temp, 0.5f).SetUpdate(true).OnComplete(delegate { finger.transform.localScale = Vector3.zero; }));
        //mySequence.AppendInterval(0.5f);
        //mySequence.SetUpdate(true).SetLoops(-1);
        //=====================================
        mySequence.Append(finger.transform.DOScale(Vector3.one, 0.4f).SetUpdate(true));
        mySequence.Append(finger.transform.DOMove(_FindTransform(BoardDemo.buttonListWithoutBlocker, (0, 1)).position, 0.4f).SetUpdate(true));
        mySequence.Append(finger.transform.DOScale(new Vector3(0.6f, 0.6f, 1), 0.4f).SetUpdate(true));
        mySequence.Append(finger.transform.DOScale(Vector3.one, 0.4f).SetUpdate(true));
        mySequence.Append(finger.transform.DOMove(_FindTransform(BoardDemo.buttonListWithoutBlocker, (1, 1)).position, 0.4f).SetUpdate(true));
        mySequence.Append(finger.transform.DOScale(new Vector3(0.6f, 0.6f, 1), 0.4f).SetUpdate(true));
        mySequence.Append(finger.transform.DOScale(Vector3.one, 0.4f).SetUpdate(true));
        mySequence.Append(finger.transform.DOMove(temp, 0.4f).SetUpdate(true));
        mySequence.Append(finger.transform.DOScale(Vector3.zero, 0.4f).SetUpdate(true));
        mySequence.AppendInterval(0.4f);
        mySequence.SetUpdate(true).SetLoops(-1);

    }

    #region Tutorial Gameplay
    public Transform _FindTransform(List<TileController> list, (int, int) index)
    {
        return list.Find(x => x.Index == index).transform;
    }

    public void _FocusOnCoupleTile(Transform t1, Transform t2)
    {
        if (order == 0)
        {
            GameplayDemo.instance.pause.transform.SetAsLastSibling();
            tutorialPanel.transform.SetAsLastSibling();
            t1.SetAsLastSibling();
            t2.SetAsLastSibling();
            connectFailTutorial.transform.SetAsLastSibling();
        }
        else
        {
            BoardDemo.instance._EnableTile(false);
            tutorialPanel.transform.GetChild(0).GetComponent<RectTransform>().localScale = Vector3.zero;
            tutorialPanel.transform.GetComponent<Image>().DOFade(0, .4f).SetEase(Ease.InOutQuad).SetUpdate(true)
                .OnComplete(delegate
                {
                    _TutTweening();
                    GameplayDemo.instance.pause.transform.SetAsLastSibling();
                    tutorialPanel.transform.SetAsLastSibling();
                    t1.SetAsLastSibling();
                    t2.SetAsLastSibling();
                    connectFailTutorial.transform.SetAsLastSibling();
                });
        }

    }

    public void _SwitchTut(GameObject connectFailPanel)
    {
        connectFailPanel.SetActive(!connectFailPanel.activeInHierarchy);
        if (order == 4)
        {
            _FocusOnCoupleTile(_FindTransform(BoardDemo.buttonListWithoutBlocker, coupleIndex[order].Item1),
                _FindTransform(BoardDemo.buttonListWithoutBlocker, coupleIndex[order].Item2));
        }
        if (order == 5)
        {
            tutorialPanel.SetActive(false);
            GameplayDemo.instance.pause.transform.SetAsLastSibling();
            tutorialPanel.transform.SetAsLastSibling();
            connectFailTutorial.transform.SetAsLastSibling();
            connectFailTutorial.transform.GetChild(2).DOScale(Vector3.one, .4f).SetEase(Ease.InOutQuad).SetUpdate(true);
        }
    }
    #endregion


}
