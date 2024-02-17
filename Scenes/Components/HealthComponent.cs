using Godot;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Components;

public partial class HealthComponent : Node2D
{
    [Export] public float maxHealth = 100;
    private float _health;
    
    [Signal] public delegate void HealthChangedEventHandler();
    [Signal] public delegate void HealthDepletedEventHandler();
    
    public HealthComponent()
    {
        _health = maxHealth;
    }
    
    public void TakeDamage(Attack damage)
    {
        _health -= damage.attackDamage;
        EmitSignal(nameof(HealthChanged));
        
        if (_health <= 0)
        {
            EmitSignal(nameof(HealthDepleted));
        }
    }
}