using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class Savior: MonoBehaviour
{
    public string filepath;
    public PersistantEntry[] Templates;
    private Dictionary<PersistanceType, GameObject> _Templates;

    public bool activated = true;
    public bool SaveMode = true;
    public bool SaveToScene = false;
    public string SceneFolder;
    public string SceneOutputName;

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



    void DoSaveToScene()
    {
        #if UNITY_EDITOR
        this.activated = false;
        //UnityEditor.SceneManagement.EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), Path.Combine(SceneFolder, SceneOutputName + ".unity"), true);
        this.activated = true;
        #endif
    }

    void SaveAll()
    {
        IPersistantObject[] mapObjects;
        mapObjects = GetMapPersistantObjects();
        Dictionary<string, Dictionary<string, string>> output = new Dictionary<string, Dictionary<string, string>>(); 
        foreach(IPersistantObject o in mapObjects)
        {
            output.Add(o.getID(), o.Save());
            output[o.getID()].Add("type", o.GetPType().ToString());
            output[o.getID()].Add("transform", o.GetMono().transform.ToSavableString());      
        }
        string jsonOutput;
        jsonOutput = JsonConvert.SerializeObject(output);
        //Debug.Log(jsonOutput);
        using (var writer = new StreamWriter(filepath))
        {
            writer.Write(jsonOutput);
        }
        Debug.Log("Saved to " + filepath +"!");
        if(SaveToScene)
        {
            DoSaveToScene();
        }
    }

    void LoadAll()
    {
        Unload();
        Dictionary<string, PersistanceType> typeLookup = new Dictionary<string, PersistanceType>();
        foreach(PersistanceType t in System.Enum.GetValues(typeof(PersistanceType)))
        {
            typeLookup.Add(t.ToString(), t);
        }
        string json;
        using (var reader = new StreamReader(filepath))
        {
            json = reader.ReadToEnd();
        }
        Dictionary<string, Dictionary<string, string>> output = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
        List<IPersistantObject> generatedObjects = new List<IPersistantObject>();
        foreach(string id in output.Keys)
        {
            Dictionary<string, string> dict = output[id];
            GameObject template = _Templates[typeLookup[dict["type"]]];
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
        Debug.Log("Loaded from " + filepath + "!");
        if (SaveToScene)
        {
            DoSaveToScene();
        }
    }

    void Unload()
    {
        IPersistantObject[] mapObjects = GetMapPersistantObjects();
        foreach (IPersistantObject ip in mapObjects)
        {
            ip.Unload();
        }
    }

    IPersistantObject[] GetMapPersistantObjects()
    {
        return FindObjectsOfType<MonoBehaviour>().OfType<IPersistantObject>().ToArray();
    }
}
