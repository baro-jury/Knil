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
    public static Sequence fingerSequence;

    public GameObject finger, tutorialPanel, notePanel;
    [SerializeField]
    private GameObject timer;

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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log(finger.transform.position);
            
        }
    }

    public void _ChangeObjectState()
    {
        timer.SetActive(false);
        finger.SetActive(true);
        _TutTweening();
        _FingerTut();
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

    void _FingerTut()
    {
        finger.transform.localPosition = new Vector3(0, -(0 - BoardDemo.instance.bottomFrame.transform.localPosition.y) * 15 / 28, 0);
        Vector3 temp = finger.transform.position;

        fingerSequence = DOTween.Sequence();
        fingerSequence.Append(finger.transform.DOScale(Vector3.one, 0.4f).SetUpdate(true));
        fingerSequence.Append(finger.transform.DOMove(_FindTransform(BoardDemo.buttonListWithoutBlocker, (0, 1)).position, 0.4f).SetUpdate(true));
        fingerSequence.Append(finger.transform.DOScale(new Vector3(0.6f, 0.6f, 1), 0.4f).SetUpdate(true));
        fingerSequence.Append(finger.transform.DOScale(Vector3.one, 0.4f).SetUpdate(true));
        fingerSequence.Append(finger.transform.DOMove(_FindTransform(BoardDemo.buttonListWithoutBlocker, (1, 1)).position, 0.4f).SetUpdate(true));
        fingerSequence.Append(finger.transform.DOScale(new Vector3(0.6f, 0.6f, 1), 0.4f).SetUpdate(true));
        fingerSequence.Append(finger.transform.DOScale(Vector3.one, 0.4f).SetUpdate(true));
        fingerSequence.Append(finger.transform.DOMove(temp, 0.4f).SetUpdate(true).OnComplete(delegate { finger.transform.localScale = Vector3.zero; }));
        fingerSequence.AppendInterval(0.4f);
        fingerSequence.SetUpdate(true).SetLoops(-1);
    }

    public void _FingerSupporter(Transform supporter)
    {
        finger.transform.position = new Vector3(supporter.position.x, supporter.position.y - 0.45f, supporter.position.z);
        finger.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        finger.SetActive(true);

        fingerSequence = DOTween.Sequence();
        fingerSequence.Append(finger.transform.DOScale(new Vector3(0.5f, 0.5f, 1), 0.5f).SetUpdate(true));
        fingerSequence.Append(finger.transform.DOScale(new Vector3(0.8f, 0.8f, 1), 0.5f).SetUpdate(true));
        fingerSequence.SetUpdate(true).SetLoops(-1);
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
            notePanel.transform.SetAsLastSibling();
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
                    notePanel.transform.SetAsLastSibling();
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
            tutorialPanel.transform.SetAsLastSibling();
            notePanel.transform.SetAsLastSibling();
            GameplayDemo.instance.pause.transform.SetAsLastSibling();
            notePanel.transform.GetChild(2).DOScale(Vector3.one, .4f).SetEase(Ease.InOutQuad).SetUpdate(true);
        }
    }
    #endregion


}
