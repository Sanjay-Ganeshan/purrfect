using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Statistics {
    private string urlBase = "https://3edgy6u.com/py/analytics?data=";

	// Use this for initialization
	void Start () {
	    	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SendData() {
        Debug.Log("STATISTICS DATA SENT.");
        UnityWebRequest www = UnityWebRequest.Get(urlBase + "\"UNITY SENT DATA!!!\"");
        www.SendWebRequest();
    }
}
