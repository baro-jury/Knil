using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileImage : MonoBehaviour
{
    public static TileImage instance;

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
    public List<Sprite> demo;

    [HideInInspector]
    public static Dictionary<Sprite, string> spritesDict = new Dictionary<Sprite, string>();
    private int idElement = 1;

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
        _CreateDictionary(demo);
    }

    public void _CreateDictionary(List<Sprite> list)
    {
        Dictionary<Sprite, string> temp = new Dictionary<Sprite, string>();
        foreach (Sprite sprite in list)
        {
            //if (spritesDict.ContainsKey(sprite))
            //{
            //    spritesDict[sprite] = idElement;
            //}
            //else
            //{
            //    spritesDict.Add(sprite, idElement);
            //}
            temp.Add(sprite, idElement.ToString());
            idElement++;
        }
        spritesDict = temp;
        idElement = 1;
    }
}
