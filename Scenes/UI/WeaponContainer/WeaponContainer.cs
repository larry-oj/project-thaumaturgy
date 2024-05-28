using System.Globalization;
using System.Linq;
using Godot;
using Godot.Collections;
using projectthaumaturgy.Extensions;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Weapons;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.UI;

public partial class WeaponContainer : MarginContainer
{
	[Export] private Control _weaponSprite;
	private TextureRect _outline;
	private TextureRect _color;

	[Export] private Control _elementPicker;
	private Array<ElementPickerButton> _elementPickerButtons = new();
	
	[Export] private Control _effectPicker;
	private Array<EffectPickerButton> _effectPickerButtons = new();

	[Export] private StatChangeContainer _damageStatContainer;
	[Export] private StatChangeContainer _speedStatContainer;
	[Export] private AudioStreamPlayer _audioStreamPlayer;

	public CurrencyComponent CurrencyComponent { get; set; }
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

	public override void _Ready()
	{
		_outline = _weaponSprite.GetNode<TextureRect>("Outline");
		_color = _weaponSprite.GetNode<TextureRect>("Color");

		// find buttons in the element picker
		foreach (var child in _elementPicker.GetChildren())
		{
			if (child is ElementPickerButton button)
			{
				_elementPickerButtons.Add(button);
			}
		}
		
		foreach (var child in _effectPicker.GetChildren())
		{
			if (child is EffectPickerButton button)
			{
				_effectPickerButtons.Add(button);
			}
		}

		_damageStatContainer.StatChanged += OnDamageStatChanged;
		_speedStatContainer.StatChanged += OnSpeedStatChanged;
		
		GetNode<Label>("%ElementCostValue").Text = Options.Balance.ElementUpgradeCost.ToString(CultureInfo.InvariantCulture);
		GetNode<Label>("%InfusionCostValue").Text = Options.Balance.InfusionUpgradeCost.ToString(CultureInfo.InvariantCulture);
	}

	// find buttons in the element picker
	// then when the weapon is set, we configure the buttons to be disabled if the weapon already has an element
	// or if the button is not the element of the weapon
	// then we add an event listener to the button to set the element of the weapon

	private void SetWeapon()
	{
		base.Name = _weapon.GetType().Name;

		// set weapon sprite
		var weaponOutline = _weapon.Sprites.Outline.As<Sprite2D>();
		var weaponColor = _weapon.Sprites.Color.As<Sprite2D>();

		_outline.Texture = weaponOutline.Texture;
		_color.Texture = weaponColor.Texture;
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
		
		foreach (var button in _effectPickerButtons)
		{
			if (_weaponStats.Infusion == Attack.AttackInfusion.None)
			{
				button.Disabled = false;
				button.EffectPicked += OnEffectButtonPressed;
			}
			else if (button.Infusion == _weaponStats.Infusion)
			{
				button.Disabled = false;
			}
			else
			{
				button.Disabled = true;
			}
		}

		// set weapon stats
		_damageStatContainer.StatLabel.Text = _weapon.StatsComponent.Damage.ToString("0.00");
		_damageStatContainer.CostLabel.Text = _weapon.StatsComponent.DamageUpgradeCost.ToString(CultureInfo.InvariantCulture);
		_damageStatContainer.IncreaseButton.Text = "+" + _weapon.StatsComponent.DamageStep.ToString("0.00");
		_speedStatContainer.StatLabel.Text = _weapon.StatsComponent.FireRate.ToString("0.00");
		_speedStatContainer.CostLabel.Text = _weapon.StatsComponent.FireRateUpgradeCost.ToString(CultureInfo.InvariantCulture);
		_speedStatContainer.IncreaseButton.Text = "+" + _weapon.StatsComponent.FireRateStep.ToString("0.00");
	}

	private void OnElementButtonPressed(Attack.AttackElement element)
	{
		if (_weaponStats.Element != Attack.AttackElement.None) return;

		var success = CurrencyComponent.TryChangeCurrency(-Options.Balance.ElementUpgradeCost);
		if (!success) return;
		
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
		
		_audioStreamPlayer.Playing = true;
	}
	
	private void OnEffectButtonPressed(Attack.AttackInfusion infusion)
	{
		if (_weaponStats.Infusion != Attack.AttackInfusion.None) return;

		var success = CurrencyComponent.TryChangeCurrency(-Options.Balance.InfusionUpgradeCost);
		if (!success) return;
		
		_weapon.StatsComponent.SetInfusion(infusion);

		// disable all other buttons
		// remove event listeners so that we cant re-pick
		foreach (var button in _effectPickerButtons)
		{
			if (button.Infusion != infusion)
				button.Disabled = true;
			
			button.EffectPicked -= OnEffectButtonPressed;
		}
		
		_audioStreamPlayer.Playing = true;
	}

	private void OnDamageStatChanged()
	{
		var success = CurrencyComponent.TryChangeCurrency(-_weaponStats.DamageUpgradeCost);
		if (!success) return;
		
		_weaponStats.IncrementDamage(_weaponStats.DamageStep);
		_damageStatContainer.StatLabel.Text = _weaponStats.Damage.ToString("0.00");
		_damageStatContainer.CostLabel.Text = _weaponStats.DamageUpgradeCost.ToString(CultureInfo.InvariantCulture);
		
		_audioStreamPlayer.Playing = true;
	}

	private void OnSpeedStatChanged()
	{
		var success = CurrencyComponent.TryChangeCurrency(-_weaponStats.FireRateUpgradeCost);
		if (!success) return;
		
		_weaponStats.IncrementFireRate(_weaponStats.FireRateStep);
		_speedStatContainer.StatLabel.Text = _weaponStats.FireRate.ToString("0.00");
		_speedStatContainer.CostLabel.Text = _weaponStats.FireRateUpgradeCost.ToString(CultureInfo.InvariantCulture);
		
		_audioStreamPlayer.Playing = true;
	}
}
