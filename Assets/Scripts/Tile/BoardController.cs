using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BoardController : MonoBehaviour
{
    public static BoardController instance;
    public int row = 6, column = 5;
    private List<Transform> buttonList = new List<Transform>();
    private int[,] Matrix;

    [SerializeField]
    private Button Tile;
    [SerializeField]
    private GameObject FirstAnchor, LastAnchor;

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
        _GenerateTiles();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            buttonList.Add(gameObject.transform.GetChild(i));
        }
        _RearrangeTiles();
        _ShuffleWhenNoPossibleLink();

        Debug.Log("Child = " + gameObject.transform.childCount + "\nList = " + buttonList.Count);
        for (int r = 0; r < (row + 2); r++)
        {
            for (int c = 0; c < (column + 2); c++)
            {
                Debug.Log(Matrix[r, c]);
            }
        }
    }

    void _GenerateTiles()
    {
        int num = NumberOfSameTiles();
        int index = 0, repeat = 0;
        int indexRow = 1, indexCol = 1;
        _SetUpMatrix();

        float boardWidth = gameObject.GetComponent<RectTransform>().sizeDelta.x;
        //float boardHeight = gameObject.GetComponent<RectTransform>().sizeDelta.y;
        float sideTile = boardWidth / column;
        float boardHeight = row * sideTile;

        Tile.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sideTile, sideTile);
        FirstAnchor.GetComponent<RectTransform>().sizeDelta = new Vector2(sideTile, sideTile);
        LastAnchor.GetComponent<RectTransform>().sizeDelta = new Vector2(sideTile, sideTile);
        for (float y = (boardHeight - sideTile) / 2; y > -boardHeight / 2; y -= sideTile)
        {
            for (float x = -(boardWidth - sideTile) / 2; x < boardWidth / 2; x += sideTile)
            {
                if (indexCol > column)
                {
                    indexRow++;
                    indexCol = 1;
                }
                Vector3 tilePos = new Vector3(x, y, 0) / 40;
                var objBtn = Instantiate(Tile, tilePos, Quaternion.identity, gameObject.transform);
                objBtn.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = ResourceController.spritesDict.ElementAt(index).Key;
                objBtn.transform.localPosition = tilePos * 40;
                Matrix[indexRow, indexCol] = ResourceController.spritesDict[objBtn.gameObject.transform.GetChild(0).GetComponent<Image>().sprite];
                repeat++;
                indexCol++;
                if (repeat == num)
                {
                    repeat = 0;
                    num = NumberOfSameTiles();
                    index++;
                }
            }
        }
    }

    void _SetUpMatrix()
    {
        Matrix = new int[row + 2, column + 2];
        for (int r = 0; r < (row + 2); r++)
        {
            for (int c = 0; c < (column + 2); c++)
            {
                if (r == 0 || r == row + 1)
                {
                    Matrix[r, c] = -1;
                }
                else
                {
                    if (c == 0 || c == column + 1)
                    {
                        Matrix[r, c] = -1;
                    }
                    else
                    {
                        //Matrix[r, c] = 0;
                    }
                }
            }
        }
    }

    int NumberOfSameTiles()
    {
        int num = Random.Range(2, 5);
        while (num % 2 == 1)
        {
            num = Random.Range(2, 5);
        }
        return num;
    }

    //void _GenerateTilesWithGrid()
    //{
    //    int NumberOfSameTiles;
    //    for (int i = 0; i < 15; i++)
    //    {
    //        NumberOfSameTiles = Random.Range(2, 5);
    //        while (NumberOfSameTiles % 2 == 1)
    //        {
    //            NumberOfSameTiles = Random.Range(2, 5);
    //        }
    //        for (int j = 0; j < NumberOfSameTiles; j++)
    //        {
    //            if (gameObject.transform.childCount == 30)
    //            {
    //                break;
    //            }
    //            Tile.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = ResourceController.spritesDict.ElementAt(i).Key;
    //            Instantiate(Tile, gameObject.transform);
    //        }
    //    }
    //}

    bool _HasPossibleConnection()
    {
        if (buttonList.Count == 0)
        {
            return true;
        }
        for (int index = 0; index < ResourceController.spritesDict.Count; index++)
        {
            List<Transform> temp = _SearchTiles(ResourceController.spritesDict.ElementAt(index).Key);
            if (temp.Count != 0)
            {
                for (int i = 0; i < (temp.Count - 1); i++)
                {
                    for (int j = (i + 1); j < temp.Count; j++)
                    {
                        if (GameplayController.instance._HasAvailableConnection(temp[i], temp[j]))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public void _ShuffleWhenNoPossibleLink()
    {
        while (!_HasPossibleConnection())
        {
            _RearrangeTiles();
        }
    }

    public void _RearrangeTiles()
    {
        for (int i = 0; i < (buttonList.Count - 1); i++)
        {
            Vector3 temp = buttonList[i].localPosition;
            int randomIndex = Random.Range(i + 1, buttonList.Count);
            buttonList[i].localPosition = buttonList[randomIndex].localPosition;
            buttonList[randomIndex].localPosition = temp;
        }

    }

    public bool _HasButtonInLocation(float x, float y)
    {
        foreach (Transform trans in buttonList)
        {
            if (new Vector3(x, y, 0) == trans.localPosition)
            {
                return true;
            }
        }
        return false;
    }

    public void _DeactivateTile(Transform t)
    {
        buttonList.Remove(t);
        if (buttonList.Count == 0)
        {
            GameplayController.instance._CompleteLevel();
        }
    }

    public List<Transform> _SearchTiles(Sprite sprite)
    {
        List<Transform> list = new List<Transform>();
        foreach (Transform trans in buttonList)
        {
            if (trans.GetChild(0).GetComponent<Image>().sprite == sprite)
            {
                list.Add(trans);
            }
        }
        return list;
    }

}
