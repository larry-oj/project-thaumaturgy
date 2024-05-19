using Godot;
using Godot.Collections;
using projectthaumaturgy.Extensions;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Levels;
using projectthaumaturgy.Scenes.Weapons;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.UI;

public partial class UI : CanvasLayer
{
	private PlayerHealthbar _playerHealthbar;
	private PlayerManabar _playerManabar;
	private PlayerCurrencyCounter _playerCurrencyCounter;
	
	private CurrencyComponent _currencyComponent;
	private Player _player;
	public Player Player
	{
		get => _player;
		set
		{
			_player = value;
			_playerHealthbar.HealthComponent = _player.GetNode<HealthComponent>("HealthComponent");
			_playerManabar.ManaComponent = _player.GetNode<ManaComponent>("ManaComponent");
			_currencyComponent = _player.GetNode<CurrencyComponent>("CurrencyComponent");
			_playerCurrencyCounter.CurrencyComponent = _currencyComponent;
			_player.Died += OnPlayerDied;
			_player.WeaponsSwapped += OnWeaponSwapped;
			_player.WeaponPickedUp += OnWeaponPickedUp;
			_player.WeaponRemoved += OnWeaponRemoved;

			var isFirst = true;
			foreach (var weapon in _player.Weapons)
			{
				OnWeaponPickedUp(weapon);
			}
		}
	}
	
	private Control _interface;
	private Control _gameOverScreen;
	private Control _weaponTabsContainer;
	[Export] private PackedScene _weaponContainerScene;
	[Export] private PackedScene _weaponIconScene;
	private Control _loadingScreen;
	private HBoxContainer _weaponIconsContainer;
	private VBoxContainer _mainMenu;
	
	private bool _isGameOver;
	private bool _isWeaponTabsOpen;
	
	[Signal] public delegate void StartRequestedEventHandler();
	[Signal] public delegate void EndRequestedEventHandler();

	public override void _Ready()
	{
		_playerHealthbar = GetNode<PlayerHealthbar>("%PlayerHealthbar");
		_playerManabar = GetNode<PlayerManabar>("%PlayerManabar");
		_playerCurrencyCounter = GetNode<PlayerCurrencyCounter>("%PlayerCurrencyCounter");
		_interface = GetNode<Control>("%Interface");
		_gameOverScreen = GetNode<VBoxContainer>("%GameOverScreen");
		_weaponTabsContainer = GetNode<Control>("%WeaponTabsContainer");
		_loadingScreen = GetNode<Control>("%LoadingScreen");
		_weaponIconsContainer = GetNode<HBoxContainer>("%WeaponIcons");
		_mainMenu = GetNode<VBoxContainer>("%MainMenu");
		
		GetNode<Button>("%StartButton").Pressed += () => EmitSignal(SignalName.StartRequested);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed(Options.Controls.Player.CraftWeapon))
		{
			if (!_isWeaponTabsOpen)
			{
				OnWeaponTabsOpen();
				return;
			}
			else
			{
				OnWeaponTabsClose();
				return;
			}
		}
		
		if (!_isGameOver) return;
		
		if (@event.IsActionPressed("ui_accept"))
		{
			GameOver(false);
			EmitSignal(SignalName.EndRequested);
		}
	}

	public void GameOver(bool isOver)
	{
		_isGameOver = isOver;
		
		_interface.Visible = !isOver;
		_gameOverScreen.Visible = isOver;
		GetTree().Paused = isOver;
	}

	private void OnPlayerDied()
	{
		GameOver(true);
	}
	
	private void OnWeaponTabsOpen()
	{
		GetTree().Paused = true;
		_weaponTabsContainer.Visible = true;
		_isWeaponTabsOpen = true;
	}
	
	private void OnWeaponTabsClose()
	{
		GetTree().Paused = false;
		_weaponTabsContainer.Visible = false;
		_isWeaponTabsOpen = false;
	}

	public void SetLoadingScreen(bool @bool)
	{
		_loadingScreen.Visible = @bool;
	}

	public void SetInterface(bool @bool)
	{
		_interface.Visible = @bool;
		_playerManabar.SetManaSettings();
		_playerHealthbar.SetHealthSettings();
	}
	
	public void SetMainMenu(bool @bool)
	{
		_mainMenu.Visible = @bool;
	}

	private void OnWeaponSwapped(Weapon weapon)
	{
		foreach (var icon in _weaponIconsContainer.GetChildren())
		{
			if (icon is not WeaponIconContainer weaponIcon) continue;
			weaponIcon.IsActive = false || weaponIcon.RelatedWeapon == weapon;
		}
	}
	
	private void OnWeaponPickedUp(Weapon weapon)
	{
		var weaponContainer = _weaponContainerScene.Instantiate() as WeaponContainer;
		_weaponTabsContainer.AddChild(weaponContainer);
		weaponContainer!.Weapon = weapon;
		weaponContainer.CurrencyComponent = _currencyComponent;
		
		var icon = _weaponIconScene.Instantiate<WeaponIconContainer>();
		_weaponIconsContainer.AddChild(icon);
		icon.Outline.Texture = weapon.Sprites.Outline.As<Sprite2D>().Texture;
		icon.Color.Texture = weapon.Sprites.Color.As<Sprite2D>().Texture;
		icon.RelatedWeapon = weapon;
		
		OnWeaponSwapped(weapon);
	}
	
	private void OnWeaponRemoved(Weapon weapon)
	{
		foreach (var child in _weaponTabsContainer.GetChildren())
		{
			if (child is not WeaponContainer weaponContainer || weaponContainer.Weapon != weapon) continue;
			weaponContainer.QueueFree();
			break;
		}

		foreach (var child in _weaponIconsContainer.GetChildren())
		{
			if (child is not WeaponIconContainer weaponIcon || weaponIcon.RelatedWeapon != weapon) continue;
			weaponIcon.QueueFree();
			break;
		}
	}
}
