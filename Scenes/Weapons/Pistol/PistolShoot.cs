using Godot;
using projectthaumaturgy.Resources.Weapons.CreatedObjects;
using projectthaumaturgy.Scenes.Characters;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scenes.Weapons.CreatedObjects;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.Pistol;

public partial class PistolShoot : State
{
    [Export] private PistolIdle _pistolIdle;
    [Export] private Timer _timer;
    [Export] private Marker2D _projectileSpawner;
    [Export] private AudioStreamPlayer2D _audioStreamPlayer;
    
    private Pistol _pistol;

    public override void _Ready()
    {
        _pistol = GetNode<Pistol>(Options.PathOptions.WeaponStateToWeapon);
        _timer.Timeout += OnTimerTimeout;
    }
    
    public override void Enter()
    {
        const string animationName = "shooting";
        var attackSpeed = _animationPlayer.GetAnimation(animationName).Length / _pistol.StatsComponent.FireRate;
        _animationPlayer.Play(animationName, customSpeed: _pistol.StatsComponent.FireRate);
        _timer.Start(attackSpeed);
        
        var rotation = Vector2.Right.Rotated(_projectileSpawner.GlobalRotation);
        var attack = _pistol.StatsComponent.CreateAttack(_pistol.Character, rotation);
        var bullet = _pistol.BulletResource.Instantiate(_projectileSpawner, attack);
        GetNode(Options.PathOptions.Level).AddChild(bullet);
        
        _audioStreamPlayer.Playing = true;
        
        if (_pistol.Character is Player p)
            p.EmitSignal(Player.SignalName.Attacked);
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