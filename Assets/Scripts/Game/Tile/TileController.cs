using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public int Id { get; set; }
    public (int, int) Index { get; set; }
    public int Size { get; set; }

    public TileController() { }

    public TileController(int id, int indexRow, int indexCol, int size)
    {
        Id = id;
        Index = (indexRow, indexCol);
        Size = size;
    }

    //object ingame
    public void _TileClicked()
    {
        GameplayController.instance._ClickTile(gameObject.transform);
    }

    //object in trial game
    public void _TileTrialClicked()
    {
        GameplayTrial.instance._ClickTile(gameObject.transform);
    }
}