using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabController : MonoBehaviour
{
    //object ingame
    public void _ClickTile()
    {
        GameplayController.instance._ClickTile(gameObject.transform);
    }

    //nut x
    public void _Close()
    {
        transform.parent.gameObject.SetActive(false);
    }

    //edit object
    public void _Click()
    {
        SetUpMap.instance._EditTile(gameObject.transform);
    }

    //object in trial game
    public void _TileTrialClick()
    {
        GameplayTrial.instance._ClickTile(gameObject.transform);
    }
}
