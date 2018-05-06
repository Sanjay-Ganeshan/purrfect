using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardOnlyRefractor : MonoBehaviour, ILightObject {
	//doesn't actually refract anything
	public LightObjectType GetLightObjectType()
	{
		return LightObjectType.GUARD_ONLY_REFRACTOR;
	}

	public MonoBehaviour GetMono()
	{
		return this;
	}

	public List<Vector2> OnLightHit(LightType type, float intensity, Vector2 origin, Vector2 destination, Vector2 direction, Collider2D collider, Vector2 normal, float refractiveIndex)
	{
		List<Vector2> laterPoints = new List<Vector2>();
		//laterPoints.Add(destination);
		laterPoints.AddRange(LightSim.EmitLight(type, Mathf.Max(intensity, 0), destination, direction, refractiveIndex));

		//laterPoints.Add(destination);
		//laterPoints.Add(nextDestination);
		return laterPoints;
	}
}
