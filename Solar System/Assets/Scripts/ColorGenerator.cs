using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator
{
    ColorSetting settings;
    Texture2D texture;
    const int texResolution = 50;
    public void UpdateSettings(ColorSetting settings)
    {
        this.settings = settings;
        if(texture == null) texture = new Texture2D(texResolution, 1);

    }
    public void UpdateElevation(MinMax heightMinMax) //Updates the shader vector, which will color from min to max
    {
        settings.planetMat.SetVector("_heightMinMax", new Vector4(heightMinMax.Min, heightMinMax.Max, 0, 0));
    }

    public void UpdateColors()
    {
        Color[] colors = new Color[texResolution];

        for(int i = 0; i < texResolution; i++)
        {
            colors[i] = settings.gradient.Evaluate(i/(texResolution - 1f));
        }

        texture.SetPixels(colors);
        texture.Apply();
        settings.planetMat.SetTexture("_texture", texture);
    }
}
