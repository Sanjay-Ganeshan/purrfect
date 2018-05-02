using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IPersistantObject: IIdentifiable
{
    void PreSave();
    void Load(Dictionary<string, string> saveData);
    void PostLoad();
    Dictionary<string, string> Save();
    void Unload();
    PersistanceType GetPType();
    IEnumerable<string> PersistThroughLoad();
    MonoBehaviour GetMono();
}