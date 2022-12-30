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


    [SerializeField]
    private AudioClip clickTileClip, blockerClip;

    //object ingame
    public void _TileClicked()
    {
        //if (transform.GetComponent<TileController>().Id == 0)
        if (Id == 0)
        {
            PlayerPrefsController.instance.audioSource.PlayOneShot(blockerClip);
        }
        else
        {
            PlayerPrefsController.instance.audioSource.PlayOneShot(clickTileClip);
        }
        GameplayController.instance._ClickTile(transform);
    }

    //object in demo game
    public void _TileDemoClicked()
    {
        if (Id == 0)
        {
            PlayerPrefsDemo.instance.audioSource.PlayOneShot(blockerClip);
        }
        else
        {
            PlayerPrefsDemo.instance.audioSource.PlayOneShot(clickTileClip);
        }
        GameplayDemo.instance._ClickTile(transform);
    }

    //object in trial game
    public void _TileTrialClicked()
    {
        if (Id == 0)
        {
            GameplayTrial.instance.audioSource.PlayOneShot(blockerClip);
        }
        else
        {
            GameplayTrial.instance.audioSource.PlayOneShot(clickTileClip);
        }
        GameplayTrial.instance._ClickTile(transform);
    }
}
