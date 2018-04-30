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

    public string GetFilepath()
    {
        return Path.Combine(Path.Combine(Application.dataPath, GameConstants.LEVEL_PATH), this.Name + ".json");
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

    public Level Save()
    {
        string jsonOutput;
        jsonOutput = JsonConvert.SerializeObject(this);
        using (var writer = new StreamWriter(this.GetFilepath()))
        {
            writer.Write(jsonOutput);
        }
        return this;
    }

    public Level Load()
    {
        string json;
        using (var reader = new StreamReader(this.GetFilepath()))
        {
            json = reader.ReadToEnd();
        }
        Level output = JsonConvert.DeserializeObject<Level>(json);
        this.Contents = output.Contents;
        this.Name = output.Name;
        return this;
    }
}
