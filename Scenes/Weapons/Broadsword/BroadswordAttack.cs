using Godot;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.Broadsword;

public partial class BroadswordAttack : State
{
	[Export] private BroadswordIdle _broadswordIdle;
	[Export] private Timer _timer;
	[Export] private HurtboxComponent _hurtboxComponent;
	[Export] private Broadsword _broadsword;
	[Export] private Marker2D _projectileSpawner;
	
	private bool _isFirstAttack = true;
	
	public override void _Ready()
	{
		_timer.Timeout += OnTimerTimeout;
	}
	
	public override void Enter()
	{
		const string animationName = "attacking";
		_timer.Start(1 / _broadsword.StatsComponent.FireRate);
		
		if (_isFirstAttack)
		{
			_animationPlayer.Play(animationName + "_from_top", customSpeed: _broadsword.StatsComponent.FireRate);
		}
		else
		{
			_animationPlayer.Play(animationName + "_from_bottom", customSpeed: _broadsword.StatsComponent.FireRate);
		}

		_isFirstAttack = !_isFirstAttack;
		_broadsword.IsInAttackAnimation = true;
	}
	
	public override void Exit()
	{
		_animationPlayer.Stop(true);
		_broadsword.IsInAttackAnimation = false;
	}

	private void OnTimerTimeout()
	{
		EmitSignal(nameof(Transitioned), this, _broadswordIdle);
	}

	public void OnAnimationAttack()
	{
		var res = _hurtboxComponent.TryGetOverlappingAreas();
		if (!res && _broadsword.StatsComponent.Infusion == Attack.AttackInfusion.Ghastly)
		{
			var rotation = Vector2.Right.Rotated(_projectileSpawner.GlobalRotation);
			var attack = _broadsword.StatsComponent.CreateAttack(_broadsword.Character, rotation);
			var bullet = _broadsword.BulletResource.Instantiate(_projectileSpawner, attack);
			GetNode(Options.PathOptions.Level).AddChild(bullet);
		}
	}
}