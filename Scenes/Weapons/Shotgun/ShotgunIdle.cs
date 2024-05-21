using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.Shotgun;

public partial class ShotgunIdle : State
{
	[Export] private ShotgunShoot _shotgunShoot;
	private Shotgun _shotgun;

	public override void _Ready()
	{
		_shotgun = GetNode<Shotgun>(Options.PathOptions.WeaponStateToWeapon);
	}

	public override void Enter()
	{
		_shotgun.OnAttack += OnAttacked;
	}

	public override void Exit()
	{
		_shotgun.OnAttack -= OnAttacked;
	}

	private void OnAttacked()
	{
		if (_shotgun.Character == null) return;
		var manaComponent = _shotgun.Character.GetNode<ManaComponent>("ManaComponent");
		if (manaComponent != null)
		{
			var success = manaComponent.TryChangeMana(-_shotgun.StatsComponent.ManaCost);
			if (!success) return;
		}
		EmitSignal(nameof(Transitioned), this, _shotgunShoot);
	}
}