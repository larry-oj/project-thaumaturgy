using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scenes.Weapons;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Characters.Player;

public partial class Player : Character
{
	public override void _Ready()
	{
		_stateMachine.Init(this, _animationPlayer);
		GetNode("Pivot").GetChild<Weapon>(0).WeaponAttack =
			new Attack(5f, Attack.AttackType.Ranged, Attack.AttackElement.Absolute);
	}
	
	public override void _Process(double delta)
	{
		_stateMachine.Process(delta);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		_stateMachine.PhysicsProcess(delta);
	}
}