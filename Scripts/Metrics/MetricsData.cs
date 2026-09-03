using System;
using System.Collections.Generic;

[Serializable]

public class MetricsData
{
    public int restartCount;

    public float totalPlayTime;

    public int maxWaveReached;

    public int zombiesKilled;

    public int shotsFired;

    public int shotsHit;

    public float accuracy;

    public int damageTaken;

    public int reloads;

    public List<string> upgradesChosen = new();

    public List<float> waveTimes = new();
}
