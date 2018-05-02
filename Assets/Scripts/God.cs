using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class God: MonoBehaviour
{
    // Singleton instance
    private static God TheOnlyGod;
    private bool isPaused;
    private Optional<Cat[]> knownCats;
    private bool initialized = false;
    private Savior MyChild;
    private Statistics stats;

    private Dictionary<string, IIdentifiable> identityLookup;

    private void Start()
    {
        TheOnlyGod = this;
        InitIfNeeded();
    }

    private void DoInit()
    {
        this.isPaused = false;
        this.knownCats = Optional<Cat[]>.Empty();
        this.identityLookup = new Dictionary<string, IIdentifiable>();
        this.MyChild = GetComponentInChildren<Savior>();
        this.stats = new Statistics();
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

    public void Reset()
    {
        Unpause();
        this.knownCats = Optional<Cat[]>.Empty();
    }


    private void _UpdateIDLookup()
    {
        IIdentifiable[] allIDs = FindObjectsOfType<MonoBehaviour>().OfType<IIdentifiable>().ToArray();
        _UpdateIDLookup(allIDs);
    }

    private void _UpdateIDLookup(IEnumerable<IIdentifiable> with)
    {
        this.identityLookup.Clear();
        foreach (IIdentifiable ident in with)
        {
            this.identityLookup.Add(ident.getID(), ident);
        }
    }



    private Optional<IIdentifiable> _GetByID(string id)
    {
        if(this.identityLookup.ContainsKey(id))
        {
            return Optional<IIdentifiable>.Of(this.identityLookup[id]);
        }
        else
        {
            return Optional<IIdentifiable>.Empty();
        }
    }

    public static Optional<IIdentifiable> GetByID(string id)
    {
        FindGod();
        return TheOnlyGod._GetByID(id);
    }

    public static void UpdateIDLookup()
    {
        FindGod();
        TheOnlyGod._UpdateIDLookup();
    }

    public static void UpdateIDLookup(IEnumerable<IIdentifiable> with)
    {
        FindGod();
        TheOnlyGod._UpdateIDLookup(with);
    }

    public static Savior GetSavior()
    {
        FindGod();
        return TheOnlyGod.MyChild;
    }
    
    public static Statistics GetStats()
    {
        FindGod();
        return TheOnlyGod.stats;
    }

}
