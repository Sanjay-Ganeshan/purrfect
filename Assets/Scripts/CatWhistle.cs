using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CatWhistle : InventoryItem, IInteractable
{
    public bool IsOn;

    public AudioSource whistleSound;

    public void Start()
    {
        IsOn = false;
        whistleSound = GetComponent<AudioSource>();
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
        whistleSound.Play();

        God.GetStats().incrementStat("whistle_uses", 1);
        God.GetStats().SendData();
    }

    public override void EndUse(Vector2 location)
    {
        this.IsOn = false;
        whistleSound.Stop();
    }

    public override void Using(Vector2 location)
    {
        Optional<Cat> theCat = God.GetCat(true);
		if(theCat.IsPresent() && theCat.Get() != null)
        {
            theCat.Get().AddPossibleTarget(this.transform.position, GameConstants.WHISTLE_PRIORITY);
        }
    }

    protected override void OnEquip()
    {
        
    }

    protected override void OnUnequip()
    {
        whistleSound.Stop();
    }

    public bool Interact(Player p)
    {
        p.Bag.Add(this);
        return this;
    }

    public bool Interact(Cat c)
    {
        return false;
    }

    public override PersistanceType GetPType()
    {
        return PersistanceType.CAT_WHISTLE;
    }
}
