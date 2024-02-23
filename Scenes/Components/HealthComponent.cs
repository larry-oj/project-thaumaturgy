using Godot;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Components;

public partial class HealthComponent : Node2D
{
    [Export] public float maxHealth;
    [Export] public Attack.AttackElement resistance = Attack.AttackElement.Absolute;
    [Export] public float ResistanceMultiplier = 1.0f;
    [Export] public Attack.AttackElement weakness = Attack.AttackElement.Absolute;
    [Export] public float WeaknessMultiplier = 1.0f;

    public float Health => _health;
    private float _health;
    
    [Signal] public delegate void HealthChangedEventHandler(HealthChange healthChange);
    [Signal] public delegate void HealthDepletedEventHandler();

    public override void _Ready()
    {
        _health = maxHealth;
    }

    public void TakeDamage(Attack attack)
    {
        var healthChange = new HealthChange();
        healthChange.BeforeHealth = Health;
        
        if (resistance == attack.Element)
        {
            _health -= attack.Damage * ResistanceMultiplier;
        }
        else if (weakness == attack.Element)
        {
            _health -= attack.Damage * WeaknessMultiplier;
        }
        else
        {
            _health -= attack.Damage;
        }

        healthChange.AfterHealth = Health;

        EmitSignal(nameof(HealthChanged), healthChange);
        if (_health <= 0)
            EmitSignal(nameof(HealthDepleted));
    }
}