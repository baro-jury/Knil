using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BoardController : MonoBehaviour
{
    public static BoardController instance;
    public int row = 6, column = 5;

    [SerializeField]
    private Button Tile;
    [HideInInspector]
    public List<Transform> buttonList = new List<Transform>();
    [HideInInspector]
    public int[,] Matrix = new int[6 + 2, 5 + 2];  //[row,col]

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
        ResourceController.instance._CreateDictionary();
        //_GenerateTiles();
        //_GenerateTilesWithGrid();

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            buttonList.Add(gameObject.transform.GetChild(i));
        }
        //Debug.Log(gameObject.transform.childCount);
        //_CheckEvenTileNumber();

        //for (int r = 0; r < 8; r++)
        //{
        //    for (int c = 0; c < 7; c++)
        //    {
        //        if (r == 0 || r == 7)
        //        {
        //            Matrix[r, c] = -1;
        //        }
        //        else
        //        {
        //            if(c == 0 || c == 6)
        //            {
        //                Matrix[r, c] = -1;
        //            }
        //            else
        //            {

        //            }
        //        }
        //    }
        //}
    }

    void _GenerateTiles()
    {
        int i = 0, j = 0;
        float boardWidth = gameObject.GetComponent<RectTransform>().sizeDelta.x;
        float boardHeight = gameObject.GetComponent<RectTransform>().sizeDelta.y;
        float sideTile = boardWidth / column;
        Tile.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sideTile, sideTile);
        for (float y = -sideTile / 2; y > -boardHeight; y -= sideTile)
        {
            for (float x = sideTile / 2; x < boardWidth; x += sideTile)
            {
                Button temp = Tile;

                temp.gameObject.GetComponent<RectTransform>().position = new Vector3(x, y, 0);
                temp.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = ResourceController.spritesDict.ElementAt(i).Key;
                //temp.gameObject.SetActive = true;
                Instantiate(temp, gameObject.transform);
                j++;
                i = j / 2;
            }
        }
        //for (float y = (boardHeight - sideTile) / 2; y > -boardHeight / 2; y -= sideTile)
        //{
        //    for (float x = -(boardWidth - sideTile) / 2; x < boardWidth / 2; x += sideTile)
        //    {
        //        Button temp = Tile;

        //        temp.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = ResourceController.spritesDict.ElementAt(i).Key;
        //        temp.gameObject.transform.localPosition = new Vector3(x, y, 0);
        //        Instantiate(temp, gameObject.transform);
        //        //Instantiate(temp);
        //        j++;
        //        i = j / 2;
        //    }
        //}
    }

    void _GenerateTilesWithGrid()
    {
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                Tile.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = ResourceController.spritesDict.ElementAt(i).Key;
                Instantiate(Tile, gameObject.transform);
            }
        }
    }

    public void _CheckEvenTileNumber()
    {
        List<Transform> temp = buttonList;
        int i = 0, n = temp.Count;

        foreach (Transform trans in temp)
        {
            bool isAlone = true;
            Transform t = trans;
            for (int j = i + 1; j < n; j++)
            {
                if (temp[j].GetChild(0).GetComponent<Image>().sprite == trans.GetChild(0).GetComponent<Image>().sprite)
                {
                    isAlone = false;
                    t = temp[j];
                    break;
                }
            }
            if (isAlone)
            {
                i++;
            }
            else
            {
                temp.Remove(t);
                temp.Remove(trans);
                n -= 2;
            }
        }

        if (temp.Count > 0)
        {
            for (i = 0; i < temp.Count; i++)
            {
                Instantiate(Tile, gameObject.transform);
                Tile.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = temp[i].GetChild(0).GetComponent<Image>().sprite;
                buttonList.Add(Tile.transform);
            }
        }
    }

    public bool _HasButtonInLocation(float x, float y)
    {
        foreach (Transform trans in buttonList)
        {
            if (new Vector3(x, y, 0) == trans.localPosition &&
                ResourceController.spritesDict[trans.GetChild(0).GetComponent<Image>().sprite] != -1)
            {
                return true;
            }
        }
        return false;
    }

}
