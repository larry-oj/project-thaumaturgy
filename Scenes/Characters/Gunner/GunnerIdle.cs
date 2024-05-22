using System;
using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Characters.Gunner;

public partial class GunnerIdle : State
{
	private Timer _intervalTimer;
	[Export] private float _wanderChance;
	[Export] private DetectorComponent _detectorComponent;
	[Export] private StatusComponent _statusComponent;
	[Export] private GunnerWander _gunnerWander;
	[Export] private GunnerAlert _gunnerAlert;
	[Export] private GunnerDead _gunnerDead;
	[Export] private GunnerStunned _gunnerStunned;

	public override void _Ready()
	{
		_intervalTimer = GetNode<Timer>("Timer");
	}

	public override void Enter()
	{
		_intervalTimer.Start();
		_detectorComponent.Detected += OnPlayerDetected;
		_statusComponent.StatusChanged += OnStatusChanged;
		_animationPlayer.AnimationFinished += OnAnimationFinished;
		_animationPlayer.Play(Options.AnimationNames.Idle);
	}
	
	public override void Exit()
	{
		_intervalTimer.Stop();
		_detectorComponent.Detected -= OnPlayerDetected;
		_statusComponent.StatusChanged -= OnStatusChanged;
		_animationPlayer.AnimationFinished -= OnAnimationFinished;
		_animationPlayer.Stop();
	}

	public override void Process(double delta)
	{
		_detectorComponent.TryDetect();
	}
	
	private void OnTimerTimeout()
	{
		if (GD.Randf() < _wanderChance)
			EmitSignal(nameof(Transitioned), this, _gunnerWander);
		
	}
	
	private void OnPlayerDetected()
	{
		EmitSignal(nameof(Transitioned), this, _gunnerAlert);
	}

	private void OnDamageTaken(GodotObject _)
	{
		_animationPlayer.Play(Options.AnimationNames.Hurt);
	}

	private void OnAnimationFinished(StringName animationName)
	{
		if (animationName == Options.AnimationNames.Hurt)
		{
			EmitSignal(State.SignalName.Transitioned, this, _gunnerAlert);
		}
	}
	
	private void OnStatusChanged(bool isCleared, Status status)
	{
		if (isCleared) return;
		
		switch (status.Type)
		{
			case Status.StatusType.Freezing:
				_gunnerAlert.TimerPeriod /= status.Multiplier;
				break;
			
			case Status.StatusType.Stunned:
				_gunnerStunned.Timer.WaitTime = status.TickPeriod;
				EmitSignal(nameof(Transitioned), this, _gunnerStunned);
				break;
			
			default:
			case Status.StatusType.Burning:
			case Status.StatusType.KnockedBack:
			case Status.StatusType.None:
				break;
		}
	}
	
	private void OnHealthDepleted()
	{
		EmitSignal(nameof(Transitioned), this, _gunnerDead);
	}
}