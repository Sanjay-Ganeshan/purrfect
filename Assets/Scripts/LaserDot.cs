using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDot : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Optional<Cat> theCat = God.GetCat(true);
		if(theCat.IsPresent() && theCat.Get() != null)
        {
            theCat.Get().AddPossibleTarget(this.transform.position, GameConstants.LASER_PRIORITY);
        }
        else
        {
            //Debug.Log("No cat");
        }
	}
}
