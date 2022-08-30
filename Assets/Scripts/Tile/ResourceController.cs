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
    public List<Sprite> demo;

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

    void Start()
    { 
        _CreateDictionary(AfternoonTeas);
    }

    public void _CreateDictionary(List<Sprite> list)
    {
        Dictionary<Sprite, int> temp = new Dictionary<Sprite, int>();
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
            temp.Add(sprite, idElement);
            idElement++;
        }
        spritesDict = temp;
        idElement = 0;
    }

}
