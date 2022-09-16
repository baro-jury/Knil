using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BoardController : MonoBehaviour
{
    public static BoardController instance;
    public static LevelData levelData;
    private List<Transform> buttonList = new List<Transform>();
    private List<Transform> buttonListWithoutBlocker = new List<Transform>();
    public int sideSmallTile = 112, sideMediumTile = 130, sideLargeTile = 180;
    private int process, row, column, orderOfPullingDirection;
    private bool pullDown, pullUp, pullLeft, pullRight;
    private string[,] matrix;
    
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
        _GoToProcess(1);
    }


    #region Khởi tạo level
    void _InstantiateProcess(int orderNumber)
    {
        process = orderNumber;
        row = levelData.process[orderNumber - 1].row;
        column = levelData.process[orderNumber - 1].column;
        pullDown = levelData.process[orderNumber - 1].pullDown;
        pullUp = levelData.process[orderNumber - 1].pullUp;
        pullLeft = levelData.process[orderNumber - 1].pullLeft;
        pullRight = levelData.process[orderNumber - 1].pullRight;
        matrix = levelData.process[orderNumber - 1].matrix;
    }

    void _GoToProcess(int order)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }
        orderOfPullingDirection = 0;
        _InstantiateProcess(order);
        _GenerateTiles();
    }
    #endregion

    #region Tạo Tiles
    void _GenerateTiles()
    {
        int num = NumberOfSameTiles();
        int repeat = 0, index = ResourceController.spritesDict.Count - 1;

        float sideTile = gameObject.GetComponent<RectTransform>().sizeDelta.x / column;
        float temp = gameObject.GetComponent<RectTransform>().sizeDelta.y / row;
        if (sideLargeTile <= sideTile)
        {
            if (sideLargeTile <= temp)
            {
                sideTile = sideLargeTile;
            }
            else if (sideMediumTile <= temp && temp < sideLargeTile)
            {
                sideTile = sideMediumTile;
            }
            else
            {
                sideTile = sideSmallTile;
            }

        }
        else if (sideMediumTile <= sideTile && sideTile < sideLargeTile)
        {
            if (sideMediumTile <= temp)
            {
                sideTile = sideMediumTile;
            }
            else
            {
                sideTile = sideSmallTile;
            }
        }
        else
        {
            sideTile = sideSmallTile;
        }
        float boardWidth = column * sideTile;
        float boardHeight = row * sideTile;

        Tile.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sideTile, sideTile);
        FirstAnchor.GetComponent<RectTransform>().sizeDelta = new Vector2(sideTile, sideTile);
        LastAnchor.GetComponent<RectTransform>().sizeDelta = new Vector2(sideTile, sideTile);

        #region Generate tile & phan tu ma tran + add vao buttonList
        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < column; c++)
            {
                if (matrix[r, c] != null && matrix[r, c] != "")
                {
                    Vector3 tilePos = _ConvertMatrixIndexToPosition(r, c, boardWidth, boardHeight, sideTile) / 40;
                    var objBtn = Instantiate(Tile, tilePos, Quaternion.identity, gameObject.transform);
                    if (matrix[r, c] == "?")
                    {
                        objBtn.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = ResourceController.spritesDict.ElementAt(index).Key;
                        repeat++;
                    }
                    else if (matrix[r, c] == "0")
                    {
                        objBtn.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = ResourceController.spritesDict.ElementAt(int.Parse(matrix[r, c])).Key;
                        objBtn.interactable = false;
                    }
                    else
                    {
                        objBtn.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = ResourceController.spritesDict.ElementAt(int.Parse(matrix[r, c])).Key;
                        buttonListWithoutBlocker.Add(objBtn.transform);
                    }
                    objBtn.transform.localPosition = tilePos * 40;
                    buttonList.Add(objBtn.transform);
                    if (repeat == num)
                    {
                        repeat = 0;
                        index--;
                        num = NumberOfSameTiles();
                    }
                }
            }
        }
        #endregion
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

    int[] _ConvertPositionToMatrixIndex(float x, float y, float boardWidth, float boardHeight, float tileSize)
    {
        int[] temp = new int[2];

        temp[0] = (int)(((boardHeight - tileSize) / 2 - y) / tileSize);  //row
        temp[1] = (int)(((boardWidth - tileSize) / 2 + x) / tileSize); //column

        return temp;
    }

    Vector3 _ConvertMatrixIndexToPosition(int row, int col, float boardWidth, float boardHeight, float tileSize)
    {
        return new Vector3((col * tileSize - (boardWidth - tileSize) / 2), ((boardHeight - tileSize) / 2 - row * tileSize), 0);
    }

    #endregion

    #region Sắp xếp Tiles
    bool _HasPossibleConnection()
    {
        if (buttonList.Count == 0)
        {
            return true;
        }
        for (int index = 0; index < ResourceController.spritesDict.Count; index++)
        {
            List<Transform> temp = _SearchSameTiles(ResourceController.spritesDict.ElementAt(index).Key);
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
        float side = Tile.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        for (int i = 0; i < (buttonList.Count - 1); i++)
        {
            int randomIndex = Random.Range(i + 1, buttonList.Count);

            Vector3 tempPos = buttonList[i].localPosition;
            buttonList[i].localPosition = buttonList[randomIndex].localPosition;
            buttonList[randomIndex].localPosition = tempPos;

            int[] tempI = _ConvertPositionToMatrixIndex(buttonList[i].localPosition.x, buttonList[i].localPosition.y, column * side, row * side, side);
            int rTempI = tempI[0];
            int cTempI = tempI[1];
            int[] tempRanI = _ConvertPositionToMatrixIndex(buttonList[i].localPosition.x, buttonList[randomIndex].localPosition.y, column * side, row * side, side);
            int rTempRanI = tempRanI[0];
            int cTempRanI = tempRanI[1]; ;

            string tempStr = matrix[rTempI, cTempI];
            matrix[rTempI, cTempI] = matrix[rTempRanI, cTempRanI];
            matrix[rTempRanI, cTempRanI] = tempStr;
        }
    }

    public void _SwapTiles(Transform t1, Transform t2)
    {
        float side = Tile.gameObject.GetComponent<RectTransform>().sizeDelta.x;

        Vector3 temp = t1.localPosition;
        t1.localPosition = t2.localPosition;
        t2.localPosition = temp;

        int rTemp1 = (int)(
            ((row * side - side) / 2 - t1.localPosition.y) / side
            );
        int cTemp1 = (int)(
            ((column * side - side) / 2 + t1.localPosition.x) / side
            );
        int rTemp2 = (int)(
            ((row * side - side) / 2 - t2.localPosition.y) / side
            );
        int cTemp2 = (int)(
            ((column * side - side) / 2 + t2.localPosition.x) / side
            );

        string temp1 = matrix[rTemp1, cTemp1];
        matrix[rTemp1, cTemp1] = matrix[rTemp2, cTemp2];
        matrix[rTemp2, cTemp2] = temp1;
    }

    #endregion

    #region Xử lý ingame
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
        float side = t.GetComponent<RectTransform>().sizeDelta.x;
        int rTemp = (int)(
                ((row * side - side) / 2 - t.localPosition.y) / side
                );
        int cTemp = (int)(
            ((column * side - side) / 2 + t.localPosition.x) / side
            );

        matrix[rTemp, cTemp] = "";
        buttonList.Remove(t);
        buttonListWithoutBlocker.Remove(t);
    }

    public void _CheckProcess()
    {
        if (buttonListWithoutBlocker.Count == 0)
        {
            if (process != levelData.process.Count)
            {
                _GoToProcess(process + 1);
            }
            else
            {
                if (levelData.level < 12 && levelData.level == ProgressController.instance._GetMarkedLevel())
                {
                    ProgressController.instance._MarkCurrentLevel(levelData.level + 1);
                }
                GameplayController.instance._CompleteLevel();
            }
        }
    }

    public List<Transform> _SearchSameTiles(Sprite sprite)
    {
        List<Transform> list = new List<Transform>();
        int value = 0;
        foreach (Transform trans in buttonListWithoutBlocker)
        {
            if (trans.GetChild(0).GetComponent<Image>().sprite == sprite)
            {
                list.Add(trans);
                value++;
            }
        }
        return list;
    }

    #endregion

    #region Xử lý quy luật sau khi ăn Tiles
    public void _SearchAndPullTile(int rowBefore, int colBefore, int rowAfter, int colAfter) // tìm Tile theo index trong ma tran
    {
        float side = Tile.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        foreach (Transform t in buttonListWithoutBlocker)
        {
            if (t.localPosition == new Vector3((colBefore * side - (column * side - side) / 2), ((row * side - side) / 2 - rowBefore * side), 0))
            {
                t.localPosition = new Vector3((colAfter * side - (column * side - side) / 2), ((row * side - side) / 2 - rowAfter * side), 0);
                break;
            }
        }
    }

    public void _ActivateGravity()
    {
        if (pullDown && !pullUp && !pullLeft && !pullRight)
        {
            _PullTilesToBottom(0, row - 1);
        }
        else if (!pullDown && pullUp && !pullLeft && !pullRight)
        {
            _PullTilesToTop(0, row - 1);
        }
        else if (!pullDown && !pullUp && pullLeft && !pullRight)
        {
            _PullTilesToLeft(0, column - 1);
        }
        else if (!pullDown && !pullUp && !pullLeft && pullRight)
        {
            _PullTilesToRight(0, column - 1);
        }
        else if (!pullDown && !pullUp && pullLeft && pullRight)
        {
            _PullTilesToLeft(0, column / 2 - 1);
            _PullTilesToRight(column / 2, column - 1);
        }
        else if (pullDown && pullUp && !pullLeft && !pullRight)
        {
            _PullTilesToTop(0, row / 2 - 1);
            _PullTilesToBottom(row / 2, row - 1);
        }
        else if (pullDown && pullUp && pullLeft && pullRight)
        {
            switch (orderOfPullingDirection % 4)
            {
                case 0:
                    _PullTilesToTop(0, row - 1);
                    break;
                case 1:
                    _PullTilesToRight(0, column - 1);
                    break;
                case 2:
                    _PullTilesToBottom(0, row - 1);
                    break;
                case 3:
                    _PullTilesToLeft(0, column - 1);
                    break;
            }
            if (levelData.level % 2 == 0)
            {
                orderOfPullingDirection++;
            }
            else
            {
                orderOfPullingDirection += 3;
            }
        }
    }

    void _PullTilesToBottom(int rowFirst, int rowLast)
    {
        var tempArr = new string[rowLast - rowFirst + 1];

        for (int c = 0; c < column; c++)
        {
            for (int r = rowFirst; r <= rowLast; r++)
            {
                tempArr[r - rowFirst] = matrix[r, c];
            }

            for (int i = 0; i < tempArr.Length - 1; i++)
            {
                for (int j = 0; j < tempArr.Length - i - 1; j++)
                {
                    if (tempArr[j] != "0" && tempArr[j + 1] != "0")
                    {
                        if (tempArr[j + 1] == "")
                        {
                            string temp = tempArr[j + 1];
                            tempArr[j + 1] = tempArr[j];
                            tempArr[j] = temp;

                            _SearchAndPullTile(j + rowFirst, c, j + rowFirst + 1, c);
                        }
                    }
                }
            }

            for (int r = rowFirst; r <= rowLast; r++)
            {
                matrix[r, c] = tempArr[r - rowFirst];
            }
        }
    }

    void _PullTilesToTop(int rowFirst, int rowLast)
    {
        var tempArr = new string[rowLast - rowFirst + 1];

        for (int c = 0; c < column; c++)
        {
            for (int r = rowFirst; r <= rowLast; r++)
            {
                tempArr[r - rowFirst] = matrix[r, c];
            }

            for (int i = 0; i < tempArr.Length - 1; i++)
            {
                for (int j = 0; j < tempArr.Length - i - 1; j++)
                {
                    if (tempArr[j] != "0" && tempArr[j + 1] != "0")
                    {
                        if (tempArr[j] == "")
                        {
                            string temp = tempArr[j];
                            tempArr[j] = tempArr[j + 1];
                            tempArr[j + 1] = temp;

                            _SearchAndPullTile(j + rowFirst + 1, c, j + rowFirst, c);
                        }
                    }
                }
            }

            for (int r = rowFirst; r <= rowLast; r++)
            {
                matrix[r, c] = tempArr[r - rowFirst];
            }
        }
    }

    void _PullTilesToLeft(int colFirst, int colLast)
    {
        var tempArr = new string[colLast - colFirst + 1];

        for (int r = 0; r < row; r++)
        {
            for (int c = colFirst; c <= colLast; c++)
            {
                tempArr[c - colFirst] = matrix[r, c];
            }

            for (int i = 0; i < tempArr.Length - 1; i++)
            {
                for (int j = 0; j < tempArr.Length - i - 1; j++)
                {
                    if (tempArr[j] != "0" && tempArr[j + 1] != "0")
                    {
                        if (tempArr[j] == "")
                        {
                            string temp = tempArr[j];
                            tempArr[j] = tempArr[j + 1];
                            tempArr[j + 1] = temp;

                            _SearchAndPullTile(r, j + colFirst + 1, r, j + colFirst);
                        }
                    }
                }
            }

            for (int c = colFirst; c <= colLast; c++)
            {
                matrix[r, c] = tempArr[c - colFirst];
            }
        }
    }

    void _PullTilesToRight(int colFirst, int colLast)
    {
        var tempArr = new string[colLast - colFirst + 1];

        for (int r = 0; r < row; r++)
        {
            for (int c = colFirst; c <= colLast; c++)
            {
                tempArr[c - colFirst] = matrix[r, c];
            }

            for (int i = 0; i < tempArr.Length - 1; i++)
            {
                for (int j = 0; j < tempArr.Length - i - 1; j++)
                {
                    if (tempArr[j] != "0" && tempArr[j + 1] != "0")
                    {
                        if (tempArr[j + 1] == "")
                        {
                            string temp = tempArr[j + 1];
                            tempArr[j + 1] = tempArr[j];
                            tempArr[j] = temp;

                            _SearchAndPullTile(r, j + colFirst, r, j + colFirst + 1);
                        }
                    }
                }
            }

            for (int c = colFirst; c <= colLast; c++)
            {
                matrix[r, c] = tempArr[c - colFirst];
            }
        }
    }

    #endregion
}

