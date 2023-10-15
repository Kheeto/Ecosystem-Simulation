using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TerrainData : UpdatableData {

    [Space(10)]
    public float scale;
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    [Space(10)]
    public bool useFlatShading;
    public bool useFalloffMap;
}
