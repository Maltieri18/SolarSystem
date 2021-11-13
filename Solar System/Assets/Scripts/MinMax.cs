using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMax
{
    public float Min { get; private set; }
    public float Max { get; private set; }

    public MinMax()
    {
        Min = float.MaxValue;
        Max = float.MinValue;
    }

    public void AddValue(float val)
    {
        if(val < Min)
        {
            Min = val;
        }
        if(val > Max)
        {
            Max = val;
        }
    }
}
