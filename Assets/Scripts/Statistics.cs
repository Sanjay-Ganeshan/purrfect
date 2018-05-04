using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class Statistics {
    private string urlBase = "https://3edgy6u.com/py/analytics?data=";
    private Dictionary<string, object> data;
    private float start_time;

    public Statistics() {
        data = new Dictionary<string, object>();
        data["game_id"] = "purrfect";
        data["user_id"] = Random.Range(0,999999);
    }

    public void initLevel(string id)
    {
        data["level_id"] = id;
        data["level_difficulty"] = 0;
        data["attempts"] = 0;
        data["cleared"] = false;
        data["duration"] = 0f;
        data["hints"] = 0;
        data["player_movement"] = 0f;
        data["mouse_movement"] = 0f;
        data["laser_shots"] = 0;

        start_time = Time.time;
    }

    public void incrementStat(string key, int count)
    {
        data[key] = (int)data[key] + count;
    }

    public void incrementStat(string key, float count)
    {
        data[key] = (float)data[key] + count;
    }
    
    public void cleared()
    {
        data["cleared"] = true;
    }

    public void SendData() {
        data["duration"] = Time.time - start_time;

        Debug.Log("STATISTICS DATA SENT.");
        UnityWebRequest www = UnityWebRequest.Get(urlBase + JsonConvert.SerializeObject(data));
        www.SendWebRequest();
    }
}
