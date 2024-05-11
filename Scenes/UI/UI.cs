using Godot;
using Godot.Collections;
using projectthaumaturgy.Extensions;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Levels;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.UI;

public partial class UI : CanvasLayer
{
	private PlayerHealthbar _playerHealthbar;
	private PlayerManabar _playerManabar;
	private PlayerCurrencyCounter _playerCurrencyCounter;
	
	private Player _player;
	public Player Player
	{
		get => _player;
		set
		{
			_player = value;
			_playerHealthbar.HealthComponent = _player.GetNode<HealthComponent>("HealthComponent");
			_playerManabar.ManaComponent = _player.GetNode<ManaComponent>("ManaComponent");
			var currencyComponent = _player.GetNode<CurrencyComponent>("CurrencyComponent");
			_playerCurrencyCounter.CurrencyComponent = currencyComponent;
			_player.Died += OnPlayerDied;
			_player.WeaponsSwapped += OnWeaponSwapped;

			var isFirst = true;
			foreach (var weapon in _player.Weapons)
			{
				var weaponContainer = _weaponContainerScene.Instantiate() as WeaponContainer;
				_weaponTabsContainer.AddChild(weaponContainer);
				weaponContainer!.Weapon = weapon;
				weaponContainer.CurrencyComponent = currencyComponent;
				
				var icon = _weaponIconScene.Instantiate<WeaponIconContainer>();
				_weaponIconsContainer.AddChild(icon);
				icon.Outline.Texture = weapon.Sprites.Outline.As<Sprite2D>().Texture;	// ugly ass downcast
				icon.Color.Texture = weapon.Sprites.Color.As<Sprite2D>().Texture;		// i sure do love inheritance
				
				if (!isFirst) continue;
				icon.IsActive = true;
				isFirst = false;
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
	
	private bool _isGameOver;
	private bool _isWeaponTabsOpen;

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
			GetTree().ReloadCurrentScene();
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
	
	private void OnWeaponSwapped()
	{
		foreach (var icon in _weaponIconsContainer.GetChildren())
		{
			if (icon is WeaponIconContainer weaponIcon)
			{
				weaponIcon.IsActive = !weaponIcon.IsActive;
			}
		}
	}
}
