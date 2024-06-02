using Godot;
using projectthaumaturgy.Resources.Weapons.CreatedObjects;
using projectthaumaturgy.Scenes.Characters;
using projectthaumaturgy.Scenes.Characters.Player;
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
    [Export] private AudioStreamPlayer2D _audioStreamPlayer;
    
    private Rifle _rifle;
    
    public override void _Ready()
    {
        _rifle = GetNode<Rifle>(Options.PathOptions.WeaponStateToWeapon);
        _timer.Timeout += OnTimerTimeout;
    }

    public override void Enter()
    {
        const string animationName = "shooting";
        var attackSpeed = _animationPlayer.GetAnimation(animationName).Length / _rifle.StatsComponent.FireRate;
        _animationPlayer.Play(animationName, customSpeed: _rifle.StatsComponent.FireRate);
        _timer.Start(attackSpeed);

        var rotation = Vector2.Right.Rotated(_projectileSpawner.GlobalRotation);
        var attack = _rifle.StatsComponent.CreateAttack(_rifle.Character, rotation);
        var bullet = _rifle.BulletResource.Instantiate(_projectileSpawner, attack);
        GetNode(Options.PathOptions.Level).AddChild(bullet);
        
        _audioStreamPlayer.Playing = true;
        
        if (_rifle.Character is Player p)
            p.EmitSignal(Player.SignalName.Attacked);
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