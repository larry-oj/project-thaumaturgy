using Godot;

namespace projectthaumaturgy.Scripts;

public partial class Attack
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
    }
    
    public enum AttackInfusion
    {
        None,
        Fiery,
        Icy,
        Earthy,
        Windy,
    }

    [Export] public float attackDamage;
    [Export] public AttackType attackType;
    [Export] public AttackElement attackElement;
    [Export] public AttackInfusion attackInfusion = AttackInfusion.None;
}