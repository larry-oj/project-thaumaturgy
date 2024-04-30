using Godot;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scripts;
using System;

namespace projectthaumaturgy.Scenes.Pickups;

public partial class Pickup : CanvasGroup
{
	public enum PickupType
	{
		Health,
		Mana,
		Elemental
	}

	private Sprite2D _color;

	[Export] private AtlasTexture _healthTexture;
	[Export] private AtlasTexture _manaTexture;
	[Export] private AtlasTexture _elementalTexture;

	public float Value { get; set; }
	public int Speed { get; set; } = 100;
	private PickupType _type;
	public PickupType Type 
	{
		get => _type;
		set
		{
			_type = value;
			switch (_type)
			{
				case PickupType.Health:
					_color.Texture = _healthTexture;
					Modulate = new Color(1, 0, 0);
					break;
				case PickupType.Mana:
					_color.Texture = _manaTexture;
					Modulate = new Color(0, 0, 1);
					break;
				case PickupType.Elemental:
					_color.Texture = _elementalTexture;
					break;
			}
		}
	
	}
	private Attack.AttackElement _element;
	public Attack.AttackElement Element
	{
		get => _element;
		set
		{
			_element = value;
			Modulate = Attack.GetElementColor(_element);
		}
	}

	private Vector2? _playerPosition;

	public override void _Ready()
	{
		_color = GetNode<Sprite2D>("%Color");
	}

    public override void _Process(double delta)
    {
		if (_playerPosition == null) return;

		Vector2 direction = _playerPosition.Value - Position;
		direction = direction.Normalized();

		Position += direction * Speed * (float)delta;
	}

	private void OnHitboxEntered(Area2D area)
	{
		if (area.Owner is Player && area is HitboxComponent hitbox)
		{
			hitbox.TakePickup(this);
			QueueFree();
		}
	}

	private void OnMagetizeEntered(Area2D area)
	{
		if (area.Owner is Player player)
		{
			_playerPosition = player.Position;
		}
	}

	private void OnMagetizeExited(Area2D area)
	{
		if (area.Owner is Player)
		{
			_playerPosition = null;
		}
	}
}
