using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance;

    private bool isCoupled = true;

    [HideInInspector]
    public TileController tile;
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

    public void _ClickTile(TileController currentTile)
    {
        isCoupled = !isCoupled;
        if (!isCoupled)
        {
            tile = currentTile;
        }
        else
        {
            _CompareTile(currentTile, tile);
        }
    }

    void _CompareTile(TileController tile1, TileController tile2)
    {
        if (tile1 == tile2)
        {
            Debug.Log("click cung 1 object");
        }
        else
        {
            if (tile1.tileID != tile2.tileID)
            {
                Debug.Log("click khac object khac ID");
            }
            else
            {
                StartCoroutine(DrawLine(tile1, tile2));
            }
        }
    }

    IEnumerator DrawLine(TileController tile1, TileController tile2)
    {
        tile1.line.positionCount = 2;
        tile1.line.SetPosition(0, tile1.transform.position);
        tile1.line.SetPosition(1, tile2.transform.position);

        yield return new WaitForSeconds(0.5f);
        Destroy(tile1.gameObject);
        Destroy(tile2.gameObject);
    }

}
