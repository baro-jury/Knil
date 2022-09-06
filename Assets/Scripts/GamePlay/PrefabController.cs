using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabController : MonoBehaviour
{
    //object ingame
    public void _ClickTile()
    {
        GameplayController.instance._ClickTile(gameObject.transform);
        //TestGameplay.instance._ClickTile(gameObject.transform);
    }

    //nut x
    public void _Close()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
