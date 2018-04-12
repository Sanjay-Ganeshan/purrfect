using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cat : MonoBehaviour {

    
    float lastUpdateTime;
    Optional<Vector3> target;
    Optional<int> currentTargetPriority;
    private bool shouldRepath = true;
    public float DISTANCE_THAT_COUNTS_AS_SAME_TARGET = 0.0f;
    public float DISTANCE_AT_TARGET = 0.1f;
    private Rigidbody2D rb;
    public float catSpeed = 10.0f;
    
	// Use this for initialization
	void Start () {
        updateTime();
        shouldRepath = true;
        rb = GetComponent<Rigidbody2D>();
        ClearTarget();
	}
	
	// Update is called once per frame
	void Update () {
        ClearTargetIfArrived();
	}


    void LateUpdate()
    {
        if(hasUpdatedThisFrame())
        {
            if(shouldRepath)
            {
                DoPath();
            }
        }
        else
        {
            ClearTarget();
        }
    }

    void ClearTargetIfArrived()
    {
        if (AtTarget()) ClearTarget();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ClearTarget();
    }

    Optional<Vector2> vectorToTarget()
    {
        if(target.IsPresent())
        {
            return Optional<Vector2>.Of((target.Get() - this.transform.position).ToVector2());
        }
        else
        {
            return Optional<Vector2>.Empty();
        }
    }

    void DoPath()
    {
        if(target.IsPresent())
        {
            rb.velocity = catSpeed * (target.Get() - this.transform.position).ToVector2().normalized;
        }
    }

    void ClearTarget()
    {
        this.target = Optional<Vector3>.Empty();
        this.currentTargetPriority = Optional<int>.Empty();
        rb.velocity = Vector2.zero;
    }

    bool AtTarget()
    {
        Optional<Vector2> diff = vectorToTarget();
        if (diff.IsPresent())
        {
            if (Mathf.Approximately(DISTANCE_AT_TARGET, diff.Get().magnitude))
            {
                return true;
            }
        }
        return false;
    }

    public void AddPossibleTarget(Vector3 position, int priority)
    {
        int currPr = currentTargetPriority.IsPresent() ? currentTargetPriority.Get() : int.MaxValue;
        if(priority <= currPr)
        {
            // If it's more important, or it's equally important, but new, we'll update the target
            bool shouldUpdate = false;
            if(this.target.IsPresent())
            {
                float dist = (this.target.Get() - position).magnitude;
                if (Mathf.Approximately(dist, DISTANCE_THAT_COUNTS_AS_SAME_TARGET) || dist <= DISTANCE_THAT_COUNTS_AS_SAME_TARGET)
                {
                    // The new target is basically the old target, do nothing
                }
                else
                {
                    // Basic sight - the cat can see 360 degrees. Will replace with vision cones later
                    shouldUpdate = true;
                }
            }
            else
            {
                shouldUpdate = true;
            }
            if (Mathf.Approximately(DISTANCE_AT_TARGET, (position - this.transform.position).ToVector2().magnitude) || (position - this.transform.position).ToVector2().magnitude <= DISTANCE_AT_TARGET)
            {
                // Already at this target.
                shouldUpdate = false;
                ClearTarget();
            }
            if (shouldUpdate)
            {
                this.target = Optional<Vector3>.Of(position);
                this.currentTargetPriority = Optional<int>.Of(priority);
                this.shouldRepath = true;
            }
        }
        updateTime();
    }

    private void updateTime()
    {
        lastUpdateTime = Time.time;
    }
    private bool hasUpdatedThisFrame()
    {
        return Time.time == lastUpdateTime;
    }
}
