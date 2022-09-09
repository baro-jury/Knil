using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Backgrounds;

    // Start is called before the first frame update
    void Start()
    {
        int bgrIndex = Random.Range(0, Backgrounds.Length);
        GameObject background = Backgrounds[bgrIndex];
        foreach (GameObject obj in Backgrounds)
        {
            if (obj != background)
            {
                obj.SetActive(false);
            }
        }

        SpriteRenderer sr = background.GetComponent<SpriteRenderer>();
        Vector3 temp = background.transform.localScale;
        float height = sr.bounds.size.y;
        float width = sr.bounds.size.x;
        float scaleHeight = Camera.main.orthographicSize * 2f;
        float scaleWidth = scaleHeight * Screen.width / Screen.height;
        //transform.localScale = new Vector3(scaleWidth, scaleHeight, 0);
        temp.y = scaleHeight / height;
        temp.x = scaleWidth / width;
        background.transform.localScale = temp;
    }
}
