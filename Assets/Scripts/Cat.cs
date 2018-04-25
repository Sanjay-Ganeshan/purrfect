using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cat : MonoBehaviour, IPersistantObject {
    // Controls the cat's behavior
    
    Optional<Vector3> currentTarget;      // Cat moves towards current target
    Optional<int> currentTargetPriority;

    // Constants should be PascalCase
    public float DISTANCE_THAT_COUNTS_AS_SAME_TARGET = 0.0f;
    public float DISTANCE_AT_TARGET = 0.1f;
    public string id;
    public float catSpeed = 10.0f;
    private Rigidbody2D rb;

    void Awake() {

        rb = GetComponent<Rigidbody2D>();
    }
    
	void Start() {

        ClearTarget();
	}
	
	void Update() {

        // Since last frame, targets have been added

        // If at current target, clear it
        ClearTargetIfArrived();

        // Set velocity based on current target
        SetVelocity();

        // Clear targets for next frame addition
        ClearTarget();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ClearTarget();
    }

    // Set velocity toward target, 0 if none
    private void SetVelocity()
    {
        if(currentTarget.IsPresent())
        {
            rb.velocity = catSpeed * VectorToTarget().normalized;
        } else {
            rb.velocity = Vector2.zero;
        }
    }

    // Call clear target if cat is at target position
    private void ClearTargetIfArrived()
    {
        if(AtTarget()) ClearTarget();
    }

    // Returns cat within distance of target
    private bool AtTarget()
    {
        Vector2 diff = VectorToTarget();
        return diff.magnitude <= DISTANCE_AT_TARGET;
    }

    // Get vector 2 from position to target
    private Vector2 VectorToTarget()
    {
        if(currentTarget.IsPresent())
        {
            return (currentTarget.Get() - this.transform.position).ToVector2();
        }
        else
        {
            return Vector2.zero;
        }
    }

    // Empty current target
    private void ClearTarget()
    {
        this.currentTarget = Optional<Vector3>.Empty();
        this.currentTargetPriority = Optional<int>.Empty();
    }

    // Try to add new target position, with priority priority (lower = importance)
    //      Checks sight, and if already reached
    public void AddPossibleTarget(Vector3 position, int priority)
    {
        int currentPriority = currentTargetPriority.IsPresent() ? currentTargetPriority.Get() : int.MaxValue;

        if(priority <= currentPriority)
        {
            // If it's more important, or it's equally important, but new, we'll update the target

            // Check reasons to not update target
            bool shouldUpdate = true;
            
            // Basic sight - the cat can see 360 degrees. Will replace with vision cones later

            if (Mathf.Approximately(DISTANCE_AT_TARGET, (position - this.transform.position).ToVector2().magnitude)
                || (position - this.transform.position).ToVector2().magnitude <= DISTANCE_AT_TARGET)
            {
                // Already at this target.
                shouldUpdate = false;
                ClearTarget();
            }

            if (shouldUpdate)
            {
                this.currentTarget = Optional<Vector3>.Of(position);
                this.currentTargetPriority = Optional<int>.Of(priority);
            }
        }
    }

    void IPersistantObject.Load(Dictionary<string, string> saveData)
    {

    }

    void IPersistantObject.PostLoad()
    {

    }

    Dictionary<string, string> IPersistantObject.Save()
    {
        return new Dictionary<string, string>();
    }

    void IPersistantObject.Unload()
    {

    }

    PersistanceType IPersistantObject.GetPType()
    {
        return PersistanceType.CAT;
    }

    bool IPersistantObject.PersistThroughLoad()
    {
        return true;
    }

    MonoBehaviour IPersistantObject.GetMono()
    {
        return this;
    }

    public virtual void LateUpdate()
    {
        this.GenerateIDIfNeeded();
    }

    string IIdentifiable.getID()
    {
        return this.id;
    }

    void IIdentifiable.setID(string id)
    {
        this.id = id;
    }
}
