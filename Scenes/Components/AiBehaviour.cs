using Godot;

namespace projectthaumaturgy.Scenes.Components;

public partial class AiBehaviour : Node
{
    public virtual Vector2 GetMovementDirection()
    {
        return Vector2.Zero;
    }
    
    public virtual bool IsAttacking()
    {
        return false;
    }
}