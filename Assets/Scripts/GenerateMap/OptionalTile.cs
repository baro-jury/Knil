using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionalTile : MonoBehaviour
{
    public static OptionalTile instance;

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

    public void _Click()
    {
        SetUpMap.instance._EditTile(gameObject.transform);
    }

}
