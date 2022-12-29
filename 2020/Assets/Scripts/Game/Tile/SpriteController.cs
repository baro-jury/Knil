using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    public static SpriteController instance;
    public static Dictionary<string, Sprite> spritesDict = new Dictionary<string, Sprite>();
    public static Dictionary<int, List<Sprite>> spriteListsDict = new Dictionary<int, List<Sprite>>();

    public List<Sprite> Icon;
    public List<Sprite> Desserts;
    public List<Sprite> Dolls;
    public List<Sprite> Food_Drink;
    public List<Sprite> Hobbies;
    public List<Sprite> Home;
    public List<Sprite> Mishmashes;
    public List<Sprite> Signs;
    public List<Sprite> Toys;
    public List<Sprite> Traveller;
    public Sprite blank, blocker, pedestal;

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
        _CreateDictionary(Icon);
        _AddListSpr();
    }

    void _AddListSpr()
    {
        spriteListsDict.Add(spriteListsDict.Count, Desserts);
        spriteListsDict.Add(spriteListsDict.Count, Dolls);
        spriteListsDict.Add(spriteListsDict.Count, Food_Drink);
        spriteListsDict.Add(spriteListsDict.Count, Hobbies);
        spriteListsDict.Add(spriteListsDict.Count, Home);
        spriteListsDict.Add(spriteListsDict.Count, Mishmashes);
        spriteListsDict.Add(spriteListsDict.Count, Signs);
        spriteListsDict.Add(spriteListsDict.Count, Toys);
        spriteListsDict.Add(spriteListsDict.Count, Traveller);
    }

    void _CreateDictionary(List<Sprite> list)
    {
        Dictionary<string, Sprite> temp = new Dictionary<string, Sprite>();
        temp.Add("0", blocker);
        foreach (Sprite sprite in list)
        {
            temp.Add(idElement.ToString(), sprite);
            idElement++;
        }
        spritesDict = temp;
        idElement = 1;
    }

    public void _CreateDictionary(int indexTheme)
    {
        Dictionary<string, Sprite> temp = new Dictionary<string, Sprite>();
        temp.Add("0", blocker);
        foreach (Sprite sprite in spriteListsDict[(indexTheme - 1) % 9])
        {
            temp.Add(idElement.ToString(), sprite);
            idElement++;
        }
        spritesDict = temp;
        idElement = 1;
    }

    public Dictionary<string, Sprite> _CreateSubDictionary(int from, int to)
    {
        Dictionary<string, Sprite> temp = new Dictionary<string, Sprite>();
        temp.Add("0", blocker);
        for (int i = from; i <= to; i++)
        {
            temp.Add(i.ToString(), spritesDict[i.ToString()]);
        }
        return temp;
    }

    public void _ShuffleImage(Dictionary<string, Sprite> dictionary)
    {
        for (int i = 1; i < (dictionary.Count - 1); i++)
        {
            int randomIndex = Random.Range(i + 1, dictionary.Count);

            //Sprite temp = dictionary[i.ToString()];
            //dictionary[i.ToString()] = dictionary[randomIndex.ToString()];
            //dictionary[randomIndex.ToString()] = temp;
            Sprite temp = dictionary.ElementAt(i).Value;
            dictionary[dictionary.ElementAt(i).Key] = dictionary.ElementAt(randomIndex).Value;
            dictionary[dictionary.ElementAt(randomIndex).Key] = temp;

        }
    }

}
