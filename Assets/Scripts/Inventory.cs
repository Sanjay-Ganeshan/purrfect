using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Inventory: ICollection<InventoryItem>
{
    public List<InventoryItem> Items;
    Transform owner;
    public Inventory(Transform owner)
    {
        this.Items = new List<InventoryItem>();
        this.owner = owner;
    }

    public int Count
    {
        get
        {
            return this.Items.Count;
        }
    }

    public bool IsReadOnly
    {
        get
        {
            return false;
        }
    }

    public void Add(InventoryItem item)
    {
        AddToInventory(item);
    }

    public void AddToInventory(InventoryItem item, bool resetPosition = true)
    {
        this.Items.Add(item);
        item.transform.SetParent(owner);
        if(resetPosition)
        {
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public bool Drop(InventoryItem item)
    {
        bool wasRemoved = this.Items.Remove(item);
        if(wasRemoved)
        {
            item.transform.SetParent(null, true);
        }
        return wasRemoved;
    }

    public bool Delete(InventoryItem item)
    {
        bool wasRemoved = this.Items.Remove(item);
        if (wasRemoved)
        {
            item.Delete();
        }
        return wasRemoved;
    }

    public void Clear()
    {
        List<InventoryItem> toRemove = new List<InventoryItem>();
        toRemove.AddRange(this);
        foreach (InventoryItem item in toRemove) {
            this.Delete(item);
        }
    }

    public bool Contains(InventoryItem item)
    {
        return this.Items.Contains(item);
    }

    public void CopyTo(InventoryItem[] array, int arrayIndex)
    {
        this.Items.CopyTo(array, arrayIndex);
    }

    public IEnumerator<InventoryItem> GetEnumerator()
    {
        return this.Items.GetEnumerator();
    }

    public bool Remove(InventoryItem item)
    {
        return Delete(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
