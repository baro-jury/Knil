using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestGameplay : MonoBehaviour
{
    public static TestGameplay instance;

    private bool isCoupled = true;
    private int siblingIndex;

    [HideInInspector]
    public Transform tile;
    [HideInInspector]
    public Transform[] points = new Transform[4];

    [SerializeField]
    private Transform tempAlignWithLeft, tempAlignWithRight, tempAlignWithTop, tempAlignWithBottom, tempAlignWithStart, tempAlignWithEnd;

    //public List<int> idElement = new List<int>();

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

    #region Click
    public void _ClickTile(Transform currentTile)
    {
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
                Debug.Log("click cung 1 object");
                Debug.Log(currentTile.parent.GetComponent<RectTransform>().sizeDelta);
                Debug.Log(currentTile.gameObject.GetComponent<RectTransform>().sizeDelta);
            }
            else
            {
                if (ResourceController.spritesDict[tile.GetChild(0).GetComponent<Image>().sprite]
                    != ResourceController.spritesDict[currentTile.GetChild(0).GetComponent<Image>().sprite])
                {
                    Debug.Log("click khac object khac ID");
                }
                else
                {
                    if (_HasAvailableConnection(tile, currentTile))
                    {
                        StartCoroutine(MakeConnection(points));
                    }
                    else
                    {
                        Debug.Log("khong noi duoc");
                    }
                }
            }
        }
    }

    IEnumerator MakeConnection(Transform[] trans)
    {
        LineController.instance._DrawLine(trans);
        //yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(0.5f);

        int n = trans.Length;
        for (int i = 0; i < trans.Length; i++)
        {
            if (trans[i] == null)
            {
                n--;
            }
        }
        for (int i = 0; i < n; i++)
        {
            if (i == 0 || i == n - 1)
            {
                trans[i].gameObject.SetActive(false);
                ResourceController.spritesDict[trans[i].GetChild(0).GetComponent<Image>().sprite] = -1;
            }
        }

        LineController.instance._EraseLine();
    }

    void _SetLinePositions(Transform t0, Transform t1, Transform t2, Transform t3)
    {
        points[0] = t0;
        points[1] = t1;
        points[2] = t2;
        points[3] = t3;
    }
    #endregion

    #region Algorithm
    bool _HasAvailableConnection(Transform tile1, Transform tile2)
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
        if (tile1.localPosition == tile2.localPosition)
        {
            return false;
        }
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
        if (tile1.localPosition == tile2.localPosition)
        {
            return false;
        }
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

    //2 button trong hinh chu nhat ngang
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
        float leftY, rightY;
        if (leftX == tile1.localPosition.x)
        {
            leftY = tile1.localPosition.y;
            rightY = tile2.localPosition.y;
        }
        else
        {
            leftY = tile2.localPosition.y;
            rightY = tile1.localPosition.y;
        }
        leftTile.localPosition = new Vector3(leftX, leftY, 0);
        rightTile.localPosition = new Vector3(rightX, rightY, 0);
        float buttonSide = tile1.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        bool checkPoint;

        #endregion

        #region check chu L
        for (float i = leftX; i <= rightX; i += (rightX - leftX))
        {
            tempAlignWithLeft.localPosition = new Vector3(i, leftY, 0);
            tempAlignWithRight.localPosition = new Vector3(i, rightY, 0);
            if (_HasHorizontalLine(leftTile, tempAlignWithLeft) &&
                _HasVerticalLine(tempAlignWithLeft, tempAlignWithRight) && _HasHorizontalLine(tempAlignWithRight, rightTile))
            {
                _SetLinePositions(leftTile, tempAlignWithLeft, tempAlignWithRight, rightTile);
                return true;
            }
        }
        #endregion

        #region check chu Z
        for (float i = (leftX + buttonSide); i < rightX; i += buttonSide)
        {
            tempAlignWithLeft.localPosition = new Vector3(i, leftY, 0);
            tempAlignWithRight.localPosition = new Vector3(i, rightY, 0);
            checkPoint = BoardController.instance._HasButtonInLocation(i, leftY) || BoardController.instance._HasButtonInLocation(i, rightY);
            if (!checkPoint && _HasHorizontalLine(leftTile, tempAlignWithLeft) &&
                _HasVerticalLine(tempAlignWithLeft, tempAlignWithRight) && _HasHorizontalLine(tempAlignWithRight, rightTile))
            {
                _SetLinePositions(leftTile, tempAlignWithLeft, tempAlignWithRight, rightTile);
                return true;
            }
        }
        #endregion
        return false;
    }

    //2 button trong hinh chu nhat dung
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
        float topX, bottomX;
        if (topY == tile1.localPosition.y)
        {
            topX = tile1.localPosition.x;
            bottomX = tile2.localPosition.x;
        }
        else
        {
            topX = tile2.localPosition.x;
            bottomX = tile1.localPosition.x;
        }
        topTile.localPosition = new Vector3(topX, topY, 0);
        bottomTile.localPosition = new Vector3(bottomX, bottomY, 0);
        float buttonSide = tile1.gameObject.GetComponent<RectTransform>().sizeDelta.y;
        bool checkPoint;

        #endregion

        #region check chu L
        for (float i = topY; i >= bottomY; i -= (topY - bottomY))
        {
            tempAlignWithTop.localPosition = new Vector3(topX, i, 0);
            tempAlignWithBottom.localPosition = new Vector3(bottomX, i, 0);
            if (_HasVerticalLine(topTile, tempAlignWithTop) &&
                _HasHorizontalLine(tempAlignWithTop, tempAlignWithBottom) && _HasVerticalLine(tempAlignWithBottom, bottomTile))
            {
                _SetLinePositions(topTile, tempAlignWithTop, tempAlignWithBottom, bottomTile);
                return true;
            }
        }
        #endregion

        #region check chu Z
        for (float i = (topY - buttonSide); i > bottomY; i -= buttonSide)
        {
            tempAlignWithTop.localPosition = new Vector3(topX, i, 0);
            tempAlignWithBottom.localPosition = new Vector3(bottomX, i, 0);
            checkPoint = BoardController.instance._HasButtonInLocation(topX, i) || BoardController.instance._HasButtonInLocation(bottomX, i);
            if (!checkPoint && _HasVerticalLine(topTile, tempAlignWithTop) &&
                _HasHorizontalLine(tempAlignWithTop, tempAlignWithBottom) && _HasVerticalLine(tempAlignWithBottom, bottomTile))
            {
                _SetLinePositions(topTile, tempAlignWithTop, tempAlignWithBottom, bottomTile);
                return true;
            }
        }
        #endregion
        return false;
    }

    //mo rong ra theo chieu ngang: check hinh chu U
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

            for (float i = (endX + buttonSide); i <= (tile1.parent.GetComponent<RectTransform>().sizeDelta.x + buttonSide) / 2; i += buttonSide)
            {
                tempAlignWithStart.localPosition = new Vector3(i, startY, 0);
                tempAlignWithEnd.localPosition = new Vector3(i, endY, 0);
                checkPoint = BoardController.instance._HasButtonInLocation(i, startY) || BoardController.instance._HasButtonInLocation(i, endY);
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

            startTile.localPosition = new Vector3(endX, endY, 0);
            endTile.localPosition = new Vector3(startX, startY, 0);

            for (float i = (startX - buttonSide); i >= -(tile1.parent.GetComponent<RectTransform>().sizeDelta.x + buttonSide) / 2; i -= buttonSide)
            {
                tempAlignWithStart.localPosition = new Vector3(i, endY, 0);
                tempAlignWithEnd.localPosition = new Vector3(i, startY, 0);
                checkPoint = BoardController.instance._HasButtonInLocation(i, startY) || BoardController.instance._HasButtonInLocation(i, endY);
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

            for (float i = (endY - buttonSide); i >= -(tile1.parent.GetComponent<RectTransform>().sizeDelta.y + buttonSide) / 2; i -= buttonSide)
            {
                tempAlignWithStart.localPosition = new Vector3(startX, i, 0);
                tempAlignWithEnd.localPosition = new Vector3(endX, i, 0);
                checkPoint = BoardController.instance._HasButtonInLocation(startX, i) || BoardController.instance._HasButtonInLocation(endX, i);
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

            startTile.localPosition = new Vector3(endX, endY, 0);
            endTile.localPosition = new Vector3(startX, startY, 0);

            for (float i = (startY + buttonSide); i <= (tile1.parent.GetComponent<RectTransform>().sizeDelta.y + buttonSide) / 2; i += buttonSide)
            {
                tempAlignWithStart.localPosition = new Vector3(endX, i, 0);
                tempAlignWithEnd.localPosition = new Vector3(startX, i, 0);
                checkPoint = BoardController.instance._HasButtonInLocation(startX, i) || BoardController.instance._HasButtonInLocation(endX, i);
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
