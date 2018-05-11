using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter2D(Collider2D player)
	{
		Debug.Log ("collide");
		Debug.Log (player.gameObject.tag);
		if ((transform.position - God.GetPlayer().transform.position).sqrMagnitude < 3) {
			God.ShowText (HintsList.CANT_GO_BACK);
		}
	}
}
