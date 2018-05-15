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
		if (this.Combo != doorCombo && !HintsList.WRONG_KEY_SAID) {
			God.ShowText (HintsList.WRONG_KEY);
			HintsList.WRONG_KEY_SAID = true;
		}
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
		if (God.GetCurrentLevel () == GameConstants.BRING_CAT_HINT_LEVEL) {
			God.ShowText (HintsList.CAT_GOT_KEY);
		}
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

    public override PersistanceType GetPType()
    {
        return PersistanceType.KEY;
    }
    public override void Load(Dictionary<string, string> saveData)
    {
        if (saveData.ContainsKey("Uses")) {
            this.NumberOfUses = int.Parse(saveData["Uses"]);
        } else {
            this.NumberOfUses = 1;
        }

        if (saveData.ContainsKey("Combo")) {
            // We have Combo Data in the level
            this.Combo = int.Parse(saveData["Combo"]);
        } else {
            // Oh no, default to 0 combo
            this.Combo = 0;
        }
    }
    public override Dictionary<string, string> Save()
    {
        Dictionary<string, string> o =  new Dictionary<string, string>();
        o.Add("Combo", this.Combo.ToString());
        o.Add("Uses",  this.NumberOfUses.ToString());
        return o;
    }
}
