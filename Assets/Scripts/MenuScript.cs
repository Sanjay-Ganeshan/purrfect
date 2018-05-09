using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MenuScript : MonoBehaviour {

	// Use this for initialization
	public void ChangeScene(string scene){
        CreateNewPlaythrough();
		UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
	}

    private void CreateNewPlaythrough()
    {
        Level fakeLevel = new Level("DefinitelyNotARealThing");
        string fp = fakeLevel.GetMasterFilepath();
        string masterPath = Path.GetDirectoryName(fp);
        string[] masterFiles = Directory.GetFiles(masterPath, "*.json");
        foreach(string lvl in masterFiles)
        {
            string levelName = Path.GetFileNameWithoutExtension(lvl);
            Level loaded = new Level(levelName);
            loaded.LoadFromMaster();
            loaded.SaveToPlaythrough();
        }
    }
}
