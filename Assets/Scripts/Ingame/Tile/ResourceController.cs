using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public static ResourceController instance;
    public static Dictionary<Sprite, string> spritesDict = new();

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

    private int idElement = 1;
    [SerializeField]
    private Sprite blocker;

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

    void Start()
    {
        
    }

    public void _CreateDictionary(List<Sprite> list)
    {
        Dictionary<Sprite, string> temp = new();
        temp.Add(blocker, "0");
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

    public void _ShuffleImage(Dictionary<Sprite, string> dictionary)
    {
        for (int i = 1; i < (dictionary.Count - 1); i++)
        {
            int randomIndex = Random.Range(i + 1, dictionary.Count);

            string temp = dictionary.ElementAt(i).Value;
            dictionary[dictionary.ElementAt(i).Key] = dictionary.ElementAt(randomIndex).Value;
            dictionary[dictionary.ElementAt(randomIndex).Key] = temp;
        }
    }

    public int _FindIndex(string value)
    {
        for (int i = 0; i < spritesDict.Count; i++)
        {
            if(spritesDict.ElementAt(i).Value == value)
            {
                return i;
            }
        }
        return default;
    }

}
