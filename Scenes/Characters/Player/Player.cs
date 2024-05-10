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
		var pivot = GetNode("Pivot");
		foreach (var child in pivot.GetChildren())
		{
			if (child is not Weapon weapon) continue;
			Weapons.Add(weapon);
			weapon.Visible = false;
			weapon.IsActive = false;
		}
		CurrentWeapon = Weapons[0];
		CurrentWeapon.Visible = true;
	}
	
	public override void _Process(double delta)
	{
		_stateMachine.Process(delta);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		_stateMachine.PhysicsProcess(delta);
	}

	// for now, player can only hold two weapons
	public void SwapWeapons()
	{
		CurrentWeapon.Visible = false;
		CurrentWeapon = CurrentWeapon == Weapons[0] ? Weapons[1] : Weapons[0]; // bruh
		CurrentWeapon.Visible = true;
		CurrentWeapon.SetCharacterColor();
	}
	
	private void OnHealthDepleted()
	{
		EmitSignal(nameof(Died));
		QueueFree();
	}
}
