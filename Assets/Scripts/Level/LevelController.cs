using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
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

    public void _PlayLv11()
    {
        level = 11;
        _PlayLevel(level);
    }

    public void _PlayLv12()
    {
        level = 12;
        _PlayLevel(level);
    }

    #endregion

}
