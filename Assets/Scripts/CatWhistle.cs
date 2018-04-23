using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CatWhistle : InventoryItem, IInteractable
{
    public bool IsOn;

    public void Start()
    {
        IsOn = false;
    }

    public void Update()
    {
    }

    void ToggleOn ()
    {
        this.IsOn = !this.IsOn;
    }

    public override void BeginUse(Vector2 location)
    {
        this.IsOn = true;
    }

    public override void EndUse(Vector2 location)
    {
        this.IsOn = false;
    }

    public override void Using(Vector2 location)
    {
        Optional<Cat> theCat = God.GetCat();
        if(theCat.IsPresent())
        {
            theCat.Get().AddPossibleTarget(this.transform.position, GameConstants.WHISTLE_PRIORITY);
        }
    }

    protected override void OnEquip()
    {
        
    }

    protected override void OnUnequip()
    {
        
    }

    public bool Interact(Player p)
    {
        p.Bag.Add(this);
        return this;
    }
}
