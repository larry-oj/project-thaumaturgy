using System;
using Godot;
using projectthaumaturgy.Scenes.Weapons.CreatedObjects;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Resources.Weapons.CreatedObjects;

public partial class BulletResource : Resource
{
    [Export] public PackedScene Scene { get; private set; }
    
    public Bullet Instantiate(Marker2D projectileSpawner, Attack attack, float rotation = -500f)
    {
        var bullet = Scene.Instantiate() as Bullet;
        bullet!.Position = new Vector2(projectileSpawner.GlobalPosition.X, projectileSpawner.GlobalPosition.Y);
        bullet!.Rotation = Math.Abs(rotation - (-500f)) < 0.01 ? projectileSpawner.GlobalRotation : rotation;
        bullet!.SetAttack(attack);
        return bullet;
    }
}