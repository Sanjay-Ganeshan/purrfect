using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour {

    public const int InteractablesToCheck = 6;

    public Inventory PlayerInventory;
    public Collider2D ZoC;

    public InventoryItem[] toAddAtStart;

    public InventoryItem currentlyEquipped;

    private Rigidbody2D rb;
    public Vector2 maxSpeed = new Vector2(1,1);

    // Use this for initialization
	void Start () {
        PlayerInventory = new Inventory(this.transform);
        this.currentlyEquipped = null;
        foreach(InventoryItem item in toAddAtStart)
        {
            PlayerInventory.AddToInventory(item);
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
            UIManager.ToggleInventory(this.PlayerInventory, OnInventorySelect);
        }
    }

    void OnInventorySelect(InventoryItem item)
    {   
        bool equippedItem = (this.currentlyEquipped == item);

        if(this.currentlyEquipped != null)
        {
            this.currentlyEquipped.Unequip();
            this.currentlyEquipped = null;
        }
        
        if(!equippedItem)
        {
            this.currentlyEquipped = item;
            this.currentlyEquipped.Equip();
        }

        UIManager.HideInventory();
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
        Collider2D[] results = new Collider2D[InteractablesToCheck];
        ZoC.OverlapCollider(new ContactFilter2D().NoFilter(), results);
        IEnumerable<int> en;

        foreach(Collider2D res in results)
        {
            if (res != null)
            {

            }
        }
    }

}
