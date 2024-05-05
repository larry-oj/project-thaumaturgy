using Godot;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Components;

public partial class VelocityComponent : Node2D
{
    [Export] public float MaxSpeed { get; set; }
    [Export] private CharacterBody2D _body;
    public float Velocity { get; set; }
    private float _multiplier = 1.0f;

    public void Move(Vector2 direction)
    {
        _body.Velocity = direction.Normalized() * MaxSpeed * _multiplier;
        _body.MoveAndSlide();
    }

    private void OnStatusChanged(bool isCleared, Status change)
    {
        _multiplier = isCleared ? 1.0f : change.Multiplier;
    }
}