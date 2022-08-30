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
            //if (i == 0 || i == (n - 1))
            //{
            //    line.SetPosition(i, points[i].position);
            //}
            //else
            //{
            //    line.SetPosition(i, new Vector3(points[i].localPosition.x / 40, points[i].localPosition.y / 40, 90));
            //}

            line.SetPosition(i, new Vector3(points[i].localPosition.x / 40, points[i].localPosition.y / 40, 0));

            //line.SetPosition(i, points[i].localPosition.normalized);
        }
    }

    public void _EraseLine()
    {
        line.positionCount = 0;
    }

}
