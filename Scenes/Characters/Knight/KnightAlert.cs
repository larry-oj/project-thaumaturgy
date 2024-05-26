using System;
using Godot;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Characters.Knight;

public partial class KnightAlert  : AlertBase<Knight>
{
	[ExportGroup("Knight")]
	[Export] private KnightDead _dead;
	[Export] private KnightStunned _stunned;
	
	protected override bool IsWithinBoundary => DistanceToPlayer <= LowerBoundary + 10;

	private bool _isInPosition = false;
	private bool _isAttacking = false;
	
	public override void Enter()
	{
		base.Enter();
		
		Timer.Stop();
		Timer.OneShot = true;
	}

	public override void Process(double delta)
	{
		base.Process(delta);

		if (_isInPosition) return;
		if (!IsWithinBoundary || !IsPlayerDetected) return;
		
		_isInPosition = true;
		Timer.WaitTime = 0.35f;
		Timer.Start();
	}
	
	public override void PhysicsProcess(double delta)
	{
		if (VelocityComponent.IsInKnockback) 
			VelocityComponent.Move(Vector2.Zero, delta);

		if (NavigationComponent.IsNavigationFinished())
		{
			if (_animationPlayer.CurrentAnimation != Options.AnimationNames.Hurt)
				_animationPlayer.Play(Options.AnimationNames.Idle);
			return;
		}

		if (_animationPlayer.CurrentAnimation != Options.AnimationNames.Hurt && _animationPlayer.CurrentAnimation != Options.AnimationNames.Run)
		{
			_animationPlayer.Play(Options.AnimationNames.Run);
		}
		VelocityComponent.Move(Self.ToLocal(NavigationComponent.GetNextPathPosition()).Normalized());
	}
	
	protected override void OnNavigationTimerTimeout()
	{
		if (_isInPosition) return;
		if (IsWithinBoundary && IsPlayerDetected) return;
		
		NavigationComponent.TargetPosition = CalculateTargetPosition(LowerBoundary);
	}
	
	private Vector2 CalculateTargetPosition(float boundary)
	{
		var distance = DistanceToPlayer;
		var percent = (distance - boundary) / distance;
		return Self.GlobalPosition.Lerp(Player.GlobalPosition, percent);
	}

	protected override void OnAttackTimerTimeout()
	{
		if (!_isAttacking)
		{
			_isAttacking = true;
			Weapon.Attack();
			Timer.Start();
		}
		else
		{
			_isAttacking = false;
			_isInPosition = false;
		}
	}
	
	protected override void OnStatusChanged(bool isCleared, Status change)
	{
		if (isCleared)
		{
			TimerPeriod = BaseTimePeriod;
			return;
		}

		switch (change.Type)
		{
			case Status.StatusType.Freezing:
				if (Math.Abs(TimerPeriod - BaseTimePeriod) < 0.001)
				{
					TimerPeriod /= change.Multiplier;
				}
				break;
			
			case Status.StatusType.Stunned:
				_stunned.Timer.WaitTime = change.TickPeriod;
				EmitSignal(nameof(Transitioned), this, _stunned);
				break;
			
			default:
			case Status.StatusType.Burning:
			case Status.StatusType.KnockedBack:
			case Status.StatusType.None:
				break;
		}
	}
	
	protected override void OnHealthDepleted()
	{
		EmitSignal(nameof(Transitioned), this, _dead);
	}
}