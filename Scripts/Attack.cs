using System;
using Godot;
using projectthaumaturgy.Scenes.Characters;

namespace projectthaumaturgy.Scripts;

public partial class Attack : GodotObject
{
    public enum AttackType
    {
        Melee,
        Ranged,
        Area,
    }
    
    public enum AttackElement
    {
        Fire,
        Water,
        Earth,
        Air,
        Absolute, // true damage equivalent
        None,
    }
    
    public enum AttackInfusion
    {
        None,
        Saturated,
        Bold,
        Ghastly
    }

    public float Damage { get; set; }
    public AttackType Type { get; set; }
    public AttackElement Element { get; set; }
    public AttackInfusion Infusion { get; set; }
    public float StatusChance { get; set; } = 0f;
    
#nullable enable
    public Status? StatusEffect { get; private set; }
#nullable disable
    
    public Character Owner { get; set; }

    public Attack() {}
    public Attack(float damage, 
        AttackType type, AttackElement element, 
        AttackInfusion infusion = AttackInfusion.None,
        Character owner = default,
        Status statusEffect = null)
    {
        Damage = damage;
        Type = type;
        Element = element;
        Infusion = infusion;
        Owner = owner;
        StatusEffect = statusEffect;
    }
    
    public static Color GetElementColor(AttackElement element)
    {
        return element switch
        {
            AttackElement.Fire => Colors.Red,
            AttackElement.Water => Colors.Blue,
            AttackElement.Earth => Colors.ForestGreen,
            AttackElement.Air => Colors.LightBlue,
            _ => Colors.DarkSlateGray,
        };
    }
    
    public Attack Copy()
    {
        return new Attack(Damage, Type, Element, Infusion, Owner, StatusEffect);
    }
}