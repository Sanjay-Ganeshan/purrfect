using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public bool isOpen;
    private bool doorClosed; // tracks true state of the door

    public GameObject ColliderObject;

	// Use this for initialization
	void Start () {
        isOpen = false;
        doorClosed = true;

    }
	
	// Update is called once per frame
	void Update () {
        CheckState();

    }

    public void ToggleDoorState() {
        isOpen = !isOpen;
        CheckState();
    }
    private void CheckState() {
        if (isOpen)
        {
            // Door is supposed to be open
            if (doorClosed) {
                // Open the door
                ColliderObject.GetComponent<BoxCollider2D>().enabled = false;
                doorClosed = false;
            }
        }
        else {
            // Door is supposed to be closed
            if (!doorClosed) {
                //Close the door
                ColliderObject.GetComponent<BoxCollider2D>().enabled = true;
                doorClosed = true;
            }
        }
    }
}
