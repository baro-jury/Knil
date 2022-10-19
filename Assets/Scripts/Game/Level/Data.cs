using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class LevelData
{
    public int level;
    public int theme;
    public float[] time = { 1, 0, 0, 0 };
    public List<ProcessData> process;

    public LevelData() { }

    public LevelData(int level, int theme, float[] time, List<ProcessData> process)
    {
        this.level = level;
        this.theme = theme;
        this.time = time;
        this.process = process;
    }
}

public class ProcessData
{
    public int TotalTile { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
    public string[,] Matrix { get; set; }
    public bool PullDown { get; set; }
    public bool PullUp { get; set; }
    public bool PullLeft { get; set; }
    public bool PullRight { get; set; }

    public ProcessData() { }

    public ProcessData(int row, int column, bool pullDown, bool pullUp, bool pullLeft, bool pullRight)
    {
        Row = row;
        Column = column;
        PullDown = pullDown;
        PullUp = pullUp;
        PullLeft = pullLeft;
        PullRight = pullRight;
    }

    public ProcessData(int totalTile, int row, int column, string[,] matrix, bool pullDown, bool pullUp, bool pullLeft, bool pullRight) : this(row, column, pullDown, pullUp, pullLeft, pullRight)
    {
        TotalTile = totalTile;
        Matrix = matrix;
    }
}