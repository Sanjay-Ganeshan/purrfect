﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transitioner : MonoBehaviour, IPersistantObject {

    public string LevelToLoad;
	public Collider2D enteredPlayer = null;

	// Use this for initialization
	void Start () {
		
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
				God.GetSavior().LoadLevel(LevelToLoad, true);
				God.IncrementHintLevel();
			}
		}
	}

    public void Load(Dictionary<string, string> saveData)
    {
        this.LevelToLoad = saveData["Level"];
    }

    public void PostLoad()
    {

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
