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
	[Export] public Timer Timer { get; private set; }

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
		UI.WeaponsModified += OnWeaponsModified;
		
		Timer.Timeout += OnTimerTimeout;
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
		Timer.Start();
		UI.SetLevelCleared(true);
	}
	
	private void AfterWorldGen()
	{
		Level.PlacePlayer()
			.PlaceEnemies()
			.PlaceInteractables();
		UI.HideAll();
		UI.SetInterface(true);
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
		_currentLevelResource = _initialLevelResource;
		UI.SetStage(_initialLevelResource.Name, Level.Stage, Level.Substage);
	}

	private void OnEnd()
	{
		Level.End();
		UI.ClearWeaponTabs();
		UI.HideAll();
		UI.SetMainMenu(true);
	}

	private void OnRetry()
	{
		Level.End();
		UI.ClearWeaponTabs();
		UI.HideAll();
		OnStart();
	}

	private void OnTimerTimeout()
	{
		UI.SetLevelCleared(false);
		UI.SetWeaponsTab(true);
	}
	
	private void OnWeaponsModified()
	{
		UI.SetWeaponsTab(false);
		UI.SetLoadingScreen(true);
		if (Level.Substage < Level.MaxSubstage)
		{
			Level.Substage++;
			Level.StartWorldGen(AfterWorldGen);
			UI.SetStage(_currentLevelResource.Name, Level.Stage, Level.Substage);
		}
		else if (Level.Stage < _levelResources.Count)
		{
			_currentLevelResource = _levelResources.First(x => x.StageNum == Level.Stage + 1);
			LoadLevel(_currentLevelResource);
			UI.SetStage(_currentLevelResource.Name, Level.Stage, Level.Substage);
		}
		else
		{
			UI.SetLoadingScreen(false);
			UI.SetGameWon(true);
		}
	}
}