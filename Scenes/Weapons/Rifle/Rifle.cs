using Godot;
using projectthaumaturgy.Resources.Weapons.CreatedObjects;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.Rifle;

public partial class Rifle : Weapon
{
	private StateMachine _stateMachine;
	private AnimationPlayer _animationPlayer;

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