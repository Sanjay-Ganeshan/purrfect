using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Key : InventoryItem, IInteractable
{
    public int NumberOfUses = 1;
    public int Combo;

    public bool CanUnlock(int doorCombo)
    {
       return this.Combo == doorCombo;
    }

    public bool Consume() {
        NumberOfUses--;

        return NumberOfUses <= 0;
    }

    public override void BeginUse(Vector2 location)
    {
        throw new NotImplementedException();
    }

    public override void EndUse(Vector2 location)
    {
        throw new NotImplementedException();
    }

    public bool Interact(Player p)
    {
        p.Bag.Add(this);
        return true;
    }

    public bool Interact(Cat c)
    {
        c.catInventory.Add(this);
        return true;
    }

    public override void Using(Vector2 location)
    {
        throw new NotImplementedException();
    }

    protected override void OnEquip()
    {
        throw new NotImplementedException();
    }

    protected override void OnUnequip()
    {
        throw new NotImplementedException();
    }
}
