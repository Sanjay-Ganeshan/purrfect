using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DefaultRefractor : MonoBehaviour, ILightObject
{
    public float refractiveIndex = 1.0f;

    public LightObjectType GetLightObjectType()
    {
        return LightObjectType.REFRACTOR;
    }

    public MonoBehaviour GetMono()
    {
        return this;
    }

	public List<Vector2> OnLightHit(LightType type, float intensity, Vector2 origin, Vector2 destination, Vector2 direction, Collider2D collider, Vector2 normal, float refractiveIndex = 1)
    {
        List<Vector2> laterPoints = new List<Vector2>();
        //laterPoints.Add(destination);
        Vector2 incomingDirection = destination - origin;
        // Amount of incoming vector in the direction of the normal
        Vector2 projection = Vector2.Dot(normal.normalized, incomingDirection) * normal.normalized;

        // Amount of incoming vector perpendicular to normal
        Vector2 perpendicular = incomingDirection - projection;

        float signedAngleOfIncidence = Vector2.SignedAngle(projection.normalized, incomingDirection.normalized) * Mathf.Deg2Rad;
        float angleOfIncidence = Mathf.Abs(signedAngleOfIncidence);

        float angleOfRefraction = Mathf.Asin(refractiveIndex / this.refractiveIndex * Mathf.Sin(angleOfIncidence));
        // n1 sin (t1) = n2 sin (t2)

        Vector2 nextDirection = projection + refractiveIndex / this.refractiveIndex * perpendicular;
        laterPoints.AddRange(LightSim.EmitLight(type, Mathf.Max(intensity, 0), destination, nextDirection, this.refractiveIndex));
        
        //laterPoints.Add(destination);
        //laterPoints.Add(nextDestination);
        return laterPoints;
    }
}
