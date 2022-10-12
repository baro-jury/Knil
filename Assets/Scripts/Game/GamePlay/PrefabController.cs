using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabController : MonoBehaviour
{
    //nut x
    public void _Close()
    {
        transform.parent.GetComponent<RectTransform>().DOScale(Vector3.zero, .25f).SetEase(Ease.InOutQuad).SetUpdate(true) //gameObject: form
            .OnComplete(() =>
            {
                transform.parent.parent.gameObject.SetActive(false); //panel
            });
    }

    //edit object
    public void _Click()
    {
        SetUpMap.instance._EditTile(gameObject.transform);
    }

}
