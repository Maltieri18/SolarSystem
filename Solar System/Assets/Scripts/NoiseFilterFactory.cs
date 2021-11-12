using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseFilterFactory
{
    public static INoiseFilter CreateNoiseFilter(NoiseSetting settings)
    {
        switch(settings.filterType)
        {
            case NoiseSetting.FilterType.Simple:
                return new SimpleNoiseFilter(settings.simpleNoiseSettings);

            case NoiseSetting.FilterType.Ridgid:
                return new RidgidNoiseFilter(settings.ridgidNoiseSettings);
        }
        return null;
    }
}
