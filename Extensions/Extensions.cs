using Godot;

namespace projectthaumaturgy.Extensions;

public static class Extentions
{
    public static Vector2 Copy(this Vector2 vector)
        => new Vector2(vector.X, vector.Y);
    
    public static Vector2I Copy(this Vector2I vector)
        => new Vector2I(vector.X, vector.Y);

    public static T As<T>(this CanvasItem obj) where T : CanvasItem
        => obj as T;
    
    public static Vector2I ToVector2I(this Rect2 rect)
        => new Vector2I((int)rect.Size.X, (int)rect.Size.Y);
}