using System;
using Godot;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Components;

public partial class HealthComponent : Node2D
{
    [Export] public float Max;
    [Export] public Attack.AttackElement Resistance = Attack.AttackElement.Absolute;
    [Export] public float ResistanceMultiplier = 1.0f;
    [Export] public Attack.AttackElement Weakness = Attack.AttackElement.Absolute;
    [Export] public float WeaknessMultiplier = 1.0f;

    [Export] public bool IsImmune { get; set; }
    public float Health
    {
        get => _health;
        private set
        {
            var healthChange = new HealthChange
            {
                Before = _health,
                After = value
            };
            _health = value;
            if (_health <= 0)
            {
                healthChange.Free();
                EmitSignal(nameof(HealthDepleted));
                return;
            }
            EmitSignal(nameof(HealthChanged), healthChange);
        }
    }
    private float _health;
    
    [Signal] public delegate void HealthChangedEventHandler(HealthChange healthChange);
    [Signal] public delegate void HealthDepletedEventHandler();

    public override void _Ready()
    {
        _health = Max;
    }

    public void TakeDamage(Attack attack)
    {
        if (IsImmune) return;
        
        if (Resistance == attack.Element)
        {
            Health -= attack.Damage * ResistanceMultiplier;
        }
        else if (Weakness == attack.Element)
        {
            Health -= attack.Damage * WeaknessMultiplier;
        }
        else
        {
            Health -= attack.Damage;
        }
        
        attack.Free(); // memory leak prevention
    }

    public void Heal(float amount)
    {
        if (Math.Abs(Health - Max) < 0.001) return;
        var result = Health + amount;
        Health = result > Max ? Max : result;
    }
}