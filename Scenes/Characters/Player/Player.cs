using Godot;
using projectthaumaturgy.Scenes.Components.StateMachine;

namespace projectthaumaturgy.Scenes.Characters.Player;

public partial class Player : CharacterBody2D
{
	[Export]
	private StateMachine _stateMachine;
	[Export]
	private AnimationPlayer _animationPlayer;

	public override void _Ready()
	{
		base._Ready();
		_stateMachine.Init(this, _animationPlayer);
	}
	
	public override void _Process(double delta)
	{
		_stateMachine.Process(delta);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		_stateMachine.PhysicsProcess(delta);
		MoveAndSlide();
	}
}