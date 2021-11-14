using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ColorSetting : ScriptableObject
{
    public Material planetMat;
    public BiomeColorSetting biomeColorSetting;

    [System.Serializable]
    public class BiomeColorSetting
    {
        public Biome[] biomes;
        public NoiseSetting noise;
        public float noiseOffset;
        public float noiseStrength;
        [Range(0, 1)]
        public float blendAmount;
        
        [System.Serializable]
        public class Biome
        {
            public Gradient gradient;
            public Color tintColor;
            [Range(0, 1)]
            public float startheight;
            [Range(0, 1)]
            public float colorPercent;
        }
    }
}
