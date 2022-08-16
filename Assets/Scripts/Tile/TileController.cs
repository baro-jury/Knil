using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
    public int tileID;
    public LineRenderer line;

    public void _Click()
    {
        GameplayController.instance._ClickTile(this);
    }
}
