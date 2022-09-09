using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using static UnityEngine.UI.Dropdown;
using System.Linq;

public class SetUpMap : MonoBehaviour
{
    public static SetUpMap instance;

    private DataLevels dataLv = new DataLevels();
    private LevelData levelData = new LevelData();
    private ProcessData processData = new ProcessData();
    private List<OptionData> tileImages = new List<OptionData>(); 

    [SerializeField]
    private GameObject background;
    [SerializeField]
    private InputField level;
    [SerializeField]
    private InputField process;
    [SerializeField]
    private InputField row;
    [SerializeField]
    private InputField column;
    [SerializeField]
    private InputField time;
    [SerializeField]
    private Dropdown theme;
    [SerializeField]
    private Dropdown pullDown;
    [SerializeField]
    private Dropdown pullUp;
    [SerializeField]
    private Dropdown pullLeft;
    [SerializeField]
    private Dropdown pullRight;
    [SerializeField]
    private GameObject preSettingPanel;
    [SerializeField]
    private InputField rowWhileEditing;
    [SerializeField]
    private InputField columnWhileEditing;
    [SerializeField]
    private Dropdown themeWhileEditing;

    [SerializeField]
    private GameObject optionPanel;
    [SerializeField]
    private Button btNone;
    [SerializeField]
    private Button btBlock;
    [SerializeField]
    private Dropdown ddImage;

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
        //List<ProcessData> temp = new List<ProcessData>() { new ProcessData(3, 4, false, false, false, false), new ProcessData(8, 9, true, true, true, true) };
        //ProcessData[] tempp = { new ProcessData(3, 4, false, false, false, false), new ProcessData(8, 9, true, true, true, true) };
        //LevelData lv = new LevelData(11, 1, 420, JsonConvert.SerializeObject(tempp));
        //string json = JsonUtility.ToJson(lv); // <-
        //File.WriteAllText(Application.dataPath + "/Resources/demo.json", json);

        //LevelData loadLv = JsonUtility.FromJson<LevelData>(json); // <-
        //foreach (ProcessData pro in JsonConvert.DeserializeObject<ProcessData[]>(loadLv.process))
        //{
        //    Debug.Log(pro.ToString());
        //}

        //int numlv = 12;
        //DataLevels lev = new DataLevels(numlv, 12, 9, 420, true, true, true, true);
        //string js = JsonUtility.ToJson(lev);
        //File.WriteAllText(Application.dataPath + "/Resources/Level_" + numlv + ".json", js);

        //level.text = "1";
        //process.text = "1";
        //row.text = "1";
        //column.text = "1";
        for(int i = 0; i < TileImage.spritesDict.Count; i++)
        {
            tileImages.Add(new OptionData(TileImage.spritesDict.ElementAt(i).Value.ToString(), TileImage.spritesDict.ElementAt(i).Key));
        }
        tileImages.Add(new OptionData("", ddImage.transform.parent.GetComponent<Image>().sprite));
        ddImage.AddOptions(tileImages);
    }

    #region Pre Setting
    public void _CheckInput(InputField input)
    {
        if (input.text == "" || int.Parse(input.text) <= 0)
        {
            input.text = "1";
        }
    }

    public void _SetBaseProperties()
    {
        dataLv.level = int.Parse(level.text); //1
        dataLv.row = int.Parse(row.text); //2
        dataLv.column = int.Parse(column.text); //4
        dataLv.time = float.Parse(time.text); //100
        dataLv.pullDown = bool.Parse(pullDown.options[pullDown.value].text); //f
        dataLv.pullUp = bool.Parse(pullUp.options[pullUp.value].text); //f
        dataLv.pullLeft = bool.Parse(pullLeft.options[pullLeft.value].text); //f
        dataLv.pullRight = bool.Parse(pullRight.options[pullRight.value].text); //f

        //string json = JsonUtility.ToJson(dataLv);
        //File.WriteAllText(Application.dataPath + "/Resources/Level_" + dataLv.level + ".json", json);

        Board.dataLevel = dataLv;
        Board.instance._StartCreating();
        _SelectBackground(theme);

        preSettingPanel.SetActive(false);
        rowWhileEditing.text = row.text;
        columnWhileEditing.text = column.text;
        themeWhileEditing.value = theme.value;
        Debug.Log("Doneeeee");
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
            //transform.localScale = new Vector3(scaleWidth, scaleHeight, 0);
            temp.y = scaleHeight / height;
            temp.x = scaleWidth / width;
            bgr.transform.localScale = temp;
        }
    }

    public void _ChangeSetting()
    {
        preSettingPanel.SetActive(true);
        //GameObject btClose = GameObject.Find("PreSettingPanel/CloseButton");
        //btClose.SetActive(true);
    }
    #endregion

    #region Setting Up
    public void _EditMap()
    {
        dataLv.row = int.Parse(rowWhileEditing.text);
        dataLv.column = int.Parse(columnWhileEditing.text);

        Board.dataLevel = dataLv;
        Board.instance._StartCreating();
        _SelectBackground(themeWhileEditing);
    }
    #endregion

    #region Edit Tile
    public void _EditTile(Transform currentTile)
    {
        optionPanel.SetActive(true);

        btNone.onClick.RemoveAllListeners();
        btBlock.onClick.RemoveAllListeners();
        ddImage.onValueChanged.RemoveAllListeners();
        ddImage.value = ddImage.options.Count - 1;

        btNone.onClick.AddListener(delegate
        {
            _NoneTile(currentTile);
        });
        btBlock.onClick.AddListener(delegate
        {
            _BlockTile(currentTile);
        });
        ddImage.onValueChanged.AddListener(delegate
        {
            _ChooseTile(currentTile);
        });
    }

    public void _NoneTile(Transform currentTile)
    {
        currentTile.GetChild(0).GetComponent<Image>().sprite = btNone.GetComponent<Image>().sprite;
        optionPanel.SetActive(false);
    }

    public void _BlockTile(Transform currentTile)
    {
        currentTile.GetChild(0).GetComponent<Image>().sprite = btBlock.GetComponent<Image>().sprite;
        optionPanel.SetActive(false);
    }

    public void _ChooseTile(Transform currentTile)
    {
        currentTile.GetChild(0).GetComponent<Image>().sprite = TileImage.spritesDict.ElementAt(ddImage.value).Key;
        optionPanel.SetActive(false);
    }
    #endregion
}
