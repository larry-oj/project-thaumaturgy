using Godot;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Levels;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes;

public partial class Game : Node2D
{
	[Export] public PackedScene PlayerScene { get; private set; }
	[Export] public PackedScene LevelScene { get; private set; }

	[Export] public UI.UI UI { get; private set; }
	
#nullable enable
	private Level? _level;
	private Player? _player;
#nullable disable

	private GameManager _gameManager;
	
	public override void _Ready()
	{
		_gameManager = GetNode<GameManager>("/root/GameManager");
		
		_gameManager.LevelLoaded += OnLevelLoaded;
		if (_gameManager.IsNodeReady())
		{
			OnLevelLoaded(_gameManager.CurrentLevelResource);
		}
	}

	private void OnLevelLoaded(LevelResource levelResource)
	{
		_level = LevelScene.Instantiate() as Level;
		_level!.SetSize(levelResource.Size)
			.SetPlayerScene(PlayerScene)
			.SetWalkerProperties(new WalkerProperties
			{
				RoomChance = levelResource.RoomChance,
				RoomSize = levelResource.RoomSize,
				TurnChance = levelResource.TurnChance,
				WalkerMax = levelResource.WalkerMax,
				WalkerChance = levelResource.WalkerChance
			});
		this.AddChild(_level);
		_level.GenerateBase()
			.PlacePlayer(out var player);
		
		_player = player;
		UI.Player = _player;
	}
}