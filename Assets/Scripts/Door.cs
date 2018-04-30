using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable {

    public bool IsOpen;
    public int LockCombo;  // Number that encodes the type of door this is. Keys with this combo can open this doors 
    public Inventory Lock;

    public Collider2D DoorCollider;

    private bool shouldCheck = true;

	// Use this for initialization
	void Start () {
        IsOpen = false;
        Lock = new Inventory(this.transform);
    }
	
	// Update is called once per frame
	void Update () {
        if(shouldCheck)
        {
            CheckState();
        }
    }

    public bool Interact(Player p) {
        if (!IsOpen)
        {

            Key k = p.Bag.GetStreamBySystemType<Key>().Where((key) => key.CanUnlock(this.LockCombo)).FirstOrDefault();

            if (k != null)
            {
                bool ShouldAdd = k.Consume();
                if (ShouldAdd)
                {
                    this.Lock.Add(k);
                }
                this.ToggleDoorState();
                return true;
            }
        }
        return false;
    }

    public bool Interact(Cat c)
    {
        return false;
    }

    public void ToggleDoorState() {
        if (IsOpen) CloseDoor();
        else OpenDoor();
    }

    private void OpenDoor() {
        // Open the door
        DoorCollider.enabled = false;
        this.IsOpen = true;
    }

    private void CloseDoor() {
        //Close the door
        DoorCollider.enabled = true;
        this.IsOpen = false;
    }

    private void CheckState() {
        if(DoorCollider == null)
        {
            return;
        }
        if (IsOpen)
        {
            // Door is supposed to be open
            OpenDoor();   
        }
        else {
            // Door is supposed to be closed
            CloseDoor();
        }
        shouldCheck = false;
    }
}
