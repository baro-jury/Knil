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

    //void _LoadDataLevel()
    //{
    //    var dataStr = Resources.Load("Levels") as TextAsset;
    //    dataLevels = JsonConvert.DeserializeObject<List<DataLevels>>(dataStr.text);
    //}

    //public DataLevels _GetDataLevel(int level)
    //{
    //    var temp = dataLevels.Find(l => l.level == level);
    //    if (temp != null)
    //    {
    //        return temp;
    //    }
    //    return default;
    //}

    public LevelData _GetLevelData(int level)
    {
        var dataStr = Resources.Load("Level_" + level) as TextAsset;
        LevelData temp = JsonConvert.DeserializeObject<LevelData>(dataStr.text);
        return temp;
    }

    #region Play
    public void _UnlockLevel(int lv)
    {
        for (int i = 0; i < lvButtons.Count; i++)
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

    public void _PlayLevel(int lv)
    {
        BoardController.levelData = _GetLevelData(lv);
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

    public void _PlayLv13()
    {
        level = 13;
        _PlayLevel(level);
    }

    public void _PlayLv14()
    {
        level = 14;
        _PlayLevel(level);
    }

    public void _PlayLv15()
    {
        level = 15;
        _PlayLevel(level);
    }

    public void _PlayLv16()
    {
        level = 16;
        _PlayLevel(level);
    }

    public void _PlayLv17()
    {
        level = 17;
        _PlayLevel(level);
    }

    public void _PlayLv18()
    {
        level = 18;
        _PlayLevel(level);
    }

    public void _PlayLv19()
    {
        level = 19;
        _PlayLevel(level);
    }

    public void _PlayLv20()
    {
        level = 20;
        _PlayLevel(level);
    }

    #endregion

}
