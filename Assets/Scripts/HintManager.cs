﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour {

	public Button HintsButton, DialogueBox, ResetButton;
	public Image DialoguePortrait;
	private bool canGetHint = true;
	private int level;
	private int nextHintNum = 0;
	private Queue<string> textsToShow = new Queue<string> ();

	// Use this for initialization
	void Start () {
		HintsButton.onClick.AddListener(HintsButtonOnClick);
		DialogueBox.onClick.AddListener(DialogueBoxOnClick);
		ResetButton.onClick.AddListener(ResetButtonOnClick);
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
		if (textsToShow.Count > 0) 
		{
			DialogueBox.GetComponentInChildren<Text> ().text = textsToShow.Dequeue ();
		} 
		else 
		{
			DialogueBox.gameObject.SetActive (false);
			DialoguePortrait.gameObject.SetActive (false);
			canGetHint = true;
		}
	}

	private void ResetButtonOnClick() 
	{
    	God.GetSavior().ReloadCurrentLevel();
	}

	public void MoveToNextLevel () { // may need modification if we branch in the future
		level += 1;
		nextHintNum = 0;
	}

	public void ShowText(string text) {
		canGetHint = false;
		DialogueBox.GetComponentInChildren<Text> ().text = text;
		DialogueBox.gameObject.SetActive (true);
		DialoguePortrait.gameObject.SetActive (true);
	}

	public void ShowTexts(string[] texts) {
		canGetHint = false;
		if (texts.Length > 0) 
		{
			DialogueBox.GetComponentInChildren<Text> ().text = texts [0];
			for (int i = 1; i < texts.Length; i++) {
				textsToShow.Enqueue (texts [i]);
			}
			DialogueBox.gameObject.SetActive (true);
			DialoguePortrait.gameObject.SetActive (true);
		}
	}

	public void CloseText() {
		DialogueBox.gameObject.SetActive (false);
		DialoguePortrait.gameObject.SetActive (false);
		int numTexts = textsToShow.Count; // clear queue
		for (int i = 0; i < numTexts; i++) {
			textsToShow.Dequeue ();
		}
	}
}
