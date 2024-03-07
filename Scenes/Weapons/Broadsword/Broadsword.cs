using Godot;
using projectthaumaturgy.Resources.Weapons.CreatedObjects;
using projectthaumaturgy.Scenes.Characters;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.Broadsword;

public partial class Broadsword : Weapon
{
	private StateMachine _stateMachine;
	private AnimationPlayer _animationPlayer;
	private HurtboxComponent _hurtboxComponent;
	private Character _character;

	public override void _Ready()
	{
		base._Ready();
        
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_stateMachine = GetNode<StateMachine>("StateMachine");
		_stateMachine.Init(this, _animationPlayer);
		_hurtboxComponent = GetNode<HurtboxComponent>("HurtboxComponent");
		_character = GetNode<Character>("../..");
		
		_hurtboxComponent.attackOwner = _character;
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