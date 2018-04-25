using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LightSim {
    public const string LIGHT_LAYER = "Lights";
    private static Optional<int> LIGHT_LAYER_MASK = Optional<int>.Empty();
    public const float DISTANCE_PER_INTENSITY = 1.0f;
    /// <summary>
    /// Default intensity to give a "never ending" light emitter - 
    /// stops infinite loops.
    /// </summary>
    public const float DEFAULT_INTENSITY = 1000f;

    public static List<Vector2> EmitLight(LightType type, float intensity, Vector2 origin, Vector2 direction, float refractiveIndex = GameConstants.DEFAULT_REFRACTIVE_INDEX)
    {

        float dist = intensity * DISTANCE_PER_INTENSITY;
        List<Vector2> points = new List<Vector2>();
        points.Add(origin);
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, intensity, GetMask());
        Vector2 hitPoint;
        if (hit.collider != null)
        {
            hitPoint = hit.point;
            ILightObject hitObject = hit.collider.gameObject.GetComponent<ILightObject>();
            float hitDistance = hit.distance;
            //Vector2 hitNormal = hitObject.GetMono().transform.TransformDirection(hit.normal);
            Vector2 hitNormal = hit.normal;
            if(hitObject != null)
            {
                List<Vector2> nextHits = hitObject.OnLightHit(type, intensity - (hitDistance * DISTANCE_PER_INTENSITY), origin, hitPoint, hit.collider, hitNormal, refractiveIndex);
                points.AddRange(nextHits);
            }
        }
        else
        {
            // No hit
            hitPoint = origin + direction * intensity;
            points.Add(hitPoint);
        }
        return points;
    }

    public static List<Vector2> EmitLightTowards(LightType type, float intensity, Vector2 origin, Vector2 destination, float refractiveIndex = GameConstants.DEFAULT_REFRACTIVE_INDEX)
    {
        Vector2 direction = (destination - origin).normalized;
        return EmitLight(type, intensity, origin, direction, refractiveIndex);
    }

    public static List<Vector2> EmitLight(this Transform t, LightType type, float intensity, Vector2 direction)
    {
        return EmitLight(type, intensity, t.position.ToVector2(), direction);
    }
    public static List<Vector2> EmitLightTowards(this Transform t, LightType type, float intensity, Vector2 destination)
    {
        return EmitLightTowards(type, intensity, t.position.ToVector2(), destination);
    }

    private static void EnsureLayerMask()
    {
        if(!LIGHT_LAYER_MASK.IsPresent())
        {
            LIGHT_LAYER_MASK = Optional<int>.Of(LayerMask.GetMask(LIGHT_LAYER));
        }
    }

    private static int GetMask()
    {
        EnsureLayerMask();
        return LIGHT_LAYER_MASK.Get();
    }
}

public interface ILightObject
{
    LightObjectType GetLightObjectType();

    List<Vector2> OnLightHit(LightType type, float intensity, Vector2 origin, Vector2 destination, Collider2D collider, Vector2 normal, float refractiveIndex = GameConstants.DEFAULT_REFRACTIVE_INDEX);

    MonoBehaviour GetMono();
}
