using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transitioner : MonoBehaviour, IPersistantObject {

    public string LevelToLoad;
	private bool hintToBringCat = false;
	private Collider2D enteredPlayer = null;

	// Use this for initialization
	void Start () {
		if (God.GetCurrentLevel () == GameConstants.BRING_CAT_HINT_LEVEL) {
			hintToBringCat = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (enteredPlayer != null) 
		{
			TryMoveToNextLevel (enteredPlayer);
		}
	}
		
    void OnTriggerEnter2D(Collider2D player)
    {
		enteredPlayer = player;
    }

	void OnTriggerExit2D(Collider2D player)
	{
		enteredPlayer = null;
	}

	void TryMoveToNextLevel(Collider2D player)
	{
		Optional<Player> collidedPlayer = Optional<Player>.Of(player.gameObject.GetComponent<Player>());
		Optional<Cat> theCat = God.GetCat(true);
		if(theCat.IsPresent() && collidedPlayer.IsPresent())
		{
			Vector2 dist = theCat.Get ().transform.position - collidedPlayer.Get().transform.position;
			if (dist.SqrMagnitude () < GameConstants.MAX_CAT_DIST_FOR_LEVEL_END) {
				// I know this is a bandaid, but it fixes a problem that occurs when you carry yarn into a scene that also has yarn in it
				GameObject[] toyBalls = GameObject.FindGameObjectsWithTag("DeleteOnTransition");
				for (int i = 0; i < toyBalls.Length; i++) {
//					Debug.Log ("destroy ball");
//					Debug.Log ("length" + (toyBalls [i]).GetComponent<ToyBall>().GetOwner ().Items.Count);
					if ((toyBalls [i]).GetComponent<ToyBall> ().GetOwner () != null) {
						(toyBalls [i]).GetComponent<ToyBall> ().GetOwner ().Items.Remove (toyBalls [i].GetComponent<ToyBall> ());
					}
//					Debug.Log ("new length" + (toyBalls [i]).GetComponent<ToyBall>().GetOwner ().Items.Count);
					Destroy (toyBalls [i]);
				}
				God.GetSavior().TransitionToNewLevel(LevelToLoad, true);
			}
			else 
			{
				if (hintToBringCat) 
				{
					God.ShowText (HintsList.HINT_TO_BRING_CAT);
					hintToBringCat = false;
				}
			}
				
		}
	}

    public void Load(Dictionary<string, string> saveData)
    {
        this.LevelToLoad = saveData["Level"];
    }

    public void PostLoad()
    {
        God.GetStats().SendData();
    }

    public Dictionary<string, string> Save()
    {
        Dictionary<string, string> d = new Dictionary<string, string>();
        d["Level"] = this.LevelToLoad;
        return d;
    }

    public void Unload()
    {
        God.Kill(this.gameObject);
    }

    public PersistanceType GetPType()
    {
        return PersistanceType.EXIT;
    }

    public IEnumerable<string> PersistThroughLoad()
    {
        return new string[] { };
    }

    public MonoBehaviour GetMono()
    {
        return this;
    }

    string id = "";
    public string getID()
    {
        return id;
    }

    public virtual void LateUpdate()
    {
        this.GenerateIDIfNeeded();
    }

    public void setID(string id)
    {
        this.id = id;
    }

    public void PreSave()
    {
        
    }
}
