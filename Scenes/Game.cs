using System.Linq;
using System.Threading.Tasks;
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
	private LevelResource _currentLevelResource;
	
	private Player _player;
	private PlayerData _playerData;

	public int EnemiesLeft { get; private set; }

	public override void _Ready()
	{
		UI.SetLoadingScreen(true);
		_playerData = GetNode<PlayerData>("PlayerData");

		_player = PlayerScene.Instantiate() as Player;
		_player.Name = "Player";
		_player.UniqueNameInOwner = true;

		Level.Player = _player;
		Level.UI = UI;
		Level.LevelCompleted += OnLevelCompleted;
		UI.Player = _player;

		LoadLevel(_initialLevelResource, true);
	}

	private void LoadLevel(LevelResource levelResource, bool isSync = false)
	{
		Level.SetSize(levelResource.Size)
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
			});
		
		if (!isSync)
		{
			Level.StartWorldGen(() =>
            {
            	Level.PlacePlayer()
            		.PlaceEnemies();
            	UI.SetLoadingScreen(false);
            });
		}
		else
		{
			Level.StartWorldGenSync()
				.PlacePlayer()
				.PlaceEnemies();
			UI.SetLoadingScreen(false);
		}

		EnemiesLeft = levelResource.MaxEnemies;
	}

	private void OnLevelCompleted()
	{
		UI.SetLoadingScreen(true);
		if (Level.Substage < Level.MaxSubstage)
		{
			Level.Substage++;
			Level.StartWorldGen(() =>
			{
				Level.PlacePlayer()
					.PlaceEnemies();
				UI.SetLoadingScreen(false);
			});
		}
		else if (Level.Stage < _levelResources.Count)
		{
			LoadLevel(_levelResources.First(x => x.StageNum == Level.Stage + 1));
		}
		else
		{
			UI.SetLoadingScreen(false);
			UI.GameOver(true);
		}
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