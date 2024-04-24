using System.Globalization;
using System.Linq;
using Godot;
using Godot.Collections;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Weapons;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.UI;

public partial class WeaponContainer : MarginContainer
{
	[Export] private Control _weaponSprite;
	[Export] private Control _elementPicker;
	private Array<ElementPickerButton> _elementPickerButtons = new();
	[Export] private Label _damageStatLabel;
	[Export] private Label _speedStatLabel;

	private Weapon _weapon;
	private WeaponStatsComponent _weaponStats;
	public Weapon Weapon
	{
		get => _weapon;
		set
		{
			_weapon = value;
			_weaponStats = _weapon.StatsComponent;
			SetWeapon();
		}
	}

	private TextureRect _outline;
	private TextureRect _color;

	public override void _Ready()
	{
		foreach (var child in _elementPicker.GetChildren())
		{
			if (child is ElementPickerButton button)
			{
				_elementPickerButtons.Add(button);
			}
		}

		_outline = _weaponSprite.GetNode<TextureRect>("Outline");
		_color = _weaponSprite.GetNode<TextureRect>("Color");
	}

	// find buttons in the element picker
	// then when the weapon is set, we configure the buttons to be disabled if the weapon already has an element
	// or if the button is not the element of the weapon
	// then we add an event listener to the button to set the element of the weapon

	private void SetWeapon()
	{
		GD.Print("Setting weapon");

		base.Name = _weapon.Name;

		// set weapon sprite
		var weaponOutline = _weapon.Sprites.GetNode<Sprite2D>("Outline");
		var weaponColor = _weapon.Sprites.GetNode<Sprite2D>("Color");
		
		_outline.Texture = new AtlasTexture
		{
			Atlas = weaponOutline.Texture,
			Region = weaponOutline.RegionRect,
		};
		_color.Texture = new AtlasTexture
		{
			Atlas = weaponColor.Texture,
			Region = weaponColor.RegionRect,
		};
		_color.SelfModulate = weaponColor.SelfModulate;

		// set element picker
		foreach (var button in _elementPickerButtons)
		{
			if (_weaponStats.Element == Attack.AttackElement.None)
			{
				button.Disabled = false;
				button.ElementPicked += OnElementButtonPressed;
			}
			else if (button.Element == _weaponStats.Element)
			{
				button.Disabled = false;
			}
			else
			{
				button.Disabled = true;
			}
		}

		// set weapon stats
		_damageStatLabel.Text = _weapon.StatsComponent.Damage.ToString(CultureInfo.InvariantCulture);   // 💊
		_speedStatLabel.Text = _weapon.StatsComponent.FireRate.ToString(CultureInfo.InvariantCulture);  // 💊
	}

	private void OnElementButtonPressed(Attack.AttackElement element)
	{

		if (_weaponStats.Element != Attack.AttackElement.None)
		{
			GD.Print("Failed to set element to a weapon: weapon already has an element.");
			return;
		}

		_weapon.StatsComponent.SetElement(element);
		_color.SelfModulate = Attack.GetElementColor(element);

		// disable all other buttons
		// remove event listeners so that we cant re-pick
		foreach (var button in _elementPickerButtons)
		{
			if (button.Element != element)
				button.Disabled = true;
			
			button.ElementPicked -= OnElementButtonPressed;
		}
	}
}
