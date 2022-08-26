using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public static LevelController instance;

    public static int level;
    [SerializeField]
    private List<Button> lvButtons = new List<Button>();
    [SerializeField]
    private List<DataLevels> dataLevels = new List<DataLevels>();

    void _MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void _LoadDataLevel()
    {
        var dataStr = Resources.Load("Levels") as TextAsset;
        dataLevels = JsonConvert.DeserializeObject<List<DataLevels>>(dataStr.text);
    }

    void Awake()
    {
        _MakeInstance();
    }

    void Start()
    {
        //StartCoroutine("_GetData");
        _LoadDataLevel();
    }

    public DataLevels _GetDataLevel(int level)
    {
        var temp = dataLevels.Find(l => l.level == level);
        if (temp != null)
        {
            return temp;
        }
        return default;
    }

    public void _UnlockLevel(int lv)
    {
        for(int i = 0; i < lvButtons.Count; i++)
        {
            if (i < lv)
            {
                lvButtons[i].interactable = true;
            }
            else
            {
                lvButtons[i].interactable = false;
            }
        }
    }

    #region Play
    public void _PlayLevel(int lv)
    {
        BoardController.dataLevel = _GetDataLevel(lv);
        SceneManager.LoadScene(1);
    }

    public void _PlayLv1()
    {
        level = 1;
        _PlayLevel(level);
    }

    public void _PlayLv2()
    {
        level = 2;
        _PlayLevel(level);
    }

    public void _PlayLv3()
    {
        level = 3;
        _PlayLevel(level);
    }

    public void _PlayLv4()
    {
        level = 4;
        _PlayLevel(level);
    }

    public void _PlayLv5()
    {
        level = 5;
        _PlayLevel(level);
    }

    public void _PlayLv6()
    {
        level = 6;
        _PlayLevel(level);
    }

    public void _PlayLv7()
    {
        level = 7;
        _PlayLevel(level);
    }

    public void _PlayLv8()
    {
        level = 8;
        _PlayLevel(level);
    }
    public void _PlayLv9()
    {
        level = 9;
        _PlayLevel(level);
    }

    public void _PlayLv10()
    {
        level = 10;
        _PlayLevel(level);
    }
    #endregion

    #region test
    private string levelsUrl;
    private RectTransform content;
    private GameObject itemPrefab;

    IEnumerator _GetData()
    {
        UnityWebRequest req = UnityWebRequest.Get(levelsUrl);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            string data = req.downloadHandler.text;
            _InstantiateItem(data);
        }
        else
        {
            Debug.Log(req.error);
        }
    }

    private void _InstantiateItem(string data)
    {
        JSONArray array = JSON.Parse(data) as JSONArray;
        JSONObject obj = new JSONObject();

        for (int i = 0; i < array.Count; i++)
        {
            obj = array[i].AsObject;
            float x = obj["X"];
            float y = obj["Y"];
            int lv = obj["level"];
            int row = obj["row"];
            int column = obj["column"];

            GameObject item = Instantiate(itemPrefab, content);
            Vector3 pos = Camera.main.ViewportToScreenPoint(new Vector3(x, y, 0));
            item.transform.position = pos;
            if (content.childCount > 1)
            {
                float distance = Mathf.Abs(item.transform.position.y - content.GetChild(i - 1).position.y);
                content.sizeDelta = new Vector2(content.sizeDelta.x, content.sizeDelta.y + distance);
            }
        }
    }
    #endregion
}
