using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.Autorifle;

public partial class AutorifleIdle  : State
{
	[Export] private AutorifleShoot _pistolShoot;
	private Autorifle _autorifle;

	public override void _Ready()
	{
		_autorifle = GetNode<Autorifle>(Options.PathOptions.WeaponStateToWeapon);
	}

	public override void Enter()
	{
		_autorifle.OnAttack += OnAttacked;
	}

	public override void Exit()
	{
		_autorifle.OnAttack -= OnAttacked;
	}

	private void OnAttacked()
	{
		if (_autorifle.Character == null) return;
		var manaComponent = _autorifle.Character.GetNode<ManaComponent>("ManaComponent");
		if (manaComponent != null)
		{
			var success = manaComponent.TryChangeMana(-_autorifle.StatsComponent.ManaCost);
			if (!success) return;
		}
		EmitSignal(nameof(Transitioned), this, _pistolShoot);
	}
}