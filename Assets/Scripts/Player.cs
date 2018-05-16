using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour, IPersistantObject, IInteractable {

    public const int InteractablesToCheck = 6;

    public Inventory Bag;
    public Collider2D ZoC;

    public InventoryItem[] toAddAtStart;

    public InventoryItem currentlyEquipped;

    private Rigidbody2D rb;
    public Vector2 maxSpeed = new Vector2(1,1);

    // private Collider2D[] overlappingColliders = new Collider2D[InteractablesToCheck];
    private Optional<ContactFilter2D> ZoCFilter = Optional<ContactFilter2D>.Empty();
    private bool initialized;

    private string inventoryItemsToAdd;

    public GameObject equipRenderer;

    // Use this for initialization
	void Start () {
        DoInitIfNeeded();   
	}
	
    private void DoInitIfNeeded()
    {
        if(!initialized)
        {
            Bag = new Inventory(this.transform);
            this.currentlyEquipped = null;
            foreach (InventoryItem item in toAddAtStart)
            {
                Bag.AddToInventory(item);
            }
            rb = GetComponent<Rigidbody2D>();
            initialized = true;
        }
    }

	// Update is called once per frame
	void Update () {
        DoInitIfNeeded();
        HandleMiscInputs();
        HandleShowInventory();
        if (!God.IsPaused())
        {
            HandleUsing();
            HandleMotion();
        }
        HandleInteraction();

		if (!God.GetSavior ().SaveMode) {
			God.GetStats ().incrementStat ("player_movement", rb.velocity.magnitude * Time.deltaTime);
		}
	}

    void HandleMiscInputs() {

        if(Input.GetButtonDown(GameConstants.BTN_RESET))
            God.GetSavior().ReloadCurrentLevel();

        equipRenderer.SetActive(this.currentlyEquipped != null);
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
        DoInitIfNeeded();
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
		InteractWithSurroundings();
//        if (Input.GetButtonDown(GameConstants.BTN_INTERACT))
//        {
//            InteractWithSurroundings();
//        }
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
                catch(NullReferenceException)
                {
                    continue;
                }
                if(collidedInteracter != null)
                {
                    bool worked = collidedInteracter.Interact(this);
                    if(worked) break;
                }
                else
                {
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

    public virtual void LateUpdate()
    {
        this.GenerateIDIfNeeded();
    }

    void IPersistantObject.Load(Dictionary<string, string> saveData)
    {
        if (saveData.ContainsKey("Inventory"))
        {
            this.inventoryItemsToAdd = saveData["Inventory"];
            Debug.Log("Inventory: " + this.inventoryItemsToAdd);
        }
        else
        {
            Debug.Log("Using compatibility mode to parse player...no inventory found.");
        }

        DoInitIfNeeded();
    }

    Dictionary<string, string> IPersistantObject.Save()
    {
        Dictionary<string, string> ret = new Dictionary<string, string>();
        string carriedItems;
        if (this.Bag.Count > 0) { 
            carriedItems = this.Bag.Select((item) => item.getID()).Aggregate((string a, string b) => a + "," + b);
        }
        else {
            carriedItems = "";
        }
        ret["Inventory"] = carriedItems;
        return ret;
    }

    public string getID()
    {
        return GameConstants.PLAYER_ID;
    }

    public bool Interact(Player p)
    {
        return false;
    }

    public bool Interact(Cat c)
    {   
        // Take items from cat
        List<InventoryItem> toTake = new List<InventoryItem>();
        foreach(InventoryItem item in c.catInventory)
        {
            toTake.Add(item);
        }
        foreach(InventoryItem item in toTake)
        {
            c.catInventory.Drop(item);
            Bag.Add(item);
        }
        return true;
    }

    void IPersistantObject.Unload()
    {
        God.Kill(this.gameObject);
    }

    PersistanceType IPersistantObject.GetPType()
    {
        return PersistanceType.PLAYER;
    }

    public void setID(string id)
    {
        
    }

    MonoBehaviour IPersistantObject.GetMono()
    {
        return this;
    }

    IEnumerable<String> IPersistantObject.PersistThroughLoad()
    {
        return new string[] { "Inventory" };
    }

    public void PostLoad()
    {
        DoInitIfNeeded();
        string[] idsToAdd = this.inventoryItemsToAdd.Split(',');
        foreach (string id in idsToAdd)
        {
            Optional<IIdentifiable> found = God.GetByID(id);
            if(found.IsPresent())
            {
                Optional<InventoryItem> toAdd = Optional<InventoryItem>.Empty();
                try
                {
                    toAdd = Optional<InventoryItem>.Of((InventoryItem) found.Get());
                }
                catch(InvalidCastException)
                {
                    Debug.Log("Failed to cast to inventory item: " + found.Get());
                }
                if(toAdd.IsPresent())
                {
                    this.Bag.Add(toAdd.Get());
                }
            }
            else
            {
            }
        }
    }

    public void PreSave()
    {
        //Debug.Log("Called player pre save");
        InventoryItem[] itemsToSet = Bag.ToArray();
        for(int i = 0; i < itemsToSet.Length; i++)
        {
            //i.SetCarryingToNextScene(true);
            itemsToSet[i].CarryToNextScene = true;
        }
    }
}
