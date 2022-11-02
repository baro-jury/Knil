using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public static TutorialController instance;
    public static int order = 0;
    public static Vector3[] point =
    {
        new Vector3(-180, 270, 0), new Vector3(-180, 90, 0),
        new Vector3(-360, 270, 0), new Vector3(0, 90, 0),
        new Vector3(-360, 90, 0), new Vector3(-360, -270, 0),
        new Vector3(180, 90, 0), new Vector3(360, 270, 0),
        new Vector3(-360, -90, 0), new Vector3(180, -90, 0)
    };
    public static (Vector3, Vector3)[] couplePoint =
    {
        (new Vector3(-180, 270, 0), new Vector3(-180, 90, 0)),
        (new Vector3(-360, 270, 0), new Vector3(0, 90, 0)),
        (new Vector3(-360, 90, 0), new Vector3(-360, -270, 0)),
        (new Vector3(180, 90, 0), new Vector3(360, 270, 0)),
        (new Vector3(-360, -90, 0), new Vector3(180, -90, 0)),
    };
    public static (int, int)[] index =
    {
        (0, 1), (1, 1),
        (0, 0), (1, 2),
        (1, 0), (3, 0),
        (1, 3), (0, 4),
        (2, 0), (2, 3)
    };
    public static ((int, int), (int, int))[] coupleIndex =
    {
        ((0, 1), (1, 1)),
        ((0, 0), (1, 2)),
        ((1, 0), (3, 0)),
        ((1, 3), (0, 4)),
        ((2, 0), (2, 3))
    };

    public GameObject fingerPoint, tutorialPanel, connectFailTutorial;
    [SerializeField]
    private GameObject booster, timer, supporter, pauseButton;

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
        tutorialPanel.SetActive(true);
        tutorialPanel.transform.GetChild(0).gameObject.SetActive(true);
        connectFailTutorial.SetActive(true);
        booster.SetActive(false);
        timer.SetActive(false);
        supporter.SetActive(false);
    }

    #region Tao anim finger
    public void _InitFinger(Vector3 pos)
    {
        _ChangeObjectState();
        _FocusOn1Tile(pos);
        Vector3 temp = new Vector3(pos.x - 140, pos.y + 30, 0);
        var finger = Instantiate(fingerPoint, temp / 96, Quaternion.identity, gameObject.transform);

        order++;
        Debug.Log(order + "  " + point.Length);
        if (order == point.Length) _FreeTile();
    }

    IEnumerator _FocusOn1Tile(Vector3 pos)
    {
        if (gameObject.transform.childCount != 0)
        {
            Destroy(gameObject.transform.GetChild(0).gameObject);
        }
        yield return new WaitForSeconds(0.2f);
        foreach (var item in BoardController.buttonListWithoutBlocker)
        {
            if (item.transform.localPosition != pos)
            {
                item.transform.GetComponent<Button>().interactable = false;
            }
            else
            {
                item.transform.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void _FreeTile()
    {
        if (gameObject.transform.childCount != 0)
        {
            Destroy(gameObject.transform.GetChild(0).gameObject);
        }
        foreach (var item in BoardController.buttonListWithoutBlocker)
        {
            item.transform.GetComponent<Button>().interactable = true;
        }
    }
    #endregion

    #region Tutorial Gameplay
    public Transform _FindTransform(List<TileController> list, (int, int) index)
    {
        return list.Find(x => x.Index == index).transform;
    }

    public void _FocusOnCoupleTile(Transform t1, Transform t2)
    {
        tutorialPanel.transform.SetAsLastSibling();
        t1.SetAsLastSibling();
        t2.SetAsLastSibling();
        connectFailTutorial.transform.SetAsLastSibling();
    }

    public void _SwitchTut(GameObject connectFailPanel)
    {
        connectFailPanel.SetActive(!connectFailPanel.activeInHierarchy);
        if (order == 4)
        {
            _FocusOnCoupleTile(_FindTransform(BoardController.buttonListWithoutBlocker, coupleIndex[order].Item1),
                _FindTransform(BoardController.buttonListWithoutBlocker, coupleIndex[order].Item2));
        }
        if (order == 5)
        {
            tutorialPanel.SetActive(false);
            pauseButton.transform.SetAsLastSibling();
            tutorialPanel.transform.SetAsLastSibling();
            connectFailTutorial.transform.SetAsLastSibling();
        }
    } 
    #endregion


}
