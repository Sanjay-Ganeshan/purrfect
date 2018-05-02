using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class Savior: MonoBehaviour
{
    public string LevelName;
    public PersistantEntry[] Templates;
    private Dictionary<PersistanceType, GameObject> _Templates;

    public bool activated = true;
    public bool SaveMode = true;

    void Start()
    {
        _Templates = new Dictionary<PersistanceType, GameObject>();
        foreach(PersistantEntry temp in Templates)
        {
            _Templates.Add(temp.PType, temp.Template);
        }
        Debug.Log("Persistance has activated. Please wait a few moments...");
        if (activated)
        {
            if (SaveMode)
            {
                Invoke("SaveAll", 1.0f);
            }
            else
            {
                Invoke("LoadAll", 1.0f);
            }
        }
    }

    void Update()
    {
        
    }

    void SaveAll()
    {
        IPersistantObject[] mapObjects;
        mapObjects = GetMapPersistantObjects();
        Dictionary<string, Dictionary<string, string>> output = new Dictionary<string, Dictionary<string, string>>(); 
        foreach(IPersistantObject o in mapObjects)
        {
            Dictionary<string, string> saved = o.Save();
            if (saved != null)
            {
                output.Add(o.getID(), saved);
                output[o.getID()].Add("type", o.GetPType().ToString());
                output[o.getID()].Add("transform", o.GetMono().transform.ToSavableString());
            }
        }
        Level lvl = new Level(LevelName);
        lvl.SetContents(output).SaveToMaster();
        Debug.Log("Saved to " + lvl.GetMasterFilepath() +"!");
    }

    public void LoadLevel(string name, bool keepCarried)
    {
        // STATS
        God.GetStats().SendData();
        // ENDSTATS

        Dictionary<string, Dictionary<string, string>> toCarry = Unload();
        Dictionary<string, PersistanceType> typeLookup = new Dictionary<string, PersistanceType>();
        foreach (PersistanceType t in System.Enum.GetValues(typeof(PersistanceType)))
        {
            typeLookup.Add(t.ToString(), t);
        }
        Level lvl = new Level(name).LoadFromMaster();
        Dictionary<string, Dictionary<string, string>> output = lvl.Contents;

        // Merge the dictionaries of the scene & file
        if(keepCarried)
        {
            // For each ID
            foreach(string key in toCarry.Keys)
            {
                if(output.ContainsKey(key))
                {
                    // If that ID is already in the file, go through each property
                    foreach(string innerKey in toCarry[key].Keys)
                    {
                        // And replace the file-property with the scene property
                        output[key][innerKey] = toCarry[key][innerKey];
                    }
                }
                else
                {
                    // If that ID is not in the file, add it from the scene
                    output.Add(key, toCarry[key]);
                }
            }
        }
        List<IPersistantObject> generatedObjects = new List<IPersistantObject>();
        bool usingCompatibility = false;
        foreach (string id in output.Keys)
        {
            Dictionary<string, string> dict = output[id];
            string t = dict["type"];
            int num;
            GameObject template;
            if (int.TryParse(t, out num))
            {
                // Allow numbers for backwards compatibility
                template = _Templates[(PersistanceType)num];
                usingCompatibility = true;
            }
            else
            {
                template = _Templates[typeLookup[dict["type"]]];
            }
            GameObject loaded = GameObject.Instantiate(template);
            IPersistantObject persistance = loaded.GetComponent<IPersistantObject>();
            persistance.setID(id);
            persistance.GetMono().transform.UpdateToSaved(dict["transform"]);
            persistance.Load(dict);
            generatedObjects.Add(persistance);
        }
        generatedObjects.ForEach(obj => obj.PostLoad());
        List<IIdentifiable> identif = new List<IIdentifiable>();
        generatedObjects.ForEach(obj => identif.Add(obj));
        God.UpdateIDLookup(identif);
        if (usingCompatibility)
        {
            Debug.Log("WARNING: Using compatibility mode to load level...things may not load as expected");
        }
        Debug.Log("Loaded from " + lvl.GetMasterFilepath() + "!");
    }

    void LoadAll()
    {
        LoadLevel(LevelName, false);
    }

    Dictionary<string, Dictionary<string,string>> Unload()
    {
        IPersistantObject[] mapObjects = GetMapPersistantObjects();
        Dictionary<string, Dictionary<string, string>> carrying = new Dictionary<string, Dictionary<string, string>>();
        foreach (IPersistantObject ip in mapObjects)
        {
            Dictionary<string, string> saved = ip.Save();
            Dictionary<string, string> carry = new Dictionary<string, string>();
            foreach(string carriedKey in ip.PersistThroughLoad())
            {
                carry.Add(carriedKey, saved[carriedKey]);
            }
            if(carry.Keys.Count > 0)
            {
                carrying.Add(ip.getID(), carry);
            }
            ip.Unload();
        }
        return carrying;
    }

    

    IPersistantObject[] GetMapPersistantObjects()
    {
        return FindObjectsOfType<MonoBehaviour>().OfType<IPersistantObject>().ToArray();
    }
}
