using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour {

	public Button HintsButton, DialogueBox;
	public Image DialoguePortrait;
	private bool canGetHint = true;
	private int level;
	private int nextHintNum = 0;

	// Use this for initialization
	void Start () {
		HintsButton.onClick.AddListener(HintsButtonOnClick);
		DialogueBox.onClick.AddListener (DialogueBoxOnClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void HintsButtonOnClick()
	{
		if (canGetHint)
        {

            canGetHint = false;
			if (nextHintNum < HintsList.HINTS_PER_LEVEL)
            {
                God.GetStats().incrementStat("hints", 1);
                God.GetStats().SendData();

                DialogueBox.GetComponentInChildren<Text> ().text = HintsList.ALL_HINTS [level, nextHintNum];
			} 
			else 
			{
				DialogueBox.GetComponentInChildren<Text> ().text = HintsList.NO_MORE_HINTS;
			}
			DialogueBox.gameObject.SetActive (true);
			DialoguePortrait.gameObject.SetActive (true);
			nextHintNum += 1;
		}
	}

	void DialogueBoxOnClick()
	{
		DialogueBox.gameObject.SetActive (false);
		DialoguePortrait.gameObject.SetActive (false);
		canGetHint = true;
	}

	public void MoveToNextLevel () { // may need modification if we branch in the future
		level += 1;
		nextHintNum = 0;
	}

	public void ShowText(string text) {
		DialogueBox.GetComponentInChildren<Text> ().text = text;
		DialogueBox.gameObject.SetActive (true);
		DialoguePortrait.gameObject.SetActive (true);
	}
}
