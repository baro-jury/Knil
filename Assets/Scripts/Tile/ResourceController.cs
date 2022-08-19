using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public static ResourceController instance;

    public List<Sprite> AfternoonTeas;
    public List<Sprite> Butterflies;
    public List<Sprite> Candies;
    public List<Sprite> Desserts;
    public List<Sprite> FastFoods;
    public List<Sprite> Fruits;
    public List<Sprite> Gems;
    public List<Sprite> IceCreams;
    public List<Sprite> Makeups;
    public List<Sprite> Pokemons;
    public List<Sprite> Vegetables;

    [HideInInspector]
    public static Dictionary<Sprite, int> spritesDict = new Dictionary<Sprite, int>();
    private int idElement = 0;

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

    public List<Sprite> _SpriteListData(List<Sprite> list)
    {
        foreach (Sprite sprite in AfternoonTeas)
        {
            list.Add(sprite);
        }
        return list;
    }

    public void _CreateDictionary()
    {
        foreach (Sprite sprite in AfternoonTeas)
        {
            spritesDict.Add(sprite, idElement);
            idElement++;
        }
    }

}
