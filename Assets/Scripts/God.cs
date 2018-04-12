using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class God: MonoBehaviour
{
    // Singleton instance
    private static God TheOnlyGod;
    private bool isPaused;
    private void Start()
    {
        TheOnlyGod = this;
        this.isPaused = false;
    }

    private void InternalKill(GameObject gameObject)
    {
        GameObject.Destroy(gameObject);
    }


    public static bool IsPaused()
    {
        if(TheOnlyGod != null)
        {
            return TheOnlyGod.isPaused;
        }
        else
        {
            // If God isn't initialized yet, we should probably wait
            return true;
        }
    }

    public static void TogglePause()
    {
        SetPause(!IsPaused());
    }

    public static void Pause()
    {
        SetPause(true);
    }

    public static void Unpause()
    {
        SetPause(false);
    }

    public static void SetPause(bool newVal)
    {
        if(TheOnlyGod != null)
        {
            TheOnlyGod.isPaused = newVal;
        }
    }

    public static void Kill(GameObject gameObject) {
        if(TheOnlyGod != null)
        {
            TheOnlyGod.InternalKill(gameObject);
        }
    }


}