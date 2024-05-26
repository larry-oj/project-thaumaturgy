using Godot;
using projectthaumaturgy.Resources.Weapons;
using projectthaumaturgy.Scenes.Characters;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scripts;
using WeaponStatsComponent = projectthaumaturgy.Scenes.Components.WeaponStatsComponent;

namespace projectthaumaturgy.Scenes.Weapons.Broadsword;

public partial class HurtboxComponent : Area2D
{
	public Character attackOwner;
	
	[Export] public bool IsActiveDetection = true;
	[Export] private WeaponStatsComponent _weaponStatsComponent;
	
	public override void _Ready()
	{
		if (IsActiveDetection)
			AreaEntered += OnHitboxAreaEntered;
	}

	public bool TryGetOverlappingAreas()
	{
		var flag = false;
		foreach (var area in GetOverlappingAreas())
		{
			OnHitboxAreaEntered(area);
			flag = true;
			break;
		}

		return flag;
	}
	
	private void OnHitboxAreaEntered(Area2D area)
	{
		if (!area.Monitorable) return;
		if (area.Owner == attackOwner) return;
		if (area is not HitboxComponent hitbox) return;
		if (attackOwner is Enemy && hitbox.Owner is Enemy) return;
		
		var rotation = Vector2.Right.Rotated(GetParent<Weapon>().GlobalRotation);
		hitbox.Damage(_weaponStatsComponent.CreateAttack(attackOwner, rotation));
	}
}