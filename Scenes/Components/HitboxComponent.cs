using Godot;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Components;

public partial class HitboxComponent : Area2D
{
    [Export]
    private HealthComponent _healthComponent;
    
    public void Damage(Attack attack)
    {
        _healthComponent?.TakeDamage(attack);
    }
}