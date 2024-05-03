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
        Fiery,
        Icy,
        Earthy,
        Windy,
    }

    public float Damage { get; private set; }
    public AttackType Type { get; private set; }
    public AttackElement Element { get; private set; }
    public AttackInfusion Infusion { get; private set; }
    
    public Character Owner { get; private set; }

    public Attack(float damage, 
        AttackType type, AttackElement element, 
        AttackInfusion infusion = AttackInfusion.None,
        Character owner = default)
    {
        Damage = damage;
        Type = type;
        Element = element;
        Infusion = infusion;
        Owner = owner;
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
}