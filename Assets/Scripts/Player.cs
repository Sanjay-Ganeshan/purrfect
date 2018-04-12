using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Inventory PlayerInventory;

    public InventoryItem[] toAddAtStart;

    public InventoryItem CurrentlyEquipped;

    private Rigidbody2D rb;
    public Vector2 maxSpeed = new Vector2(1,1);

    // Use this for initialization
	void Start () {
        PlayerInventory = new Inventory(this.transform);
        this.CurrentlyEquipped = null;
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
        if(Input.GetButtonDown(GameConstants.DISPLAY_CHARACTER_INVENTORY))
        {
            UIManager.ToggleInventory(this.PlayerInventory, OnInventorySelect);
        }
    }

    void OnInventorySelect(InventoryItem item)
    {
        if (this.CurrentlyEquipped != null)
        {
            this.CurrentlyEquipped.Unequip();
            this.CurrentlyEquipped = null;
        }
        else
        {
            this.CurrentlyEquipped = item;
            this.CurrentlyEquipped.Equip();
        }
        UIManager.HideInventory();
    }

    void HandleUsing()
    {
        if(Input.GetButtonDown(GameConstants.USE_CURRENTLY_EQUIPPED_ITEM))
        {
            if(this.CurrentlyEquipped != null)
            {
                this.CurrentlyEquipped.BeginUse(Common.GetMousePosition());
            }
        }
        else if (Input.GetButton(GameConstants.USE_CURRENTLY_EQUIPPED_ITEM))
        {
            if(this.CurrentlyEquipped != null)
            {
                this.CurrentlyEquipped.Using(Common.GetMousePosition());
            }
        }
        else if (Input.GetButtonUp(GameConstants.USE_CURRENTLY_EQUIPPED_ITEM))
        {
            if (this.CurrentlyEquipped != null)
            {
                this.CurrentlyEquipped.EndUse(Common.GetMousePosition());
            }
        }
    }


}
