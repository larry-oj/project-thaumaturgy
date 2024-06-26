using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using projectthaumaturgy.Extensions;
using projectthaumaturgy.Scenes.Characters;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Interactables;
using projectthaumaturgy.Scenes.Levels;
using projectthaumaturgy.Scenes.Pickups;
using projectthaumaturgy.Scenes.Weapons.CreatedObjects;
using projectthaumaturgy.Scripts;
using projectthaumaturgy.Scenes.Characters.Gunner;

namespace projectthaumaturgy.Scenes.Levels;

public partial class Level : Node
{
	public int Size { get; private set; }
	public int Stage = 1;
	public int Substage = 1;
	public int MaxSubstage = 3;

	private Player _player;
	public Player Player 
	{
		get => _player;
		set
		{
			_player = value;
			// _player.GetParent()?.RemoveChild(_player);
			// this.AddChild(_player);
		}
	}

	public UI.UI UI { get; set; }
	
	[Export] public PackedScene WorldScene { get; private set; }
	[Export] public Camera2D PlayerCamera { get; private set; }
	[Export] public PackedScene WeaponPickupScene { get; private set; }
	[Export] public ColorRect Background { get; private set; }

	private World _world;
	private World _loadingWorld;

	private WalkerProperties _walkerProperties;
	private EnemyProperties _enemyProperties;
	private InteractableProperties _interactableProperties;

	public int EnemiesLeft { get; private set; }
	[Signal] public delegate void LevelCompletedEventHandler();

	public override void _Process(double delta)
	{
		// await world generation
		if (_loadingWorld == null) return;
		if (!_loadingWorld.IsLoadingFinished) return;
		_world = _loadingWorld;
		_loadingWorld = null;

		Clear();
		AddChild(_world);
		_world.OnWorldLoaded();
	}
	
	public Level SetSize(int size)
	{
		this.Size = size;
		return this;
	}

	public Level SetStage(int stage, int maxSubstage)
	{
		this.Stage = stage;
		this.MaxSubstage = maxSubstage;
		this.Substage = 1;
		return this;
	}

	public Level SetWalkerProperties(WalkerProperties walkerProperties)
	{
		_walkerProperties?.Free(); // !!!
		_walkerProperties = walkerProperties;
		return this;
	}

	public Level SetEnemyProperties(EnemyProperties enemyProperties)
	{
		_enemyProperties?.Free(); // !!!
		_enemyProperties = enemyProperties;

		var tmp = 0f;
		foreach (var key in _enemyProperties.Enemies.Keys)
		{
			tmp += _enemyProperties.Enemies[key];
			_enemyProperties.Enemies[key] = tmp;
		}

		return this;
	}
	
	public Level SetInteractableProperties(InteractableProperties props)
	{
		_interactableProperties?.Free(); // !!!
		_interactableProperties = props;
		return this;
	}

	public Level Clear()
	{
		foreach (var child in GetChildren())
		{
			switch (child)
			{
				case Enemy:
				case Pickup:
				case Projectile:
				case World:
				case Interactable:
				case WeaponPickup:
					child.QueueFree();
					break;
			}
		}

		return this;
	}
	
	public Level StartWorldGen(World.WorldLoaded callback)
	{
		_loadingWorld = WorldScene.Instantiate() as World;
		_loadingWorld!.Init(Size, _walkerProperties, Background);
		_loadingWorld.OnWorldLoaded = callback;
		_loadingWorld.LoadWorld(Stage);
	
		return this;
	}

	public Level StartWorldGenSync()
	{
		_world = WorldScene.Instantiate() as World;
		_world!.Init(Size, _walkerProperties, Background);
		_world.LoadWorld(Stage);
		_world.Wait();
		Clear();
		AddChild(_world);

		return this;
	}

	public Level PlacePlayer()
	{
		var c = _world.WalkableTiles.First().Position;
		var s = Options.Sizes.TilesetSize;
		var hs = Options.Sizes.TilesetHalfsize;
		Player.Position = c * s + new Vector2(hs, hs);
		
		Player.GetParent()?.RemoveChild(Player);
		AddChild(Player);

		// camera follow
		PlayerCamera.GetParent()?.RemoveChild(PlayerCamera);
		Player.AddChild(PlayerCamera);

		return this;
	}

	public Level PlaceEnemies()
	{
		var mult = ((Stage - 1) * 3 + Substage) * 2 + (Stage == 2 ? 5 : 0);
		
		// dont place enemies right next to the player
		var eligibleTiles = _world.WalkableTiles
			.Where(x => x.DijkstraValue > 8)
			.Select(x => (x, GD.Randf()))
			.OrderBy(x => x.Item2)
			.Select(x => x.x)
			.Take(_enemyProperties.MaxEnemies)
			.ToArray();

		foreach (var tile in eligibleTiles)
		{
			var r = GD.Randf();
			var winner = _enemyProperties.Enemies.Keys
				.First(x => r < _enemyProperties.Enemies[x]);
			
			var enemy = winner.Instantiate() as Enemy;
			enemy!.SetDeferred(Node2D.PropertyName.Position, (tile.Position * Options.Sizes.TilesetSize) + new Vector2(Options.Sizes.TilesetHalfsize, Options.Sizes.TilesetHalfsize));
			enemy.BodyToDetect = Player as CharacterBody2D;
			enemy.GetNode<HealthComponent>("HealthComponent").Max += mult;
			enemy.Died += OnEnemyKilled;
			enemy.Ready += () => enemy.CurrentWeapon.StatsComponent.SetElement((Attack.AttackElement)GD.RandRange(0, 3));
			this.CallDeferred(Node.MethodName.AddChild, enemy);
		}

		EnemiesLeft = _enemyProperties.MaxEnemies;

		return this;
	}

	public Level PlaceInteractables()
	{
		var eligibleTiles = _world.WalkableTiles
			.OrderByDescending(x => x.DijkstraValue)
			.Take(_interactableProperties.MaxInteractables);

		var props = _interactableProperties.Copy();

		foreach (var tile in eligibleTiles)
		{
			props.MaxInteractables--;
			
			var r = GD.Randf();
			var winner = props.Interactables.Keys
				.FirstOrDefault(x => r < props.Interactables[x][0] && props.Interactables[x][1] > 0);
			if (winner == null) continue;
			props.Interactables[winner][1]--;

			var @int = winner.Instantiate();
			if (@int is Chest chest)
			{
				r = GD.Randf();
				chest.WeaponResource = props.Weapons.Keys
					.First(x => r < props.Weapons[x]);
			}
			this.CallDeferred(Node.MethodName.AddChild, @int);
			@int.SetDeferred(Node2D.PropertyName.Position,
				(tile.Position * Options.Sizes.TilesetSize) +
				new Vector2(Options.Sizes.TilesetHalfsize, Options.Sizes.TilesetHalfsize));
			
			if (props.MaxInteractables == 0) break;
		}

		return this;
	}

	public Level End()
	{
		Clear();
		PlayerCamera.GetParent()?.RemoveChild(PlayerCamera);
		Player.QueueFree();
		Background.Color = new Color("4c4c70");
		
		return this;
	}
	
	private void OnEnemyKilled()
	{
		EnemiesLeft--;
		if (EnemiesLeft != 0) return;
		
		EmitSignal(nameof(LevelCompleted));
	}
}
