using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ToyBall : InventoryItem, IInteractable
{
    private Rigidbody2D rb;

    public bool IsOn;

    // public AudioSource ballSoumd;

    public void Start()
    {
        IsOn = false;
        // ballSoumd = GetComponent<AudioSource>();
        this.rb = transform.GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if(IsOn)
        {
            Optional<Cat> theCat = God.GetCat(true);
            if(theCat.IsPresent() && theCat.Get() != null)
            {
                theCat.Get().AddPossibleTarget(this.transform.position, GameConstants.BALL_PRIORITY);
            }
        }
    }

    void ToggleOn ()
    {
        this.IsOn = !this.IsOn;
    }

    public override void BeginUse(Vector2 location)
    {
        this.IsOn = true;
        // ballSoumd.Play();
        Vector2 direction = location - (Vector2)transform.position;

        this.rb.simulated = true;
        this.rb.velocity = direction.normalized * GameConstants.BALL_SPEED;
        God.GetPlayer().Bag.Drop(this);
        God.GetPlayer().currentlyEquipped = null;

        // God.GetStats().incrementStat("whistle_uses", 1);
        God.GetStats().SendData();
    }

    public override void EndUse(Vector2 location)
    {
        // this.IsOn = false;
        // ballSoumd.Stop();
    }

    public override void Using(Vector2 location)
    {
    }

    protected override void OnEquip()
    {
        
    }

    protected override void OnUnequip()
    {
        // ballSoumd.Stop();
    }

    public bool Interact(Player p)
    {
        this.IsOn = false;
        this.rb.velocity = Vector3.zero;
        this.rb.simulated = false;

        p.Bag.Add(this);
		// God.ShowText (HintsList.ON_BALL_PICKUP);
        return this;
    }

    public bool Interact(Cat c)
    {
        return false;
    }

    public override PersistanceType GetPType()
    {
        return PersistanceType.TOY_BALL;
    }
}
