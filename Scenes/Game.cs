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

	public int EnemiesLeft { get; private set; }

	public override void _Ready()
	{
		Level.UI = UI;
		Level.LevelCompleted += OnLevelCompleted;

		UI.StartRequested += OnStart;
		UI.EndRequested += OnEnd;
		UI.RetryRequested += OnRetry;
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
			})
			.SetInteractableProperties(new InteractableProperties
			{
				MaxInteractables = levelResource.MaxInteractables,
				Interactables = levelResource.Interactables,
				Weapons = levelResource.Weapons
			});
		
		if (!isSync)
		{
			Level.StartWorldGen(AfterWorldGen);
		}
		else
		{
			Level.StartWorldGenSync();
			AfterWorldGen();
		}

		EnemiesLeft = levelResource.MaxEnemies;
	}

	private void OnLevelCompleted()
	{
		UI.SetLoadingScreen(true);
		if (Level.Substage < Level.MaxSubstage)
		{
			Level.Substage++;
			Level.StartWorldGen(AfterWorldGen);
		}
		else if (Level.Stage < _levelResources.Count)
		{
			LoadLevel(_levelResources.First(x => x.StageNum == Level.Stage + 1));
		}
		else
		{
			UI.SetLoadingScreen(false);
			UI.SetGameOver(true);
		}
	}
	
	private void AfterWorldGen()
	{
		Level.PlacePlayer()
			.PlaceEnemies()
			.PlaceInteractables();
		UI.SetMainMenu(false);
		UI.SetLoadingScreen(false);
		UI.SetInterface(true);
		UI.SetGameOver(false);
		_player.Visible = true;
	}

	private void OnStart()
	{
		UI.SetLoadingScreen(true);
		_player = PlayerScene.Instantiate() as Player;
		_player!.Name = "Player";
		_player.UniqueNameInOwner = true;
		AddChild(_player);
		_player.Visible = false;

		Level.Player = _player;
		UI.Player = _player;
		
		LoadLevel(_initialLevelResource);
	}

	private void OnEnd()
	{
		Level.End();
		UI.ClearWeaponTabs();
		UI.SetPauseMenu(false);
		UI.SetLoadingScreen(false);
		UI.SetInterface(false);
		UI.SetGameOver(false);
		UI.SetMainMenu(true);
	}

	private void OnRetry()
	{
		Level.End();
		UI.ClearWeaponTabs();
		UI.SetPauseMenu(false);
		UI.SetLoadingScreen(false);
		UI.SetInterface(false);
		UI.SetMainMenu(false);
		OnStart();
	}

	// public override void _UnhandledInput(InputEvent @event)
    // {
    // 	if (!@event.IsActionPressed("ui_accept")) return;
    // 	
    // 	UI.SetLoadingScreen(true);
    // 	LoadLevel(_initialLevelResource);
    // }
}