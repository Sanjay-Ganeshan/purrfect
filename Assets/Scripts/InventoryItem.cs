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
        OnEquip();
    }

    public void Unequip()
    {
        this.isEquipped = false;
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
