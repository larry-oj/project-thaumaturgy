using Godot;
using projectthaumaturgy.Resources.Weapons.CreatedObjects;
using projectthaumaturgy.Scenes.Components.StateMachine;

namespace projectthaumaturgy.Scenes.Weapons.Rifle;

public partial class Rifle : Weapon
{
	private StateMachine _stateMachine;
	private AnimationPlayer _animationPlayer;
	[Export] public BulletResource BulletResource { get; private set; }

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_stateMachine = GetNode<StateMachine>("StateMachine");
		_stateMachine.Init(this, _animationPlayer);
	}
	
	public override void _Process(double delta)
	{
		_stateMachine.Process(delta);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		_stateMachine.PhysicsProcess(delta);
	}

	public override void Attack()
	{
		EmitSignal(nameof(OnAttack));
	}
}