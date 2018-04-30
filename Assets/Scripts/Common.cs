using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A set of core utilities shred by many components
/// </summary>
public static class Common {

    /// <summary>
    /// How long objects should wait when they want to erase themselves
    /// </summary>
    public const float DestroyTime = 0.001f;

    /// <summary>
    /// Turn this transform such that it's right vector faces a given target in 2D world space
    /// </summary>
    /// <param name="t">A transform</param>
    /// <param name="target">A 2D world space point to face</param>
    public static void TurnToFace(this Transform t, Vector2 target, bool towards = true)
    {
        // Just call the Vector3 version
        t.TurnToFace(target.ToVector3(), towards);
    }

    /// <summary>
    /// Turn this transform such that it's right vector faces a given 3D target in 2D world space
    /// </summary>
    /// <param name="t">A transform</param>
    /// <param name="target">A 2D world space point to face as a 3D point (ignores z)</param>
    public static void TurnToFace(this Transform t, Vector3 target, bool towards = true)
    {
        // Might want to optimize this more, we're converting a lot between
        // the same vectors, and some of these unity would already do for us

        // To get from t.position to target, we need to go in the direction
        // vector (target - t.position)
        Vector3 direction = target - t.position.To2DVector3();

        // We don't care about z-movement - any that exists is an error
        // 2 dimensional movement can be classified exclusively as a magnitude 
        // and a z-angle

        // The angle = ArcTan( y / x), we use Atan2 because Arctan has 
        // a range of 0 < theta < PI, and we want 0 < theta < 2 PI (full circle)
        float desiredAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if(!towards)
        {
            desiredAngle *= -1;
        }

        // Set our rotation to a z-rotation of the desired angle
        t.rotation = Quaternion.AngleAxis(desiredAngle, Vector3.forward);
    }

    /// <summary>
    /// Turn this transform such that it's right vector points in a given direction in 2D world space.
    /// </summary>
    /// <param name="t">This transform</param>
    /// <param name="direction">A target direction to face.</param>
    public static void TurnToFaceDirection(this Transform t, Vector2 direction)
    {
        // Convert our current transform's position into a Vector2
        Vector2 currentPosition = t.position.ToVector2();

        // Add the direction as if it were a velocity vector
        // and get a new position - that's where we have to face.
        // for our forward vector to point in that direction.
        t.TurnToFace(currentPosition + direction);
    }

    /// <summary>
    /// Gets the 2D world position that the mouse is pointing to.
    /// </summary>
    /// <returns>The 2D world position beneath the user's mouse</returns>
    public static Vector2 GetMousePosition()
    {
        // Input.mousePosition is a Screen position, we want the 2D world position.
        //Vector3 mousePos = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0.0f);
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Convert it to a vector2
        return target.ToVector2();
    }

    /// <summary>
    /// Gets the 2D world position as a 3D point that the mouse is pointing to.
    /// </summary>
    /// <returns>The 2D world position beneath the user's mouse (z = 0)</returns>
    public static Vector3 GetMousePosition3D()
    {
        // Get the mouse position as a vector2
        Vector2 target = GetMousePosition();

        // And convert to a vector3
        return target.ToVector3();
    }

    /// <summary>
    /// Converts a 3D world point into its corresponding 2D world point by eliminating the z coordinate.
    /// </summary>
    /// <param name="v">A Vector3</param>
    /// <returns>A Vector2 with the same 2D coordinates, but ignoring the z</returns>
    public static Vector2 ToVector2(this Vector3 v)
    {
        // Pretty basic, take just the x,y
        return new Vector2(v.x, v.y);
    }

    /// <summary>
    /// Converts a 2D world point into its corresponding 3D position (z=0)
    /// </summary>
    /// <param name="v">A Vector2</param>
    /// <returns>A Vector3 with </returns>
    public static Vector3 ToVector3(this Vector2 v)
    {
        // Just take x,y and set z=0
        return new Vector3(v.x, v.y, 0);
    }

    /// <summary>
    /// Removes the z-component of a vector, making it a point in 2D space
    /// </summary>
    /// <param name="v">A Vector3</param>
    /// <returns>An equivalent Vector3 with z=0</returns>
    public static Vector3 To2DVector3(this Vector3 v)
    {
        // Just take x,y and set z=0
        return new Vector3(v.x, v.y, 0);
    }

    public static float ToWorldAngleRadians(this Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x);
    }

    public static Vector2 ToWorldVector(this float WorldAngle)
    {
        return new Vector2(Mathf.Cos(WorldAngle), Mathf.Sin(WorldAngle)).normalized;
    }

    public static string ToSavableString(this Transform t)
    {
        string stringRep = string.Format("{0},{1},{2}", t.position.x, t.position.y, t.rotation.eulerAngles.z, t.localScale.x, t.localScale.y);
        return stringRep;
    }

    public static void UpdateToSaved(this Transform t, string savedTransform)
    {
        string[] split = savedTransform.Split(',');
        float x = float.Parse(split[0]);
        float y = float.Parse(split[1]);
        float rot = float.Parse(split[2]);
        t.position = new Vector2(x, y).ToVector3();
        t.rotation = Quaternion.Euler(0, 0, rot);

        try
        {
            float scaleX = float.Parse(split[3]);
            float scaleY = float.Parse(split[4]);
            t.localScale = new Vector3(scaleX, scaleY, 1.0f);
        }
        catch(System.IndexOutOfRangeException)
        {
            Debug.Log("Using compatibility mode for transform loading..." + t.name + " may not be at the right scale.");
        }
    }

    public static void GenerateIDIfNeeded(this IIdentifiable ident)
    {
        if(ident.getID().Length == 0)
        {
            // Empty string, reassign
            ident.setID(System.Guid.NewGuid().ToString());
        }
    }
}
