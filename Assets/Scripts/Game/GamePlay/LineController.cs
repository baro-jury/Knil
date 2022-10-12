using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public static LineController instance;

    private LineRenderer line;

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
        line = gameObject.GetComponent<LineRenderer>();
    }

    public void _DrawLine(float width, params Transform[] points)
    {
        line.widthMultiplier = width;

        int n = points.Length;
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i] == null)
            {
                n--;
            }
        }
        line.positionCount = n;
        for (int i = 0; i < n; i++)
        {
            float temp = points[i].position.y / points[i].localPosition.y;
            if (i == 0 || i == n - 1)
            {
                line.SetPosition(i, new Vector3(points[i].position.x, points[i].position.y, 0));
            }
            else
            {
                line.SetPosition(i, new Vector3(points[i].position.x, points[i].position.y + 50 * temp, 0));
            }
            
            //line.SetPosition(i, new Vector3(points[i].localPosition.x / 40, points[i].localPosition.y / 40, 0));

            //line.SetPosition(i, new Vector3(points[i].localPosition.x / (96 * Screen.height / 1920), (points[i].localPosition.y + 50) / (96 * Screen.height / 1920), 0));
        }
    }

    public void _EraseLine()
    {
        line.positionCount = 0;
    }

}
