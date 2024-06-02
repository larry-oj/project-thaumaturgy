using Godot;
using projectthaumaturgy.Scenes.Characters.Gunner;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Characters.Knight;

public partial class KnightWander  : State
{
	private Enemy _character;
	
	[Export] private float _wanderRadius;
	[Export] private DetectorComponent _detectorComponent;
	[Export] private VelocityComponent _velocityComponent;
	[Export] private NavigationComponent _navigationComponent;
	[Export] private StatusComponent _statusComponent;
	[Export] private KnightIdle _idle;
	[Export] private KnightAlert _alert;
	[Export] private KnightDead _dead;
	[Export] private KnightStunned _stunned;
	
	public override void _Ready()
	{
		_character = GetNode<Enemy>(Options.PathOptions.CharacterStateToCharacter);
	}
	
	public override void Enter()
	{
		_detectorComponent.Detected += OnPlayerDetected;
		_statusComponent.StatusChanged += OnStatusChanged;
		_animationPlayer.AnimationFinished += OnAnimationFinished;
		
		// do
		// {
		// 	var x = (float)GD.RandRange(-1 * _wanderRadius, _wanderRadius) + _character.GlobalPosition.X;
		// 	var y = (float)GD.RandRange(-1 * _wanderRadius, _wanderRadius) + _character.GlobalPosition.Y;
  //           
		// 	_navigationComponent.TargetPosition = new Vector2(x, y);
		// } while (!_navigationComponent.IsTargetReachable());
		// _animationPlayer.Play(Options.AnimationNames.Run);
	}
	
	public override void Exit()
	{
		_detectorComponent.Detected -= OnPlayerDetected;
		_statusComponent.StatusChanged -= OnStatusChanged;
		_animationPlayer.AnimationFinished -= OnAnimationFinished;
		_animationPlayer.Stop();
	}
	
	public override void Process(double delta)
	{
		_detectorComponent.TryDetect();
	}
	
	public override void PhysicsProcess(double delta)
	{
		if (_navigationComponent.IsNavigationFinished())
		{
			EmitSignal(nameof(Transitioned), this, _idle);
		}
		_velocityComponent.Move(_character.ToLocal(_navigationComponent.GetNextPathPosition()).Normalized());
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

	private void OnHealthDepleted()
	{
		EmitSignal(nameof(Transitioned), this, _alert);
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
}