using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DefaultAbsorber : LightObject
{
    public override List<Vector2> OnLightHit(LightType type, float intensity, Vector2 origin, Vector2 destination, Vector2 normal)
    {
        List<Vector2> hits = new List<Vector2>();
        hits.Add(destination);
        return hits;
    }

    protected override LightObjectType GetLightObjectType()
    {
        return LightObjectType.ABSORBER;
    }
}
