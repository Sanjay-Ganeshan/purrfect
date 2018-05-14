using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour {

	public const float DISTANCE_AT_TARGET = 0.2f;
	public float guardSpeed;

	private Vector2 basePosition;
	private Vector2 baseDirection;
	private Vector2 currentDirection;
	Optional<Vector2> currentTarget;
	private bool moving = false;
	private Rigidbody2D rb;

	void Awake() {
		rb = GetComponent<Rigidbody2D>();
	}

	// Use this for initialization
	void Start () {
		basePosition = this.transform.position;
		Vector3 baseEulerAngles = this.transform.eulerAngles;
		baseDirection = new Vector2 (Mathf.Cos (baseEulerAngles.z*(float)Math.PI/180), Mathf.Sin (baseEulerAngles.z*(float)Math.PI/180));
		Debug.Log (baseDirection);
		currentDirection = baseDirection;
		currentTarget = Optional<Vector2>.Empty();
	}

	// Update is called once per frame
	void Update () {
		List<Vector2> emissionResults = this.transform.EmitLight (LightType.GUARD_VISION, GameConstants.GUARD_SIGHT_RANGE, currentDirection);
		Bounds playerBounds = God.GetPlayer ().GetComponentInChildren<BoxCollider2D> ().bounds;
		if (SeeObjectInBounds (playerBounds, emissionResults).IsPresent ()) {
			God.ShowText (HintsList.GUARD_SEES_PLAYER);
			God.GetSavior ().ReloadCurrentLevel ();
		}
		if (!moving) {
			Optional<Cat> cat = God.GetCat (true);
			if (cat.Get () != null) {
				Bounds catBounds = God.GetCat ().Get ().GetComponentInChildren<BoxCollider2D> ().bounds;
				Optional<Vector2> catLoc = SeeObjectInBounds (catBounds, emissionResults);
				if (catLoc.IsPresent ()) {
					currentTarget = catLoc;
					moving = true;
//						Debug.Log ("guard sees cat");
				}
			}
		} else {
			rb.velocity = guardSpeed * currentDirection;
			if (AtTarget ()) {
				if (currentTarget.Get () != basePosition) 
				{
					currentTarget = Optional<Vector2>.Of (basePosition);
					currentDirection = -baseDirection;
				} 
				else 
				{
					moving = false;
					rb.velocity = Vector2.zero;
					currentDirection = baseDirection;
					gameObject.transform.position = basePosition;
				}
			}
		}
		rb.rotation = 0;


//		if (!moving) 
//		{
//			List<Vector2> emissionResults = this.transform.EmitLight (LightType.KITTY_VISION, GameConstants.GUARD_SIGHT_RANGE, baseDirection);
//		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		currentTarget = Optional<Vector2>.Of (basePosition);
		currentDirection = (basePosition - (Vector2)this.transform.position).normalized;
	}

	private Optional<Vector2> SeeObjectInBounds(Bounds bounds, List<Vector2> emissionResults) {
		foreach (Vector2 vec in emissionResults) 
		{
			if (bounds.SqrDistance (vec) < DISTANCE_AT_TARGET) {
				return Optional<Vector2>.Of(vec);
			}
		}
		return Optional<Vector2>.Empty();
	}

	// Set velocity toward target, 0 if none
	private void SetVelocity()
	{
		if(currentTarget.IsPresent())
		{
			// do this check to avoid letting the cat see infinitely far through windows
			if (VectorToTarget ().magnitude < GameConstants.GUARD_SIGHT_RANGE)
			{
				rb.velocity = guardSpeed * (VectorToTarget().normalized);
			}
		} else {
			rb.velocity = Vector2.zero;
		}
	}

	// Get vector 2 from position to target
	private Vector2 VectorToTarget()
	{
		if(currentTarget.IsPresent())
		{
			return currentTarget.Get() - (this.transform.position).ToVector2();
		}
		else
		{
			return Vector2.zero;
		}
	}

	private bool AtTarget()
	{
		Vector2 diff = VectorToTarget();
		return diff.magnitude <= DISTANCE_AT_TARGET;
	}
}
