using System;
using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scenes.Weapons;
using projectthaumaturgy.Scenes.Weapons.Shotgun;
using projectthaumaturgy.Scripts;

	namespace projectthaumaturgy.Scenes.Characters.Gunner;

public partial class GunnerAlert : AlertBase<Gunner>
{
	[ExportGroup("Gunner")]
	[Export] private GunnerIdle _gunnerIdle;
	[Export] private GunnerDead _gunnerDead;
	[Export] private GunnerStunned _gunnerStunned;
	[Export] protected float UpperBoundary;
	
	protected override bool IsWithinBoundary => DistanceToPlayer <= UpperBoundary && DistanceToPlayer >= LowerBoundary;
	
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
		if (IsWithinBoundary && IsPlayerDetected)
			return;
		
		var boundary = DistanceToPlayer > UpperBoundary ? UpperBoundary : LowerBoundary;
		if (!IsPlayerDetected && ForcePlayerInView) boundary = 10;
		
		NavigationComponent.TargetPosition = CalculateTargetPosition(boundary);
	}

	private Vector2 CalculateTargetPosition(float boundary)
	{
		var distance = DistanceToPlayer;
		var percent = (distance - boundary) / distance;
		return Self.GlobalPosition.Lerp(Player.GlobalPosition, percent);
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
				_gunnerStunned.Timer.WaitTime = change.TickPeriod;
				EmitSignal(nameof(Transitioned), this, _gunnerStunned);
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
		EmitSignal(nameof(Transitioned), this, _gunnerDead);
	}
}