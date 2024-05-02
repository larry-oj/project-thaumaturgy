using Godot;
using projectthaumaturgy.Scenes.Components.StateMachine;

namespace projectthaumaturgy.Scenes.Weapons.Broadsword;

public partial class BroadswordAttack : State
{
	[Export] private BroadswordIdle _broadswordIdle;
	[Export] private Timer _timer;
	[Export] private HurtboxComponent _hurtboxComponent;
	[Export] private Broadsword _broadsword;
	
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
		_hurtboxComponent.TryGetOverlappingAreas();
	}
}