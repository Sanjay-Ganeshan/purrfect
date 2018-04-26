using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSavable : MonoBehaviour, IPersistantObject {

    public string id = "";
    public PersistanceType PType;

    string IIdentifiable.getID()
    {
        return this.id;
    }

    MonoBehaviour IPersistantObject.GetMono()
    {
        return this;
    }

    PersistanceType IPersistantObject.GetPType()
    {
        return this.PType;
    }

    void IPersistantObject.Load(Dictionary<string, string> saveData)
    {
        
    }

    bool IPersistantObject.PersistThroughLoad()
    {
        return false;
    }

    void IPersistantObject.PostLoad()
    {
        
    }

    Dictionary<string, string> IPersistantObject.Save()
    {
        return new Dictionary<string, string>();
    }

    void IIdentifiable.setID(string id)
    {
        this.id = id;
    }

    // Use this for initialization
    void Start () {
		
	}

    void IPersistantObject.Unload()
    {
        God.Kill(this.gameObject);
    }

    public virtual void LateUpdate()
    {
        this.GenerateIDIfNeeded();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
