using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public static TutorialController instance;
    public static int index = 0;
    public static Vector3[] point =
    {
        new Vector3(-180, 270, 0), new Vector3(-180, 90, 0),
        new Vector3(-360, 270, 0), new Vector3(0, 90, 0),
        new Vector3(-360, 90, 0), new Vector3(-360, -270, 0),
        new Vector3(180, 90, 0), new Vector3(360, 270, 0),
        new Vector3(-360, -90, 0), new Vector3(180, -90, 0)
    };
    //public static (Vector3, Vector3)[] couplePoint =
    //{
    //    (new Vector3(-180, 270, 0), new Vector3(-180, 90, 0)),
    //    (new Vector3(-360, 270, 0), new Vector3(0, 90, 0)),
    //    (new Vector3(-360, 90, 0), new Vector3(-360, -270, 0)),
    //    (new Vector3(180, 90, 0), new Vector3(360, 270, 0)),
    //    (new Vector3(-360, -90, 0), new Vector3(180, -90, 0)),
    //};

    [SerializeField]
    private GameObject fingerPoint, booster, timer, supporter;

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

    public void _DisableObject()
    {
        booster.SetActive(false);
        timer.SetActive(false);
        supporter.SetActive(false);
        TimeController.instance._FreezeTime(true);
    }

    public void _InitFinger(Vector3 pos)
    {
        _DisableObject();
        _FocusOn1Tile(pos);
        Vector3 temp = new Vector3(pos.x - 140, pos.y + 30, 0);
        var finger = Instantiate(fingerPoint, temp / 96, Quaternion.identity, gameObject.transform);

        index++;
        Debug.Log(index + "  " + point.Length);
        if (index == point.Length) _FreeTile();
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
}
