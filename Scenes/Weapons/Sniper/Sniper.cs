using Godot;
using projectthaumaturgy.Scenes.Characters;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;

namespace projectthaumaturgy.Scenes.Weapons.Sniper;

public partial class Sniper : Weapon
{
	private StateMachine _stateMachine;
	private AnimationPlayer _animationPlayer;
	[Export] public Line2D Laser { get; private set; }
	[Export] public RayCast2D Ray { get; private set; }
	
	public override void _Ready()
	{
		base._Ready();
		
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_stateMachine = GetNode<StateMachine>("StateMachine");
	}
	
	public override void _Process(double delta)
	{
		_stateMachine.Process(delta);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		_stateMachine.PhysicsProcess(delta);
	}

	public override void Attack(bool isPlayer = false)
	{
		if (isPlayer && !InputComponent.IsJustAttacked) return;
		
		EmitSignal(nameof(OnAttack));
	}
	
	internal override void OnCharacterSetup()
	{
		base.OnCharacterSetup();
		_stateMachine.Init(this, _animationPlayer);
	}
    
	internal override void OnCharacterClear()
	{
		base.OnCharacterClear();
		_stateMachine.Shutdown();
	}
}