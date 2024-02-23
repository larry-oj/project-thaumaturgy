using Godot;

namespace projectthaumaturgy.Scenes.Components;

public partial class VelocityComponent : Node2D
{
    [Export] public float MaxSpeed { get; set; }
    [Export] private CharacterBody2D _body;
    public float Velocity { get; set; }

    public void Move(Vector2 direction)
    {
        _body.Velocity = direction.Normalized() * MaxSpeed;
        _body.MoveAndSlide();
    }
}