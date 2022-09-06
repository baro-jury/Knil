using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance;

    private bool isCoupled = true, activateSwap = false;
    private int siblingIndex;
    private Transform[] points = new Transform[4];
    private Transform tile;

    [SerializeField]
    private Button settingButton;
    [SerializeField]
    private GameObject boosterPanel;
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject quitPanel;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private Transform tempAlignWithLeft, tempAlignWithRight, tempAlignWithTop, tempAlignWithBottom, tempAlignWithStart, tempAlignWithEnd;
    [SerializeField]
    private Ease ease = Ease.InOutQuad;

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
        Time.timeScale = 0;
    }

    #region Ingame

    public void _Pause()
    {
        pausePanel.SetActive(true);

        settingButton.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        settingButton.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        settingButton.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

        settingButton.transform.GetChild(0).GetComponent<RectTransform>().DOAnchorPosY(-150, .5f).SetEase(ease).SetUpdate(true);
        settingButton.transform.GetChild(1).GetComponent<RectTransform>().DOAnchorPosY(-300, .5f).SetEase(ease).SetUpdate(true);
        settingButton.transform.GetChild(2).GetComponent<RectTransform>().DOAnchorPosY(-450, .5f).SetEase(ease).SetUpdate(true).OnComplete(() =>
        {
            settingButton.enabled = true;
        });
        
        Time.timeScale = 0;
    }

    public void _Resume()
    {
        settingButton.enabled = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void _GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void _TurnOffSound()
    {
        pausePanel.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
        //               setting button      sound on button      sound off button
    }

    public void _TurnOnSound()
    {
        pausePanel.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
        //               setting button      sound on button      sound off button
    }

    public void _TurnOffMusic()
    {
        pausePanel.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
        //               setting button      music on button      music off button
    }

    public void _TurnOnMusic()
    {
        pausePanel.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(false);
        //               setting button      music on button      music off button
    }

    public void _Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void _QuitConfirmation()
    {
        quitPanel.SetActive(true);
    }

    public void _QuitLevel()
    {
        //Application.Quit();
        SceneManager.LoadScene(0);
    }

    public void _NotExit()
    {
        quitPanel.SetActive(false);
    }

    public void _GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void _WatchAds()
    {

    }

    public void _CompleteLevel()
    {
        Time.timeScale = 0;
        winPanel.SetActive(true);

    }

    public void _GoToNextLevel()
    {
        if (LevelController.level == 12)
        {
            _GoToMenu();
        }
        else
        {
            LevelController.level = LevelController.level + 1;
            LevelController.instance._PlayLevel(LevelController.level);
        }
    }

    public void _ShareResult()
    {

    }

    #endregion

    #region Click Tiles
    public void _ClickTile(Transform currentTile) //edit
    {
        isCoupled = !isCoupled;
        if (!isCoupled)
        {
            tile = currentTile;
            siblingIndex = currentTile.GetSiblingIndex();
        }
        else
        {
            if (activateSwap)
            {
                BoardController.instance._SwapTiles(tile, currentTile);
                EventSystem.current.SetSelectedGameObject(null);
                activateSwap = false;
            }
            else
            {
                if (siblingIndex == currentTile.GetSiblingIndex())
                {
                    Debug.Log("Click cung 1 tile");
                    tile = currentTile;
                    siblingIndex = currentTile.GetSiblingIndex();
                    isCoupled = !isCoupled;
                }
                else
                {
                    if (!_AreTheSameTiles(tile, currentTile))
                    {
                        Debug.Log("Click khac tile khac ID");
                        tile = currentTile;
                        siblingIndex = currentTile.GetSiblingIndex();
                        isCoupled = !isCoupled;
                    }
                    else
                    {
                        if (_HasAvailableConnection(tile, currentTile))
                        {
                            StopAllCoroutines();
                            StartCoroutine(MakeConnection(points));
                            StartCoroutine(DestroyTiles(points));
                        }
                        else
                        {
                            Debug.Log("Khong the ket noi");
                        }
                    }
                }
            }
        }
    }

    bool _AreTheSameTiles(Transform tile1, Transform tile2)
    {
        return ResourceController.spritesDict[tile1.GetChild(0).GetComponent<Image>().sprite]
            == ResourceController.spritesDict[tile2.GetChild(0).GetComponent<Image>().sprite];
    }

    void _SetLinePositions(Transform t0, Transform t1, Transform t2, Transform t3)
    {
        points[0] = t0;
        points[1] = t1;
        points[2] = t2;
        points[3] = t3;
    }
    #endregion

    #region Supporter

    public void _SupporterShuffle() //Đổi vị trí: Khi người chơi sử dụng, các item thay đổi vị trí cho nhau.
    {
        BoardController.instance._RearrangeTiles();
    }

    public void _SupporterHint() //Hint: Khi sử dụng sẽ gợi ý 1 kết quả
    {
        bool isFounded = false;
        while (!isFounded)
        {
            int index = Random.Range(0, ResourceController.spritesDict.Count);
            List<Transform> temp = BoardController.instance._SearchTiles(ResourceController.spritesDict.ElementAt(index).Key);
            //Dictionary<Transform, int> temp = BoardController.instance._SearchTiles(ResourceController.spritesDict.ElementAt(index).Key);
            if (temp.Count != 0)
            {
                for (int i = 0; i < (temp.Count - 1); i++)
                {
                    for (int j = (i + 1); j < temp.Count; j++)
                    {
                        if (_HasAvailableConnection(temp[i], temp[j]))
                        //if (_HasAvailableConnection(temp.ElementAt(i).Key, temp.ElementAt(j).Key))
                        {
                            StartCoroutine(MakeConnection(points));
                            isFounded = true;
                            break;
                        }
                    }
                }
            }
        }
    }

    public void _SupporterSwap() //Swap: Người chơi chọn 2 ô, sau đó 2 ô đổi vị trí cho nhau
    {
        //activateSwap = true;
    }

    public void _SupporterMagicWand() //Đũa thần: 2 kết quả hoàn thành
    {
        int numCouple = 0;
        Transform[] trans = new Transform[4];
        for (int index = 0; index < ResourceController.spritesDict.Count; index++)
        {
        check:
            if (numCouple == 2)
            {
                break;
            }
            List<Transform> temp = BoardController.instance._SearchTiles(ResourceController.spritesDict.ElementAt(index).Key);
            //Dictionary<Transform, int> temp = BoardController.instance._SearchTiles(ResourceController.spritesDict.ElementAt(index).Key);

            #region chia truong hop list 2 tile va 4 tile
            //if (temp.Count == 2)
            //{
            //    if (_HasAvailableConnection(temp[0], temp[1]))
            //    //if (_HasAvailableConnection(temp.ElementAt(0).Key, temp.ElementAt(1).Key))
            //    {
            //        trans[2 * numCouple] = temp[0];
            //        trans[2 * numCouple + 1] = temp[1];
            //        //trans[2 * numCouple] = temp.ElementAt(0).Key;
            //        //trans[2 * numCouple + 1] = temp.ElementAt(1).Key;
            //        numCouple++;
            //    }
            //}
            //else if (temp.Count > 2)
            //{
            //    if (numCouple == 0)
            //    {
            //        trans = _ConnectableTilesFromList(temp);
            //        for (int i = 0; i < temp.Count; i++)
            //        {
            //            if (temp.ElementAt(i).Value == -1)
            //            {
            //                numCouple++;
            //            }
            //        }
            //        numCouple /= 2;
            //    }
            //    else
            //    {
            //for (int i = 0; i < (temp.Count - 1); i++)
            //{
            //    for (int j = (i + 1); j < temp.Count; j++)
            //    {
            //        if (_HasAvailableConnection(temp.ElementAt(i).Key, temp.ElementAt(j).Key))
            //        {
            //            trans[2 * numCouple] = temp.ElementAt(i).Key;
            //            trans[2 * numCouple + 1] = temp.ElementAt(j).Key;
            //            //StartCoroutine(MagicWand(temp.ElementAt(i).Key, temp.ElementAt(j).Key));
            //            numCouple++;
            //            goto check;
            //        }
            //    }
            //}
            //    }
            //}
            #endregion

            if (temp.Count != 0)
            {
                for (int i = 0; i < temp.Count / 2; i++)
                {
                    if (numCouple == 2)
                    {
                        goto check;
                    }

                    //trans[2 * numCouple] = temp[2 * i];
                    //trans[2 * numCouple + 1] = temp[2 * i + 1];
                    StopAllCoroutines();
                    StartCoroutine(DestroyTiles(temp[2 * i], temp[2 * i + 1]));
                    numCouple++;
                }
            }
        }
        LineController.instance._EraseLine();
        //StartCoroutine(MagicWand(trans));
    }

    Transform[] _ConnectableTilesFromList(List<Transform> list)
    {
        Transform[] temp = new Transform[4];
        int num = 0;
        for (int i = 0; i < (list.Count - 1); i++)
        {
            for (int j = (i + 1); j < list.Count; j++)
            {
                if (num == 2)
                {
                    goto end;
                }
                if (_HasAvailableConnection(list[i], list[j]))
                {
                    temp[2 * num] = list[i];
                    temp[2 * num + 1] = list[j];
                    //StartCoroutine(MagicWand(dict.ElementAt(i).Key, dict.ElementAt(j).Key));
                    num++;
                }
            }
        }
    end:
        return temp;
    }
    Transform[] _ConnectableTilesFromList(Dictionary<Transform, int> dict)
    {
        Transform[] temp = new Transform[4];
        int num = 0;
        for (int i = 0; i < (dict.Count - 1); i++)
        {
            for (int j = (i + 1); j < dict.Count; j++)
            {
                if (num == 2)
                {
                    goto end;
                }
                if (dict.ElementAt(i).Value != -1 && dict.ElementAt(j).Value != -1 &&
                    _HasAvailableConnection(dict.ElementAt(i).Key, dict.ElementAt(j).Key))
                {
                    temp[2 * num] = dict.ElementAt(i).Key;
                    temp[2 * num + 1] = dict.ElementAt(j).Key;
                    //StartCoroutine(MagicWand(dict.ElementAt(i).Key, dict.ElementAt(j).Key));
                    dict[dict.ElementAt(i).Key] = -1;
                    dict[dict.ElementAt(j).Key] = -1;
                    num++;
                }
            }
        }
    end:
        return temp;
    }

    public void _SupporterFreezeTime() //Snow: Đóng băng thời gian 10 giây
    {
        Debug.Log("Đóng băng thời gian 10 giây");
        StartCoroutine(FreezeTime());
    }

    #endregion

    #region Booster

    public void _BoosterTimeWizard() //Time : Tăng 10 giây khi sử dụng
    {
        boosterPanel.SetActive(false);
        TimeController.instance._SetTimeForSlider(true);
        Time.timeScale = 1;
    }

    //public void _BoosterEarthquake() //Remove item: Mỗi 1 hình xoá 1 cặp ( tối đa 5 cặp ) Random
    //{
    //    boosterPanel.SetActive(false);
    //    int n = 0, numCouple = Random.Range(3, 6);
    //    Transform[] trans = new Transform[2 * numCouple];
    //    for (int i = 0; i < ResourceController.spritesDict.Count; i++)
    //    {
    //        if (n == numCouple)
    //        {
    //            break;
    //        }
    //        Dictionary<Transform, int> temp = BoardController.instance._SearchTiles(ResourceController.spritesDict.ElementAt(i).Key);
    //        if (temp.Count != 0)
    //        {
    //            trans[2 * n] = temp.ElementAt(0).Key;
    //            trans[2 * n + 1] = temp.ElementAt(1).Key;
    //            n++;
    //        }
    //    }
    //    StartCoroutine(Earthquake(trans));
    //    Time.timeScale = 1;
    //}

    public void _BoosterEarthquake() //Remove All: Xoá 1 hình bất kỳ
    {
        boosterPanel.SetActive(false);
        bool isFounded = false;
        while (!isFounded)
        {
            int index = Random.Range(0, ResourceController.spritesDict.Count);
            List<Transform> temp = BoardController.instance._SearchTiles(ResourceController.spritesDict.ElementAt(index).Key);
            //Dictionary<Transform, int> temp = BoardController.instance._SearchTiles(ResourceController.spritesDict.ElementAt(index).Key);
            if (temp.Count != 0)
            {
                for (int i = 0; i < temp.Count; i++)
                {
                    StartCoroutine(DestroyTiles(temp[i]));
                    //StartCoroutine(DestroyTiles(temp.ElementAt(i).Key));
                }
                isFounded = true;
            }
        }
        Time.timeScale = 1;
    }

    public void _BoosterMirror() //Mirror: gấp đôi số sao đạt được
    {
        boosterPanel.SetActive(false);

        Time.timeScale = 1;
    }

    public void _NoBooster()
    {
        boosterPanel.SetActive(false);
        Time.timeScale = 1;
    }

    #endregion

    #region IEnumerator

    IEnumerator MakeConnection(params Transform[] linePositions)
    {
        LineController.instance._DrawLine(0.15f * tile.gameObject.GetComponent<RectTransform>().sizeDelta.x / 100, linePositions);

        yield return new WaitForSeconds(0.75f);

        LineController.instance._EraseLine();
    }

    IEnumerator DestroyTiles(params Transform[] linePositions)
    {
        int n = linePositions.Length;
        for (int i = 0; i < linePositions.Length; i++)
        {
            if (linePositions[i] == null)
            {
                n--;
            }
        }
        //for (int i = 0; i < n; i++)
        //{
        //    if (i == 0 || i == n - 1)
        //    {
        //        //linePositions[i].transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutQuad).OnComplete(() =>
        //        //{
        //        //    linePositions[i].gameObject.SetActive(false);
        //        //    BoardController.instance._DeactivateTile(linePositions[i]);
        //        //});

        //        linePositions[i].transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InOutQuad);
        //    }
        //}

        for (int i = 0; i < n; i++)
        {
            if (i == 0 || i == n - 1)
            {
                linePositions[i].gameObject.SetActive(false);
                BoardController.instance._DeactivateTile(linePositions[i]);
            }
        }
        yield return new WaitForSeconds(0.75f);
        BoardController.instance._ActivateGravity();
        BoardController.instance._ShuffleWhenNoPossibleLink();
    }

    //IEnumerator MagicWand(params Transform[] transforms)
    //{
    //    for (int i = 0; i < (transforms.Length / 2); i++)
    //    {
    //        if (_HasAvailableConnection(transforms[2 * i], transforms[2 * i + 1]))
    //        {
    //            StopAllCoroutines();
    //            StartCoroutine(MakeConnection(points));
    //            StartCoroutine(DestroyTiles(points));
    //        }
    //        yield return new WaitForSeconds(0.75f);
    //    }
    //}

    IEnumerator MagicWand(params Transform[] transforms)
    {
        yield return new WaitForSeconds(0);
        for (int i = 0; i < (transforms.Length / 2); i++)
        {
            StopAllCoroutines();
            StartCoroutine(DestroyTiles(transforms[2 * i], transforms[2 * i + 1]));
        }
    }

    IEnumerator FreezeTime()
    {
        TimeController.instance._FreezeTime(true);
        yield return new WaitForSeconds(10);
        TimeController.instance._FreezeTime(false);
    }

    #endregion

    #region Algorithm

    public bool _HasAvailableConnection(Transform tile1, Transform tile2)
    {
        // check line with x
        if (tile1.localPosition.x == tile2.localPosition.x)
        {
            if (_HasVerticalLine(tile1, tile2))
            {
                _SetLinePositions(tile1, tile2, null, null);
                return true;
            }
        }
        // check line with y
        if (tile1.localPosition.y == tile2.localPosition.y)
        {
            if (_HasHorizontalLine(tile1, tile2))
            {
                _SetLinePositions(tile1, tile2, null, null);
                return true;
            }
        }

        if (_HasLineInHorizontalRectangle(tile1, tile2))
        {
            return true;
        }
        if (_HasLineInVerticalRectangle(tile1, tile2))
        {
            return true;
        }

        // check more right
        if (_HasLineInHorizontalExtent(tile1, tile2, 1))
        {
            return true;
        }
        // check more left
        if (_HasLineInHorizontalExtent(tile1, tile2, -1))
        {
            return true;
        }
        // check more down
        if (_HasLineInVerticalExtent(tile1, tile2, 1))
        {
            return true;
        }
        // check more up
        if (_HasLineInVerticalExtent(tile1, tile2, -1))
        {
            return true;
        }

        return false;
    }

    //button cung hang
    bool _HasHorizontalLine(Transform tile1, Transform tile2)
    {
        float y = tile1.localPosition.y;
        float min = Mathf.Min(tile1.localPosition.x, tile2.localPosition.x);
        float max = Mathf.Max(tile1.localPosition.x, tile2.localPosition.x);
        float buttonSide = tile1.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        if (max - min == buttonSide)
        {
            return true;
        }
        else
        {
            for (float i = min + buttonSide; i < max; i += buttonSide)
            {
                if (BoardController.instance._HasButtonInLocation(i, y))
                {
                    return false;
                }
            }
        }
        return true;
    }

    //button cung cot
    bool _HasVerticalLine(Transform tile1, Transform tile2)
    {
        float x = tile1.localPosition.x;
        float min = Mathf.Min(tile1.localPosition.y, tile2.localPosition.y);
        float max = Mathf.Max(tile1.localPosition.y, tile2.localPosition.y);
        float buttonSide = tile1.gameObject.GetComponent<RectTransform>().sizeDelta.y;
        if (max - min == buttonSide)
        {
            return true;
        }
        else
        {
            for (float i = min + buttonSide; i < max; i += buttonSide)
            {
                if (BoardController.instance._HasButtonInLocation(x, i))
                {
                    return false;
                }
            }
        }
        return true;
    }

    //2 button trong hinh chu nhat ngang: check hinh chu Z
    bool _HasLineInHorizontalRectangle(Transform tile1, Transform tile2)
    {
        #region Setup toa do, kich thuoc, dieu kien

        if (tile1.localPosition.x == tile2.localPosition.x || tile1.localPosition.y == tile2.localPosition.y)
        {
            return false;
        }

        Transform leftTile = tile1, rightTile = tile2;
        float leftX = Mathf.Min(tile1.localPosition.x, tile2.localPosition.x);
        float rightX = Mathf.Max(tile1.localPosition.x, tile2.localPosition.x);
        if (leftX == tile2.localPosition.x)
        {
            leftTile = tile2;
            rightTile = tile1;
        }
        float buttonSide = tile1.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        bool checkPoint;

        #endregion

        for (float i = (leftX + buttonSide); i < rightX; i += buttonSide)
        {
            tempAlignWithLeft.localPosition = new Vector3(i, leftTile.localPosition.y, 0);
            tempAlignWithRight.localPosition = new Vector3(i, rightTile.localPosition.y, 0);
            checkPoint = BoardController.instance._HasButtonInLocation(i, leftTile.localPosition.y)
                || BoardController.instance._HasButtonInLocation(i, rightTile.localPosition.y);
            if (!checkPoint && _HasHorizontalLine(leftTile, tempAlignWithLeft) &&
                _HasVerticalLine(tempAlignWithLeft, tempAlignWithRight) && _HasHorizontalLine(tempAlignWithRight, rightTile))
            {
                _SetLinePositions(leftTile, tempAlignWithLeft, tempAlignWithRight, rightTile);
                return true;
            }
        }
        return false;
    }

    //2 button trong hinh chu nhat dung: check hinh chu Z
    bool _HasLineInVerticalRectangle(Transform tile1, Transform tile2)
    {
        #region Setup toa do, kich thuoc, dieu kien

        if (tile1.localPosition.x == tile2.localPosition.x || tile1.localPosition.y == tile2.localPosition.y)
        {
            return false;
        }

        Transform topTile = tile1, bottomTile = tile2;
        float topY = Mathf.Max(tile1.localPosition.y, tile2.localPosition.y);
        float bottomY = Mathf.Min(tile1.localPosition.y, tile2.localPosition.y);
        if (topY == tile2.localPosition.y)
        {
            topTile = tile2;
            bottomTile = tile1;
        }
        float buttonSide = tile1.gameObject.GetComponent<RectTransform>().sizeDelta.y;
        bool checkPoint;

        #endregion

        for (float i = (topY - buttonSide); i > bottomY; i -= buttonSide)
        {
            tempAlignWithTop.localPosition = new Vector3(topTile.localPosition.x, i, 0);
            tempAlignWithBottom.localPosition = new Vector3(bottomTile.localPosition.x, i, 0);
            checkPoint = BoardController.instance._HasButtonInLocation(topTile.localPosition.x, i)
                || BoardController.instance._HasButtonInLocation(bottomTile.localPosition.x, i);
            if (!checkPoint && _HasVerticalLine(topTile, tempAlignWithTop) &&
                _HasHorizontalLine(tempAlignWithTop, tempAlignWithBottom) && _HasVerticalLine(tempAlignWithBottom, bottomTile))
            {
                _SetLinePositions(topTile, tempAlignWithTop, tempAlignWithBottom, bottomTile);
                return true;
            }
        }
        return false;
    }

    //mo rong ra theo chieu ngang: check hinh chu U, L
    bool _HasLineInHorizontalExtent(Transform tile1, Transform tile2, int direct)
    {
        #region Setup toa do, kich thuoc, dieu kien

        if (tile1.localPosition.y == tile2.localPosition.y)
        {
            return false;
        }

        Transform startTile = tile1, endTile = tile2;
        float startX = Mathf.Min(tile1.localPosition.x, tile2.localPosition.x);
        float endX = Mathf.Max(tile1.localPosition.x, tile2.localPosition.x);
        float startY, endY;
        if (startX == tile1.localPosition.x)
        {
            startY = tile1.localPosition.y;
            endY = tile2.localPosition.y;
        }
        else
        {
            startY = tile2.localPosition.y;
            endY = tile1.localPosition.y;
        }
        float buttonSide = tile1.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        bool checkPoint;

        #endregion

        if (direct > 0)
        {
            #region Di tu trai sang phai

            startTile.localPosition = new Vector3(startX, startY, 0);
            endTile.localPosition = new Vector3(endX, endY, 0);

            for (float i = endX; i <= (tile1.parent.GetComponent<RectTransform>().sizeDelta.x + buttonSide) / 2; i += buttonSide)
            {
                tempAlignWithStart.localPosition = new Vector3(i, startY, 0);
                tempAlignWithEnd.localPosition = new Vector3(i, endY, 0);
                if (i == endX)
                {
                    if (!BoardController.instance._HasButtonInLocation(i, startY) &&
                        _HasHorizontalLine(startTile, tempAlignWithStart) && _HasVerticalLine(tempAlignWithStart, endTile))
                    {
                        _SetLinePositions(startTile, tempAlignWithStart, endTile, null);
                        return true;
                    }
                }
                else
                {
                    checkPoint = BoardController.instance._HasButtonInLocation(i, startY) || BoardController.instance._HasButtonInLocation(i, endY);
                    if (!checkPoint && _HasHorizontalLine(startTile, tempAlignWithStart) &&
                        _HasVerticalLine(tempAlignWithStart, tempAlignWithEnd) && _HasHorizontalLine(tempAlignWithEnd, endTile))
                    {
                        _SetLinePositions(startTile, tempAlignWithStart, tempAlignWithEnd, endTile);
                        return true;
                    }
                }
            }

            #endregion
        }
        else
        {
            #region Di tu phai sang trai

            startTile.localPosition = new Vector3(endX, endY, 0);
            endTile.localPosition = new Vector3(startX, startY, 0);

            for (float i = startX; i >= -(tile1.parent.GetComponent<RectTransform>().sizeDelta.x + buttonSide) / 2; i -= buttonSide)
            {
                tempAlignWithStart.localPosition = new Vector3(i, endY, 0);
                tempAlignWithEnd.localPosition = new Vector3(i, startY, 0);
                if (i == startX)
                {
                    if (!BoardController.instance._HasButtonInLocation(i, endY) &&
                        _HasHorizontalLine(startTile, tempAlignWithStart) && _HasVerticalLine(tempAlignWithStart, endTile))
                    {
                        _SetLinePositions(startTile, tempAlignWithStart, endTile, null);
                        return true;
                    }
                }
                else
                {
                    checkPoint = BoardController.instance._HasButtonInLocation(i, startY) || BoardController.instance._HasButtonInLocation(i, endY);
                    if (!checkPoint && _HasHorizontalLine(startTile, tempAlignWithStart) &&
                        _HasVerticalLine(tempAlignWithStart, tempAlignWithEnd) && _HasHorizontalLine(tempAlignWithEnd, endTile))
                    {
                        _SetLinePositions(startTile, tempAlignWithStart, tempAlignWithEnd, endTile);
                        return true;
                    }
                }
            }

            #endregion
        }

        return false;
    }

    //mo rong ra theo chieu doc: check hinh chu U, L
    bool _HasLineInVerticalExtent(Transform tile1, Transform tile2, int direct)
    {
        #region Setup toa do, kich thuoc, dieu kien

        if (tile1.localPosition.x == tile2.localPosition.x)
        {
            return false;
        }

        Transform startTile = tile1, endTile = tile2;
        float startY = Mathf.Max(tile1.localPosition.y, tile2.localPosition.y);
        float endY = Mathf.Min(tile1.localPosition.y, tile2.localPosition.y);
        float startX, endX;
        if (startY == tile1.localPosition.y)
        {
            startX = tile1.localPosition.x;
            endX = tile2.localPosition.x;
        }
        else
        {
            startX = tile2.localPosition.x;
            endX = tile1.localPosition.x;
        }
        float buttonSide = tile1.gameObject.GetComponent<RectTransform>().sizeDelta.y;
        bool checkPoint;

        #endregion

        if (direct > 0)
        {
            #region Di tu tren xuong duoi

            startTile.localPosition = new Vector3(startX, startY, 0);
            endTile.localPosition = new Vector3(endX, endY, 0);

            for (float i = endY; i >= -(tile1.parent.GetComponent<RectTransform>().sizeDelta.y + buttonSide) / 2; i -= buttonSide)
            {
                tempAlignWithStart.localPosition = new Vector3(startX, i, 0);
                tempAlignWithEnd.localPosition = new Vector3(endX, i, 0);
                if (i == endY)
                {
                    if (!BoardController.instance._HasButtonInLocation(startX, i) &&
                        _HasVerticalLine(startTile, tempAlignWithStart) && _HasHorizontalLine(tempAlignWithStart, endTile))
                    {
                        _SetLinePositions(startTile, tempAlignWithStart, endTile, null);
                        return true;
                    }
                }
                else
                {
                    checkPoint = BoardController.instance._HasButtonInLocation(startX, i) || BoardController.instance._HasButtonInLocation(endX, i);
                    if (!checkPoint && _HasVerticalLine(startTile, tempAlignWithStart) &&
                        _HasHorizontalLine(tempAlignWithStart, tempAlignWithEnd) && _HasVerticalLine(tempAlignWithEnd, endTile))
                    {
                        _SetLinePositions(startTile, tempAlignWithStart, tempAlignWithEnd, endTile);
                        return true;
                    }
                }
            }

            #endregion
        }
        else
        {
            #region Di tu duoi len tren

            startTile.localPosition = new Vector3(endX, endY, 0);
            endTile.localPosition = new Vector3(startX, startY, 0);

            for (float i = startY; i <= (tile1.parent.GetComponent<RectTransform>().sizeDelta.y + buttonSide) / 2; i += buttonSide)
            {
                tempAlignWithStart.localPosition = new Vector3(endX, i, 0);
                tempAlignWithEnd.localPosition = new Vector3(startX, i, 0);
                if (i == startY)
                {
                    if (!BoardController.instance._HasButtonInLocation(endX, i) &&
                        _HasVerticalLine(startTile, tempAlignWithStart) && _HasHorizontalLine(tempAlignWithStart, endTile))
                    {
                        _SetLinePositions(startTile, tempAlignWithStart, endTile, null);
                        return true;
                    }
                }
                else
                {
                    checkPoint = BoardController.instance._HasButtonInLocation(startX, i) || BoardController.instance._HasButtonInLocation(endX, i);
                    if (!checkPoint && _HasVerticalLine(startTile, tempAlignWithStart) &&
                        _HasHorizontalLine(tempAlignWithStart, tempAlignWithEnd) && _HasVerticalLine(tempAlignWithEnd, endTile))
                    {
                        _SetLinePositions(startTile, tempAlignWithStart, tempAlignWithEnd, endTile);
                        return true;
                    }
                }
            }

            #endregion
        }

        return false;
    }

    #endregion

}
