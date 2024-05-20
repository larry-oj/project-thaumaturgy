using Godot;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Components;

public partial class VelocityComponent : Node2D
{
    [Export] public float MaxSpeed { get; set; }
    [Export] private CharacterBody2D _body;
    public Vector2 Velocity { get; set; }
    public bool IsInKnockback { get; set; }
    private float _friction = 0.9f;
    private float _multiplier = 1.0f;

    public void Move(Vector2 direction, double delta = 0)
    {
        if (IsInKnockback)
        {
            Velocity *= _friction;
            _body.Velocity = Velocity;
            _body.MoveAndSlide();
            if (_body.Velocity.Length() < 1.0f) IsInKnockback = false;
            return;
        }
        _body.Velocity = direction.Normalized() * MaxSpeed * _multiplier;
        _body.MoveAndSlide();
    }
    
    public void Knockback(Vector2 direction, float force)
    {
        IsInKnockback = true;
        this.Velocity = direction.Normalized() * force;
    }

    private void OnStatusChanged(bool isCleared, Status change)
    {
        _multiplier = isCleared ? 1.0f : change.Multiplier;
    }
}