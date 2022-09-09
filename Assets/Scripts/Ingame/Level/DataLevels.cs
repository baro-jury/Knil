using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class DataLevels
{
    public int level;
    public int row;
    public int column;
    public float time;
    public bool pullDown;
    public bool pullUp;
    public bool pullLeft;
    public bool pullRight;

    public DataLevels() { }

    public DataLevels(int level, int row, int column, float time, bool pullDown, bool pullUp, bool pullLeft, bool pullRight)
    {
        this.level = level;
        this.row = row;
        this.column = column;
        this.time = time;
        this.pullDown = pullDown;
        this.pullUp = pullUp;
        this.pullLeft = pullLeft;
        this.pullRight = pullRight;
    }
}

public class LevelData
{
    public int level;
    public int theme;
    public float time;
    public string process;

    public LevelData() { }

    public LevelData(int level, int theme, float time, string process)
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
    public bool pullUp;
    public bool pullDown;
    public bool pullLeft;
    public bool pullRight;

    public ProcessData() { }

    public ProcessData(int row, int column, bool pullUp, bool pullDown, bool pullLeft, bool pullRight)
    {
        this.row = row;
        this.column = column;
        this.pullUp = pullUp;
        this.pullDown = pullDown;
        this.pullLeft = pullLeft;
        this.pullRight = pullRight;
    }

    public override string ToString()
    {
        return "Process: " + row + " " + column + " " + pullUp + " " + pullDown + " " + pullLeft + " " + pullRight;
    }
}
