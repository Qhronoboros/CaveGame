
using UnityEngine;

public static class VectorHelper
{
    // Converts a Vector3 to a Vector2 with the values x and z
    public static Vector2 Vector3ToVector2(Vector3 vector3) => new Vector2(vector3.x, vector3.z);

    // Converts a Vector2 to a Vector3 with the values x, 0.0f, y
    public static Vector2 Vector2ToVector3(Vector2 vector2) => new Vector3(vector2.x, 0.0f, vector2.y);
}