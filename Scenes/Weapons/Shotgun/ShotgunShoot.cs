using System;
using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.Shotgun;

public partial class ShotgunShoot  : State
{
	[Export] private ShotgunIdle _shotgunIdle;
	[Export] private Timer _timer;
	[Export] private Marker2D _projectileSpawner;
    
	private Shotgun _autorifle;

	public override void _Ready()
	{
		_autorifle = GetNode<Shotgun>(Options.PathOptions.WeaponStateToWeapon);
		_timer.Timeout += OnTimerTimeout;
	}
    
	public override void Enter()
	{
		var animationName = Options.AnimationNames.Shoot;
		var attackSpeed = _animationPlayer.GetAnimation(animationName).Length / _autorifle.StatsComponent.FireRate;
		_animationPlayer.Play(animationName, customSpeed: _autorifle.StatsComponent.FireRate);
		_timer.Start(attackSpeed);

		var pallets = Options.Balance.ShotgunPallets;
		for (var i = 0; i < pallets; i++)
		{
			var rotationRads = _projectileSpawner.GlobalRotation;
            var spreadAngle = Options.Balance.ShotgunSpreadAngle;
            var offsetRads = (float)Mathf.DegToRad(GD.RandRange(-spreadAngle, spreadAngle));
            var rotation = rotationRads + offsetRads;
            
            var attack = _autorifle.StatsComponent.CreateAttack(_autorifle.Character, Vector2.Right.Rotated(rotation), pallets);
            var bullet = _autorifle.BulletResource.Instantiate(_projectileSpawner, attack, rotation);
            var vc = bullet.GetNode<VelocityComponent>("VelocityComponent");
            vc.MaxSpeed += GD.RandRange(-100, 0);
            GetNode(Options.PathOptions.Level).AddChild(bullet);
		}
	}
    
	public override void Exit()
	{
		_animationPlayer.Stop();
	}
    
	private void OnTimerTimeout()
	{
		EmitSignal(nameof(Transitioned), this, _shotgunIdle);
	}
}