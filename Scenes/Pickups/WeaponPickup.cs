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

	public Weapon Weapon
	{
		get => _weapon;
		set
		{
			_weapon = value;
			if (_weapon.Character != null)
				_weapon.Character = null;
			_weaponHolder.AddChild(_weapon);
		}
	}

	[Export] private Node2D _weaponHolder;
	private Node2D _ui;
	
	public override void _Ready()
	{
		_ui = GetNode<Node2D>("%UI");
	}
	
	private void OnAreaEntered(Node area)
	{
		if (area.Owner is Player)
		{
			_ui.Visible = true;
		}
	}
	
	private void OnAreaExited(Node area)
	{
		if (area.Owner is Player)
		{
			_ui.Visible = false;
		}
	}

	public override void _ExitTree()
	{
		_ui.Visible = false;
	}
}