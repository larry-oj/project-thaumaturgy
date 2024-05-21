using System;
using Godot;
using projectthaumaturgy.Scenes.Characters;
using projectthaumaturgy.Scripts;
using Vector2 = Godot.Vector2;

namespace projectthaumaturgy.Scenes.Components;

public partial class WeaponStatsComponent : Node
{
    [ExportCategory("Weapon Stats")]
    [Export] public float Damage { get; set; } = 0;
    [Export] public float FireRate { get; set; } = 1;
    [Export] public float ManaCost { get; set; } = 1;
    [Export] public float StatusChance { get; set; } = 0f;
    [Export] public Attack.AttackType Type { get; set; } = Attack.AttackType.Melee;
    [Export] public Attack.AttackElement Element { get; set; } = Attack.AttackElement.None;
    [Export] public Attack.AttackInfusion Infusion { get; set; } = Attack.AttackInfusion.None;
    
    [ExportGroup("Balance")]
    [Export] public float DamageStep { get; set; } = 1;
    [Export] public int DamageCostIncrease { get; set; } = 1;
    [Export] public int DamageBaseCost { get; set; } = 5;
    public int DamageUpgradeCount { get; private set; } = 0;
    public int DamageUpgradeCost => DamageBaseCost + DamageCostIncrease * DamageUpgradeCount;
    
    [Export] public float FireRateStep { get; set; } = 0.25f;
    [Export] public int FireRateCostIncrease { get; set; } = 1;
    [Export] public int FireRateBaseCost { get; set; } = 5;
    public int FireRateUpgradeCount { get; private set; } = 0;
    public int FireRateUpgradeCost => FireRateBaseCost + FireRateCostIncrease * FireRateUpgradeCount;

    [Signal] public delegate void UpdatedEventHandler();

    public Attack CreateAttack(Character owner, Vector2 direction = default)
    {
        Status status = default;
        if (GD.Randf() < (Element == Attack.AttackElement.Air ? 1 : StatusChance))
        {
            status = new Status();
            switch (this.Element)
            {
                case Attack.AttackElement.Fire:
                    status.Type = Status.StatusType.Burning;
                    status.TickPeriod = Options.Balance.StatusTypes.BurningTickPeriod;
                    status.TicksAmount = Options.Balance.StatusTypes.BurningTicksAmount;
                    status.Damage = Options.Balance.StatusTypes.BurningDamage;
                    break;
                
                case Attack.AttackElement.Water:
                    status.Type = Status.StatusType.Freezing;
                    status.TickPeriod = Options.Balance.StatusTypes.FreezingTickPeriod;
                    status.TicksAmount = Options.Balance.StatusTypes.FreezingTicksAmount;
                    status.Damage = Options.Balance.StatusTypes.FreezingDamage;
                    status.Multiplier = Options.Balance.StatusTypes.FreezingMultiplier;
                    break;
                
                case Attack.AttackElement.Earth:
                    status.Type = Status.StatusType.Stunned;
                    status.TickPeriod = Options.Balance.StatusTypes.StunnedTickPeriod;
                    status.TicksAmount = Options.Balance.StatusTypes.StunnedTicksAmount;
                    status.Damage = Options.Balance.StatusTypes.StunnedDamage;
                    break;
                
                case Attack.AttackElement.Air:
                    status.Type = Status.StatusType.KnockedBack;
                    status.Multiplier = Options.Balance.StatusTypes.KnockedBackForce;
                    status.Direction = direction;
                    break;
                
                default:
                case Attack.AttackElement.Absolute:
                case Attack.AttackElement.None:
                    status.Free();
                    status = default;
                    break;
            }
        }
        
        return new Attack(Damage, Type, Element, Infusion, owner, status);
    }

    public WeaponStatsComponent SetDamage(float damage)
    {
        Damage = damage;
        return this;
    }
    
    public WeaponStatsComponent IncrementDamage(float damage)
    {
        Damage += damage;
        DamageUpgradeCount++;
        return this;
    }
    
    public WeaponStatsComponent SetFireRate(float fireRate)
    {
        FireRate = fireRate;
        return this;
    }
    
    public WeaponStatsComponent IncrementFireRate(float fireRate)
    {
        FireRate += fireRate;
        FireRateUpgradeCount++;
        return this;
    }
    
    public WeaponStatsComponent SetType(Attack.AttackType type)
    {
        Type = type;
        return this;
    }
    
    public WeaponStatsComponent SetElement(Attack.AttackElement element)
    {
        Element = element;
        EmitSignal(nameof(Updated));
        return this;
    }
    
    public WeaponStatsComponent SetInfusion(Attack.AttackInfusion infusion)
    {
        Infusion = infusion;
        return this;
    }
}