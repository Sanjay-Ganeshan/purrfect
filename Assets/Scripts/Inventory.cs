using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Inventory
{
    public HashSet<InventoryItem> Items;
    Transform owner;
    public Inventory(Transform owner)
    {
        this.Items = new HashSet<InventoryItem>();
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
}
