using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.Sniper;

public partial class SniperShoot  : State
{
	[Export] private SniperIdle _rifleIdle;
	[Export] private Timer _timer;
	[Export] private Marker2D _projectileSpawner;
    
	private Sniper _rifle;
    
	public override void _Ready()
	{
		_rifle = GetNode<Sniper>(Options.PathOptions.WeaponStateToWeapon);
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
		bullet.GetNode<VelocityComponent>("VelocityComponent").MaxSpeed = 700;
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