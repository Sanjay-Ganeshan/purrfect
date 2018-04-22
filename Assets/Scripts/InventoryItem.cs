using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryItem : MonoBehaviour {

    public int ID;
    public ItemType ItType;
    public string Name;
    public string Description;
    private bool isEquipped;

    public Collider2D ZoC;
    public SpriteRenderer InventoryRenderer;
    public SpriteRenderer WorldRenderer;

    private Inventory owner;
    public Sprite Icon;

    protected abstract void OnEquip();
    protected abstract void OnUnequip();
    public abstract void Using(Vector2 location);
    public abstract void BeginUse(Vector2 location);
    public abstract void EndUse(Vector2 location);

    public void Equip()
    {
        this.isEquipped = true;
        RenderInventory();
        OnEquip();
    }

    public void Unequip()
    {
        this.isEquipped = false;
        RenderNone();
        OnUnequip();
    }

    public void ToggleEquip()
    {
        if(this.isEquipped)
        {
            Unequip();
        }
        else
        {
            Equip();
        }
    }

    private void RenderWorld()
    {
        if (this.InventoryRenderer != null)
        {
            this.InventoryRenderer.enabled = false;
        }
        if (this.WorldRenderer != null)
        {
            this.WorldRenderer.enabled = true;
        }
    }

    private void RenderInventory()
    {
        if (this.InventoryRenderer != null)
        {
            this.InventoryRenderer.enabled = true;
        }
        if (this.WorldRenderer != null)
        {
            this.WorldRenderer.enabled = false;
        }
    }
    
    private void RenderNone()
    {
        if (this.InventoryRenderer != null)
        {
            this.InventoryRenderer.enabled = false;
        }
        if (this.WorldRenderer != null)
        {
            this.WorldRenderer.enabled = false;
        }
    }

    public virtual void OnDrop()
    {
        if (this.ZoC != null) {
            this.ZoC.enabled = true;
        }
        RenderWorld();
    }

    public virtual void OnPickup()
    {
        if (this.ZoC != null)
        {
            this.ZoC.enabled = false;
        }
        RenderNone();
    }


    public void SetOwner(Inventory inventory)
    {
        this.owner = inventory;
    }

    public Inventory GetOwner()
    {
        return this.owner;
    }

    public override bool Equals(object other)
    {
        return this.ToString().Equals(other.ToString());
    }

    public override int GetHashCode()
    {
        return this.ID;
    }

    public override string ToString()
    {
        return string.Format("{0}: {1}", ItType.ToString(), ID);
    }

    public void Delete()
    {
        God.Kill(this.gameObject);
    }

    public Sprite GetSpriteOrDefault(Sprite IfNotFound)
    {
        if(Icon != null)
        {
            return Icon;
        }
        else
        {
            return IfNotFound;
        }
    }
}
