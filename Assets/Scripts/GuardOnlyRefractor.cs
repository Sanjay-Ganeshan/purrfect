using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardOnlyRefractor : DefaultRefractor {

	public LightObjectType GetLightObjectType()
	{
		return LightObjectType.GUARD_ONLY_REFRACTOR;
	}
}
