using Godot;
using projectthaumaturgy.Resources.Weapons.CreatedObjects;
using projectthaumaturgy.Scenes.Characters;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scenes.Weapons.CreatedObjects;
using projectthaumaturgy.Scenes.Weapons.Pistol;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.Rifle;

public partial class RifleShoot : State
{
    [Export] private RifleIdle _rifleIdle;
    [Export] private Timer _timer;
    [Export] private Marker2D _projectileSpawner;
    
    private BulletResource _bulletResource;
    private Rifle _rifle;
    private Character _character;
    
    public override void _Ready()
    {
        _bulletResource = GetNode<Rifle>("../..").BulletResource;
        _timer.Timeout += OnTimerTimeout;
        _rifle = GetNode<Rifle>("../..");
        _character = GetNode<Character>("../../../.."); // bruhhh
    }

    public override void Enter()
    {
        const string animationName = "shooting";
        var attackSpeed = _animationPlayer.GetAnimation(animationName).Length / _rifle.WeaponStatsComponent.FireRate;
        _animationPlayer.Play(animationName, customSpeed: _rifle.WeaponStatsComponent.FireRate);
        _timer.Start(attackSpeed);

        var bullet = _bulletResource.Instantiate(_projectileSpawner, _rifle.WeaponStatsComponent.CreateAttack(_character));
        GetNode("/root/Game/World").AddChild(bullet);
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