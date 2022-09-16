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
    public int row;
    public int column;
    public bool pullDown;
    public bool pullUp;
    public bool pullLeft;
    public bool pullRight;
    public string[,] matrix;

    public ProcessData() { }

    public ProcessData(int row, int column, bool pullDown, bool pullUp, bool pullLeft, bool pullRight)
    {
        this.row = row;
        this.column = column;
        this.pullDown = pullDown;
        this.pullUp = pullUp;
        this.pullLeft = pullLeft;
        this.pullRight = pullRight;
    }

    public ProcessData(int row, int column, bool pullDown, bool pullUp, bool pullLeft, bool pullRight, string[,] matrix) : this(row, column, pullDown, pullUp, pullLeft, pullRight)
    {
        this.matrix = matrix;
    }

    public override string ToString()
    {
        return "Process: " + row + " " + column + " " + pullDown + " " + pullUp + " " + pullLeft + " " + pullRight;
    }
}
