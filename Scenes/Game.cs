using System.Linq;
using Godot;
using Godot.Collections;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Levels;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes;

public partial class Game : Node2D
{
	[ExportCategory("Main")]
	[Export] public PackedScene PlayerScene { get; private set; }
	[Export] public UI.UI UI { get; private set; }
	[Export] public Level Level { get; private set; }

	[ExportCategory("Levels")]
	[Export] private Array<LevelResource> _levelResources = new();
	private LevelResource _initialLevelResource => _levelResources[0];

	private Player _player;
	private PlayerData _playerData;

	public int EnemiesLeft { get; private set; }

	public override void _Ready()
	{
		_playerData = GetNode<PlayerData>("PlayerData");

		_player = PlayerScene.Instantiate() as Player;
		_player.Name = "Player";
		_player.UniqueNameInOwner = true;
		this.AddChild(_player);

		Level.Player = _player;
		Level.LevelCompleted += OnLevelCompleted;
		UI.Player = _player;

		LoadLevel(_initialLevelResource);
	}

	private void LoadLevel(LevelResource levelResource)
	{
		Level.Clear()
			.SetSize(levelResource.Size)
			.SetStage(levelResource.StageNum, levelResource.MaxSubstagesNum)
			.SetWalkerProperties(new WalkerProperties
			{
				RoomChance = levelResource.RoomChance,
				RoomSize = levelResource.RoomSize,
				TurnChance = levelResource.TurnChance,
				WalkerMax = levelResource.WalkerMax,
				WalkerChance = levelResource.WalkerChance
			})
			.SetEnemyProperties(new EnemyProperties
			{
				MaxEnemies = levelResource.MaxEnemies,
				Enemies = levelResource.Enemies
			})
			.GenerateBase()
			.PlacePlayer()
			.PlaceEnemies();

		EnemiesLeft = levelResource.MaxEnemies;
	}

	private void OnLevelCompleted()
	{
		UI.SetLoadingScreen(true);
		if (Level.Substage < Level.MaxSubstage)
		{
			Level.Substage++;
			Level.Clear()
				.GenerateBase()
				.PlacePlayer()
				.PlaceEnemies();
		}
		else if (Level.Stage < _levelResources.Count)
		{
			LoadLevel(_levelResources.First(x => x.StageNum == Level.Stage + 1));
		}
		else
		{
			GD.Print("Game Over");
		}
		UI.SetLoadingScreen(false);
	}

	// public override void _UnhandledInput(InputEvent @event)
	// {
	// 	if (@event.IsActionPressed("ui_accept"))
	// 	{
	// 		Level.Clear()
	// 			.GenerateBase()
	// 			.PlacePlayer()
	// 			.PlaceEnemies();
	// 	}
	// }
}