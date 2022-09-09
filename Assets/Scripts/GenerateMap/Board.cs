using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public static Board instance;
    public static DataLevels dataLevel;
    private int row, column, orderOfPullingDirection;
    private bool pullDown, pullUp, pullLeft, pullRight;
    private List<Transform> buttonList = new List<Transform>();
    private int[,] Matrix;
    public int sideSmallTile = 112, sideMediumTile = 130, sideLargeTile = 180;

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

    public void _StartCreating()
    {
        //orderOfPullingDirection = 0;
        _ResetBoard();
        _InstantiateLevel();
        _GenerateTiles();
        //for (int i = 0; i < gameObject.transform.childCount; i++)
        //{
        //    buttonList.Add(gameObject.transform.GetChild(i));
        //}
        //_RearrangeTiles();
        //_ShuffleWhenNoPossibleLink();
    }

    #region Khởi tạo creative map
    void _ResetBoard()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    void _InstantiateLevel()
    {
        row = dataLevel.row;
        column = dataLevel.column;
        TimeController.time = dataLevel.time;
        pullDown = dataLevel.pullDown;
        pullUp = dataLevel.pullUp;
        pullLeft = dataLevel.pullLeft;
        pullRight = dataLevel.pullRight;

        //TimeController.instance._SetTimeForSlider(false);
    }

    void _GenerateTiles()
    {
        Matrix = new int[row, column];

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

        #region Generate theo toa do
        for (float y = (boardHeight - sideTile) / 2; y > -boardHeight / 2; y -= sideTile)
        {
            for (float x = -(boardWidth - sideTile) / 2; x < boardWidth / 2; x += sideTile)
            {
                Vector3 tilePos = new Vector3(x, y, 0) / 40;
                var objBtn = Instantiate(Tile, tilePos, Quaternion.identity, gameObject.transform);
                objBtn.transform.localPosition = tilePos * 40;
                //PrefabUtility.UnpackPrefabInstance(objBtn.gameObject, PrefabUnpackMode.Completely, 0);
            }
        }
        #endregion

        #region Generate tile & phan tu ma tran
        //for (int y = 0; y < row; y++)
        //{
        //    for (int x = 0; x < column; x++)
        //    {
        //        Vector3 tilePos = new Vector3((x * sideTile - (boardWidth - sideTile) / 2), ((boardHeight - sideTile) / 2 - y * sideTile), 0) / 40;
        //        var objBtn = Instantiate(Tile, tilePos, Quaternion.identity, gameObject.transform);
        //        objBtn.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = ResourceController.spritesDict.ElementAt(index).Key;
        //        objBtn.transform.localPosition = tilePos * 40;
        //        Matrix[y, x] = ResourceController.spritesDict[objBtn.gameObject.transform.GetChild(0).GetComponent<Image>().sprite];
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
    }

    void _ConvertPositionToMatrixIndex(float x, float y)
    {

    }

    void _ConvertMatrixIndexToPosition(int r, int c)
    {

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

        int temp1 = Matrix[rTemp1, cTemp1];
        Matrix[rTemp1, cTemp1] = Matrix[rTemp2, cTemp2];
        Matrix[rTemp2, cTemp2] = temp1;
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

        Matrix[rTemp, cTemp] = -1;
        buttonList.Remove(t);

        if (buttonList.Count == 0)
        {
            if (dataLevel.level < 12 && dataLevel.level == ProgressController.instance._GetMarkedLevel())
            {
                ProgressController.instance._MarkCurrentLevel(dataLevel.level + 1);
            }
            orderOfPullingDirection = 0;

            StartCoroutine(ShowPopupWhenComplete());
        }
    }

    IEnumerator ShowPopupWhenComplete()
    {
        yield return new WaitForSeconds(0.75f);
        GameplayController.instance._CompleteLevel();
    }

    public List<Transform> _SearchSameTiles(Sprite sprite)
    {
        List<Transform> list = new List<Transform>();
        int value = 0;
        foreach (Transform trans in buttonList)
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
            if (dataLevel.level % 2 == 0)
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
        var tempArr = new int[rowLast - rowFirst + 1];

        for (int c = 0; c < column; c++)
        {
            for (int r = rowFirst; r <= rowLast; r++)
            {
                tempArr[r - rowFirst] = Matrix[r, c];
            }

            for (int i = 0; i < tempArr.Length - 1; i++)
            {
                for (int j = 0; j < tempArr.Length - i - 1; j++)
                {
                    if (tempArr[j] > tempArr[j + 1] && tempArr[j + 1] < 0)
                    {
                        int temp = tempArr[j + 1];
                        tempArr[j + 1] = tempArr[j];
                        tempArr[j] = temp;

                        _SearchAndPullTile(j + rowFirst, c, j + rowFirst + 1, c);
                    }
                }
            }

            for (int r = rowFirst; r <= rowLast; r++)
            {
                Matrix[r, c] = tempArr[r - rowFirst];
            }
        }
    }

    void _PullTilesToTop(int rowFirst, int rowLast)
    {
        var tempArr = new int[rowLast - rowFirst + 1];

        for (int c = 0; c < column; c++)
        {
            for (int r = rowFirst; r <= rowLast; r++)
            {
                tempArr[r - rowFirst] = Matrix[r, c];
            }

            for (int i = 0; i < tempArr.Length - 1; i++)
            {
                for (int j = 0; j < tempArr.Length - i - 1; j++)
                {
                    if (tempArr[j] < tempArr[j + 1] && tempArr[j] < 0)
                    {
                        int temp = tempArr[j];
                        tempArr[j] = tempArr[j + 1];
                        tempArr[j + 1] = temp;

                        _SearchAndPullTile(j + rowFirst + 1, c, j + rowFirst, c);
                    }
                }
            }

            for (int r = rowFirst; r <= rowLast; r++)
            {
                Matrix[r, c] = tempArr[r - rowFirst];
            }
        }
    }

    void _PullTilesToLeft(int colFirst, int colLast)
    {
        var tempArr = new int[colLast - colFirst + 1];

        for (int r = 0; r < row; r++)
        {
            for (int c = colFirst; c <= colLast; c++)
            {
                tempArr[c - colFirst] = Matrix[r, c];
            }

            for (int i = 0; i < tempArr.Length - 1; i++)
            {
                for (int j = 0; j < tempArr.Length - i - 1; j++)
                {
                    if (tempArr[j] < tempArr[j + 1] && tempArr[j] < 0)
                    {
                        int temp = tempArr[j];
                        tempArr[j] = tempArr[j + 1];
                        tempArr[j + 1] = temp;

                        _SearchAndPullTile(r, j + colFirst + 1, r, j + colFirst);
                    }
                }
            }

            for (int c = colFirst; c <= colLast; c++)
            {
                Matrix[r, c] = tempArr[c - colFirst];
            }
        }
    }

    void _PullTilesToRight(int colFirst, int colLast)
    {
        var tempArr = new int[colLast - colFirst + 1];

        for (int r = 0; r < row; r++)
        {
            for (int c = colFirst; c <= colLast; c++)
            {
                tempArr[c - colFirst] = Matrix[r, c];
            }
            //tempArr = SortRow(tempArr, 0, column-1);

            for (int i = 0; i < tempArr.Length - 1; i++)
            {
                for (int j = 0; j < tempArr.Length - i - 1; j++)
                {
                    if (tempArr[j] > tempArr[j + 1] && tempArr[j + 1] < 0)
                    {
                        int temp = tempArr[j + 1];
                        tempArr[j + 1] = tempArr[j];
                        tempArr[j] = temp;

                        _SearchAndPullTile(r, j + colFirst, r, j + colFirst + 1);
                    }
                }
            }

            for (int c = colFirst; c <= colLast; c++)
            {
                Matrix[r, c] = tempArr[c - colFirst];
            }
        }
    }

    #endregion
}
