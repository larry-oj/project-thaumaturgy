using Godot;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.Lance;

public partial class LanceIdle  : State
{
	[Export] private LanceAttack _lanceAttack;
	private Lance _lance;

	public override void _Ready()
	{
		_lance = GetNode<Lance>(Options.PathOptions.WeaponStateToWeapon);
		_lance.OnAttack += OnAttacked;
	}
	
	private void OnAttacked()
	{
		EmitSignal(nameof(Transitioned), this, _lanceAttack);
	}
}