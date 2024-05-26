using System;
using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Characters.Knight;

public partial class KnightAlert  : AlertBase
{
	[ExportGroup("Knight")]
	[Export] private KnightDead _dead;
	[Export] private KnightStunned _stunned;
	[Export] private Area2D _attackRange;
	
	protected override bool IsWithinBoundary => DistanceToPlayer <= LowerBoundary;
	private bool IsWithinAttackRange => DistanceToPlayer <= LowerBoundary + 10;

	private bool _isInPosition = false;
	private bool _isAttacking = false;
	private bool _isExited = true;
	
	public override void Enter()
	{
		base.Enter();
		
		Timer.Stop();
		Timer.OneShot = true;
		
		_attackRange.AreaEntered += OnAttackRangeEntered;
		_attackRange.AreaExited += OnAttackRangeExited;
	}

	public override void Exit()
	{
		_attackRange.AreaEntered -= OnAttackRangeEntered;
		_attackRange.AreaExited -= OnAttackRangeExited;
		OnAttackRangeExited(null);
		Timer.Stop();
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
	
	private void OnAttackRangeEntered(Area2D area)
	{
		if (area is HitboxComponent && area.Owner is Player.Player)
		{
			_isInPosition = true;
			_isExited = false;
			Timer.WaitTime = 0.35f;
			Timer.Start();
		}
	}
	
	private void OnAttackRangeExited(Area2D area)
	{
		_isExited = true;
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
			
			if (!_isExited)
			{
				Timer.Start();
			}
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