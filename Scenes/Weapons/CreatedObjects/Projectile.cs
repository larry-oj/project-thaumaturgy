using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.CreatedObjects;

public partial class Projectile : Area2D
{
    public Attack Attack;
    public Character AttackOwner;
    internal VelocityComponent _velocityComponent;
    internal RayCast2D _rayCast2D;
    
    public override void _Ready()
    {
        _velocityComponent = GetNode<VelocityComponent>("VelocityComponent");
        _rayCast2D = GetNodeOrNull<RayCast2D>("RayCast2D");
    }
}