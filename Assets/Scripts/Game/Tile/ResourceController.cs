using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public static ResourceController instance;
    public static Dictionary<string, Sprite> spritesDict = new();

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
        Dictionary<string, Sprite> temp = new();
        temp.Add("0", blocker);
        foreach (Sprite sprite in list)
        {
            temp.Add(idElement.ToString(), sprite);
            idElement++;
        }
        spritesDict = temp;
        idElement = 1;
    }

    public void _ShuffleImage(Dictionary<string, Sprite> dictionary)
    {
        for (int i = 1; i < (dictionary.Count - 1); i++)
        {
            int randomIndex = Random.Range(i + 1, dictionary.Count);

            Sprite temp = dictionary[i.ToString()];
            dictionary[i.ToString()] = dictionary[randomIndex.ToString()];
            dictionary[randomIndex.ToString()] = temp;
        }
    }

}
