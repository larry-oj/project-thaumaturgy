using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Characters.Knight;

public partial class KnightIdle  : State
{
	private Timer _intervalTimer;
	[Export] private float _wanderChance;
	[Export] private DetectorComponent _detectorComponent;
	[Export] private StatusComponent _statusComponent;
	[Export] private KnightWander _wander;
	[Export] private KnightAlert _alert;
	[Export] private KnightDead _dead;
	[Export] private KnightStunned _stunned;

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
			EmitSignal(nameof(Transitioned), this, _wander);
		
	}
	
	private void OnPlayerDetected()
	{
		EmitSignal(nameof(Transitioned), this, _alert);
	}

	private void OnDamageTaken(GodotObject _)
	{
		_animationPlayer.Play(Options.AnimationNames.Hurt);
	}

	private void OnAnimationFinished(StringName animationName)
	{
		if (animationName == Options.AnimationNames.Hurt)
		{
			EmitSignal(State.SignalName.Transitioned, this, _alert);
		}
	}
	
	private void OnStatusChanged(bool isCleared, Status status)
	{
		if (isCleared) return;
		
		switch (status.Type)
		{
			case Status.StatusType.Freezing:
				_alert.TimerPeriod /= status.Multiplier;
				break;
			
			case Status.StatusType.Stunned:
				_stunned.Timer.WaitTime = status.TickPeriod;
				EmitSignal(nameof(Transitioned), this, _stunned);
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
		EmitSignal(nameof(Transitioned), this, _dead);
	}
}