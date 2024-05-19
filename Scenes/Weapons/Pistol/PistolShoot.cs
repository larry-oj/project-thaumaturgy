using Godot;
using projectthaumaturgy.Resources.Weapons.CreatedObjects;
using projectthaumaturgy.Scenes.Characters;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scenes.Weapons.CreatedObjects;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.Pistol;

public partial class PistolShoot : State
{
    [Export] private PistolIdle _pistolIdle;
    [Export] private Timer _timer;
    [Export] private Marker2D _projectileSpawner;
    
    private BulletResource _bulletResource;
    private Pistol _pistol;

    public override void _Ready()
    {
        _pistol = GetNode<Pistol>(Options.PathOptions.WeaponStateToWeapon);
        _bulletResource = _pistol.BulletResource;
        _timer.Timeout += OnTimerTimeout;
    }
    
    public override void Enter()
    {
        const string animationName = "shooting";
        _animationPlayer.Play(animationName, customSpeed: _pistol.StatsComponent.FireRate);
        _timer.Start(_animationPlayer.GetAnimation(animationName).Length / _pistol.StatsComponent.FireRate);
        
        var rotation = Vector2.Right.Rotated(_projectileSpawner.GlobalRotation);
        var attack = _pistol.StatsComponent.CreateAttack(_pistol.Character, rotation);
        var bullet = _bulletResource.Instantiate(_projectileSpawner, attack);
        GetNode(Options.PathOptions.Level).AddChild(bullet);
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