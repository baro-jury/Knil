using DG.Tweening;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayTrial : MonoBehaviour
{
    public static GameplayTrial instance;
    public static int level;

    private bool isCoupled = true;
    private int siblingIndex;
    private Transform[] points = new Transform[4];
    private Transform tile;

    [SerializeField]
    private TextMeshProUGUI titleLv;
    [SerializeField]
    private GameObject chooseLvPanel;
    [SerializeField]
    private Button pauseButton;
    [SerializeField]
    private Transform tempAlignWithLeft, tempAlignWithRight, tempAlignWithTop, tempAlignWithBottom, tempAlignWithStart, tempAlignWithEnd;

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
        Time.timeScale = 0;
    }

    #region Choose level
    public void _ChooseLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void _PlayLevel(int lv)
    {
        EventSystem.current.SetSelectedGameObject(null);
        var dataStr = Resources.Load("Level_" + lv) as TextAsset;
        TrialBoard.levelData = JsonConvert.DeserializeObject<LevelData>(dataStr.text);
        titleLv.text = "LEVEL " + lv;
        TrialBoard.instance._GoToProcess(1);
        Time.timeScale = 0;
        Timer.time = 0;
        chooseLvPanel.SetActive(false);
    }

    public void _PlayLv1()
    {
        level = 1;
        _PlayLevel(level);
    }

    public void _PlayLv2()
    {
        level = 2;
        _PlayLevel(level);
    }

    public void _PlayLv3()
    {
        level = 3;
        _PlayLevel(level);
    }

    public void _PlayLv4()
    {
        level = 4;
        _PlayLevel(level);
    }

    public void _PlayLv5()
    {
        level = 5;
        _PlayLevel(level);
    }

    public void _PlayLv6()
    {
        level = 6;
        _PlayLevel(level);
    }

    public void _PlayLv7()
    {
        level = 7;
        _PlayLevel(level);
    }

    public void _PlayLv8()
    {
        level = 8;
        _PlayLevel(level);
    }
    public void _PlayLv9()
    {
        level = 9;
        _PlayLevel(level);
    }

    public void _PlayLv10()
    {
        level = 10;
        _PlayLevel(level);
    }

    public void _PlayLv11()
    {
        level = 11;
        _PlayLevel(level);
    }

    public void _PlayLv12()
    {
        level = 12;
        _PlayLevel(level);
    }

    #endregion

    #region Play trial
    public void _Pause()
    {
        pauseButton.transform.GetChild(0).gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void _Resume()
    {
        pauseButton.transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void _Quit()
    {
        SceneManager.LoadScene("GenerateMap");
    }

    public void _ClickTile(Transform currentTile)
    {
        Time.timeScale = 1;
        isCoupled = !isCoupled;
        if (!isCoupled)
        {
            tile = currentTile;
            siblingIndex = currentTile.GetSiblingIndex();
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

    bool _AreTheSameTiles(Transform tile1, Transform tile2)
    {
        return ResourceController.spritesDict[tile1.GetChild(0).GetComponent<Image>().sprite]
            == ResourceController.spritesDict[tile2.GetChild(0).GetComponent<Image>().sprite];
    }

    #endregion

    #region Supporter & Booster
    public void _SupporterShuffle() //Đổi vị trí: Khi người chơi sử dụng, các item thay đổi vị trí cho nhau.
    {
        TrialBoard.instance._RearrangeTiles();
    }

    public void _SupporterHint() //Hint: Khi sử dụng sẽ gợi ý 1 kết quả
    {
        bool isFounded = false;
        while (!isFounded)
        {
            int index = Random.Range(0, ResourceController.spritesDict.Count);
            List<Transform> temp = TrialBoard.instance._SearchSameTiles(ResourceController.spritesDict.ElementAt(index).Key);
            if (temp.Count != 0)
            {
                for (int i = 0; i < (temp.Count - 1); i++)
                {
                    for (int j = (i + 1); j < temp.Count; j++)
                    {
                        if (_HasAvailableConnection(temp[i], temp[j]))
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
            List<Transform> temp = TrialBoard.instance._SearchSameTiles(ResourceController.spritesDict.ElementAt(index).Key);

            if (temp.Count != 0)
            {
                for (int i = 0; i < temp.Count / 2; i++)
                {
                    if (numCouple == 2)
                    {
                        goto check;
                    }

                    StopAllCoroutines();
                    StartCoroutine(DestroyTiles(temp[2 * i], temp[2 * i + 1]));
                    numCouple++;
                }
            }
        }
        LineController.instance._EraseLine();
    }

    public void _SupporterFreezeTime() //Snow: Đóng băng thời gian 10 giây
    {
        Debug.Log("Đóng băng thời gian 10 giây");
        StartCoroutine(FreezeTime());
    }

    public void _BoosterTimeWizard() //Time : Tăng 10 giây khi sử dụng
    {

        Timer.time += 10;
    }

    public void _BoosterEarthquake() //Remove All: Xoá 1 hình bất kỳ
    {
        bool isFounded = false;
        while (!isFounded)
        {
            int index = Random.Range(0, ResourceController.spritesDict.Count);
            List<Transform> temp = TrialBoard.instance._SearchSameTiles(ResourceController.spritesDict.ElementAt(index).Key);
            if (temp.Count != 0)
            {
                for (int i = 0; i < temp.Count; i++)
                {
                    StartCoroutine(DestroyTiles(temp[i]));
                }
                isFounded = true;
            }
        }
    }

    public void _BoosterMirror() //Mirror: gấp đôi số sao đạt được
    {

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

        for (int i = 0; i < n; i++)
        {
            if (i == 0 || i == n - 1)
            {
                linePositions[i].gameObject.SetActive(false);
                TrialBoard.instance._DeactivateTile(linePositions[i]);
            }
        }
        yield return new WaitForSeconds(0.75f);
        TrialBoard.instance._ActivateGravity();
        TrialBoard.instance._ShuffleWhenNoPossibleLink();
        TrialBoard.instance._CheckProcess();
    }

    IEnumerator FreezeTime()
    {
        Timer.instance._FreezeTime(true);
        yield return new WaitForSeconds(10);
        Timer.instance._FreezeTime(false);
    }

    #endregion

    #region Algorithm
    void _SetLinePositions(Transform t0, Transform t1, Transform t2, Transform t3)
    {
        points[0] = t0;
        points[1] = t1;
        points[2] = t2;
        points[3] = t3;
    }

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
                if (TrialBoard.instance._HasButtonInLocation(i, y))
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
                if (TrialBoard.instance._HasButtonInLocation(x, i))
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
            checkPoint = TrialBoard.instance._HasButtonInLocation(i, leftTile.localPosition.y)
                || TrialBoard.instance._HasButtonInLocation(i, rightTile.localPosition.y);
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
            checkPoint = TrialBoard.instance._HasButtonInLocation(topTile.localPosition.x, i)
                || TrialBoard.instance._HasButtonInLocation(bottomTile.localPosition.x, i);
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
                    if (!TrialBoard.instance._HasButtonInLocation(i, startY) &&
                        _HasHorizontalLine(startTile, tempAlignWithStart) && _HasVerticalLine(tempAlignWithStart, endTile))
                    {
                        _SetLinePositions(startTile, tempAlignWithStart, endTile, null);
                        return true;
                    }
                }
                else
                {
                    checkPoint = TrialBoard.instance._HasButtonInLocation(i, startY) || TrialBoard.instance._HasButtonInLocation(i, endY);
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
                    if (!TrialBoard.instance._HasButtonInLocation(i, endY) &&
                        _HasHorizontalLine(startTile, tempAlignWithStart) && _HasVerticalLine(tempAlignWithStart, endTile))
                    {
                        _SetLinePositions(startTile, tempAlignWithStart, endTile, null);
                        return true;
                    }
                }
                else
                {
                    checkPoint = TrialBoard.instance._HasButtonInLocation(i, startY) || TrialBoard.instance._HasButtonInLocation(i, endY);
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
                    if (!TrialBoard.instance._HasButtonInLocation(startX, i) &&
                        _HasVerticalLine(startTile, tempAlignWithStart) && _HasHorizontalLine(tempAlignWithStart, endTile))
                    {
                        _SetLinePositions(startTile, tempAlignWithStart, endTile, null);
                        return true;
                    }
                }
                else
                {
                    checkPoint = TrialBoard.instance._HasButtonInLocation(startX, i) || TrialBoard.instance._HasButtonInLocation(endX, i);
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
                    if (!TrialBoard.instance._HasButtonInLocation(endX, i) &&
                        _HasVerticalLine(startTile, tempAlignWithStart) && _HasHorizontalLine(tempAlignWithStart, endTile))
                    {
                        _SetLinePositions(startTile, tempAlignWithStart, endTile, null);
                        return true;
                    }
                }
                else
                {
                    checkPoint = TrialBoard.instance._HasButtonInLocation(startX, i) || TrialBoard.instance._HasButtonInLocation(endX, i);
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
