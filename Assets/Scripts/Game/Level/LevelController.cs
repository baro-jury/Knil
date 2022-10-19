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

    public static int level = 1;
    [SerializeField]
    private List<Button> lvButtons = new();

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
        var dataStr = Resources.Load("Levels/Level_" + level) as TextAsset;
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

    public void _PlayLvTest()
    {
        var dataStr = Resources.Load("demo") as TextAsset;
        BoardController.levelData = JsonConvert.DeserializeObject<LevelData>(dataStr.text);
        SceneManager.LoadScene(1);
    }
    public void _PlayLevel(int lv)
    {
        level = lv;
        BoardController.levelData = _GetLevelData(lv);
        SceneManager.LoadScene(1);
    }

    #endregion

}
