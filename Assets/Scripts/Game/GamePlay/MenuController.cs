using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;

    [SerializeField]
    private GameObject menuPanel, settingPanel, guidePanel;
    [SerializeField]
    private Text stars, coins;
    [SerializeField]
    private TextMeshProUGUI level;
    [SerializeField]
    private AudioClip clickButtonClip, switchClip;

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
        level.text = "LEVEL " + ProgressController.instance._GetMarkedLevel();
    }

    public void _PlayGame()
    {
        ProgressController.instance.audioSource.PlayOneShot(clickButtonClip);
        LevelController.instance._PlayLevel(ProgressController.instance._GetMarkedLevel());
    }

    public void _ChooseLevel()
    {
        ProgressController.instance.audioSource.PlayOneShot(clickButtonClip);
        menuPanel.SetActive(false);
    }

    public void _BackToMenu()
    {
        ProgressController.instance.audioSource.PlayOneShot(clickButtonClip);
        menuPanel.SetActive(true);
    }

    public void _CreateCustomMap()
    {
        ProgressController.instance.audioSource.PlayOneShot(clickButtonClip);
        SceneManager.LoadScene("GenerateMap");
    }

    public void _GoToSetting()
    {
        ProgressController.instance.audioSource.PlayOneShot(clickButtonClip);
        settingPanel.SetActive(true);
    }

    public void _TurnOffSound()
    {
        ProgressController.instance.audioSource.mute = true;
        ProgressController.instance.audioSource.PlayOneShot(switchClip);
        settingPanel.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
        //              setting form         body        middle      sound on   sound off
    }

    public void _TurnOnSound()
    {
        ProgressController.instance.audioSource.mute = false;
        ProgressController.instance.audioSource.PlayOneShot(switchClip);
        settingPanel.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
        //              setting form         body        middle      sound on   sound off
    }

    public void _TurnOffMusic()
    {
        ProgressController.instance.audioSource.PlayOneShot(switchClip);
        ProgressController.instance.musicSource.mute = true;
        settingPanel.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(true);
        //              setting form         body        middle      music on   music off
    }

    public void _TurnOnMusic()
    {
        ProgressController.instance.audioSource.PlayOneShot(switchClip);
        ProgressController.instance.musicSource.mute = false;
        settingPanel.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(false);
        //              setting form         body        middle      music on   music off
    }

    public void _ShowHowToPlay()
    {
        ProgressController.instance.audioSource.PlayOneShot(clickButtonClip);
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
