using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transitioner : MonoBehaviour, IPersistantObject {

    public string LevelToLoad;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D player)
    {
        Optional<Player> collidedPlayer = Optional<Player>.Of(player.gameObject.GetComponent<Player>());
        God.GetSavior().LoadLevel(LevelToLoad, true);
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
