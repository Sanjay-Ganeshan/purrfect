using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSaver : MonoBehaviour, IPersistantObject {

	public string id = "";
	public PersistanceType PType;

	string IIdentifiable.getID()
	{
		return this.id;
	}

	MonoBehaviour IPersistantObject.GetMono()
	{
		return this;
	}

	PersistanceType IPersistantObject.GetPType()
	{
		return this.PType;
	}

	void IPersistantObject.Load(Dictionary<string, string> saveData)
	{
		Vector2 sizeVec = new Vector2(float.Parse(saveData["rendx"]), float.Parse(saveData["rendy"]));
		this.gameObject.GetComponent<SpriteRenderer>().size = sizeVec;
		this.gameObject.transform.GetChild (0).transform.UpdateToSaved (saveData ["childTrans"]);
		this.gameObject.transform.GetChild (1).transform.UpdateToSaved (saveData ["childTrans"]);

	}
		
	IEnumerable<string> IPersistantObject.PersistThroughLoad()
	{
		return new string[]{};
	}

	void IPersistantObject.PostLoad()
	{

	}

	Dictionary<string, string> IPersistantObject.Save()
	{
		Dictionary<string, string> dict = new Dictionary<string, string> ();

		Vector2 size = this.gameObject.GetComponent<SpriteRenderer> ().size;

		dict.Add ("rendx", size.x.ToString());
		dict.Add ("rendy", size.y.ToString());

		string childrenTransforms = this.gameObject.transform.GetChild (0).transform.ToSavableString ();
		dict.Add ("childTrans", childrenTransforms);
		return dict;
	}

	void IIdentifiable.setID(string id)
	{
		this.id = id;
	}

	// Use this for initialization
	void Start () {

	}

	void IPersistantObject.Unload()
	{
		God.Kill(this.gameObject);
	}

	public virtual void LateUpdate()
	{
		this.GenerateIDIfNeeded();
	}

	// Update is called once per frame
	void Update () {

	}

	public void PreSave()
	{

	}
}
