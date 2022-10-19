using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    public static SpriteController instance;
    public static Dictionary<string, Sprite> spritesDict = new();

    public List<Sprite> Icon;

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
        _CreateDictionary(Icon);
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
