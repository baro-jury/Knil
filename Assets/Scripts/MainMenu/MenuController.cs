using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;

    [SerializeField]
    private Button continueLevelButton;
    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private GameObject settingPanel;
    [SerializeField]
    private GameObject guidePanel;

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
        if (!ProgressController.instance._IsFirstPlay())
        {
            continueLevelButton.gameObject.SetActive(true);
        }
    }

    public void _PlayNewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void _ChooseLevel()
    {
        menuPanel.SetActive(false);
    }

    public void _PlaySpecificLevel()
    {
        SceneManager.LoadScene(1);
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

}
