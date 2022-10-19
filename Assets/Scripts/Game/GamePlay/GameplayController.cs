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

    private bool isCoupled = true, isHinted = false;
    private int siblingIndex, numberOfStars;
    private Transform[] points = new Transform[4];
    private Transform tile;

    [SerializeField]
    private GameObject boosterPanel;
    [SerializeField]
    private Text titleLv, titleGameover, titleComplete;
    [SerializeField]
    private GameObject pausePanel, quitPanel;
    [SerializeField]
    private Button settingOnButton, settingOffButton;
    [SerializeField]
    private Button btSpHint, btSpMagicWand, btSpFreeze, btSpShuffle;
    [SerializeField]
    private GameObject lastChancePanel, rejectChancePanel, gameOverPanel, winPanel, starsAchieved;
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
        titleLv.text = "LEVEL " + LevelController.level;
        titleGameover.text = "LEVEL " + LevelController.level;
        titleComplete.text = "LEVEL " + LevelController.level;
        Time.timeScale = 0;
    }

    #region Ingame
    public void _Pause()
    {
        settingOnButton.interactable = false;
        pausePanel.SetActive(true);
        pausePanel.transform.GetComponent<Image>().DOFade(.8f, .5f).SetEase(Ease.InOutQuad).SetUpdate(true);
        settingOffButton.transform.DORotate(new Vector3(0, 0, -180), .5f).SetEase(Ease.InOutQuad).SetUpdate(true)
            .OnComplete(() =>
            {
                settingOffButton.transform.rotation = Quaternion.Euler(0, 0, 0);
            });

        #region Children cua setting button
        //settingButton.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        //settingButton.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        //settingButton.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

        //settingButton.transform.GetChild(0).GetComponent<RectTransform>().DOAnchorPosY(-150, .5f).SetEase(ease).SetUpdate(true);
        //settingButton.transform.GetChild(1).GetComponent<RectTransform>().DOAnchorPosY(-300, .5f).SetEase(ease).SetUpdate(true);
        //settingButton.transform.GetChild(2).GetComponent<RectTransform>().DOAnchorPosY(-450, .5f).SetEase(ease).SetUpdate(true)
        //    .OnComplete(() =>
        //    {
        //        settingButton.enabled = true;
        //    });
        #endregion

        #region Children cua panel
        pausePanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector3(460, 875, 0);
        pausePanel.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector3(460, 875, 0);
        pausePanel.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector3(460, 875, 0);

        pausePanel.transform.GetChild(0).GetComponent<RectTransform>().DOAnchorPosY(725, .4f).SetEase(Ease.InOutQuad).SetUpdate(true);
        pausePanel.transform.GetChild(1).GetComponent<RectTransform>().DOAnchorPosY(575, .4f).SetEase(Ease.InOutQuad).SetUpdate(true);
        pausePanel.transform.GetChild(2).GetComponent<RectTransform>().DOAnchorPosY(425, .4f).SetEase(Ease.InOutQuad).SetUpdate(true)
            .OnComplete(() =>
            {
                settingOffButton.interactable = true;
                //pausePanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().DOFade(1f, 0.1f).SetEase(Ease.InOutQuad).SetUpdate(true);
                //pausePanel.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().DOFade(1f, 0.1f).SetEase(Ease.InOutQuad).SetUpdate(true);
                //pausePanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(1f, 0.1f).SetEase(Ease.InOutQuad).SetUpdate(true);
            });
        #endregion

        Time.timeScale = 0;
    }

    public void _Resume()
    {
        settingOffButton.interactable = false;
        pausePanel.transform.GetComponent<Image>().DOFade(0f, .5f).SetEase(Ease.InOutQuad).SetUpdate(true);
        settingOffButton.transform.DORotate(new Vector3(0, 0, 180), .5f).SetEase(Ease.InOutQuad).SetUpdate(true)
            .OnComplete(() =>
            {
                settingOffButton.transform.rotation = Quaternion.Euler(0, 0, 0);
            }); ;

        pausePanel.transform.GetChild(0).GetComponent<RectTransform>().DOAnchorPosY(875, .5f).SetEase(Ease.InOutQuad).SetUpdate(true);
        pausePanel.transform.GetChild(1).GetComponent<RectTransform>().DOAnchorPosY(875, .5f).SetEase(Ease.InOutQuad).SetUpdate(true);
        pausePanel.transform.GetChild(2).GetComponent<RectTransform>().DOAnchorPosY(875, .5f).SetEase(Ease.InOutQuad).SetUpdate(true)
            .OnComplete(() =>
            {
                pausePanel.SetActive(false);
                settingOnButton.interactable = true;
            });
        Time.timeScale = 1;
    }

    public void _TurnOffSound()
    {
        pausePanel.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        //             sound on button   sound off button
    }

    public void _TurnOnSound()
    {
        pausePanel.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        //            sound on button    sound off button
    }

    public void _TurnOffMusic()
    {
        pausePanel.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        //           music on button     music off button
    }

    public void _TurnOnMusic()
    {
        pausePanel.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        //           music on button     music off button
    }

    public void _QuitConfirmation()
    {
        quitPanel.transform.GetChild(0).GetComponent<RectTransform>().localScale = Vector3.zero;
        quitPanel.SetActive(true);
        quitPanel.transform.GetChild(0).GetComponent<RectTransform>().DOScale(Vector3.one, .25f).SetEase(Ease.InOutQuad).SetUpdate(true);
    }

    public void _GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void _TakeTheLastChance()
    {
        //lastChancePanel.transform.GetChild(0).GetComponent<RectTransform>().localScale = Vector3.zero;
        lastChancePanel.SetActive(true);
        //lastChancePanel.transform.GetChild(0).GetComponent<RectTransform>().DOScale(Vector3.one, .15f).SetEase(Ease.InOutQuad).SetUpdate(true);
    }

    public void _GetMoreTime()
    {
        if (ProgressController.instance._GetCoinsInPossession() >= 100)
        {
            ProgressController.instance._SetCoinsInPossession(100, false);
            TimeController.time += 60;
            lastChancePanel.transform.GetChild(0).GetComponent<RectTransform>().DOScale(Vector3.zero, .25f).SetEase(Ease.InOutQuad).SetUpdate(true)
            .OnComplete(() =>
            {
                lastChancePanel.SetActive(false);
            });
            TimeController.isSaved = true;
        }
    }

    public void _WatchAds()
    {
        //code xem quang cao

        TimeController.time += 15;
        lastChancePanel.transform.GetChild(0).GetComponent<RectTransform>().DOScale(Vector3.zero, .25f).SetEase(Ease.InOutQuad).SetUpdate(true)
        .OnComplete(() =>
        {
            lastChancePanel.SetActive(false);
        });
        TimeController.isSaved = true;
    }

    public void _RejectChance()
    {
        rejectChancePanel.transform.GetChild(0).GetComponent<RectTransform>().localScale = Vector3.zero;
        rejectChancePanel.SetActive(true);
        rejectChancePanel.transform.GetChild(0).GetComponent<RectTransform>().DOScale(Vector3.one, .25f).SetEase(Ease.InOutQuad).SetUpdate(true);
    }

    public void _GameOver()
    {
        //gameOverPanel.transform.GetChild(0).GetComponent<RectTransform>().localScale = Vector3.zero;
        gameOverPanel.SetActive(true);
        //gameOverPanel.transform.GetChild(0).GetComponent<RectTransform>().DOScale(Vector3.one, .25f).SetEase(Ease.InOutQuad).SetUpdate(true);
    }

    public void _Replay()
    {
        SceneManager.LoadScene(1);
    }

    public void _CompleteLevel()
    {
        Time.timeScale = 0;

        numberOfStars = TimeController.instance._NumberOfStarsAchieved();
        for (int i = 0; i < numberOfStars; i++)
        {
            starsAchieved.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
        }
        winPanel.transform.GetChild(0).GetComponent<RectTransform>().localScale = Vector3.zero;
        winPanel.SetActive(true);
        winPanel.transform.GetChild(0).GetComponent<RectTransform>().DOScale(Vector3.one, .25f).SetEase(Ease.InOutQuad).SetUpdate(true);

        ProgressController.instance._SetStarsAchieved(numberOfStars);
        ProgressController.instance._SetCoinsInPossession(numberOfStars * 50, true);
    }

    public void _GoToNextLevel()
    {
        if (LevelController.level == Resources.LoadAll("Levels").Length)
        {
            _GoToMenu();
        }
        else
        {
            LevelController.level++;
            LevelController.instance._PlayLevel(LevelController.level);
        }
    }

    #endregion

    #region Click Tiles
    public void _ClickTile(Transform currentTile)
    {
        currentTile.DOKill();
        currentTile.GetComponent<RectTransform>().DOScale(new Vector3(.8f, .8f, 1), .1f).SetEase(Ease.InOutQuad).SetUpdate(true)
            .OnComplete(() =>
            {
                currentTile.GetComponent<RectTransform>().DOScale(Vector3.one, .1f).SetEase(Ease.InOutQuad).SetUpdate(true);
                //currentTile.GetChild(0).GetComponent<Image>().color = Color.green;
            });
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
                //tile = currentTile;
                //siblingIndex = currentTile.GetSiblingIndex();
                isCoupled = !isCoupled;
            }
            else
            {
                if (tile.GetComponent<TileController>().Id != currentTile.GetComponent<TileController>().Id)
                {
                    Debug.Log("Click khac tile khac ID");
                    //tile.GetChild(0).GetComponent<Image>().color = Color.white;
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

    void _ResetTileState()
    {
        DOTween.Clear();
        foreach (var tile in BoardController.buttonList)
        {
            tile.localScale = Vector3.one;
        }
        isHinted = false;
    }

    #endregion

    #region Supporter
    public void _EnableSupporter(bool isEnabled)
    {
        btSpHint.interactable = isEnabled;
        btSpMagicWand.interactable = isEnabled;
        btSpFreeze.interactable = isEnabled;
        btSpShuffle.interactable = isEnabled;
    }

    public void _SupporterHint() //Hint: Khi sử dụng sẽ gợi ý 1 kết quả
    {
        _ResetTileState();
        while (!isHinted)
        {
            int index = Random.Range(1, SpriteController.spritesDict.Count);
            List<Transform> temp = BoardController.instance._SearchSameTiles(SpriteController.spritesDict[index.ToString()]);
            if (temp.Count != 0)
            {
                for (int i = 0; i < (temp.Count - 1); i++)
                {
                    for (int j = (i + 1); j < temp.Count; j++)
                    {
                        if (_HasAvailableConnection(temp[i], temp[j]))
                        {
                            isHinted = true;
                            goto endloop;
                        }
                    }
                }
            endloop:;
            }
        }

        int n = points.Length;
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i] == null)
            {
                n--;
            }
        }
        for (int i = 0; i < n; i++)
        {
            if (i == 0 || i == n - 1)
            {
                points[i].DOScale(new Vector3(.8f, .8f, .8f), .5f).SetEase(Ease.InOutQuad).SetUpdate(true).SetLoops(-1, LoopType.Yoyo);
            }
        }
    }

    public void _SupporterMagicWand() //Đũa thần: 2 kết quả hoàn thành
    {
        Time.timeScale = 1;
        _ResetTileState();
        EventSystem.current.SetSelectedGameObject(null);
        int numCouple = 0;
        for (int index = 1; index < SpriteController.spritesDict.Count; index++)
        {
        check:
            if (numCouple == 2)
            {
                break;
            }
            List<Transform> temp = BoardController.instance._SearchSameTiles(SpriteController.spritesDict[index.ToString()]);

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
    }

    public void _SupporterFreezeTime() //Snow: Đóng băng thời gian 10 giây
    {
        Debug.Log("Đóng băng thời gian 10 giây");
        Time.timeScale = 1;
        StartCoroutine(FreezeTime());
    }

    public void _SupporterShuffle() //Đổi vị trí: Khi người chơi sử dụng, các item thay đổi vị trí cho nhau.
    {
        _ResetTileState();
        isCoupled = true;
        EventSystem.current.SetSelectedGameObject(null);
        BoardController.instance._RearrangeTiles();
    }

    #endregion

    #region Booster
    public void _BoosterTimeWizard() //Time : Tăng 10 giây khi sử dụng
    {
        boosterPanel.SetActive(false);
        TimeController.instance._SetTimeForSlider(true);
    }

    public void _BoosterEarthquake() //Remove All: Xoá 1 hình bất kỳ
    {
        boosterPanel.SetActive(false);
        bool isFounded = false;
        while (!isFounded)
        {
            int index = Random.Range(1, SpriteController.spritesDict.Count);
            List<Transform> temp = BoardController.instance._SearchSameTiles(SpriteController.spritesDict[index.ToString()]);
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
        boosterPanel.SetActive(false);

    }

    #endregion

    #region IEnumerator
    IEnumerator MakeConnection(params Transform[] linePositions)
    {
        LineController.instance._DrawLine(0.15f * tile.GetComponent<TileController>().Size / 100, linePositions);
        yield return new WaitForSeconds(0.4f);
        LineController.instance._EraseLine();
    }

    IEnumerator DestroyTiles(params Transform[] linePositions)
    {
        settingOnButton.interactable = false;
        _EnableSupporter(false);
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
                int index = i;
                linePositions[index].GetComponent<Button>().interactable = false;
                linePositions[index].GetComponent<RectTransform>().DOSizeDelta(Vector2.zero, 0.4f).SetEase(Ease.InBack).SetUpdate(true)
                    .OnComplete(delegate { linePositions[index].gameObject.SetActive(false); });
                BoardController.instance._DeactivateTile(linePositions[index].GetComponent<TileController>());
            }
        }
        yield return new WaitForSeconds(0.4f);
        _ResetTileState();
        BoardController.instance._ActivateGravity();
        yield return new WaitForSeconds(0.2f);
        settingOnButton.interactable = true;
        _EnableSupporter(true);
        BoardController.instance._CheckPossibleConnection();
        BoardController.instance._CheckProcess();
    }

    IEnumerator FreezeTime()
    {
        TimeController.instance._FreezeTime(true);
        yield return new WaitForSeconds(10);
        TimeController.instance._FreezeTime(false);
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

        #region 1 line
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
        #endregion

        #region 2 lines
        if (_HasHorizontalLshapedPath(tile1, tile2))
        {
            return true;
        }
        if (_HasVerticalLshapedPath(tile1, tile2))
        {
            return true;
        }
        #endregion

        #region 3 lines
        if (_HasHorizontalZshapedPath(tile1, tile2))
        {
            return true;
        }
        if (_HasVerticalZshapedPath(tile1, tile2))
        {
            return true;
        }

        // check more right
        if (_HasHorizontalUshapedPath(tile1, tile2, 1))
        {
            return true;
        }
        // check more left
        if (_HasHorizontalUshapedPath(tile1, tile2, -1))
        {
            return true;
        }
        // check more down
        if (_HasVerticalUshapedPath(tile1, tile2, 1))
        {
            return true;
        }
        // check more up
        if (_HasVerticalUshapedPath(tile1, tile2, -1))
        {
            return true;
        }
        #endregion

        return false;
    }

    //button cung hang
    bool _HasHorizontalLine(Transform tile1, Transform tile2)
    {
        float y = tile1.localPosition.y;
        float min = Mathf.Min(tile1.localPosition.x, tile2.localPosition.x);
        float max = Mathf.Max(tile1.localPosition.x, tile2.localPosition.x);
        float buttonSide = tile1.GetComponent<TileController>().Size;
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
        float buttonSide = tile1.GetComponent<TileController>().Size;
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

    //2 button trong hinh chu nhat ngang: check hinh chu L
    bool _HasHorizontalLshapedPath(Transform tile1, Transform tile2)
    {
        #region Setup toa do, kich thuoc, dieu kien

        if (tile1.localPosition.x == tile2.localPosition.x || tile1.localPosition.y == tile2.localPosition.y)
        {
            return false;
        }

        Transform leftTile, rightTile;
        float leftX = Mathf.Min(tile1.localPosition.x, tile2.localPosition.x);
        float rightX = Mathf.Max(tile1.localPosition.x, tile2.localPosition.x);
        if (leftX == tile1.localPosition.x)
        {
            leftTile = tile1;
            rightTile = tile2;
        }
        else
        {
            leftTile = tile2;
            rightTile = tile1;
        }

        #endregion

        for (float i = leftX; i <= rightX; i += (rightX - leftX))
        {
            tempAlignWithLeft.localPosition = new Vector3(i, leftTile.localPosition.y, 0);
            tempAlignWithRight.localPosition = new Vector3(i, rightTile.localPosition.y, 0);
            if (i == leftX)
            {
                if (!BoardController.instance._HasButtonInLocation(i, rightTile.localPosition.y) &&
                    _HasVerticalLine(leftTile, tempAlignWithRight) && _HasHorizontalLine(tempAlignWithRight, rightTile))
                {
                    _SetLinePositions(leftTile, tempAlignWithRight, rightTile, null);
                    return true;
                }
            }
            else
            {
                if (!BoardController.instance._HasButtonInLocation(i, leftTile.localPosition.y) &&
                    _HasHorizontalLine(leftTile, tempAlignWithLeft) && _HasVerticalLine(tempAlignWithLeft, rightTile))
                {
                    _SetLinePositions(leftTile, tempAlignWithLeft, rightTile, null);
                    return true;
                }
            }
        }
        return false;
    }

    //2 button trong hinh chu nhat dung: check hinh chu L
    bool _HasVerticalLshapedPath(Transform tile1, Transform tile2)
    {
        #region Setup toa do, kich thuoc, dieu kien

        if (tile1.localPosition.x == tile2.localPosition.x || tile1.localPosition.y == tile2.localPosition.y)
        {
            return false;
        }

        Transform topTile, bottomTile;
        float topY = Mathf.Max(tile1.localPosition.y, tile2.localPosition.y);
        float bottomY = Mathf.Min(tile1.localPosition.y, tile2.localPosition.y);
        if (topY == tile1.localPosition.y)
        {
            topTile = tile1;
            bottomTile = tile2;
        }
        else
        {
            topTile = tile2;
            bottomTile = tile1;
        }

        #endregion

        for (float i = topY; i >= bottomY; i -= (topY - bottomY))
        {
            tempAlignWithTop.localPosition = new Vector3(topTile.localPosition.x, i, 0);
            tempAlignWithBottom.localPosition = new Vector3(bottomTile.localPosition.x, i, 0);
            if (i == topY)
            {
                if (!BoardController.instance._HasButtonInLocation(bottomTile.localPosition.x, i) &&
                    _HasHorizontalLine(topTile, tempAlignWithBottom) && _HasVerticalLine(tempAlignWithBottom, bottomTile))
                {
                    _SetLinePositions(topTile, tempAlignWithBottom, bottomTile, null);
                    return true;
                }
            }
            else
            {
                if (!BoardController.instance._HasButtonInLocation(topTile.localPosition.x, i) &&
                        _HasVerticalLine(topTile, tempAlignWithTop) && _HasHorizontalLine(tempAlignWithTop, bottomTile))
                {
                    _SetLinePositions(topTile, tempAlignWithStart, bottomTile, null);
                    return true;
                }
            }
        }
        return false;
    }

    //2 button trong hinh chu nhat ngang: check hinh chu Z
    bool _HasHorizontalZshapedPath(Transform tile1, Transform tile2)
    {
        #region Setup toa do, kich thuoc, dieu kien

        if (tile1.localPosition.x == tile2.localPosition.x || tile1.localPosition.y == tile2.localPosition.y)
        {
            return false;
        }

        Transform leftTile, rightTile;
        float leftX = Mathf.Min(tile1.localPosition.x, tile2.localPosition.x);
        float rightX = Mathf.Max(tile1.localPosition.x, tile2.localPosition.x);
        if (leftX == tile1.localPosition.x)
        {
            leftTile = tile1;
            rightTile = tile2;
        }
        else
        {
            leftTile = tile2;
            rightTile = tile1;
        }
        float buttonSide = tile1.GetComponent<TileController>().Size;
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
    bool _HasVerticalZshapedPath(Transform tile1, Transform tile2)
    {
        #region Setup toa do, kich thuoc, dieu kien

        if (tile1.localPosition.x == tile2.localPosition.x || tile1.localPosition.y == tile2.localPosition.y)
        {
            return false;
        }

        Transform topTile, bottomTile;
        float topY = Mathf.Max(tile1.localPosition.y, tile2.localPosition.y);
        float bottomY = Mathf.Min(tile1.localPosition.y, tile2.localPosition.y);
        if (topY == tile1.localPosition.y)
        {
            topTile = tile1;
            bottomTile = tile2;
        }
        else
        {
            topTile = tile2;
            bottomTile = tile1;
        }
        float buttonSide = tile1.GetComponent<TileController>().Size;
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

    //mo rong ra theo chieu ngang: check hinh chu U
    bool _HasHorizontalUshapedPath(Transform tile1, Transform tile2, int direct)
    {
        #region Setup toa do, kich thuoc, dieu kien

        if (tile1.localPosition.y == tile2.localPosition.y)
        {
            return false;
        }

        Transform startTile, endTile;
        float lowerX = Mathf.Min(tile1.localPosition.x, tile2.localPosition.x);
        float higherX = Mathf.Max(tile1.localPosition.x, tile2.localPosition.x);
        float buttonSide = tile1.GetComponent<TileController>().Size;
        float width = BoardController.column * buttonSide;
        bool checkPoint;

        #endregion

        if (direct > 0)
        {
            #region Di tu trai sang phai

            if (lowerX == tile1.localPosition.x)
            {
                startTile = tile1;
                endTile = tile2;
            }
            else
            {
                startTile = tile2;
                endTile = tile1;
            }

            for (float i = (higherX + buttonSide); i <= (width + buttonSide) / 2; i += buttonSide)
            {
                tempAlignWithStart.localPosition = new Vector3(i, startTile.localPosition.y, 0);
                tempAlignWithEnd.localPosition = new Vector3(i, endTile.localPosition.y, 0);
                checkPoint = BoardController.instance._HasButtonInLocation(i, startTile.localPosition.y)
                    || BoardController.instance._HasButtonInLocation(i, endTile.localPosition.y);
                if (!checkPoint && _HasHorizontalLine(startTile, tempAlignWithStart) &&
                    _HasVerticalLine(tempAlignWithStart, tempAlignWithEnd) && _HasHorizontalLine(tempAlignWithEnd, endTile))
                {
                    _SetLinePositions(startTile, tempAlignWithStart, tempAlignWithEnd, endTile);
                    return true;
                }
            }

            #endregion
        }
        else
        {
            #region Di tu phai sang trai

            if (lowerX == tile1.localPosition.x)
            {
                startTile = tile2;
                endTile = tile1;
            }
            else
            {
                startTile = tile1;
                endTile = tile2;
            }

            for (float i = (lowerX - buttonSide); i >= -(width + buttonSide) / 2; i -= buttonSide)
            {
                tempAlignWithStart.localPosition = new Vector3(i, startTile.localPosition.y, 0);
                tempAlignWithEnd.localPosition = new Vector3(i, endTile.localPosition.y, 0);
                checkPoint = BoardController.instance._HasButtonInLocation(i, startTile.localPosition.y)
                    || BoardController.instance._HasButtonInLocation(i, endTile.localPosition.y);
                if (!checkPoint && _HasHorizontalLine(startTile, tempAlignWithStart) &&
                    _HasVerticalLine(tempAlignWithStart, tempAlignWithEnd) && _HasHorizontalLine(tempAlignWithEnd, endTile))
                {
                    _SetLinePositions(startTile, tempAlignWithStart, tempAlignWithEnd, endTile);
                    return true;
                }
            }

            #endregion
        }

        return false;
    }

    //mo rong ra theo chieu doc: check hinh chu U
    bool _HasVerticalUshapedPath(Transform tile1, Transform tile2, int direct)
    {
        #region Setup toa do, kich thuoc, dieu kien

        if (tile1.localPosition.x == tile2.localPosition.x)
        {
            return false;
        }

        Transform startTile, endTile;
        float higherY = Mathf.Max(tile1.localPosition.y, tile2.localPosition.y);
        float lowerY = Mathf.Min(tile1.localPosition.y, tile2.localPosition.y);
        float buttonSide = tile1.GetComponent<TileController>().Size;
        float height = BoardController.row * buttonSide;
        bool checkPoint;

        #endregion

        if (direct > 0)
        {
            #region Di tu tren xuong duoi

            if (higherY == tile1.localPosition.y)
            {
                startTile = tile1;
                endTile = tile2;
            }
            else
            {
                startTile = tile2;
                endTile = tile1;
            }

            for (float i = (lowerY - buttonSide); i >= -(height + buttonSide) / 2; i -= buttonSide)
            {
                tempAlignWithStart.localPosition = new Vector3(startTile.localPosition.x, i, 0);
                tempAlignWithEnd.localPosition = new Vector3(endTile.localPosition.x, i, 0);
                checkPoint = BoardController.instance._HasButtonInLocation(startTile.localPosition.x, i)
                    || BoardController.instance._HasButtonInLocation(endTile.localPosition.x, i);
                if (!checkPoint && _HasVerticalLine(startTile, tempAlignWithStart) &&
                    _HasHorizontalLine(tempAlignWithStart, tempAlignWithEnd) && _HasVerticalLine(tempAlignWithEnd, endTile))
                {
                    _SetLinePositions(startTile, tempAlignWithStart, tempAlignWithEnd, endTile);
                    return true;
                }
            }

            #endregion
        }
        else
        {
            #region Di tu duoi len tren

            if (higherY == tile1.localPosition.y)
            {
                startTile = tile2;
                endTile = tile1;
            }
            else
            {
                startTile = tile1;
                endTile = tile2;
            }

            for (float i = (higherY + buttonSide); i <= (height + buttonSide) / 2; i += buttonSide)
            {
                tempAlignWithStart.localPosition = new Vector3(startTile.localPosition.x, i, 0);
                tempAlignWithEnd.localPosition = new Vector3(endTile.localPosition.x, i, 0);
                checkPoint = BoardController.instance._HasButtonInLocation(startTile.localPosition.x, i)
                    || BoardController.instance._HasButtonInLocation(endTile.localPosition.x, i);
                if (!checkPoint && _HasVerticalLine(startTile, tempAlignWithStart) &&
                    _HasHorizontalLine(tempAlignWithStart, tempAlignWithEnd) && _HasVerticalLine(tempAlignWithEnd, endTile))
                {
                    _SetLinePositions(startTile, tempAlignWithStart, tempAlignWithEnd, endTile);
                    return true;
                }
            }

            #endregion
        }

        return false;
    }

    #endregion

}
