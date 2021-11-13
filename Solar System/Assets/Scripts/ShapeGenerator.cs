using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    ShapeSetting setting;
    INoiseFilter[] noiseFilters;
    public MinMax heightMinMax;
    public void UpdateSettings(ShapeSetting setting)
    {
        this.setting = setting;
        this.noiseFilters = new INoiseFilter[setting.noiseLayers.Length];
        heightMinMax = new MinMax();
        for(int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(setting.noiseLayers[i].noiseSetting);
        }
        
    }

    public Vector3 GetPointOnPlanet(Vector3 pointOnSphere)
    {
        float firstLayerVal = 0;
        float elevation = 0;

        if(noiseFilters.Length > 0)
        {
            firstLayerVal = noiseFilters[0].Evaluate(pointOnSphere);
            if(setting.noiseLayers[0].enabled)
            {
                elevation = firstLayerVal;
            }
        }

        for(int i = 1; i < noiseFilters.Length; i++)
        {
            if (setting.noiseLayers[i].enabled)
            {
                float mask = (setting.noiseLayers[i].useFirstLayerAsMask) ? firstLayerVal : 1; 
                elevation += noiseFilters[i].Evaluate(pointOnSphere) * mask; //Layermask so subsequent layers from form base layer
            }
        }
        elevation = setting.radius * (1 + elevation);
        heightMinMax.AddValue(elevation);
        return pointOnSphere * elevation;
    }
}
