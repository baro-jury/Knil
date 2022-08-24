using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressController : MonoBehaviour
{
    public static ProgressController instance;

    private const string FIRST_TIME = "IsGameStartedForTheFirstTime";
    private const string PROGRESS = "Progress";

    void _MakeSingleInstance()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void _CheckFirstTimePlay()
    {
        if (PlayerPrefs.HasKey(FIRST_TIME) == true)
        {
            PlayerPrefs.SetString(PROGRESS, "Level_1");
            PlayerPrefs.SetInt(FIRST_TIME, 0);
        }
    }

    void Awake()
    {
        _MakeSingleInstance();
        _CheckFirstTimePlay();
    }

    public bool _IsFirstPlay()
    {
        return PlayerPrefs.HasKey(FIRST_TIME);
    }

    public void _MarkCurrentLevel(string level)
    {
        PlayerPrefs.SetString(PROGRESS, level);
    }

    public string _GetMarkedLevel()
    {
        return PlayerPrefs.GetString(PROGRESS);
    }
}
