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
    private const string COINS = "Coins";
    private const string STARS = "Stars";

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

    public int _GetMarkedLevel()
    {
        return PlayerPrefs.GetInt(PROGRESS);
    }

    public void _SetMarkedLevel(int level)
    {
        PlayerPrefs.SetInt(PROGRESS, level);
    }

    public int _GetStarsAchieved()
    {
        return PlayerPrefs.GetInt(STARS);
    }

    public void _SetStarsAchieved(int star)
    {
        PlayerPrefs.SetInt(STARS, PlayerPrefs.GetInt(STARS) + star);
    }

    public int _GetCoinsInPossession()
    {
        return PlayerPrefs.GetInt(COINS);
    }

    public void _SetCoinsInPossession(int coin, bool isEarning)
    {
        int temp = -1;
        if (isEarning) temp = 1;
        PlayerPrefs.SetInt(COINS, PlayerPrefs.GetInt(COINS) + temp * coin);
    }

}
