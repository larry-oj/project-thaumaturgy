using System.Globalization;
using System.Linq;
using Godot;
using Godot.Collections;
using projectthaumaturgy.Scenes.Weapons;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.UI;

public partial class WeaponContainer : MarginContainer
{
	[Export] private Control _weaponSprite;
	[Export] private Control _elementPicker;
	private Array<Button> _elementPickerButtons = new();
	[Export] private Label _damageStatLabel;
	[Export] private Label _speedStatLabel;

	private Weapon _weapon;
	public Weapon Weapon
	{
		get => _weapon;
		set
		{
			_weapon = value;
			SetWeapon();
		}
	}

	public override void _Ready()
	{
		foreach (var child in _elementPicker.GetChildren())
		{
			if (child is Button button)
			{
				_elementPickerButtons.Add(button);
			}
		}
	}

	private void SetWeapon()
	{
		base.Name = _weapon.Name;

		var weaponOutline = _weapon.Sprites.GetNode<Sprite2D>("Outline");
		var weaponColor = _weapon.Sprites.GetNode<Sprite2D>("Color");
		
		var outline = _weaponSprite.GetNode<TextureRect>("Outline");
		outline.Texture = new AtlasTexture
		{
			Atlas = weaponOutline.Texture,
			Region = weaponOutline.RegionRect,
		};
		var color = _weaponSprite.GetNode<TextureRect>("Color");
		color.Texture = new AtlasTexture
		{
			Atlas = weaponColor.Texture,
			Region = weaponColor.RegionRect,
		};
		color.SelfModulate = weaponColor.SelfModulate;

		_damageStatLabel.Text = _weapon.StatsComponent.Damage.ToString(CultureInfo.InvariantCulture);	// 💊
		_speedStatLabel.Text = _weapon.StatsComponent.FireRate.ToString(CultureInfo.InvariantCulture);	// 💊
	}
}