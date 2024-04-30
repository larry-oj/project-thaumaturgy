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

	[Export] private Sprite2D _color;

	[ExportCategory("Textures")]
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
					Value = Options.Balance.HealthPickupValueDefault;
					break;
				case PickupType.Mana:
					_color.Texture = _manaTexture;
					Modulate = new Color(0, 0, 1);
					Value = Options.Balance.ManaPickupValueDefault;
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

#nullable enable
	private Player? _player;
#nullable disable
	private bool _isInFreeFlow = true;
	private Vector2 _freeFlowDirection;
	private float _currFreeFlowSpeed = 80;

	public override void _Ready()
	{
		var randX = GD.Randf() * 2 - 1;
		var randY = GD.Randf() * 2 - 1;
		_freeFlowDirection = new Vector2(randX, randY).Normalized();
	}

    public override void _Process(double delta)
    {
		if (_isInFreeFlow) FreeFlow(delta);

		if (_player == null) return;

		var direction = (_player.Position - Position).Normalized();

		Position += direction * Speed * (float)delta;
	}

	private void FreeFlow(double delta)
	{
		_currFreeFlowSpeed *= 0.9f; // friction
		Position += _freeFlowDirection * _currFreeFlowSpeed * (float)delta;
	}

	private void OnTimerTimeout()
	{
		_isInFreeFlow = false;
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
		_isInFreeFlow = false;

		if (area.Owner is Player player)
		{
			_player = player;
		}
	}

	private void OnMagetizeExited(Area2D area)
	{
		if (area.Owner is Player)
		{
			_player = null;
		}
	}
}
