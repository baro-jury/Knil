using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    private int id;

    // Start is called before the first frame update
    void Start()
    {
        //this.transform.parent = BoardController.instance.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void _Click()
    {
        //GameplayController.instance._ClickTile(gameObject.transform);
        TestGameplay.instance._ClickTile(gameObject.transform);
    }
}
