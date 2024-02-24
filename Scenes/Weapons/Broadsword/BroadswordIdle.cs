using Godot;
using projectthaumaturgy.Scenes.Components.StateMachine;

namespace projectthaumaturgy.Scenes.Weapons.Broadsword;

public partial class BroadswordIdle : State
{
	[Export] private BroadswordAttack _broadswordAttack;
	private Broadsword _sword;

	public override void _Ready()
	{
		_sword = GetNode<Broadsword>("../..");
		_sword.OnAttack += OnAttacked;
	}
	
	private void OnAttacked()
	{
		EmitSignal(nameof(Transitioned), this, _broadswordAttack);
	}
}