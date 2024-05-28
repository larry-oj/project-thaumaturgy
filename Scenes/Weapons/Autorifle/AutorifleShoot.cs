using Godot;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.Autorifle;

public partial class AutorifleShoot  : State
{
	[Export] private AutorifleIdle _autorifleIdle;
	[Export] private Timer _timer;
	[Export] private Marker2D _projectileSpawner;
	[Export] private AudioStreamPlayer2D _audioStreamPlayer;
    
	private Autorifle _autorifle;

	public override void _Ready()
	{
		_autorifle = GetNode<Autorifle>(Options.PathOptions.WeaponStateToWeapon);
		_timer.Timeout += OnTimerTimeout;
	}
    
	public override void Enter()
	{
		var animationName = Options.AnimationNames.Shoot;
		var attackSpeed = _animationPlayer.GetAnimation(animationName).Length / _autorifle.StatsComponent.FireRate;
		_animationPlayer.Play(animationName, customSpeed: _autorifle.StatsComponent.FireRate);
		_timer.Start(attackSpeed);
        
		var rotationRads = _projectileSpawner.GlobalRotation;
		var spreadAngle = Options.Balance.AutorifleSpreadAngle;
		var offsetRads = (float)Mathf.DegToRad(GD.RandRange(-spreadAngle, spreadAngle));
		var rotation = rotationRads + offsetRads;
		
		var attack = _autorifle.StatsComponent.CreateAttack(_autorifle.Character, Vector2.Right.Rotated(rotation));
		var bullet = _autorifle.BulletResource.Instantiate(_projectileSpawner, attack, rotation);
		GetNode(Options.PathOptions.Level).AddChild(bullet);
		
		_audioStreamPlayer.Playing = true;
	}
    
	public override void Exit()
	{
		_animationPlayer.Stop();
	}
    
	private void OnTimerTimeout()
	{
		EmitSignal(nameof(Transitioned), this, _autorifleIdle);
	}
}