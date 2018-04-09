using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LaserPointer : InventoryItem
{
    public bool IsOn;
    public LineRenderer LaserRenderer;
    public float MaxRange;
    List<Vector2> points = new List<Vector2>();

    public void Start()
    {
        IsOn = false;
    }

    public void Update()
    {
        this.LaserRenderer.SetPositions(GetLaserPositions());
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
    }

    public override void EndUse(Vector2 location)
    {
        this.IsOn = false;
    }

    public override void Using(Vector2 location)
    {
        if (this.IsOn)
        {
            points.Clear();
            Vector2 direction = (location - this.transform.position.ToVector2()).normalized;
            Vector2 startPoint = this.transform.position.ToVector2();
            points.Add(startPoint);
            RaycastHit2D results = Physics2D.Raycast(
                startPoint, 
                direction, 
                MaxRange, 
                LayerMask.GetMask("Light"), 
                0.0f);
            if(results.collider == null)
            {
                // Nothing hit
                points.Add(direction * MaxRange + startPoint);
            }
            else
            {
                points.Add(results.point);
            }
        }
        else
        {
            points.Clear();
        }
    }

    protected override void OnEquip()
    {
        
    }

    protected override void OnUnequip()
    {
        
    }
}
