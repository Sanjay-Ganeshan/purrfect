using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class DefaultReflector : LightObject
{
    public override List<Vector2> OnLightHit(LightType type, float intensity, Vector2 origin, Vector2 destination, Vector2 normal)
    {
        List<Vector2> laterPoints = new List<Vector2>();
        //laterPoints.Add(destination);
        Vector2 incomingDirection = destination - origin;
        Vector2 vectorToOrigin = origin - destination;
        Vector2 projection = Vector2.Dot(normal.normalized, incomingDirection) * normal.normalized;
        Vector2 nextDestination = destination + incomingDirection - (2 * projection);
        Collider2D myCollider = this.GetComponent<Collider2D>();
        //myCollider.enabled = false;
        laterPoints.AddRange(EmitLightTowards(type, Mathf.Max(intensity, 0), destination, nextDestination, myCollider));
        //myCollider.enabled = true;
        Vector3[] debugPoints = new Vector3[] {
            destination.ToVector3(),
            nextDestination.ToVector3()
        };
        //laterPoints.Add(destination);
        //laterPoints.Add(nextDestination);
        return laterPoints;
        
    }

    protected override LightObjectType GetLightObjectType()
    {
        return LightObjectType.REFLECTOR;
    }

}
