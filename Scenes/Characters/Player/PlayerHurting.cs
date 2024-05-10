using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;

namespace projectthaumaturgy.Scenes.Characters.Player;

public partial class PlayerHurting : State
{
	[Export] private PlayerIdle _playerIdle;
	[Export] private PlayerRunning _playerRunning;
	[Export] private InputComponent _inputComponent;
	[Export] private VelocityComponent _velocityComponent;

	public override void Enter()
	{
		_animationPlayer.Play("hurting");
		_animationPlayer.AnimationFinished += OnAnimationFinished;
	}
	
	public override void Exit()
	{
		_animationPlayer.AnimationFinished -= OnAnimationFinished;
	}
	
	private void OnAnimationFinished(StringName _)
	{
		{
			if (_inputComponent.MovementDirection != Vector2.Zero)
			{
				EmitSignal(nameof(Transitioned), this, _playerRunning);
				return;
			}
			EmitSignal(nameof(Transitioned), this, _playerIdle);
		}
	}
	
	public override void PhysicsProcess(double delta)
	{
		_velocityComponent.Move(_inputComponent.MovementDirection);
	}
}