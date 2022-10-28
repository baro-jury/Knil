using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public static TutorialController instance;
    public static (Vector3, Vector3)[] point =
    {
        (new Vector3(-180, 270, 0), new Vector3(-180, 90, 0)),
        (new Vector3(-360, 270, 0), new Vector3(0, 90, 0)),
        (new Vector3(-360, 90, 0), new Vector3(-360, -270, 0)),
        (new Vector3(180, 90, 0), new Vector3(360, 270, 0)),
        (new Vector3(-360, -90, 0), new Vector3(180, -90, 0)),
    };

    [SerializeField]
    private GameObject fingerPoint, timer, supporter;

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

    public void _CheckTut()
    {
        if (LevelController.level == 1)
        {
            Time.timeScale = 1;
            timer.SetActive(false);
            supporter.SetActive(false);
            TimeController.instance._FreezeTime(true);
            var finger = Instantiate(fingerPoint, point[0].Item1 / 40, Quaternion.identity, gameObject.transform);
        }
    }

    public void _FocusOn1Tile(float x, float y)
    {
        if (gameObject.transform.childCount != 0)
        {
            Destroy(gameObject.transform.GetChild(0));
        }
        foreach (var item in BoardController.buttonListWithoutBlocker)
        {
            if (item.transform.localPosition != new Vector3(x, y, 0))
            {
                item.transform.GetComponent<Button>().interactable = false;
            }
        }
        var finger = Instantiate(fingerPoint, new Vector3(x, y, 0) / 40, Quaternion.identity, gameObject.transform);
    }

    public void _FreeTile()
    {
        if (gameObject.transform.childCount != 0)
        {
            Destroy(gameObject.transform.GetChild(0));
        }
        foreach (var item in BoardController.buttonListWithoutBlocker)
        {
            item.transform.GetComponent<Button>().interactable = true;
        }
    }
}
