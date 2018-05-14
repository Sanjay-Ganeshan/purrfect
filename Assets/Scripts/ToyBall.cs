using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ToyBall : InventoryItem, IInteractable
{
    private Rigidbody2D rb;
	private bool onWall = false;

    public bool IsOn;

    // public AudioSource ballSoumd;

    public void Start()
    {
        IsOn = true;
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
		if (!HintsList.YARN_SAID) {
			God.ShowTexts (HintsList.YARN);
			HintsList.YARN_SAID = true;
		}
		this.rb.velocity = c.GetComponent<Rigidbody2D> ().velocity;
		if (onWall) {
			this.IsOn = false;
			this.rb.velocity = Vector3.zero;
			this.rb.simulated = false;

			c.catInventory.Add(this);
			onWall = false;
		}
		return this;
    }

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag != GameConstants.LBL_CAT) {
			onWall = true;
		}
	}
		
	public override void OnPickup()
	{
		if (this.ZoC != null)
		{
			this.ZoC.enabled = false;
		}
		Debug.Log (this.GetOwner ().GetOwner ().tag);
		if (this.GetOwner ().GetOwner().tag == GameConstants.LBL_CAT) 
		{
			RenderWorld ();
		}
		else 
		{
			RenderNone ();
		}
	}

	public override IEnumerable<string> PersistThroughLoad()
	{
		return new string[0];
	}

    public override PersistanceType GetPType()
    {
        return PersistanceType.TOY_BALL;
    }
}
