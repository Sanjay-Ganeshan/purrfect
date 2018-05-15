using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable, IPersistantObject {

    public bool IsOpen;
    public int LockCombo;  // Number that encodes the type of door this is. Keys with this combo can open this doors 
    public Inventory Lock;
    public string id = "";
    public PersistanceType PType;
	public Sprite OpenSprite;

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

    public virtual void LateUpdate()
    {
        this.GenerateIDIfNeeded();
    }

    public bool Interact(Player p) {
        if (!IsOpen)
        {

            Key k = p.Bag.GetStreamBySystemType<Key>().Where((key) => key.CanUnlock(this.LockCombo)).FirstOrDefault();

			if (k != null) 
			{
				bool ShouldAdd = k.Consume ();
				if (ShouldAdd) 
				{
					this.Lock.Add (k);
				}
				this.ToggleDoorState ();
				return true;
			}
			else 
			{
				if (!HintsList.INTERACT_GATE_WITHOUT_KEY_SAID) {
					God.ShowText (HintsList.INTERACT_GATE_WITHOUT_KEY);
					HintsList.INTERACT_GATE_WITHOUT_KEY_SAID = true;
				}
			}
        }
        return false;
    }

    public bool Interact(Cat c)
    {
        return false;
    }

    public void ToggleDoorState() {
		if (God.GetCurrentLevel () == GameConstants.FIRST_YARN_LEVEL) {
			God.ShowText (HintsList.NO_YARN_PERSIST);
		}
        if (IsOpen) CloseDoor();
        else OpenDoor();
    }

    private void OpenDoor() {
        // Open the door
        DoorCollider.enabled = false;
        this.IsOpen = true;
		this.GetComponentInChildren<SpriteRenderer> ().sprite = OpenSprite;
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

    string IIdentifiable.getID()
    {
        return this.id;
    }
    
    void IIdentifiable.setID(string id)
    {
        this.id = id;
    }

    void IPersistantObject.Load(Dictionary<string, string> saveData) {
        // I want to set the combo from the save data
        if (saveData.ContainsKey("IsOpen")) {
            this.IsOpen = bool.Parse(saveData["IsOpen"]);
        } else {
            this.IsOpen = false;
        }

        if (saveData.ContainsKey("Combo")) {
            // We have Combo Data in the level
            this.LockCombo = int.Parse(saveData["Combo"]);
        } else {
            // Oh no, default to 0 combo
            this.LockCombo = 0;
        }
        // want to check if i should be open, and my inventory eventually
    }
    
    void IPersistantObject.PostLoad() {
        // leave empty
    }
    
    Dictionary<string, string> IPersistantObject.Save() {
        // save my combo and if I am open
        Dictionary<string, string> o = new Dictionary<string, string>();
        o.Add("Combo", this.LockCombo.ToString());
        o.Add("IsOpen",this.IsOpen.ToString());

        return o;
    }
    
    void IPersistantObject.Unload() {
        God.Kill(this.gameObject);
    }
    
    public PersistanceType GetPType() {
        return PersistanceType.DOOR;
    }
    
    IEnumerable<string> IPersistantObject.PersistThroughLoad() {
        return new string[]{};
    }
    
    MonoBehaviour IPersistantObject.GetMono() {
        return this;
    }

    public void PreSave()
    {
        
    }
}
