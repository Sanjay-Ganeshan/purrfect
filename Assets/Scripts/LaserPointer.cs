using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LaserPointer : InventoryItem, IInteractable, IPersistantObject
{
    public bool IsOn;
    public LineRenderer LaserRenderer;
    public float MaxIntensity;
    List<Vector2> points = new List<Vector2>();
    public LaserDot TargetDot;

    public void Start()
    {
        IsOn = false;
    }

    public void Update()
    {
        Vector3[] laserPos = GetLaserPositions();
        this.LaserRenderer.positionCount = laserPos.Length;
        this.LaserRenderer.SetPositions(laserPos);
    }

    Vector3[] GetLaserPositions()
    {
        Vector3[] pos = new Vector3[points.Count];
        for(int i = 0; i < points.Count; i++)
        {
            pos[i] = points[i].ToVector3();
        }
        return pos;
    }

    void ToggleOn ()
    {
        this.IsOn = !this.IsOn;
    }

    public override void BeginUse(Vector2 location)
    {
        this.IsOn = true;
        this.LaserRenderer.enabled = true;
    }

    public override void EndUse(Vector2 location)
    {
        this.IsOn = false;
        this.LaserRenderer.enabled = false;
        points.Clear();
        points.Add(this.transform.position.ToVector2());
        points.Add(this.transform.position.ToVector2());
        TargetDot.gameObject.SetActive(false); // Disable the target dot
    }

    public override void Using(Vector2 location)
    {
        points.Clear();
        //if (this.IsOn)
        //{
        List<Vector2> emissionResults = this.transform.EmitLightTowards(LightType.KITTY_LASER, MaxIntensity, location);
        points.AddRange(emissionResults);
        Vector2 endpoint = points.Last();
        TargetDot.transform.position = endpoint.ToVector3();
        TargetDot.gameObject.SetActive(true); // enable the target dot
        //}
    }

    protected override void OnEquip()
    {
        
    }

    protected override void OnUnequip()
    {
        
    }

    public bool Interact(Player p)
    {
        p.Bag.AddToInventory(this);
        return true;
    }

    public bool Interact(Cat c)
    {
        return false;
    }

    void IPersistantObject.Load(Dictionary<string, string> saveData)
    {
        
    }

    Dictionary<string, string> IPersistantObject.Save()
    {
        Dictionary<string, string> r = new Dictionary<string, string>();
        return r;
    }

    void IPersistantObject.Unload()
    {
        God.Kill(this.gameObject);
    }

    PersistanceType IPersistantObject.GetPType()
    {
        return PersistanceType.LASER_POINTER;
    }

    IEnumerable<string> IPersistantObject.PersistThroughLoad()
    {
        return new string[]{};
    }

    MonoBehaviour IPersistantObject.GetMono()
    {
        return this;
    }

    public void PostLoad()
    {

    }
}
