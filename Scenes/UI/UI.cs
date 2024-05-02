using Godot;
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
			_playerCurrencyCounter.CurrencyComponent = _player.GetNode<CurrencyComponent>("CurrencyComponent");
			_player.Died += OnPlayerDied;

			foreach (var weapon in _player.Weapons)
			{
				var weaponContainer = _weaponContainerScene.Instantiate() as WeaponContainer;
				_weaponTabsContainer.AddChild(weaponContainer);
				weaponContainer!.Weapon = weapon;
			}
		}
	}
	
	private Control _interface;
	private Control _gameOverScreen;
	private Control _weaponTabsContainer;
	[Export] private PackedScene _weaponContainerScene;
	private Control _loadingScreen;
	
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
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed(Options.Controls.PlayerCraftWeapon))
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
}
