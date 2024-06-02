using Godot;
using projectthaumaturgy.Resources.Weapons.CreatedObjects;
using projectthaumaturgy.Scenes.Characters.Gunner;
using projectthaumaturgy.Scenes.Weapons;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Characters.Sniper;

public partial class Sniper : Enemy
{
	[Export] private BulletResource _bulletResource;
	
	public override void _Ready()
	{
		_stateMachine.Init(this, _animationPlayer);
		CurrentWeapon = GetNode("Pivot").GetChild<Weapon>(0);
		CurrentWeapon.Character = this;
		CurrentWeapon.BulletResource = _bulletResource;

		CurrentWeapon.StatsComponent.SetElement(Attack.AttackElement.Fire);
	}
	
	public override void _Process(double delta)
	{
		_stateMachine.Process(delta);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		_stateMachine.PhysicsProcess(delta);
	}
	
	private void OnHealthDepleted()
	{
		EmitSignal(nameof(Died));
	}
}
