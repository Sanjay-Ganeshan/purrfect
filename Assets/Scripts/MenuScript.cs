using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

	// Use this for initialization
	public void ChangeScene(string scene){
		UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
	}
}
