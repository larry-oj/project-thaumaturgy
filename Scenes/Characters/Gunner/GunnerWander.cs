using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Characters.Gunner;

public partial class GunnerWander : State
{
	private Gunner _character;
	
	[Export] private float _wanderRadius;
	[Export] private DetectorComponent _detectorComponent;
	[Export] private VelocityComponent _velocityComponent;
	[Export] private NavigationComponent _navigationComponent;
	[Export] private GunnerIdle _gunnerIdle;
	[Export] private GunnerAlert _gunnerAlert;
	
	public override void _Ready()
	{
		_character = GetNode<Gunner>(Options.PathOptions.CharacterStateToCharacter);
	}
	
	public override void Enter()
	{
		_detectorComponent.Detected += OnPlayerDetected;
		
		do
		{
			var x = (float)GD.RandRange(-1 * _wanderRadius, _wanderRadius) + _character.GlobalPosition.X;
            var y = (float)GD.RandRange(-1 * _wanderRadius, _wanderRadius) + _character.GlobalPosition.Y;
            
            _navigationComponent.TargetPosition = new Vector2(x, y);
		} while (!_navigationComponent.IsTargetReachable());
	}
	
	public override void Exit()
	{
		_detectorComponent.Detected -= OnPlayerDetected;
	}
	
	public override void Process(double delta)
	{
		_detectorComponent.TryDetect();
	}
	
	public override void PhysicsProcess(double delta)
	{
		if (_navigationComponent.IsNavigationFinished())
		{
			EmitSignal(nameof(Transitioned), this, _gunnerIdle);
		}
		_velocityComponent.Move(_character.ToLocal(_navigationComponent.GetNextPathPosition()).Normalized());
	}

	private void OnPlayerDetected()
	{
		EmitSignal(nameof(Transitioned), this, _gunnerAlert);
	}
}