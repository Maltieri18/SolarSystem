using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSetting 
{
    public float strength = 1;
    [Range(1, 12)]
    public int numNoiseLayers = 1;
    public float baseRoughness = 1;
    public float roughness = 2; //Keep this above 1
    public float persistance = 0.5f; //Keep this less than 1
    public Vector3 centre;
    public float minValue;
}
