using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System;

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
        if (activated)
        {
            Debug.Log("Persistance has activated. Please wait a few moments...");
            if (SaveMode)
            {
                Invoke("SaveAll", 1.0f);
            }
            else
            {
                Invoke("LoadAll", 0.05f);
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
        PreSave(mapObjects);
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
        /* Do stats stuff */
		God.SetCurrentLevel (name);
        God.GetStats().initLevel(name);

        Dictionary<string, Dictionary<string, string>> toCarry = Unload();
        //Debug.Log("Carrying " + toCarry.Count + " objects to next scene");
        //foreach(string key in toCarry.Keys)
        //{
        //    Debug.Log("Carrying " + key);
        //}
        Dictionary<string, PersistanceType> typeLookup = new Dictionary<string, PersistanceType>();
        foreach (PersistanceType t in System.Enum.GetValues(typeof(PersistanceType)))
        {
            typeLookup.Add(t.ToString(), t);
        }
        Level lvl = new Level(name).LoadFromPlaythrough();
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
            if (dict.ContainsKey("transform"))
            {
                persistance.GetMono().transform.UpdateToSaved(dict["transform"]);
            }
            persistance.Load(dict);
            generatedObjects.Add(persistance);
        }
        List<IIdentifiable> identif = new List<IIdentifiable>();
        generatedObjects.ForEach(obj => identif.Add(obj));
        God.UpdateIDLookup(identif);
        generatedObjects.ForEach(obj => obj.PostLoad());
        if (usingCompatibility)
        {
            Debug.Log("WARNING: Using compatibility mode to load level...things may not load as expected");
        }
        lvl.SaveToPlaythrough();
		if (name == GameConstants.OPENING_NARRATIVE_LEVEL) {
			God.ShowTexts (HintsList.OPENING_NARRATIVE);
		}
		if (name == GameConstants.FIRST_GUARD_LEVEL) {
			God.ShowTexts (HintsList.GUARDS);
		}
		if (name == GameConstants.FINAL_LEVEL) {
			God.ShowTextsMoreSpeakers (HintsList.FINAL_LEVEL_NARRATIVE, HintsList.FINAL_LEVEL_NARRATIVE_SAYERS);
		}
		if (name == GameConstants.GAME_END) {
			God.ShowText (HintsList.GAME_END_YAY);
		}
    }
    

    void LoadAll()
    {
        LoadLevel(LevelName, false);
    }

    void PreSave(IPersistantObject[] mapObjects)
    {
        foreach (IPersistantObject ip in mapObjects)
        {
            ip.PreSave();
        }
    }

    Dictionary<string, Dictionary<string,string>> Unload()
    {
        IPersistantObject[] mapObjects = GetMapPersistantObjects();
        PreSave(mapObjects);
        Level newPlaythroughVersion = new Level(God.GetCurrentLevel());
        Dictionary<string, Dictionary<string, string>> levelState = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, Dictionary<string, string>> carrying = new Dictionary<string, Dictionary<string, string>>();
        foreach (IPersistantObject ip in mapObjects)
        {
            Dictionary<string, string> saved = ip.Save();
            Dictionary<string, string> carry = new Dictionary<string, string>();

            bool shouldAdd = false;
            IEnumerable<string> toPersist = ip.PersistThroughLoad();
            List<string> toPersistL = toPersist.ToList();
            foreach (string carriedKey in toPersistL)
            {
                if (saved.ContainsKey(carriedKey))
                {
                    carry.Add(carriedKey, saved[carriedKey]);
                }
                shouldAdd = true;
            }
            carry.Add("type", ip.GetPType().ToString());
            //carry.Add("transform", ip.GetMono().transform.ToSavableString());
            if (shouldAdd)
            {
                carrying.Add(ip.getID(), carry);
            }
        }
        foreach (IPersistantObject ip in mapObjects)
        {
            ip.Unload();
        }
        return carrying;
    }

	public Dictionary<PersistanceType, GameObject> GetTemplates()
	{
		return _Templates;
	}

    IPersistantObject[] GetMapPersistantObjects()
    {
        return FindObjectsOfType<MonoBehaviour>().OfType<IPersistantObject>().ToArray();
    }

    public void TransitionToNewLevel(string newLevel, bool keepCarried) {
        God.GetStats().cleared();
        God.GetStats().SendData();

        LoadLevel(newLevel, keepCarried);
        God.CloseText();
        God.IncrementHintLevel();
    }

    public void ReloadCurrentLevel() {
        God.GetStats().incrementStat("attempts", 1);
        God.GetStats().SendData();

        LoadLevel(God.GetCurrentLevel(), false);
        // Handle if we want story to reset as well
        // Should hint level remain?
    }
}
