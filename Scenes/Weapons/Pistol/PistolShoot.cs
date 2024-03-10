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
    private Character _character;

    public override void _Ready()
    {
        _pistol = GetNode<Pistol>(Options.PathOptions.WeaponStateToWeapon);
        _bulletResource = _pistol.BulletResource;
        _timer.Timeout += OnTimerTimeout;
        _character = GetNode<Character>(Options.PathOptions.WeaponStateToCharacter);
    }
    
    public override void Enter()
    {
        const string animationName = "shooting";
        _animationPlayer.Play(animationName, customSpeed: _pistol.StatsComponent.FireRate);
        _timer.Start(_animationPlayer.GetAnimation(animationName).Length / _pistol.StatsComponent.FireRate);
        
        var bullet = _bulletResource.Instantiate(_projectileSpawner, _pistol.StatsComponent.CreateAttack(_character));
        GetNode("/root/Game/World").AddChild(bullet);
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