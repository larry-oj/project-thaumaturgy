using Godot;

namespace projectthaumaturgy.Scenes.Weapons;

public partial class Weapon : Node2D
{
    [Signal] public delegate void OnAttackEventHandler();
    
    public virtual void Attack()
    {
    }
}