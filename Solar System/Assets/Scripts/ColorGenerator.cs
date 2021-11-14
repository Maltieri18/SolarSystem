using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator
{
    ColorSetting settings;
    Texture2D texture;
    const int texResolution = 50;
    INoiseFilter biomeNoiseFilter;
    public void UpdateSettings(ColorSetting settings)
    {
        this.settings = settings;
        if (texture == null || texture.height != settings.biomeColorSetting.biomes.Length)
        {
            texture = new Texture2D(texResolution, settings.biomeColorSetting.biomes.Length, TextureFormat.RGBA32, false);
        }

        biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.biomeColorSetting.noise);
    }
    public void UpdateElevation(MinMax heightMinMax) //Updates the shader vector, which will color from min to max
    {
        settings.planetMat.SetVector("_heightMinMax", new Vector4(heightMinMax.Min, heightMinMax.Max, 0, 0));
    }

    public float BiomePercentPoint(Vector3 spherePoint) //0 First biome, 1 Last biome. Range in between biomes
    {
        float heightPercent = (spherePoint.y + 1) / 2f; //0 south pole, 1 north pole
        heightPercent += (biomeNoiseFilter.Evaluate(spherePoint) - settings.biomeColorSetting.noiseOffset) * settings.biomeColorSetting.noiseStrength;
        float biomeIndex = 0;
        int numBiomes = settings.biomeColorSetting.biomes.Length;
        float blendRange = settings.biomeColorSetting.blendAmount / 2f + 0.002f;

        for(int i = 0; i < numBiomes; i++)
        {
            float dist = heightPercent - settings.biomeColorSetting.biomes[i].startheight;
            float weight = Mathf.InverseLerp(-blendRange, blendRange, dist);
            biomeIndex *= (1 - weight);
            biomeIndex += i * weight;
        }
        return biomeIndex / Mathf.Max(numBiomes - 1, 1);

    }
    public void UpdateColors()
    {
        Color[] colors = new Color[texture.width * texture.height];
        int colorIndex = 0;

        foreach (var biome in settings.biomeColorSetting.biomes)
        {
            for (int i = 0; i < texResolution; i++)
            {
                Color gradCol = biome.gradient.Evaluate(i / (texResolution - 1f));
                Color tintCol = biome.tintColor;
                colors[colorIndex++] = gradCol * (1 - biome.colorPercent) + tintCol * biome.colorPercent; //So if no tint, then color entirely based on gradient
            }
        }

        texture.SetPixels(colors);
        texture.Apply();
        settings.planetMat.SetTexture("_texture", texture);
    }
}
