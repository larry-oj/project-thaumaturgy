﻿using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scripts;
using Character = projectthaumaturgy.Scenes.Characters.Character;

namespace projectthaumaturgy.Scenes.Weapons.CreatedObjects;

public partial class Projectile : Area2D
{
    public Attack Attack;
    internal VelocityComponent _velocityComponent;
    [Export] internal SpritesComponent _spritesComponent;
    internal RayCast2D _rayCast2D;

    public override void _Ready()
    {
        _velocityComponent = GetNode<VelocityComponent>("VelocityComponent");
        _rayCast2D = GetNodeOrNull<RayCast2D>("RayCast2D");
    }

    public virtual void SetAttack(Attack attack)
    {
        this.Attack = attack;
    }
}