using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable {

    public bool IsOpen;
    private bool doorClosed; // tracks true state of the door
    public int LockCombo;  // Number that encodes the type of door this is. Keys with this combo can open this doors 
    public Inventory Lock;

    public GameObject ColliderObject;

	// Use this for initialization
	void Start () {
        IsOpen = false;
        doorClosed = true;
        Lock = new Inventory(this.transform);
    }
	
	// Update is called once per frame
	void Update () {
        CheckState();

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

    public void ToggleDoorState() {
        IsOpen = !IsOpen;
        CheckState();
    }

    private void OpenDoor() {
        if (doorClosed)
        {
            // Open the door
            ColliderObject.GetComponent<BoxCollider2D>().enabled = false;
            doorClosed = false;
        }
    }

    private void CloseDoor() {
        if (!doorClosed)
        {
            //Close the door
            ColliderObject.GetComponent<BoxCollider2D>().enabled = true;
            doorClosed = true;
        }
    }

    private void CheckState() {
        if (IsOpen)
        {
            // Door is supposed to be open
            OpenDoor();   
        }
        else {
            // Door is supposed to be closed
            CloseDoor();
        }
    }
}
