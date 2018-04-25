﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class Savior: MonoBehaviour
{
    public string filepath;
    public PersistantEntry[] Templates;
    private Dictionary<PersistanceType, GameObject> _Templates;
    

    public bool SaveMode = true;

    void Start()
    {
        _Templates = new Dictionary<PersistanceType, GameObject>();
        foreach(PersistantEntry temp in Templates)
        {
            _Templates.Add(temp.PType, temp.Template);
        }
        Debug.Log("Persistance has activated. Please wait a few moments...");
        if (SaveMode)
        {
            Invoke("SaveAll", 1.0f);
        }
        else
        {
            Invoke("LoadAll", 1.0f);
        }
    }

    void Update()
    {
        
    }

    void SaveAll()
    {
        IPersistantObject[] mapObjects;
        mapObjects = GetMapPersistantObjects();
        Dictionary<int, Dictionary<string, string>> output = new Dictionary<int, Dictionary<string, string>>(); 
        foreach(IPersistantObject o in mapObjects)
        {
            output.Add(o.getID(), o.Save());
            output[o.getID()].Add("type", ((int)(o.GetPType())).ToString());
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
    }

    void LoadAll()
    {
        Unload();
        string json;
        using (var reader = new StreamReader(filepath))
        {
            json = reader.ReadToEnd();
        }
        Dictionary<int, Dictionary<string, string>> output = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<string, string>>>(json);
        foreach(int id in output.Keys)
        {
            Dictionary<string, string> dict = output[id];
            GameObject template = _Templates[(PersistanceType)int.Parse(dict["type"])];
            GameObject loaded = GameObject.Instantiate(template);
            IPersistantObject persistance = loaded.GetComponent<IPersistantObject>();
            persistance.setID(id);
            persistance.GetMono().transform.UpdateToSaved(dict["transform"]);
            persistance.Load(dict);
        }
        Debug.Log("Loaded from " + filepath + "!");
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
