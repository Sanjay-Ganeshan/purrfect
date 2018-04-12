using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    private static UIManager theManager;

    public InventoryViewer InventoryScreen;

	// Use this for initialization
	void Start () {
        UIManager.theManager = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void ShowInventory_(Inventory inven, Action<InventoryItem> onSelection = null)
    {
        this.InventoryScreen.gameObject.SetActive(true);
        this.InventoryScreen.ShowInventory(inven, onSelection);
    }

    private void HideInventory_()
    {
        this.InventoryScreen.gameObject.SetActive(false);
    }
    
    private void ToggleInventory_(Inventory inven, Action<InventoryItem> onSelection = null)
    {
        if(this.InventoryScreen.gameObject.activeSelf)
        {
            HideInventory_();
        }
        else
        {
            ShowInventory_(inven, onSelection);
        }
    }

    public static void ShowInventory(Inventory inven, Action<InventoryItem> onSelection = null)
    {
        if(theManager != null)
        {
            theManager.ShowInventory_(inven, onSelection);
            God.Pause();
        }
        else
        {
            Debug.Log("No manager found");
        }
    }

    public static void HideInventory()
    {
        if(theManager != null)
        {
            theManager.HideInventory_();
            God.Unpause();
        }
    }

    public static void ToggleInventory(Inventory inven, Action<InventoryItem> onSelection = null)
    {
        if (theManager != null) {
            theManager.ToggleInventory_(inven, onSelection);
        }
    }

    public static void HideAllUI()
    {
        HideInventory();
    }


}
