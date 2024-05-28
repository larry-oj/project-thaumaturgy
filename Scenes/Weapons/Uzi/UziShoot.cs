using Godot;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scenes.Weapons.Autorifle;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.Uzi;

public partial class UziShoot  : State
{
	[Export] private UziIdle _idle;
	[Export] private Timer _timer;
	[Export] private Marker2D _projectileSpawner;
	[Export] private AudioStreamPlayer2D _audioStreamPlayer;
    
	private Uzi _uzi;

	public override void _Ready()
	{
		_uzi = GetNode<Uzi>(Options.PathOptions.WeaponStateToWeapon);
		_timer.Timeout += OnTimerTimeout;
	}
    
	public override void Enter()
	{
		var animationName = Options.AnimationNames.Shoot;
		var attackSpeed = _animationPlayer.GetAnimation(animationName).Length / _uzi.StatsComponent.FireRate;
		_animationPlayer.Play(animationName, customSpeed: _uzi.StatsComponent.FireRate);
		_timer.Start(attackSpeed);
        
		var rotationRads = _projectileSpawner.GlobalRotation;
		var spreadAngle = Options.Balance.AutorifleSpreadAngle * 1.5;
		var offsetRads = (float)Mathf.DegToRad(GD.RandRange(-spreadAngle, spreadAngle));
		var rotation = rotationRads + offsetRads;
		
		var attack = _uzi.StatsComponent.CreateAttack(_uzi.Character, Vector2.Right.Rotated(rotation));
		var bullet = _uzi.BulletResource.Instantiate(_projectileSpawner, attack, rotation);
		GetNode(Options.PathOptions.Level).AddChild(bullet);
		
		_audioStreamPlayer.Playing = true;
	}
    
	public override void Exit()
	{
		_animationPlayer.Stop();
	}
    
	private void OnTimerTimeout()
	{
		EmitSignal(nameof(Transitioned), this, _idle);
	}
}