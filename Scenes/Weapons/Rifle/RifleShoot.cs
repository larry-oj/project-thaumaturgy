using Godot;
using projectthaumaturgy.Resources.Weapons.CreatedObjects;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scenes.Weapons.CreatedObjects;
using projectthaumaturgy.Scenes.Weapons.Pistol;

namespace projectthaumaturgy.Scenes.Weapons.Rifle;

public partial class RifleShoot : State
{
    [Export] private RifleIdle _rifleIdle;
    [Export] private Timer _timer;
    [Export] private Marker2D _projectileSpawner;
    private BulletResource _bulletResource;
    private PackedScene _bulletProjectile;
    
    public override void _Ready()
    {
        _bulletResource = GetNode<Rifle>("../..").BulletResource;
        _bulletProjectile = _bulletResource.Scene;
        _timer.Timeout += OnTimerTimeout;
    }

    public override void Enter()
    {
        const string animationName = "shooting";
        _animationPlayer.Play(animationName);
        _timer.Start(_animationPlayer.GetAnimation(animationName).Length);
        
        var bullet = _bulletProjectile.Instantiate() as Bullet;
        bullet!.Position = new Vector2(_projectileSpawner.GlobalPosition.X, _projectileSpawner.GlobalPosition.Y);
        bullet!.Rotation = _projectileSpawner.GlobalRotation;
        GetNode("/root/World").AddChild(bullet);
    }

    public override void Exit()
    {
        _animationPlayer.Stop();
    }
    
    private void OnTimerTimeout()
    {
        EmitSignal(nameof(Transitioned), this, _rifleIdle);
    }
}