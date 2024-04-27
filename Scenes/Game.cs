using Godot;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Levels;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes;

public partial class Game : Node2D
{
	[Export] public PackedScene PlayerScene { get; private set; }

	[Export] public UI.UI UI { get; private set; }
	[Export] public Level Level { get; private set; }

	private Player _player;

	private GameManager _gameManager;

	public override void _Ready()
	{
		_player = PlayerScene.Instantiate() as Player;
		this.AddChild(_player);

		Level.SetPlayer(_player);
		UI.Player = _player;

		_gameManager = GetNode<GameManager>(Options.PathOptions.GameManager);
		_gameManager.LevelLoaded += OnLevelLoaded;
		if (_gameManager.IsNodeReady())
		{
			OnLevelLoaded(_gameManager.CurrentLevelResource);
		}
	}

	private void OnLevelLoaded(LevelResource levelResource)
	{
		Level.SetSize(levelResource.Size)
			.SetWalkerProperties(new WalkerProperties
			{
				RoomChance = levelResource.RoomChance,
				RoomSize = levelResource.RoomSize,
				TurnChance = levelResource.TurnChance,
				WalkerMax = levelResource.WalkerMax,
				WalkerChance = levelResource.WalkerChance
			});
		Level.GenerateBase()
			.PlacePlayer();
	}

/*
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept"))
		{
			Level.Clear()
				.GenerateBase()
				.PlacePlayer();
		}
	}
*/
}