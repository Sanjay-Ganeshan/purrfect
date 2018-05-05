using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cat : MonoBehaviour, IPersistantObject {
    // Controls the cat's behavior
    
    // == Movement Behavior ==
    Optional<Vector3> currentTarget;      // Cat moves towards current target
    Optional<int> currentTargetPriority;

    // Constants should be PascalCase
    public const float DISTANCE_THAT_COUNTS_AS_SAME_TARGET = 0.01f;
    public const float DISTANCE_AT_TARGET = 0.1f;
    public string id;

    private const float MaxVisionRange = 100f;

    public float catSpeed = 10.0f;
    private Rigidbody2D rb;

    public LineRenderer debugVision;

    // == Inventory Behavior ==
    public const int InteractablesToCheck = 6;

    public Inventory catInventory;

    public Collider2D ZoC;
    private Optional<ContactFilter2D> ZoCFilter = Optional<ContactFilter2D>.Empty();

    void Awake() {

        rb = GetComponent<Rigidbody2D>();
    }
    
	void Start() {

        catInventory = new Inventory(this.transform);
        ClearTarget();
	}
	
	void Update() {

        // Updates are done in a single thread so
        //  don't use late update for this type of thing.

        // Since last frame, targets have been added

        // If at current target, clear it
        ClearTargetIfArrived();

        // Set velocity based on current target
        SetVelocity();

        // Clear targets for next frame addition
        ClearTarget();

        // Interact with surrounding items
        InteractWithSurroundings();
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
			// do this check to avoid letting the cat see infinitely far through windows
			if (VectorToTarget ().magnitude < GameConstants.CAT_SIGHT_RANGE)
			{
				rb.velocity = catSpeed * VectorToTarget ().normalized;
			}
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
            
            // Don't update if target cannot be seen
            // Basic sight - the cat can see 360 degrees. Will replace with vision cones later
            Vector2 direction = position - this.transform.position;
            float range = Mathf.Min(MaxVisionRange, direction.magnitude);

            List<Vector2> emissionResults = this.transform.EmitLightTowards(LightType.KITTY_VISION, range, position);
            Vector2 endpoint = emissionResults[emissionResults.Count - 1];

            Vector3[] pos = new Vector3[emissionResults.Count];
            for(int i = 0; i < emissionResults.Count; i++)
                pos[i] = emissionResults[i].ToVector3();
            debugVision.positionCount = pos.Length;
            debugVision.SetPositions(pos);

            float sightDelta = (endpoint - (Vector2)position).magnitude;
            if (sightDelta > DISTANCE_THAT_COUNTS_AS_SAME_TARGET)
            {
                shouldUpdate = false;
            }

            // Don't update if already at this target.
            if (Mathf.Approximately(DISTANCE_AT_TARGET, (position - this.transform.position).ToVector2().magnitude)
                || (position - this.transform.position).ToVector2().magnitude <= DISTANCE_AT_TARGET)
            {
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

    // Interact with surroundings
    //  Pick up items and give items to player
    private void InteractWithSurroundings()
    {
        MakeContactFilter();
        Collider2D[] results = new Collider2D[InteractablesToCheck];
        ZoC.OverlapCollider(ZoCFilter.Get(), results);
        foreach(Collider2D res in results)
        {
            if (res != null)
            {
                IInteractable collidedInteracter;
                try
                {
                    collidedInteracter = res.transform.parent.GetComponent<IInteractable>();
                }
                catch(NullReferenceException e)
                {
                    Debug.Log("ZoC does not have parent! e:"+e);
                    continue;
                }
                
                if(collidedInteracter != null)
                {
                    bool worked = collidedInteracter.Interact(this);
                    // Debug.Log("Cat interacting with" + collidedInteracter);
                    if(worked) break;
                }
                else
                {
                    // Debug.Log("Collided with non interactable " + collidedInteracter);
                }
            }
        }
    }

    private void MakeContactFilter()
    {
        if(!ZoCFilter.IsPresent())
        {
            ContactFilter2D filt;
            filt = new ContactFilter2D();
            filt.NoFilter();
            filt.SetLayerMask(LayerMask.GetMask(GameConstants.ZOC_LAYER));
            ZoCFilter = Optional<ContactFilter2D>.Of(filt);
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
		God.Kill(this.gameObject);
    }

    PersistanceType IPersistantObject.GetPType()
    {
        return PersistanceType.CAT;
    }

    IEnumerable<string> IPersistantObject.PersistThroughLoad()
    {
        return new string[] { };
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

    public void PreSave()
    {
        
    }
}
