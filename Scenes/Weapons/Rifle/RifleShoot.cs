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
    
    private Rifle _rifle;
    private Character _character;
    
    public override void _Ready()
    {
        _rifle = GetNode<Rifle>(Options.PathOptions.WeaponStateToWeapon);
        _timer.Timeout += OnTimerTimeout;
        
        _character = GetNode<Character>(Options.PathOptions.WeaponStateToCharacter);
    }

    public override void Enter()
    {
        const string animationName = "shooting";
        var attackSpeed = _animationPlayer.GetAnimation(animationName).Length / _rifle.StatsComponent.FireRate;
        _animationPlayer.Play(animationName, customSpeed: _rifle.StatsComponent.FireRate);
        _timer.Start(attackSpeed);

        var bullet = _rifle.BulletResource.Instantiate(_projectileSpawner, _rifle.StatsComponent.CreateAttack(_character));
        GetNode(Options.PathOptions.Level).AddChild(bullet);
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