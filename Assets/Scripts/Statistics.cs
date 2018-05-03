﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class Statistics {
    private string urlBase = "https://3edgy6u.com/py/analytics?data=";
    private Dictionary<string, string> data;
    public Statistics() {
        this.data = new Dictionary<string, string>();
    }

	// Use this for initialization
	void Start () {
	    	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SendData() {
        Debug.Log("STATISTICS DATA SENT.");
        UnityWebRequest www = UnityWebRequest.Get(urlBase + JsonConvert.SerializeObject(data));
        www.SendWebRequest();
    }
}