using Godot;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Pickups;
using System;

public partial class HealthPickup : Pickup
{
	public override void _Ready()
	{
		base._Ready();
	}

	internal override void OnHitboxEntered(Area2D area)
	{
		base.OnHitboxEntered(area);
	}
}
