using Godot;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons;

public partial class Weapon : Node2D
{
    [Signal] public delegate void OnAttackEventHandler();
    public Attack WeaponAttack;
    
    public virtual void Attack()
    {
    }
}