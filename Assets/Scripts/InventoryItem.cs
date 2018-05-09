using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class InventoryItem : MonoBehaviour, IPersistantObject {

    public string ID = "";
    public ItemType ItType;
    public string Name;
    public string Description;
    private bool isEquipped;

    public bool isEquippable;

    public bool CarryToNextScene;

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

    public virtual void LateUpdate()
    {
        this.GenerateIDIfNeeded();
    }

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

    protected void RenderWorld()
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

    protected void RenderInventory()
    {
        if (this.InventoryRenderer != null)
        {
            this.InventoryRenderer.enabled = true;
            InventoryRenderer.transform.position = GameObject.Find("EquipSlot").transform.position;
        }
        if (this.WorldRenderer != null)
        {
            this.WorldRenderer.enabled = false;
        }
    }
    
    protected void RenderNone()
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
        if(this.owner != null)
        {
            this.owner.Drop(this);
        }
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
        return this.ID.GetHashCode();
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

    public string getID()
    {
        return this.ID;
    }

    public void setID(string id)
    {
        this.ID = id;
    }

    public void SetCarryingToNextScene(bool newVal)
    {
        this.CarryToNextScene = newVal;
    }

    public virtual void Load(Dictionary<string, string> saveData)
    {

    }
    public virtual void PostLoad()
    {

    }
    public virtual Dictionary<string, string> Save()
    {
        return new Dictionary<string, string>();
    }
    public virtual void Unload()
    {
        God.Kill(this.gameObject);
    }
    public abstract PersistanceType GetPType();
    public virtual IEnumerable<string> PersistThroughLoad()
    {
        if(CarryToNextScene)
        {
            string[] keys = this.Save().Keys.ToArray();
            if(keys.Length == 0) { return new string[] { "persists" }; }
            else { return keys; }
        }
        else
        {
            return new string[0];
        }
    }
    public MonoBehaviour GetMono()
    {
        return this;
    }

    public virtual void PreSave()
    {
        
    }
}
