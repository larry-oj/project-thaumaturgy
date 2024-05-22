using System.Globalization;
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
	private VBoxContainer _pauseMenu;
	private VBoxContainer _settingsMenu;
	private VBoxContainer _wonMenu;
	private PanelContainer _weaponTabs;
	private VBoxContainer _levelCleared;
	private Label _stageNameLabel;
	private Label _stageLabel;
	private Label _substageLabel;
	
	// private bool _isWeaponTabsOpen;
	
	[Signal] public delegate void StartRequestedEventHandler();
	[Signal] public delegate void EndRequestedEventHandler();
	[Signal] public delegate void RetryRequestedEventHandler();
	[Signal] public delegate void WeaponsModifiedEventHandler();

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
		_pauseMenu = GetNode<VBoxContainer>("%PauseMenu");
		_settingsMenu = GetNode<VBoxContainer>("%SettingsMenu");
		_wonMenu = GetNode<VBoxContainer>("%GameWonScreen");
		_weaponTabs = GetNode<PanelContainer>("%WeaponTabsScreen");
		_levelCleared = GetNode<VBoxContainer>("%LevelClearedScreen");
		_stageNameLabel = GetNode<Label>("%StageNameLabel");
		_stageLabel = GetNode<Label>("%StageLabel");
		_substageLabel = GetNode<Label>("%SubstageLabel");
		
		
		GetNode<Button>("%StartButton").Pressed += () => EmitSignal(SignalName.StartRequested);
		GetNode<Button>("%PauseExitButton").Pressed += () => EmitSignal(SignalName.EndRequested);
		GetNode<Button>("%OverExitButton").Pressed += () => EmitSignal(SignalName.EndRequested);
		GetNode<Button>("%MainExitButton").Pressed += () => GetTree().Quit();
		GetNode<Button>("%SettingsButton").Pressed += () => { SetMainMenu(false); SetSettingsMenu(true); };
		GetNode<Button>("%BackButton").Pressed += () => { SetSettingsMenu(false); SetMainMenu(true); };
		GetNode<Button>("%OverRetryButton").Pressed += () => EmitSignal(SignalName.RetryRequested);
		GetNode<Button>("%WonRetryButton").Pressed += () => EmitSignal(SignalName.RetryRequested);
		GetNode<Button>("%WonExitButton").Pressed += () => EmitSignal(SignalName.EndRequested);
		GetNode<Button>("%ContinueButton").Pressed += () => EmitSignal(SignalName.WeaponsModified);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		// if (@event.IsActionPressed(Options.Controls.Player.CraftWeapon))
		// {
		// 	if (!_isWeaponTabsOpen)
		// 	{
		// 		OnWeaponTabsOpen();
		// 		return;
		// 	}
		// 	else
		// 	{
		// 		OnWeaponTabsClose();
		// 		return;
		// 	}
		// }
		
		if (@event.IsActionPressed(Options.Controls.Player.Pause))
		{
			if (_weaponTabs.Visible) return;
			if (_mainMenu.Visible) return;
			if (_settingsMenu.Visible) return;
			SetPauseMenu(!_pauseMenu.Visible);
		}
	}

	public void SetGameOver(bool @bool)
	{
		_gameOverScreen.Visible = @bool;
		GetTree().Paused = @bool;
	}

	public void SetGameWon(bool @bool)
	{
		_wonMenu.Visible = @bool;
		GetTree().Paused = @bool;
	}
	
	private void OnPlayerDied()
	{
		SetGameOver(true);
	}
	
	public void SetWeaponsTab(bool @bool)
	{
		GetTree().Paused = @bool;
		_weaponTabs.Visible = @bool;
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

	public void SetStage(string stageLabel, int stage, int substage)
	{
		_stageNameLabel.Text = stageLabel;
		_stageLabel.Text = stage.ToString(CultureInfo.InvariantCulture);
		_substageLabel.Text = substage.ToString(CultureInfo.InvariantCulture);
	}
	
	public void SetMainMenu(bool @bool)
	{
		_mainMenu.Visible = @bool;
	}

	public void SetPauseMenu(bool @bool)
	{
		_pauseMenu.Visible = @bool;
		GetTree().Paused = @bool;
	}

	public void SetSettingsMenu(bool @bool)
	{
		_settingsMenu.Visible = @bool;
	}

	public void SetLevelCleared(bool @bool)
	{
		_levelCleared.Visible = @bool;
	}
	
	public void HideAll()
	{
		SetPauseMenu(false);
		SetLoadingScreen(false);
		SetInterface(false);
		SetMainMenu(false);
		SetGameWon(false);
		SetGameOver(false);
		SetWeaponsTab(false);
		SetLevelCleared(false);
	}
	
	public void ClearWeaponTabs()
	{
		foreach (var child in _weaponTabsContainer.GetChildren())
		{
			if (child is WeaponContainer weaponContainer)
			{
				weaponContainer.QueueFree();
			}
		}
		
		foreach (var child in _weaponIconsContainer.GetChildren())
		{
			if (child is WeaponIconContainer weaponContainer)
			{
				weaponContainer.QueueFree();
			}
		}
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
