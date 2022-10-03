using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class TrialBoard : MonoBehaviour
{
    public static TrialBoard instance;
    public static LevelData levelData;
    private List<Transform> buttonList = new();
    private List<TileController> buttonListWithoutBlocker = new();
    private Dictionary<Sprite, string> dict = new();
    public int sideSmallTile = 112, sideMediumTile = 130, sideLargeTile = 180;
    private int process, row, column, orderOfPullingDirection;
    private bool pullDown, pullUp, pullLeft, pullRight;
    private string[,] matrix;

    [SerializeField]
    private TileController Tile;
    [SerializeField]
    private GameObject FirstAnchor, LastAnchor;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            Debug.Log(JsonConvert.SerializeObject(matrix));
            string temp = null;
            foreach (var item in buttonList)
            {
                temp += item.name + " ";
            }
            Debug.Log(temp);
        }
    }

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
        foreach (KeyValuePair<Sprite, string> kvp in ResourceController.spritesDict)
        {
            dict.Add(kvp.Key, kvp.Value);
        }
    }

    #region Khởi tạo màn chơi
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

    public void _GoToProcess(int order)
    {
        buttonList.Clear();
        buttonListWithoutBlocker.Clear();
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
        //ResourceController.instance._ShuffleImage(dict);

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
                if (matrix[r, c] != "")
                {
                    Vector3 tilePos = _ConvertMatrixIndexToPosition(r, c, boardWidth, boardHeight, sideTile) / 40;
                    var objBtn = Instantiate(Tile, tilePos, Quaternion.identity, gameObject.transform);
                    int ID = -1;
                    if (matrix[r, c] == "?")
                    {
                        ID = int.Parse(dict.ElementAt(index).Value);
                        objBtn.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = dict.ElementAt(ID).Key;
                        buttonListWithoutBlocker.Add(objBtn);
                        repeat++;
                    }
                    else if (matrix[r, c] == "0")
                    {
                        ID = int.Parse(dict.ElementAt(0).Value);
                        objBtn.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = dict.ElementAt(0).Key;
                        objBtn.GetComponent<Button>().interactable = false;
                    }
                    else
                    {
                        for (int i = 0; i < dict.Count; i++)
                        {
                            if (dict.ElementAt(i).Value == matrix[r, c])
                            {
                                ID = int.Parse(dict.ElementAt(i).Value);
                                objBtn.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = dict.ElementAt(i).Key;
                                break;
                            }
                        }
                        buttonListWithoutBlocker.Add(objBtn);
                    }
                    //objBtn.gameObject.GetComponent<TileController>().Id = ID;
                    //objBtn.gameObject.GetComponent<TileController>().Index = (r, c);
                    objBtn.Id = ID;
                    objBtn.Index = (r, c);

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
        if (buttonListWithoutBlocker.Count == 0)
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
                        if (GameplayTrial.instance._HasAvailableConnection(temp[i], temp[j]))
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
        DOTween.Clear();
        GameplayTrial.instance._PlayParticles(false, false, true);
        float side = Tile.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        //for (int i = 0; i < (buttonListWithoutBlocker.Count - 1); i++)
        //{
        //    int randomIndex = Random.Range(i + 1, buttonListWithoutBlocker.Count);
        //    _SwapPos(buttonListWithoutBlocker[i].transform, buttonListWithoutBlocker[randomIndex].transform, 0);

        //    //Vector3 tempPos1 = buttonListWithoutBlocker[i].localPosition;
        //    //Vector3 tempPos2 = buttonListWithoutBlocker[randomIndex].localPosition;

        //    //buttonListWithoutBlocker[i].DOLocalMove(Vector3.zero, .25f).SetEase(Ease.InOutQuad)
        //    //    .OnComplete(() =>
        //    //    {
        //    //        buttonListWithoutBlocker[i].DOLocalMove(tempPos2, .25f).SetEase(Ease.InOutQuad);
        //    //    });
        //    //buttonListWithoutBlocker[randomIndex].DOLocalMove(Vector3.zero, .25f).SetEase(Ease.InOutQuad)
        //    //    .OnComplete(() =>
        //    //    {
        //    //        buttonListWithoutBlocker[randomIndex].DOLocalMove(tempPos1, .25f).SetEase(Ease.InOutQuad);
        //    //    });

        //    //sequence
        //    //    .Insert(1, buttonListWithoutBlocker[i].DOLocalMove(Vector3.zero, .25f).SetEase(Ease.InOutQuad)
        //    //        .OnComplete(() =>
        //    //        {
        //    //            buttonListWithoutBlocker[i].DOLocalMove(tempPos2, .25f).SetEase(Ease.InOutQuad);
        //    //        }))
        //    //    .Insert(1, buttonListWithoutBlocker[randomIndex].DOLocalMove(Vector3.zero, .25f).SetEase(Ease.InOutQuad)
        //    //        .OnComplete(() =>
        //    //        {
        //    //            buttonListWithoutBlocker[randomIndex].DOLocalMove(tempPos1, .25f).SetEase(Ease.InOutQuad);
        //    //        }))
        //    //    .OnComplete(() =>
        //    //    {
        //    //        int[] tempI = _ConvertPositionToMatrixIndex(
        //    //            buttonListWithoutBlocker[i].localPosition.x, buttonListWithoutBlocker[i].localPosition.y, column * side, row * side, side);
        //    //        int rTempI = tempI[0];
        //    //        int cTempI = tempI[1];
        //    //        int[] tempRanI = _ConvertPositionToMatrixIndex(
        //    //            buttonListWithoutBlocker[randomIndex].localPosition.x, buttonListWithoutBlocker[randomIndex].localPosition.y, column * side, row * side, side);
        //    //        int rTempRanI = tempRanI[0];
        //    //        int cTempRanI = tempRanI[1]; ;

        //    //        string tempStr = matrix[rTempI, cTempI];
        //    //        matrix[rTempI, cTempI] = matrix[rTempRanI, cTempRanI];
        //    //        matrix[rTempRanI, cTempRanI] = tempStr;
        //    //    });
        //    //sequence.Play();

        //    //foreach (var tile in buttonListWithoutBlocker)
        //    //{
        //    //    sequence.Append(tile.DOLocalMove(Vector3.zero, 9.25f).SetEase(Ease.InOutQuad).SetUpdate(true))
        //    //        .OnComplete(()=> 
        //    //        {
        //    //            tile.localPosition = Vector3.zero;
        //    //        });
        //    //}
        //    //sequence.OnStepComplete(() =>
        //    //{
        //    //    for (int i = 0; i < buttonListWithoutBlocker.Count; i++)
        //    //    {
        //    //        Vector3 temp = tempList[i].localPosition;
        //    //        sequence.Append(buttonListWithoutBlocker[i].DOLocalMove(temp, 9.25f).SetEase(Ease.InOutQuad).SetUpdate(true))
        //    //        .OnComplete(() =>
        //    //        {
        //    //            buttonListWithoutBlocker[i].localPosition = temp;
        //    //        });
        //    //    }
        //    //});

        //    int[] tempI = _ConvertPositionToMatrixIndex(
        //        buttonListWithoutBlocker[i].transform.localPosition.x, buttonListWithoutBlocker[i].transform.localPosition.y, column * side, row * side, side);
        //    int rTempI = tempI[0];
        //    int cTempI = tempI[1];
        //    int[] tempRanI = _ConvertPositionToMatrixIndex(
        //        buttonListWithoutBlocker[randomIndex].transform.localPosition.x, buttonListWithoutBlocker[randomIndex].transform.localPosition.y, column * side, row * side, side);
        //    int rTempRanI = tempRanI[0];
        //    int cTempRanI = tempRanI[1]; ;

        //    string tempStr = matrix[rTempI, cTempI];
        //    matrix[rTempI, cTempI] = matrix[rTempRanI, cTempRanI];
        //    matrix[rTempRanI, cTempRanI] = tempStr;
        //}

        List<int> source = new List<int>();
        List<Transform> trsfTiles = new List<Transform>();
        List<Vector3> posTiles = new List<Vector3>();
        int index = 0;
        foreach (var item in buttonListWithoutBlocker)
        {
            source.Add(index);
            trsfTiles.Add(item.transform);
            posTiles.Add(item.transform.position);
            index++;
        }
        foreach (var item in trsfTiles)
        {
            item.DOLocalMove(Vector3.zero, .5f).SetEase(Ease.InBack).SetUpdate(true);
        }

        source = Shuffle(source.Count);
        int total = source.Count;
        for (int i = 0; i < total; i++)
        {
            //var colRow1 = buttonListWithoutBlocker[i].GetColRow();
            //var colRow2 = buttonListWithoutBlocker[source[i]].GetColRow();
            //matrix[colRow1.Item2, colRow1.Item1] = buttonListWithoutBlocker[source[i]].ID.ToString();
            //matrix[colRow2.Item2, colRow2.Item1] = buttonListWithoutBlocker[i].ID.ToString();

            //var temp = buttonListWithoutBlocker[i];
            //buttonListWithoutBlocker[i] = buttonListWithoutBlocker[source[i]];
            //buttonListWithoutBlocker[source[i]] = temp;
            //Debug.Log(matrix[colRow1.Item2, colRow1.Item1] + " <=> "+ matrix[colRow2.Item2, colRow2.Item1]+": " + JsonConvert.SerializeObject(matrix));

            //buttonListWithoutBlocker[i].ChangeColRow(buttonListWithoutBlocker[source[i]].GetColRow());
            //buttonListWithoutBlocker[source[i]].ChangeColRow(buttonListWithoutBlocker[i].GetColRow());

            trsfTiles[i].DOMove(posTiles[source[i]], 0.5f).SetEase(Ease.OutBack).SetUpdate(true).SetDelay(0.5f);

            if(i == total - 1)
            {
                GameplayTrial.instance._PlayParticles(false, false, false);
            }
        }
    }

    public List<int> Shuffle(int total = 1000)
    {
        int count = 1;
        List<int> temp = new List<int>();
        temp.Add(0);
        for (int i = 1; i < total; i++)
        {
            temp.Insert(UnityEngine.Random.Range(0, count), i);
            count++;
        }
        return temp;
    }

    void _SwapPos(Transform t1, Transform t2, int index)
    {
        var sequence = DOTween.Sequence();

        Vector3 tempPos1 = t1.localPosition;
        Vector3 tempPos2 = t2.localPosition;

        //sequence.Insert(1, t1.DOLocalMove(Vector3.zero, .25f).SetEase(Ease.InOutQuad).SetDelay(.25f))
        //    .Insert(1, t2.DOLocalMove(Vector3.zero, .25f).SetEase(Ease.InOutQuad).SetDelay(.25f))
        //    .Insert(2, t1.DOLocalMove(tempPos2, .25f).SetEase(Ease.InOutQuad).SetDelay(.25f))
        //    .Insert(2, t2.DOLocalMove(tempPos1, .25f).SetEase(Ease.InOutQuad).SetDelay(.25f));

        //sequence.Insert(1, t1.DOLocalMove(Vector3.zero, .25f).SetEase(Ease.InOutQuad))
        //    .Insert(1, t2.DOLocalMove(Vector3.zero, .25f).SetEase(Ease.InOutQuad))
        //    .Insert(2, t1.DOLocalMove(tempPos2, .25f).SetEase(Ease.InOutQuad))
        //    .Insert(2, t2.DOLocalMove(tempPos1, .25f).SetEase(Ease.InOutQuad));

        //sequence.Play();

        t1.DOLocalMove(Vector3.zero, .25f).SetEase(Ease.InOutQuad).SetDelay(index*.5f)
            .OnComplete(() =>
            {
                t1.DOLocalMove(tempPos2, .25f).SetEase(Ease.InOutQuad).OnComplete(delegate { Debug.Log($"{t1.name}: {tempPos2}"); });
            });
        t2.DOLocalMove(Vector3.zero, .25f).SetEase(Ease.InOutQuad).SetDelay(index*.5f)
            .OnComplete(() =>
            {
                t2.DOLocalMove(tempPos1, .25f).SetEase(Ease.InOutQuad).OnComplete(delegate { Debug.Log($"{t2.name}: {tempPos1}"); });
            });

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

    public void _DeactivateTile(TileController t)
    {
        float side = t.GetComponent<RectTransform>().sizeDelta.x;
        int rTemp = (int)(
                ((row * side - side) / 2 - t.transform.localPosition.y) / side
                );
        int cTemp = (int)(
            ((column * side - side) / 2 + t.transform.localPosition.x) / side
            );

        matrix[rTemp, cTemp] = "";
        buttonList.Remove(t.transform);
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
                Time.timeScale = 0;
            }
        }
        Debug.Log(JsonConvert.SerializeObject(matrix));
    }

    public List<Transform> _SearchSameTiles(Sprite sprite)
    {
        List<Transform> list = new();
        foreach (var trans in buttonListWithoutBlocker)
        {
            if (trans.transform.GetChild(0).GetComponent<Image>().sprite == sprite)
            {
                list.Add(trans.transform);
            }
        }
        return list;
    }

    #endregion

    #region Xử lý quy luật sau khi ăn Tiles
    void _SearchAndPullTile(int rowBefore, int colBefore, int rowAfter, int colAfter) // tìm Tile theo index trong ma tran
    {
        string temp = matrix[rowBefore, colBefore];
        matrix[rowBefore, colBefore] = matrix[rowAfter, colAfter];
        matrix[rowAfter, colAfter] = temp;

        float side = Tile.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        Vector3 posBefore = _ConvertMatrixIndexToPosition(rowBefore, colBefore, column * side, row * side, side);
        Vector3 posAfter = _ConvertMatrixIndexToPosition(rowAfter, colAfter, column * side, row * side, side);
        foreach (var t in buttonListWithoutBlocker)
        {
            if (t.transform.localPosition == posBefore)
            {
                //Debug.Log("posAfter: " + posAfter);

                //t.DOLocalMove(posAfter, .25f).SetEase(Ease.InOutQuad).SetUpdate(true).OnComplete(() =>
                //{
                //    t.localPosition = posAfter;
                //}).OnStepComplete(delegate { Debug.Log(t.name + "  " + t.localPosition); });

                //t.DOLocalMove(posAfter, .25f).SetEase(Ease.InOutQuad).SetUpdate(true).OnComplete(() =>
                //{
                //    t.localPosition = posAfter;
                //});

                t.transform.localPosition = posAfter;
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
        int blankTiles = 0;
        for (int c = 0; c < column; c++)
        {
            for (int r = rowFirst; r <= rowLast; r++)
            {
                tempArr[r - rowFirst] = matrix[r, c];
            }

            //for (int i = 0; i < tempArr.Length - 1; i++)
            //{
            //    for (int j = 0; j < tempArr.Length - i - 1; j++)
            //    {
            //        if (tempArr[j] != "0" && tempArr[j + 1] != "0")
            //        {
            //            if (tempArr[j + 1] == "")
            //            {
            //                string temp = tempArr[j + 1];
            //                tempArr[j + 1] = tempArr[j];
            //                tempArr[j] = temp;
            //                _SearchAndPullTile(j + rowFirst, c, j + rowFirst + 1, c);
            //            }
            //        }
            //    }
            //}

            for (int i = tempArr.Length - 1; i >= 0; i--)
            {
                if (tempArr[i] == "")
                {
                    blankTiles++;
                }
                else
                {
                    if (blankTiles != 0)
                    {
                        if (tempArr[i] == "0")
                        {
                            blankTiles = 0;
                        }
                        else
                        {
                            _SearchAndPullTile(rowFirst + i, c, rowFirst + i + blankTiles, c);
                        }
                    }
                }
            }
            blankTiles = 0;
        }
    }

    void _PullTilesToTop(int rowFirst, int rowLast)
    {
        var tempArr = new string[rowLast - rowFirst + 1];
        int blankTiles = 0;

        for (int c = 0; c < column; c++)
        {
            for (int r = rowFirst; r <= rowLast; r++)
            {
                tempArr[r - rowFirst] = matrix[r, c];
            }

            //for (int i = 0; i < tempArr.Length - 1; i++)
            //{
            //    for (int j = 0; j < tempArr.Length - i - 1; j++)
            //    {
            //        if (tempArr[j] != "0" && tempArr[j + 1] != "0")
            //        {
            //            if (tempArr[j] == "")
            //            {
            //                string temp = tempArr[j];
            //                tempArr[j] = tempArr[j + 1];
            //                tempArr[j + 1] = temp;

            //                _SearchAndPullTile(j + rowFirst + 1, c, j + rowFirst, c);
            //            }
            //        }
            //    }
            //}

            for (int i = 0; i < tempArr.Length; i++)
            {
                if (tempArr[i] == "")
                {
                    blankTiles++;
                }
                else
                {
                    if (blankTiles != 0)
                    {
                        if (tempArr[i] == "0")
                        {
                            blankTiles = 0;
                        }
                        else
                        {
                            _SearchAndPullTile(rowFirst + i, c, rowFirst + i - blankTiles, c);
                        }
                    }
                }
            }
            blankTiles = 0;
        }
    }

    void _PullTilesToLeft(int colFirst, int colLast)
    {
        var tempArr = new string[colLast - colFirst + 1];
        int blankTiles = 0;

        for (int r = 0; r < row; r++)
        {
            for (int c = colFirst; c <= colLast; c++)
            {
                tempArr[c - colFirst] = matrix[r, c];
            }

            //for (int i = 0; i < tempArr.Length - 1; i++)
            //{
            //    for (int j = 0; j < tempArr.Length - i - 1; j++)
            //    {
            //        if (tempArr[j] != "0" && tempArr[j + 1] != "0")
            //        {
            //            if (tempArr[j] == "")
            //            {
            //                string temp = tempArr[j];
            //                tempArr[j] = tempArr[j + 1];
            //                tempArr[j + 1] = temp;

            //                _SearchAndPullTile(r, j + colFirst + 1, r, j + colFirst);
            //            }
            //        }
            //    }
            //}

            for (int i = 0; i < tempArr.Length; i++)
            {
                if (tempArr[i] == "")
                {
                    blankTiles++;
                }
                else
                {
                    if (blankTiles != 0)
                    {
                        if (tempArr[i] == "0")
                        {
                            blankTiles = 0;
                        }
                        else
                        {
                            _SearchAndPullTile(r, colFirst + i, r, colFirst + i - blankTiles);
                        }
                    }
                }
            }
            blankTiles = 0;
        }
    }

    void _PullTilesToRight(int colFirst, int colLast)
    {
        var tempArr = new string[colLast - colFirst + 1];
        int blankTiles = 0;

        for (int r = 0; r < row; r++)
        {
            for (int c = colFirst; c <= colLast; c++)
            {
                tempArr[c - colFirst] = matrix[r, c];
            }

            //for (int i = 0; i < tempArr.Length - 1; i++)
            //{
            //    for (int j = 0; j < tempArr.Length - i - 1; j++)
            //    {
            //        if (tempArr[j] != "0" && tempArr[j + 1] != "0")
            //        {
            //            if (tempArr[j + 1] == "")
            //            {
            //                string temp = tempArr[j + 1];
            //                tempArr[j + 1] = tempArr[j];
            //                tempArr[j] = temp;

            //                _SearchAndPullTile(r, j + colFirst, r, j + colFirst + 1);
            //            }
            //        }
            //    }
            //}

            for (int i = tempArr.Length - 1; i >= 0; i--)
            {
                if (tempArr[i] == "")
                {
                    blankTiles++;
                }
                else
                {
                    if (blankTiles != 0)
                    {
                        if (tempArr[i] == "0")
                        {
                            blankTiles = 0;
                        }
                        else
                        {
                            _SearchAndPullTile(r, colFirst + i, r, colFirst + i + blankTiles);
                        }
                    }
                }
            }
            blankTiles = 0;
        }
    }

    #endregion
}
