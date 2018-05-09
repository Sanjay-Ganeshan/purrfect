using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class Level
{
    public Dictionary<string, Dictionary<string, string>> Contents;
    public string Name;

    public Level(string name)
    {
        this.Name = name;
    } 

    public string GetMasterFilepath()
    {
        return Path.Combine(Path.Combine(Application.dataPath, GameConstants.LEVEL_PATH), this.Name + ".json");
    }

    public string GetPlaythroughFilepath() {
        return Path.Combine(Path.Combine(Application.dataPath, GameConstants.SAVE_PATH), this.Name + ".json");
    }

    public Level SetContents(Dictionary<string, Dictionary<string,string>> contents)
    {
        this.Contents = contents;
        return this;
    }

    public Level SetName(string name)
    {
        this.Name = name;
        return this;
    }

    public Level SaveToPath(string path)
    {
        Debug.Log("Saving to " + path);
        string jsonOutput;
        jsonOutput = JsonConvert.SerializeObject(this,Formatting.Indented);
        using (var writer = new StreamWriter(path))
        {
            writer.Write(jsonOutput);
        }
        return this;
    }

    public Level LoadFromPath(string path)
    {
        Debug.Log("Loading from " + path);
        string json;
        using (var reader = new StreamReader(path))
        {
            json = reader.ReadToEnd();
        }
        Level output = JsonConvert.DeserializeObject<Level>(json);
        this.Contents = output.Contents;
        this.Name = output.Name;
        return this;
    }

    public Level SaveToMaster()
    {
        return SaveToPath(GetMasterFilepath());
    }

    public Level LoadFromMaster()
    {
        return LoadFromPath(GetMasterFilepath());   
    }

    public Level SaveToPlaythrough()
    {
        return SaveToPath(GetPlaythroughFilepath());
    }

    public Level LoadFromPlaythrough()
    {
        return LoadFromPath(GetPlaythroughFilepath());
    }
}
