using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour, IPersistantObject {

    public const int InteractablesToCheck = 6;

    public Inventory Bag;
    public Collider2D ZoC;

    public InventoryItem[] toAddAtStart;

    public InventoryItem currentlyEquipped;

    private Rigidbody2D rb;
    public Vector2 maxSpeed = new Vector2(1,1);

    private Collider2D[] overlappingColliders = new Collider2D[InteractablesToCheck];
    private Optional<ContactFilter2D> ZoCFilter = Optional<ContactFilter2D>.Empty();
    

    // Use this for initialization
	void Start () {
        Bag = new Inventory(this.transform);
        this.currentlyEquipped = null;
        foreach(InventoryItem item in toAddAtStart)
        {
            Bag.AddToInventory(item);
        }
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        HandleShowInventory();
        if (true || !God.IsPaused())
        {
            HandleUsing();
            HandleMotion();
        }
        HandleInteraction();
	}

    void HandleMotion()
    {
        float horizontal = Input.GetAxis(GameConstants.AXIS_MOVE_HORIZONTAL);
        float vertical = Input.GetAxis(GameConstants.AXIS_MOVE_VERTICAL);
        rb.velocity = new Vector2(horizontal * maxSpeed.x, vertical * maxSpeed.y);
    }

    void HandleShowInventory()
    {
        if(Input.GetButtonDown(GameConstants.BTN_DISPLAY_CHARACTER_INVENTORY))
        {
            UIManager.ToggleInventory(this.Bag, OnInventorySelect);
        }
    }

    void OnInventorySelect(InventoryItem item)
    {
        if (item.isEquippable)
        {
            bool equippedItem = (this.currentlyEquipped == item);

            if (this.currentlyEquipped != null)
            {
                this.currentlyEquipped.Unequip();
                this.currentlyEquipped = null;
            }

            if (!equippedItem)
            {
                this.currentlyEquipped = item;
                this.currentlyEquipped.Equip();
            }

            UIManager.HideInventory();
        }
    }

    void HandleUsing()
    {
        if(Input.GetButtonDown(GameConstants.BTN_USE_CURRENTLY_EQUIPPED_ITEM))
        {
            if(this.currentlyEquipped != null)
            {
                this.currentlyEquipped.BeginUse(Common.GetMousePosition());
            }
        }
        else if (Input.GetButton(GameConstants.BTN_USE_CURRENTLY_EQUIPPED_ITEM))
        {
            if(this.currentlyEquipped != null)
            {
                this.currentlyEquipped.Using(Common.GetMousePosition());
            }
        }
        else if (Input.GetButtonUp(GameConstants.BTN_USE_CURRENTLY_EQUIPPED_ITEM))
        {
            if (this.currentlyEquipped != null)
            {
                this.currentlyEquipped.EndUse(Common.GetMousePosition());
            }
        }
    }

    void HandleInteraction()
    {
        if (Input.GetButtonDown(GameConstants.BTN_INTERACT))
        {
            InteractWithSurroundings();
        }
    }

    void InteractWithSurroundings()
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
                    Debug.Log("ZoC does not have parent!");
                    continue;
                }
                if(collidedInteracter != null)
                {
                    bool worked = collidedInteracter.Interact(this);
                    Debug.Log("Interacting with" + collidedInteracter);
                    if(worked) break;
                }
                else
                {
                    Debug.Log("Collided with non interactable " + collidedInteracter);
                }
            }
        }
    }

    void MakeContactFilter()
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

    Dictionary<string, string> IPersistantObject.Save()
    {
        Dictionary<string, string> ret = new Dictionary<string, string>();
        return ret;
    }

    int IPersistantObject.getID()
    {
        return GameConstants.PLAYER_ID;
    }

    void IPersistantObject.Unload()
    {
        God.Kill(this.gameObject);
    }

    PersistanceType IPersistantObject.GetPType()
    {
        return PersistanceType.PLAYER;
    }

    void IPersistantObject.setID(int id)
    {
        
    }

    MonoBehaviour IPersistantObject.GetMono()
    {
        return this;
    }
}
