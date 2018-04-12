using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LightObject : MonoBehaviour {
    public const string LIGHT_LAYER = "Lights";
    private int LIGHT_LAYER_MASK;
    public const float DISTANCE_PER_INTENSITY = 1.0f;
    /// <summary>
    /// Default intensity to give a "never ending" light emitter - 
    /// stops infinite loops.
    /// </summary>
    public const float DEFAULT_INTENSITY = 1000f;

    protected abstract LightObjectType GetLightObjectType();

    public abstract List<Vector2> OnLightHit(LightType type, float intensity, Vector2 origin, Vector2 destination, Vector2 normal);


    public List<Vector2> EmitLight(LightType type, float intensity, Vector2 origin, Vector2 direction, Collider2D ignoreCollider = null)
    {

        float dist = intensity * DISTANCE_PER_INTENSITY;
        List<Vector2> points = new List<Vector2>();
        points.Add(origin);
        if(ignoreCollider != null)
        {
            ignoreCollider.enabled = false;
        }
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, intensity, LIGHT_LAYER_MASK);
        if(ignoreCollider != null)
        {
            ignoreCollider.enabled = true;
        }
        Vector2 hitPoint;
        if (hit.collider != null)
        {
            hitPoint = hit.point;
            LightObject hitObject = hit.collider.gameObject.GetComponent<LightObject>();
            float hitDistance = hit.distance;
            Vector2 hitNormal = hitObject.transform.InverseTransformDirection(hit.normal);
            if(hitObject != null)
            {
                List<Vector2> nextHits = hitObject.OnLightHit(type, intensity - (hitDistance * DISTANCE_PER_INTENSITY), origin, hitPoint, hitNormal);
                points.AddRange(nextHits);
            }
        }
        else
        {
            //Debug.Log("No hit");
            // No hit
            hitPoint = origin + direction * intensity;
            points.Add(hitPoint);
        }
        return points;
    }

    public List<Vector2> EmitLightTowards(LightType type, float intensity, Vector2 origin, Vector2 destination, Collider2D ignoreCollider = null)
    {
        Vector2 direction = (destination - origin).normalized;
        return EmitLight(type, intensity, origin, direction, ignoreCollider);
    }

    public List<Vector2> EmitLight(LightType type, float intensity, Vector2 direction)
    {
        return EmitLight(type, intensity, this.transform.position.ToVector2(), direction);
    }
    public List<Vector2> EmitLightTowards(LightType type, float intensity, Vector2 destination)
    {
        return EmitLightTowards(type, intensity, this.transform.position.ToVector2(), destination);
    }

    protected void Awake()
    {
        LIGHT_LAYER_MASK = LayerMask.GetMask(LIGHT_LAYER);
    }

}
