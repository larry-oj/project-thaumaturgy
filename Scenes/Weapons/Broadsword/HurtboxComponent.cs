using Godot;
using System;
using projectthaumaturgy.Scenes.Characters;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scripts;

public partial class HurtboxComponent : Area2D
{
	public Attack Attack;
	public Character AttackOwner;
	[Export] public bool IsActiveDetection = true;
	
	public override void _Ready()
	{
		if (IsActiveDetection)
			AreaEntered += OnHitboxAreaEntered;
	}

	public void TryGetOverlappingAreas()
	{
		foreach (var area in GetOverlappingAreas())
		{
			OnHitboxAreaEntered(area);
		}
	}
	
	private void OnHitboxAreaEntered(Area2D area)
	{
		if (area.Owner == AttackOwner) return;
		if (area is not HitboxComponent hitbox) return;
		
		hitbox.Damage(Attack);
	}
}
