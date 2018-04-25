using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IPersistantObject
{
    void Load(Dictionary<string, string> saveData);
    Dictionary<string, string> Save();
    int getID();
    void Unload();
    PersistanceType GetPType();
    void setID(int id);
    bool PersistThroughLoad();
    MonoBehaviour GetMono();
}