using Godot;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scenes.Weapons.CreatedObjects;

namespace projectthaumaturgy.Scenes.Weapons.Pistol;

public partial class PistolShoot : State
{
    [Export] private PistolIdle _pistolIdle;
    [Export] private Timer _timer;
    [Export] private Node2D _projectileSpawner;
    
    private PackedScene _projectile;

    public override void _Ready()
    {
        _projectile = ResourceLoader.Load("res://Scenes/Weapons/CreatedObjects/bullet.tscn") as PackedScene;
    }
    
    public override void Enter()
    {
        const string animationName = "shooting";
        _animationPlayer.Play(animationName);
        _timer.Start(_animationPlayer.GetAnimation(animationName).Length);
        
        var bullet = _projectile.Instantiate() as Bullet;
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
        EmitSignal(nameof(Transitioned), this, _pistolIdle);
    }
}