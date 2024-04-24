using Godot;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Levels;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.UI;

public partial class UI : CanvasLayer
{
	private PlayerHealthbar _playerHealthbar;
	private Player _player;
	
	private World _world;
	public World World
	{
		get => _world;
		set
		{
			_world = value;
			_player = _world.Player;
			_playerHealthbar.HealthComponent = _player.GetNode<HealthComponent>("HealthComponent");
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
	
	private bool _isGameOver;
	private bool _isWeaponTabsOpen;

	public override void _Ready()
	{
		_playerHealthbar = GetNode<PlayerHealthbar>("%PlayerHealthbar");
		_interface = GetNode<Control>("%Interface");
		_gameOverScreen = GetNode<VBoxContainer>("%GameOverScreen");
		_weaponTabsContainer = GetNode<Control>("%WeaponTabsContainer");
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
		World.GetTree().Paused = isOver;
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
}
