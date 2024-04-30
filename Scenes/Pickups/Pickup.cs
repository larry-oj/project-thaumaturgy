using Godot;
using projectthaumaturgy.Scenes.Characters.Player;
using System;

namespace projectthaumaturgy.Scenes.Pickups;

public partial class Pickup : CanvasGroup
{
	internal Sprite2D _color;
	internal Area2D _hitbox;

	public override void _Ready()
	{
		_color = GetNode<Sprite2D>("%Color");
		_hitbox = GetNode<Area2D>("%Hitbox");
	}

	internal virtual void OnHitboxEntered(Area2D area)
	{
		if (area.Owner is Player player)
		{
			QueueFree();
		}
	}
}
