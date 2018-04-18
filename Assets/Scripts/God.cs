using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class God: MonoBehaviour
{
    // Singleton instance
    private static God TheOnlyGod;
    private bool isPaused;
    private Optional<Cat[]> knownCats;
    private bool initialized = false;
    private void Start()
    {
        TheOnlyGod = this;
        InitIfNeeded();
    }

    private void DoInit()
    {
        this.isPaused = false;
        this.knownCats = Optional<Cat[]>.Empty();
    }

    private void InitIfNeeded()
    {
        if(!initialized)
        {
            DoInit();
            initialized = true;
        }
    }

    private void Smite(GameObject gameObject)
    {
        GameObject.Destroy(gameObject);
    }


    private Cat[] GetCats_(bool forceRefresh = false)
    {
        if (forceRefresh || !this.knownCats.IsPresent())
        {
            GameObject[] catsGO = GameObject.FindGameObjectsWithTag(GameConstants.LBL_CAT);
            Cat[] cats = new Cat[catsGO.Length];
            for (int i = 0; i < catsGO.Length; i++)
            {
                cats[i] = catsGO[i].GetComponent<Cat>();
            }

            this.knownCats = Optional<Cat[]>.Of(cats);
        }
        return this.knownCats.Get();
    }

    public static Optional<Cat> GetCat(bool forceRefresh = false)
    {
        Cat[] cats = GetCats(forceRefresh);
        if(cats.Length >= 1)
        {
            return Optional<Cat>.Of(cats[0]);
        }
        else
        {
            return Optional<Cat>.Empty();
        }
    }

    public static Cat[] GetCats(bool forceRefresh = false)
    {
        FindGod();
        return TheOnlyGod.GetCats_(forceRefresh);
    }

    private static void FindGod()
    {
        if(TheOnlyGod == null)
        {
            TheOnlyGod = GameObject.FindGameObjectWithTag("God").GetComponent<God>();
            TheOnlyGod.InitIfNeeded();
        }
    }

    public static bool IsPaused()
    {
        FindGod();
        return TheOnlyGod.isPaused;
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
        FindGod();
        TheOnlyGod.isPaused = newVal;
    }

    public static void Kill(GameObject gameObject) {
        FindGod();
        TheOnlyGod.Smite(gameObject);
    }


}