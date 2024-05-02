using Godot;

namespace projectthaumaturgy.Extensions;

public static class Extentions
{
    public static Vector2 Copy(this Vector2 vector)
        => new Vector2(vector.X, vector.Y);
    
    public static Vector2I Copy(this Vector2I vector)
        => new Vector2I(vector.X, vector.Y);
}