using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenButton : MonoBehaviour, IPersistantObject {

	public Sprite ButtonPressed;
	public GameObject gameChild;
	private IPersistantObject child;
	private string childID = "";
	private bool pressed = false;

	// Use this for initialization
	void Start () {
		if (gameChild != null) {
			child = gameChild.GetComponent<IPersistantObject>();
		}
	}

	// Update is called once per frame
	void Update () {
		if (gameChild == null) {
			SearchForChild ();
		}
	}

	void OnTriggerEnter2D(Collider2D character)
	{
		if (!pressed) 
		{
			pressed = true;
			this.GetComponentInChildren<SpriteRenderer> ().sprite = ButtonPressed;
			gameChild.SetActive (true);
			if (character.gameObject.tag == "Witch") {
				StartCoroutine (WitchText ());
			}
		}
	}

	IEnumerator WitchText() {
		yield return new WaitForSeconds (0.5f);
		God.ShowTextWithWitch (HintsList.WITCH_GIVES_KEY);
	}

	private bool SearchForChild() {
		Optional<IIdentifiable> possibleChild = God.GetByID (childID);
		if (possibleChild.IsPresent ()) {
			child = (IPersistantObject)(possibleChild.Get ());
			gameChild = child.GetMono ().gameObject;
			gameChild.SetActive (false);
			return true;
		}
		return false;
	}
		
	public void Load(Dictionary<string, string> saveData) {
		childID = saveData ["childID"];
	}

	public void PostLoad()
	{
		God.GetStats().SendData();
	}
		
	public Dictionary<string, string> Save()
	{
		Dictionary<string, string> d = new Dictionary<string, string>();
		child.GenerateIDIfNeeded ();
		childID = child.getID ();
		d ["childID"] = childID;
		return d;
	}

	public void Unload()
	{
		God.Kill(this.gameObject);
	}

	public PersistanceType GetPType()
	{
		return PersistanceType.BUTTON;
	}

	public IEnumerable<string> PersistThroughLoad()
	{
		return new string[] { };
	}

	public MonoBehaviour GetMono()
	{
		return this;
	}

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

	public void PreSave()
	{

	}

}
