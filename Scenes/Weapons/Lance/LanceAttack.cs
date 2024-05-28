using Godot;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scenes.Weapons.Broadsword;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.Lance;

public partial class LanceAttack  : State
{
	[Export] private LanceIdle _lanceIdle;
	[Export] private Timer _timer;
	[Export] private HurtboxComponent _hurtboxComponent;
	[Export] private Lance _lance;
	[Export] private Marker2D _projectileSpawner;
	[Export] private AudioStreamPlayer2D _audioStreamPlayer;
	
	public override void _Ready()
	{
		_timer.Timeout += OnTimerTimeout;
	}
	
	public override void Enter()
	{
		const string animationName = "attacking";
		_timer.Start(1 / _lance.StatsComponent.FireRate);

		_animationPlayer.Play(animationName);
		
		_lance.IsInAttackAnimation = true;
		_audioStreamPlayer.Playing = true;
	}
	
	public override void Exit()
	{
		_animationPlayer.Stop(true);
		_lance.IsInAttackAnimation = false;
	}

	private void OnTimerTimeout()
	{
		EmitSignal(nameof(Transitioned), this, _lanceIdle);
	}

	public void OnAnimationAttack()
	{
		var res = _hurtboxComponent.TryGetOverlappingAreas();
		if (!res && _lance.StatsComponent.Infusion == Attack.AttackInfusion.Ghastly)
		{
			var rotation = Vector2.Right.Rotated(_projectileSpawner.GlobalRotation);
			var attack = _lance.StatsComponent.CreateAttack(_lance.Character, rotation);
			var bullet = _lance.BulletResource.Instantiate(_projectileSpawner, attack);
			GetNode(Options.PathOptions.Level).AddChild(bullet);
		}
	}
}