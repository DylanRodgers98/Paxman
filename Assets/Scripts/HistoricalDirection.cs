using System;

[Serializable]
public class HistoricalDirection : IComparable<HistoricalDirection>
{
    public string Actor { get; set; }
    public float Time { get; set; }
    public float X { get; set; }
    public float Y { get; set; }

    public HistoricalDirection(string actor, float time, float x, float y)
    {
        Actor = actor;
        Time = time;
        X = x;
        Y = y;
    }

    public int CompareTo(HistoricalDirection other)
    {
        return Time.CompareTo(other.Time);
    }
}