﻿using Godot;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Components;

public partial class HealthComponent : Node2D
{
    [Export] public float MaxHealth;
    [Export] public Attack.AttackElement Resistance = Attack.AttackElement.Absolute;
    [Export] public float ResistanceMultiplier = 1.0f;
    [Export] public Attack.AttackElement Weakness = Attack.AttackElement.Absolute;
    [Export] public float WeaknessMultiplier = 1.0f;

    public bool IsImmune { get; private set; }
    public float Health
    {
        get => _health;
        private set
        {
            var healthChange = new HealthChange();
            healthChange.BeforeHealth = _health;
            _health = value;
            healthChange.AfterHealth = _health;
            EmitSignal(nameof(HealthChanged), healthChange);
        }
    }
    private float _health;
    
    [Signal] public delegate void HealthChangedEventHandler(HealthChange healthChange);
    [Signal] public delegate void HealthDepletedEventHandler();

    public override void _Ready()
    {
        _health = MaxHealth;
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
        
        if (Health <= 0)
            EmitSignal(nameof(HealthDepleted));
        
        attack.Free(); // memory leak prevention
    }
    
    public void SetImmune(bool immune)
    {
        IsImmune = immune;
    }
}