using System;

[Serializable]
public class HistoricalDirection
{
    public float Time { get; set; }
    public float X { get; set; }
    public float Y { get; set; }

    public HistoricalDirection(float time, float x, float y)
    {
        Time = time;
        X = x;
        Y = y;
    }
}