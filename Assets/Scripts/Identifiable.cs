using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IIdentifiable
{
    string getID();
    void setID(string id);
}

public class IdentifiableTemplate: MonoBehaviour, IIdentifiable
{
    string id = "";
    public string getID()
    {
        return id;
    }

    public virtual void LateUpdate()
    {
        this.GenerateIDIfNeeded();
    }

    public void setID(string id)
    {
        this.id = id;
    }
}
