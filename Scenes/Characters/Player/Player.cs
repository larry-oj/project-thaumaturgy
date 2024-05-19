using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scenes.Weapons;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Characters.Player;

public partial class Player : Character
{
	[Signal] public delegate void WeaponsSwappedEventHandler(Weapon weapon);
	[Signal] public delegate void WeaponPickedUpEventHandler(Weapon weapon);
	[Signal] public delegate void WeaponRemovedEventHandler(Weapon weapon);
	
	public override void _Ready()
	{
		_stateMachine.Init(this, _animationPlayer);
		var pivot = GetNode("Pivot");
		foreach (var child in pivot.GetChildren())
		{
			if (child is not Weapon weapon) continue;
			Weapons.Add(weapon);
			weapon.Visible = false;
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
		if (Weapons.Count < 2) return;
		CurrentWeapon.Visible = false;
		CurrentWeapon = CurrentWeapon == Weapons[0] ? Weapons[1] : Weapons[0]; // bruh
		CurrentWeapon.Visible = true;
		CurrentWeapon.SetCharacterColor();
		EmitSignal(SignalName.WeaponsSwapped, CurrentWeapon);
	}

	public void TakeWeapon(Weapon weapon)
	{
		Weapons.Add(weapon);
		_pivot.AddChild(weapon);
		CurrentWeapon = weapon;
		EmitSignal(SignalName.WeaponPickedUp, weapon);
	}
	
	public void ReplaceWeapon(Weapon weapon)
	{
		var old = CurrentWeapon;
		Weapons.Remove(old);
		_pivot.RemoveChild(old);
		EmitSignal(SignalName.WeaponRemoved, old);
		TakeWeapon(weapon);
	}
	
	private void OnHealthDepleted()
	{
		EmitSignal(nameof(Died));
		QueueFree();
	}
}
