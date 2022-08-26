using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BoardController : MonoBehaviour
{
    public static BoardController instance;
    public static DataLevels dataLevel;
    public int row = 6, column = 5;
    private bool pullDown, pullUp, pullLeft, pullRight;
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

    void _InstantiateLevel()
    {
        ProgressController.instance._MarkCurrentLevel(dataLevel.level);
        row = dataLevel.row;
        column = dataLevel.column;
        TimeController.time = dataLevel.time;
        pullDown = dataLevel.pullDown;
        pullUp = dataLevel.pullUp;
        pullLeft = dataLevel.pullLeft;
        pullRight = dataLevel.pullRight;

        TimeController.instance._SetTimeForSlider(false);
    }

    void Awake()
    {
        _MakeInstance();
    }

    void Start()
    {
        _InstantiateLevel();
        _GenerateTiles();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            buttonList.Add(gameObject.transform.GetChild(i));
        }
        _RearrangeTiles();
        _ShuffleWhenNoPossibleLink();

        //Debug.Log("Child = " + gameObject.transform.childCount + "\nList = " + buttonList.Count);
    }

    #region Khởi tạo màn chơi
    void _GenerateTiles()
    {
        int num = NumberOfSameTiles();
        int index = 0, repeat = 0;
        Matrix = new int[row, column];

        float boardWidth = gameObject.GetComponent<RectTransform>().sizeDelta.x;
        //float boardHeight = gameObject.GetComponent<RectTransform>().sizeDelta.y;
        float sideTile = boardWidth / column;
        float boardHeight = row * sideTile;

        Tile.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sideTile, sideTile);
        FirstAnchor.GetComponent<RectTransform>().sizeDelta = new Vector2(sideTile, sideTile);
        LastAnchor.GetComponent<RectTransform>().sizeDelta = new Vector2(sideTile, sideTile);

        #region Generate theo toa do
        //for (float y = (boardHeight - sideTile) / 2; y > -boardHeight / 2; y -= sideTile)
        //{
        //    for (float x = -(boardWidth - sideTile) / 2; x < boardWidth / 2; x += sideTile)
        //    {
        //        Vector3 tilePos = new Vector3(x, y, 0) / 40;
        //        var objBtn = Instantiate(Tile, tilePos, Quaternion.identity, gameObject.transform);
        //        objBtn.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = ResourceController.spritesDict.ElementAt(index).Key;
        //        objBtn.transform.localPosition = tilePos * 40;
        //        repeat++;
        //        if (repeat == num)
        //        {
        //            repeat = 0;
        //            num = NumberOfSameTiles();
        //            index++;
        //        }
        //    }
        //}
        #endregion

        #region Generate tile & phan tu ma tran
        for (int y = 0; y < row; y++)
        {
            for (int x = 0; x < column; x++)
            {
                Vector3 tilePos = new Vector3((x * sideTile - (boardWidth - sideTile) / 2), ((boardHeight - sideTile) / 2 - y * sideTile), 0) / 40;
                var objBtn = Instantiate(Tile, tilePos, Quaternion.identity, gameObject.transform);
                objBtn.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = ResourceController.spritesDict.ElementAt(index).Key;
                objBtn.transform.localPosition = tilePos * 40;
                Matrix[y, x] = ResourceController.spritesDict[objBtn.gameObject.transform.GetChild(0).GetComponent<Image>().sprite];
                repeat++;
                if (repeat == num)
                {
                    repeat = 0;
                    num = NumberOfSameTiles();
                    index++;
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
        float side = Tile.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        for (int i = 0; i < (buttonList.Count - 1); i++)
        {
            int randomIndex = Random.Range(i + 1, buttonList.Count);

            Vector3 temp = buttonList[i].localPosition;
            buttonList[i].localPosition = buttonList[randomIndex].localPosition;
            buttonList[randomIndex].localPosition = temp;

            int rTempI = (int)(
                ((row * side - side) / 2 - buttonList[i].localPosition.y) / side
                );
            int cTempI = (int)(
                ((column * side - side) / 2 + buttonList[i].localPosition.x) / side
                );
            int rTempRanI = (int)(
                ((row * side - side) / 2 - buttonList[randomIndex].localPosition.y) / side
                );
            int cTempRanI = (int)(
                ((column * side - side) / 2 + buttonList[randomIndex].localPosition.x) / side
                );

            int tempI = Matrix[rTempI, cTempI];
            Matrix[rTempI, cTempI] = Matrix[rTempRanI, cTempRanI];
            Matrix[rTempRanI, cTempRanI] = tempI;
        }
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

    public void _DeactivateTile(Transform t) //okay
    {
        float side = t.GetComponent<RectTransform>().sizeDelta.x;
        int rTemp = (int)(
                ((row * side - side) / 2 - t.localPosition.y) / side
                );
        int cTemp = (int)(
            ((column * side - side) / 2 + t.localPosition.x) / side
            );

        Matrix[rTemp, cTemp] = -1;
        buttonList.Remove(t);
        if (buttonList.Count == 0)
        {
            GameplayController.instance._CompleteLevel();
        }
    }

    public List<Transform> _SearchTiles(Sprite sprite) // tìm Tiles giống nhau
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

    #endregion

    #region Xử lý quy luật sau khi ăn Tiles

    #region MergeSort
    void MergeArray(int[] array, int left, int middle, int right)
    {
        var leftArrayLength = middle - left + 1;
        var rightArrayLength = right - middle;
        var leftTempArray = new int[leftArrayLength];
        var rightTempArray = new int[rightArrayLength];
        int i, j;
        for (i = 0; i < leftArrayLength; ++i)
            leftTempArray[i] = array[left + i];
        for (j = 0; j < rightArrayLength; ++j)
            rightTempArray[j] = array[middle + 1 + j];
        i = 0;
        j = 0;
        int k = left;
        while (i < leftArrayLength && j < rightArrayLength)
        {
            if (leftTempArray[i] <= rightTempArray[j] && leftTempArray[i] < 0)
            {
                array[k++] = leftTempArray[i++];
            }
            else if (leftTempArray[i] > rightTempArray[j] && rightTempArray[j] < 0)
            {
                array[k++] = rightTempArray[j++];
            }
            else
            {
                array[k++] = leftTempArray[i++];
            }
        }
        while (i < leftArrayLength)
        {
            array[k++] = leftTempArray[i++];
        }
        while (j < rightArrayLength)
        {
            array[k++] = rightTempArray[j++];
        }
    }

    int[] SortRow(int[] array, int left, int right)
    {
        if (left < right)
        {
            int middle = left + (right - left) / 2;
            SortRow(array, left, middle);
            SortRow(array, middle + 1, right);
            MergeArray(array, left, middle, right);
        }
        return array;
    }
    #endregion

    public void _SearchAndPullTile(int rowBefore, int colBefore, int rowAfter, int colAfter) // tìm Tile theo index trong ma tran
    {
        float side = Tile.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        foreach (Transform t in buttonList)
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
            _PullTilesToBottom();
        }
        else if (!pullDown && pullUp && !pullLeft && !pullRight)
        {
            _PullTilesToTop();
        }
        else if (!pullDown && !pullUp && pullLeft && !pullRight)
        {
            _PullTilesToLeft();
        }
        else if (!pullDown && !pullUp && !pullLeft && pullRight)
        {
            _PullTilesToRight();
        }
    }

    void _PullTilesToBottom()
    {
        var tempArr = new int[row];
        int temp;
        for (int c = 0; c < column; c++)
        {
            for (int r = 0; r < row; r++)
            {
                tempArr[r] = Matrix[r, c];
            }

            for (int i = 0; i < tempArr.Length - 1; i++)
            {
                for (int j = 0; j < tempArr.Length - i - 1; j++)
                {
                    if (tempArr[j] > tempArr[j + 1] && tempArr[j + 1] < 0)
                    {
                        temp = tempArr[j + 1];
                        tempArr[j + 1] = tempArr[j];
                        tempArr[j] = temp;

                        _SearchAndPullTile(j, c, j + 1, c);
                    }
                }
            }

            for (int r = 0; r < row; r++)
            {
                Matrix[r, c] = tempArr[r];
            }
        }
    }

    void _PullTilesToTop()
    {
        var tempArr = new int[row];
        int temp;
        for (int c = 0; c < column; c++)
        {
            for (int r = 0; r < row; r++)
            {
                tempArr[r] = Matrix[r, c];
            }

            for (int i = 0; i < tempArr.Length - 1; i++)
            {
                for (int j = 0; j < tempArr.Length - i - 1; j++)
                {
                    if (tempArr[j] < tempArr[j + 1] && tempArr[j] < 0)
                    {
                        temp = tempArr[j];
                        tempArr[j] = tempArr[j + 1];
                        tempArr[j + 1] = temp;

                        _SearchAndPullTile(j + 1, c, j, c);
                    }
                }
            }

            for (int r = 0; r < row; r++)
            {
                Matrix[r, c] = tempArr[r];
            }
        }
    }

    void _PullTilesToLeft()
    {
        var tempArr = new int[column];
        int temp;
        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < column; c++)
            {
                tempArr[c] = Matrix[r, c];
            }

            for (int i = 0; i < tempArr.Length - 1; i++)
            {
                for (int j = 0; j < tempArr.Length - i - 1; j++)
                {
                    if (tempArr[j] < tempArr[j + 1] && tempArr[j] < 0)
                    {
                        temp = tempArr[j];
                        tempArr[j] = tempArr[j + 1];
                        tempArr[j + 1] = temp;

                        _SearchAndPullTile(r, j + 1, r, j);
                    }
                }
            }

            for (int c = 0; c < column; c++)
            {
                Matrix[r, c] = tempArr[c];
            }
        }
    }

    void _PullTilesToRight()
    {
        var tempArr = new int[column];
        int temp;
        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < column; c++)
            {
                tempArr[c] = Matrix[r, c];
            }
            //tempArr = SortRow(tempArr, 0, column-1);

            for (int i = 0; i < tempArr.Length - 1; i++)
            {
                for (int j = 0; j < tempArr.Length - i - 1; j++)
                {
                    if (tempArr[j] > tempArr[j + 1] && tempArr[j + 1] < 0)
                    {
                        temp = tempArr[j + 1];
                        tempArr[j + 1] = tempArr[j];
                        tempArr[j] = temp;

                        _SearchAndPullTile(r, j, r, j + 1);
                    }
                }
            }

            for (int c = 0; c < column; c++)
            {
                Matrix[r, c] = tempArr[c];
            }
        }
        Debug.Log("Start");
        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < column; c++)
            {
                Debug.Log(Matrix[r, c]);
            }
        }
        Debug.Log("End");
    }

    #endregion
}
