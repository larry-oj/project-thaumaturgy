using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.Sniper;

public partial class SniperIdle  : State
{
	[Export] private SniperShoot _pistolShoot;
	private Sniper _weapon;

	public override void _Ready()
	{
		_weapon = GetNode<Sniper>(Options.PathOptions.WeaponStateToWeapon);
	}

	public override void Enter()
	{
		_weapon.OnAttack += OnAttacked;
	}

	public override void Exit()
	{
		_weapon.OnAttack -= OnAttacked;
	}

	private void OnAttacked()
	{
		if (_weapon.Character == null) return;
		var manaComponent = _weapon.Character.GetNode<ManaComponent>("ManaComponent");
		if (manaComponent != null)
		{
			var success = manaComponent.TryChangeMana(-_weapon.StatsComponent.ManaCost);
			if (!success) return;
		}
		EmitSignal(nameof(Transitioned), this, _pistolShoot);
	}
}