using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using static UnityEngine.UI.Dropdown;
using System.Linq;
using UnityEngine.SceneManagement;

public class SetUpMap : MonoBehaviour
{
    public static SetUpMap instance;

    private LevelData levelData = new();
    private List<OptionData> tileImages = new();
    private List<ProcessData> map;
    private ProcessData dataMap = new();
    private int index = 0;
    private bool setIngame;
    private string[,] matrix;

    [SerializeField]
    private GameObject background;

    [SerializeField]
    private GameObject preSettingPanel;
    [SerializeField]
    private InputField level, process, time;
    [SerializeField]
    private Dropdown theme;

    [SerializeField]
    private InputField timeWhileEditing;
    [SerializeField]
    private InputField timestampFor1Star, timestampFor2Star, timestampFor3Star;

    [SerializeField]
    private GameObject ingameSettingPanel;
    [SerializeField]
    private InputField row, column;
    [SerializeField]
    private Dropdown themeWhileEditing, pullDown, pullUp, pullLeft, pullRight;

    [SerializeField]
    private TextMeshProUGUI titleLv;
    [SerializeField]
    private GameObject optionPanel;

    public Button btNone, btRandom, btBlock;
    [SerializeField]
    private Dropdown ddImage;

    [SerializeField]
    private Button btPrev, btNext, btPlayTrial;

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
        //List<ProcessData> temp = new List<ProcessData>() { new ProcessData(3, 4, false, false, false, false), new ProcessData(12, 9, true, true, true, true) };
        ////LevelData lv = new LevelData(11, 1, 420, JsonConvert.SerializeObject(tempp));
        ////string json = JsonUtility.ToJson(lv); // <-
        //string json = JsonConvert.SerializeObject(new LevelData(12, 1, 550, temp));
        ////File.WriteAllText(Application.dataPath + "/Resources/demo.json", json);
        //File.WriteAllText(Application.dataPath + "/Resources/Level_12.json", json);

        ////LevelData loadLv = JsonUtility.FromJson<LevelData>(json); // <-
        //LevelData loadLv = JsonConvert.DeserializeObject<LevelData>(json);
        //foreach (ProcessData pro in loadLv.process)
        //{
        //    Debug.Log(pro.ToString());
        //}

        tileImages.Add(new OptionData("", btNone.GetComponent<Image>().sprite));
        for (int i = 1; i < ResourceController.spritesDict.Count; i++)
        {
            tileImages.Add(new OptionData(ResourceController.spritesDict.ElementAt(i).Key, ResourceController.spritesDict.ElementAt(i).Value));
        }
        ddImage.AddOptions(tileImages);
    }

    void _SelectBackground(Dropdown drop)
    {
        bool hasBackground = false;
        for (int i = 0; i < background.transform.childCount; i++)
        {
            if (drop.value - 1 != i)
            {
                background.transform.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                background.transform.GetChild(i).gameObject.SetActive(true);
                hasBackground = true;
            }
        }
        if (hasBackground)
        {
            GameObject bgr = background.transform.GetChild(drop.value - 1).gameObject;
            SpriteRenderer sr = bgr.GetComponent<SpriteRenderer>();
            Vector3 temp = bgr.transform.localScale;
            float height = sr.bounds.size.y;
            float width = sr.bounds.size.x;
            float scaleHeight = Camera.main.orthographicSize * 2f;
            float scaleWidth = scaleHeight * Screen.width / Screen.height;
            bgr.transform.localScale = new Vector3(scaleWidth / width, scaleHeight / height, 0);
        }
    }

    #region Pre Setting
    public void _CheckInput(InputField input)
    {
        if (input.text == "" || int.Parse(input.text) <= 0)
        {
            input.text = "1";
        }
    }

    public void _CheckInputTime(InputField lowerTimestamp, InputField higherTimestamp)
    {
        if (lowerTimestamp.text == "" || int.Parse(lowerTimestamp.text) <= 0)
        {
            lowerTimestamp.text = "0";
        }
        if (int.Parse(lowerTimestamp.text) >= int.Parse(higherTimestamp.text))
        {
            lowerTimestamp.text = higherTimestamp.text;
        }
    }

    public void _SetBaseProperties()
    {
        levelData.level = int.Parse(level.text);
        levelData.theme = theme.value;
        levelData.time[0] = float.Parse(time.text);
        map = new List<ProcessData>();
        matrix = new string[1, 1];
        for (int i = 0; i < int.Parse(process.text); i++)
        {
            map.Add(new ProcessData(1, 1, false, false, false, false, matrix));
        }

        SetUpBoard.setup = this;
        SetUpBoard.levelData = levelData;
        SetUpBoard.processData = map[0];
        SetUpBoard.instance._StartCreating();
        _SelectBackground(theme);

        preSettingPanel.SetActive(false);
        titleLv.text = "LEVEL " + levelData.level;
        timeWhileEditing.text = time.text;
        themeWhileEditing.value = theme.value;
        index = 0;

        if (int.Parse(process.text) > 1)
        {
            btNext.gameObject.SetActive(true);
        }
        else
        {
            btNext.gameObject.SetActive(false);
            btPlayTrial.interactable = true;
        }
    }

    #endregion

    #region Setting Up
    void _CheckMatrix()
    {
        for(int r = 0; r < matrix.GetLength(0); r++)
        {
            for(int c = 0; c < matrix.GetLength(1); c++)
            {
                if (matrix[r, c] == null)
                {
                    matrix[r, c] = "";
                }
            }
        }
    }

    public void _ChangeSetting()
    {
        preSettingPanel.SetActive(true);
        //GameObject btClose = GameObject.Find("PreSettingPanel/CloseButton");
        //btClose.SetActive(true);
    }
    
    public void _SetProperties()
    {
        setIngame = ingameSettingPanel.activeInHierarchy;
        setIngame = !setIngame;
        ingameSettingPanel.SetActive(setIngame);
    }

    public void _EditBoard()
    {
        _CheckInput(row);
        _CheckInput(column);

        matrix = new string[int.Parse(row.text), int.Parse(column.text)];
        _EditDataMap();

        SetUpBoard.levelData = levelData;
        SetUpBoard.processData = dataMap;
        SetUpBoard.instance._StartCreating();
    }

    public void _EditDataMap()
    {
        _CheckInput(timeWhileEditing);
        _CheckInputTime(timestampFor1Star, timestampFor2Star);
        _CheckInputTime(timestampFor2Star, timestampFor3Star);
        _CheckInputTime(timestampFor3Star, timeWhileEditing);

        levelData.time[0] = float.Parse(timeWhileEditing.text);
        levelData.time[1] = float.Parse(timestampFor1Star.text);
        levelData.time[2] = float.Parse(timestampFor2Star.text);
        levelData.time[3] = float.Parse(timestampFor3Star.text);
        levelData.theme = themeWhileEditing.value;
        dataMap.Row = int.Parse(row.text);
        dataMap.Column = int.Parse(column.text);
        dataMap.PullDown = bool.Parse(pullDown.options[pullDown.value].text);
        dataMap.PullUp = bool.Parse(pullUp.options[pullUp.value].text);
        dataMap.PullLeft = bool.Parse(pullLeft.options[pullLeft.value].text);
        dataMap.PullRight = bool.Parse(pullRight.options[pullRight.value].text);
        dataMap.Matrix = matrix;

        _SelectBackground(themeWhileEditing);
    }

    #endregion

    #region Edit Tile
    public void _EditTile(Transform currentTile)
    {
        float tileSize = currentTile.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        int[] temp = SetUpBoard.instance._ConvertPositionToMatrixIndex(
            currentTile.localPosition.x, currentTile.localPosition.y,
            tileSize * dataMap.Column, tileSize * dataMap.Row, tileSize);

        optionPanel.SetActive(true);

        btNone.onClick.RemoveAllListeners();
        btRandom.onClick.RemoveAllListeners();
        btBlock.onClick.RemoveAllListeners();
        ddImage.onValueChanged.RemoveAllListeners();
        ddImage.value = 0;

        btNone.onClick.AddListener(delegate
        {
            _NoneTile(currentTile);
            matrix[temp[0], temp[1]] = "";
        });
        btRandom.onClick.AddListener(delegate
        {
            _RandomTile(currentTile);
            matrix[temp[0], temp[1]] = "?";
        });
        btBlock.onClick.AddListener(delegate
        {
            _BlockTile(currentTile);
            matrix[temp[0], temp[1]] = "0";
        });
        ddImage.onValueChanged.AddListener(delegate
        {
            _ChooseTile(currentTile);
            matrix[temp[0], temp[1]] = ddImage.options[ddImage.value].text;
        });
    }

    void _NoneTile(Transform currentTile)
    {
        currentTile.GetChild(0).GetComponent<Image>().sprite = btNone.GetComponent<Image>().sprite;
        optionPanel.SetActive(false);
    }

    void _BlockTile(Transform currentTile)
    {
        currentTile.GetChild(0).GetComponent<Image>().sprite = btBlock.GetComponent<Image>().sprite;
        optionPanel.SetActive(false);
    }

    void _RandomTile(Transform currentTile)
    {
        currentTile.GetChild(0).GetComponent<Image>().sprite = btRandom.GetComponent<Image>().sprite;
        optionPanel.SetActive(false);
    }

    void _ChooseTile(Transform currentTile)
    {
        currentTile.GetChild(0).GetComponent<Image>().sprite = ResourceController.spritesDict[ddImage.value.ToString()];
        optionPanel.SetActive(false);
    }

    #endregion

    #region Interact
    public void _GoToNextProcess()
    {
        _CheckMatrix();
        map[index] = new ProcessData(dataMap.Row, dataMap.Column, dataMap.PullDown,
            dataMap.PullUp, dataMap.PullLeft, dataMap.PullRight, matrix);
        index++;

        btPrev.gameObject.SetActive(true);
        if (index == int.Parse(process.text) - 1)
        {
            btNext.gameObject.SetActive(false);
            btPlayTrial.interactable = true;
        }
        row.text = map[index].Row.ToString();
        column.text = map[index].Column.ToString();
        pullDown.value = map[index].PullDown == true ? 1 : 0;
        pullUp.value = map[index].PullUp == true ? 1 : 0;
        pullLeft.value = map[index].PullLeft == true ? 1 : 0;
        pullRight.value = map[index].PullRight == true ? 1 : 0;
        matrix = map[index].Matrix;
        _EditDataMap();

        SetUpBoard.levelData = levelData;
        SetUpBoard.processData = map[index];
        SetUpBoard.instance._StartCreating();

        Debug.Log(index);
    }

    public void _GoToPreviousProcess()
    {
        _CheckMatrix();
        map[index] = new ProcessData(dataMap.Row, dataMap.Column, dataMap.PullDown,
            dataMap.PullUp, dataMap.PullLeft, dataMap.PullRight, matrix);
        btNext.gameObject.SetActive(true);
        btPlayTrial.interactable = false;
        index--;
        if (index == 0)
        {
            btPrev.gameObject.SetActive(false);
        }
        row.text = map[index].Row.ToString();
        column.text = map[index].Column.ToString();
        pullDown.value = map[index].PullDown == true ? 1 : 0;
        pullUp.value = map[index].PullUp == true ? 1 : 0;
        pullLeft.value = map[index].PullLeft == true ? 1 : 0;
        pullRight.value = map[index].PullRight == true ? 1 : 0;
        matrix = map[index].Matrix;
        _EditDataMap();

        SetUpBoard.levelData = levelData;
        SetUpBoard.processData = map[index];
        SetUpBoard.instance._StartCreating();
        
        Debug.Log(index);
    }

    public void _Complete()
    {
        _CheckMatrix();
        map[index] = new ProcessData(dataMap.Row, dataMap.Column, dataMap.PullDown,
            dataMap.PullUp, dataMap.PullLeft, dataMap.PullRight, matrix);
        levelData.process = map;

        string json = JsonConvert.SerializeObject(levelData);
        File.WriteAllText(Application.dataPath + "/Resources/Level_" + levelData.level + ".json", json);
        //File.WriteAllText(Application.dataPath + "/Resources/demo.json", json);

        Debug.Log("Doneeeee");
    }

    public void _PlayTrial()
    {
        SceneManager.LoadScene("PlayTrial");
    }

    #endregion
}
