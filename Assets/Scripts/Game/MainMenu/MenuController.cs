using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;

    public Button continueLevelButton;

    [SerializeField]
    private GameObject menuPanel, settingPanel, guidePanel;
    [SerializeField]
    private Text stars, coins;
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

    void Start()
    {
        stars.text = ProgressController.instance._GetStarsAchieved() + "";
        coins.text = ProgressController.instance._GetCoinsInPossession() + "";
    }

    public void _PlayNewGame()
    {
        LevelController.instance._PlayLv1();
        ProgressController.instance._SetMarkedLevel(1);
    }

    public void _PlaySavedGame()
    {
        LevelController.instance._PlayLevel(ProgressController.instance._GetMarkedLevel());
    }

    public void _ChooseLevel()
    {
        menuPanel.SetActive(false);
        //LevelController.instance._UnlockLevel(ProgressController.instance._GetMarkedLevel());
    }

    public void _BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void _GoToSetting()
    {
        settingPanel.SetActive(true);
    }

    public void _TurnOffVolume()
    {
        
    }

    public void _TurnOnVolume()
    {
        
    }

    public void _ShowHowToPlay()
    {
        guidePanel.SetActive(true);
    }

    public void _TurnOffGuidePanel()
    {
        guidePanel.SetActive(false);
    }

    public void _TurnOffSettingPanel()
    {
        settingPanel.SetActive(false);
    }

    public void _BuyCoins()
    {
        
    }
}
