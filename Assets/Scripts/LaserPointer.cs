﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LaserPointer : InventoryItem
{
    public bool IsOn;
    public LineRenderer LaserRenderer;
    public float MaxIntensity;
    List<Vector2> points = new List<Vector2>();
    public LightObject Pointer;

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
    }

    public override void Using(Vector2 location)
    {

        points.Clear();
        if (this.IsOn)
        {
            List<Vector2> emissionResults = Pointer.EmitLightTowards(LightType.KITTY_LASER, MaxIntensity, location);
            points.AddRange(emissionResults);
            
        }
        else
        {
            points.Add(this.transform.position.ToVector2());
            points.Add(this.transform.position.ToVector2());
        }
    }

    protected override void OnEquip()
    {
        
    }

    protected override void OnUnequip()
    {
        
    }
}
