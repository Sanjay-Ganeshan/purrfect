using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour, IPersistantObject {
    string IIdentifiable.getID()
    {
        return GameConstants.CAMERA_ID;
    }

    MonoBehaviour IPersistantObject.GetMono()
    {
        return this;
    }

    PersistanceType IPersistantObject.GetPType()
    {
        return PersistanceType.CAMERA;
    }

    void IPersistantObject.Load(Dictionary<string, string> saveData)
    {
        this.GetComponent<Camera>().orthographicSize = float.Parse(saveData["size"]);
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, float.Parse(saveData["z"]));
    }

    IEnumerable<string> IPersistantObject.PersistThroughLoad()
    {
        return new string[] { };
    }

    void IPersistantObject.PostLoad()
    {
        
    }

    Dictionary<string, string> IPersistantObject.Save()
    {
        Dictionary<string, string> o = new Dictionary<string, string>();
        o.Add("size", this.GetComponent<Camera>().orthographicSize.ToString());
        o.Add("z", this.transform.position.z.ToString());
        return o;
    }

    void IIdentifiable.setID(string id)
    {
        
    }

    // Use this for initialization
    void Start () {
		
	}

    void IPersistantObject.Unload()
    {
        God.Kill(this.gameObject);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
