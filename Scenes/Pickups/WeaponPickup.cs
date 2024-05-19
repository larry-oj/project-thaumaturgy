using Godot;
using projectthaumaturgy.Extensions;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Weapons;

namespace projectthaumaturgy.Scenes.Pickups;

public partial class WeaponPickup : CanvasGroup
{
	public WeaponResource WeaponResource { get; set; }
	private Weapon _weapon;
	private HitboxComponent _playerHitbox;

	public Weapon Weapon
	{
		get => _weapon;
		set
		{
			_weapon = value;
			if (_weapon.Character != null)
				_weapon.Character = null;
			_weaponHolder.AddChild(_weapon);
			_weapon.Visible = true;
		}
	}

	[Export] private Node2D _weaponHolder;
	private Node2D _ui;
	
	public override void _Ready()
	{
		_ui = GetNode<Node2D>("%UI");
	}

	public override void _Process(double delta)
	{
		if (_playerHitbox != null && InputComponent.IsJustInteracted)
		{
			_weaponHolder.RemoveChild(_weapon);
			var old = _playerHitbox.TakeWeapon(Weapon);
			if (old != null)
			{
				Weapon = old;
                return;
			}
			QueueFree();
		}
	}
	
	private void OnAreaEntered(Node area)
	{
		if (area.Owner is Player && area is HitboxComponent hitbox)
		{
			_playerHitbox = hitbox;
			_ui.Visible = true;
		}
	}
	
	private void OnAreaExited(Node area)
	{
		if (area.Owner is Player && area is HitboxComponent hitbox)
		{
			_playerHitbox = null;
			_ui.Visible = false;
		}
	}

	public override void _ExitTree()
	{
		_ui.Visible = false;
	}
}