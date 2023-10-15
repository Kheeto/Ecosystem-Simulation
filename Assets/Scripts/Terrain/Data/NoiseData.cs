using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class NoiseData : UpdatableData {
    
    [Space(10)]
    public int seed;
    public float scale;
    public Noise.NormalizeMode normalizeMode;

    [Space(10)]
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;
    public Vector2 offset;

    protected override void OnValidate()
    {
        if (scale < 0) scale = 0;
        if (octaves < 0) octaves = 0;
        if (lacunarity < 1) lacunarity = 1;

        base.OnValidate();
    }
}
