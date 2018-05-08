using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChanger : MonoBehaviour {

	public DirectedSpriteEntry[] Sprites;
	private Dictionary<Vector2, Sprite> _Sprites;
	private Rigidbody2D rb;

	void Awake() {
		rb = GetComponent<Rigidbody2D>();
	}

	// Use this for initialization
	void Start () {
		_Sprites = new Dictionary<Vector2, Sprite>();
		foreach(DirectedSpriteEntry temp in Sprites)
		{
			_Sprites.Add(new Vector2(temp.x, temp.y), temp.Sprite);
		}
	}
	
	// Update is called once per frame
	void Update () {
		SetDirectedSprite ();
	}

	private void SetDirectedSprite() 
	{
		if (rb.velocity != Vector2.zero) 
		{
			float minAngle = 180;
			Sprite directedSprite = this.GetComponentInChildren<SpriteRenderer> ().sprite;
			foreach(Vector2 dir in _Sprites.Keys) {
				// pick sprite corresponding to direction closest to current velocity
				float angle = Math.Min (Math.Abs (Vector2.Angle (rb.velocity, dir)), Math.Abs (Vector2.Angle (dir, rb.velocity)));
				if (angle < minAngle) {
					minAngle = angle;
					directedSprite = _Sprites [dir];
				}
			}
			this.GetComponentInChildren<SpriteRenderer> ().sprite = directedSprite;
		}
	}
}
