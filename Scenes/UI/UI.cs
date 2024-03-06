using Godot;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Levels;

namespace projectthaumaturgy.Scenes.UI;

public partial class UI : CanvasLayer
{
	private PlayerHealthbar _playerHealthbar;
	private Player _player;
	private World _world;
	
	private Control _interface;
	private VBoxContainer _gameOverScreen;
	
	private bool _isGameOver;

	public override void _Ready()
	{
		_playerHealthbar = GetNode<PlayerHealthbar>("%PlayerHealthbar");
		_world = GetNode<World>("../World");
		_player = _world.Player;
		
		_interface = GetNode<Control>("%Interface");
		_gameOverScreen = GetNode<VBoxContainer>("%GameOverScreen");

		_playerHealthbar.HealthComponent = _player.GetNode<HealthComponent>("HealthComponent");
		_player.Died += OnPlayerDied;
		
	}

	public override void _UnhandledInput(InputEvent @event)
	{
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
		_world.GetTree().Paused = isOver;
	}

	private void OnPlayerDied()
	{
		GameOver(true);
	}
}