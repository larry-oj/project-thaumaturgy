using Godot;
using projectthaumaturgy.Scenes.Weapons.CreatedObjects;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Resources.Weapons.CreatedObjects;

public partial class BulletResource : Resource
{
    [Export] public PackedScene Scene { get; private set; }
    
    public Bullet Instantiate(Marker2D projectileSpawner, Attack attack)
    {
        var bullet = Scene.Instantiate() as Bullet;
        bullet!.Position = new Vector2(projectileSpawner.GlobalPosition.X, projectileSpawner.GlobalPosition.Y);
        bullet!.Rotation = projectileSpawner.GlobalRotation;
        bullet!.SetAttack(attack);
        return bullet;
    }
}