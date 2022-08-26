using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressController : MonoBehaviour
{
    public static ProgressController instance;

    [SerializeField]
    private GameObject menu;
    
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

    void _CheckFirstTimePlayGame()
    {
        if (!PlayerPrefs.HasKey("_CheckFirstTimePlayGame"))
        {
            PlayerPrefs.SetInt(PROGRESS, 1);
            PlayerPrefs.SetInt("_CheckFirstTimePlayGame", 0);
            menu.GetComponent<MenuController>().continueLevelButton.gameObject.SetActive(false);
        }
    }

    void Awake()
    {
        _MakeSingleInstance();
        _CheckFirstTimePlayGame();
    }

    public void _MarkCurrentLevel(int level)
    {
        PlayerPrefs.SetInt(PROGRESS, level);
    }

    public int _GetMarkedLevel()
    {
        return PlayerPrefs.GetInt(PROGRESS);
    }

}
