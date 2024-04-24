using Godot;
using projectthaumaturgy.Scenes.Characters;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Components;

public partial class WeaponStatsComponent : Node
{
    [Export] public float Damage { get; set; } = 0;
    [Export] public float FireRate { get; set; } = 1;
    [Export] public Attack.AttackType Type { get; set; } = Attack.AttackType.Melee;
    [Export] public Attack.AttackElement Element { get; set; } = Attack.AttackElement.None;
    [Export] public Attack.AttackInfusion Infusion { get; set; } = Attack.AttackInfusion.None;

    [Signal] public delegate void UpdatedEventHandler();

    public Attack CreateAttack(Character owner)
    {
        return new Attack(Damage, Type, Element, Infusion, owner);
    }

    public WeaponStatsComponent SetDamage(float damage)
    {
        Damage = damage;
        return this;
    }
    
    public WeaponStatsComponent IncrementDamage(float damage)
    {
        Damage += damage;
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