using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.Uzi;

public partial class UziIdle  : State
{
	[Export] private UziShoot _shoot;
	private Uzi _uzi;

	public override void _Ready()
	{
		_uzi = GetNode<Uzi>(Options.PathOptions.WeaponStateToWeapon);
	}

	public override void Enter()
	{
		_uzi.OnAttack += OnAttacked;
	}

	public override void Exit()
	{
		_uzi.OnAttack -= OnAttacked;
	}

	private void OnAttacked()
	{
		if (_uzi.Character == null) return;
		var manaComponent = _uzi.Character.GetNode<ManaComponent>("ManaComponent");
		if (manaComponent != null)
		{
			var success = manaComponent.TryChangeMana(-_uzi.StatsComponent.ManaCost);
			if (!success) return;
		}
		EmitSignal(nameof(Transitioned), this, _shoot);
	}
}