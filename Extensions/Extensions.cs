using Godot;

namespace projectthaumaturgy.Extensions;

public static class Extentions
{
    public static Vector2 Copy(this Vector2 vector)
        => new Vector2(vector.X, vector.Y);
}