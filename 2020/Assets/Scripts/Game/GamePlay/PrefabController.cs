using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabController : MonoBehaviour
{
    [SerializeField]
    private AudioClip clickButtonClip, clickTileClip, closeClip;

    //close in game
    public void _Close()
    {
        PlayerPrefsController.instance.audioSource.PlayOneShot(closeClip);
        transform.parent.GetComponent<RectTransform>().DOScale(Vector3.zero, .25f).SetEase(Ease.InOutQuad).SetUpdate(true) //gameObject: form
            .OnComplete(() =>
            {
                transform.parent.parent.gameObject.SetActive(false); //panel
                transform.parent.localScale = Vector3.one;
            });
    }

    //close in demo game
    public void _DemoClose()
    {
        PlayerPrefsDemo.instance.audioSource.PlayOneShot(closeClip);
        transform.parent.GetComponent<RectTransform>().DOScale(Vector3.zero, .25f).SetEase(Ease.InOutQuad).SetUpdate(true) //gameObject: form
            .OnComplete(() =>
            {
                transform.parent.parent.gameObject.SetActive(false); //panel
                transform.parent.localScale = Vector3.one;
            });
    }

    //close while custom map
    public void _X()
    {
        SetUpMap.instance.audioSource.PlayOneShot(closeClip);
        transform.parent.DOScale(Vector3.zero, .25f).SetEase(Ease.InOutQuad).SetUpdate(true) //gameObject: form
            .OnComplete(() =>
            {
                transform.parent.parent.gameObject.SetActive(false); //panel
                transform.parent.localScale = Vector3.one;
            });
    }

    //edit object
    public void _SetObject()
    {
        SetUpMap.instance.audioSource.PlayOneShot(clickTileClip);
        SetUpMap.instance._EditTile(gameObject.transform);
    }

}
