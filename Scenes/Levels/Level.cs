using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using projectthaumaturgy.Scenes.Characters;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Levels;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Levels;

public partial class Level : Node
{
    public int Size;
    public int Stage;
    public int Substage;
    public int MaxSubstage;
    public Array<WalkableTile> walkableTiles;
    public Array<Vector2I> wallTiles;

    private Player _player;
    public Player Player 
    {
        get => _player;
        set
        {
            _player = value;
            _player.GetParent()?.RemoveChild(_player);
            this.AddChild(_player);
        }
    }

    [Export] public TileMap TileMap { get; private set; }
    [Export] public Camera2D PlayerCamera { get; private set; }

    private WalkerProperties _walkerProperties;
    private EnemyProperties _enemyProperties;

    public int EnemiesLeft { get; private set; }
    [Signal] public delegate void LevelCompletedEventHandler();

    public Level()
    {
        walkableTiles = new Array<WalkableTile>();
        wallTiles = new Array<Vector2I>();
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

    public Level Clear()
    {
        walkableTiles.Clear();
        wallTiles.Clear();
        TileMap.Clear();

        // ✨🌈 kill all "wrong" children 🌈✨
        foreach (var child in GetChildren())
        {
            if (!(child is Player || child is Camera2D || child is TileMap))
            {
                child.QueueFree();
            }
        }

        return this;
    }

    public Level GenerateBase()
    {
        new WalkerOrchestrator(this, Vector2I.Zero)
            .AddProperties(_walkerProperties)
            .Walk()
            .QueueFree(); // !!!

        TileMap.SetCellsTerrainConnect(0, new Array<Vector2I>(walkableTiles.Select(x => x.Position)), 0, 0, false);
        TileMap.SetCellsTerrainConnect(0, wallTiles, 0, 1, false);

        return this;
    }

    public Level PlacePlayer()
    {
        Player.Position = new Vector2(Options.Sizes.TilesetHalfsize, Options.Sizes.TilesetHalfsize);

        // camera follow
        PlayerCamera.GetParent()?.RemoveChild(PlayerCamera);
        Player.AddChild(PlayerCamera);

        return this;
    }

    public Level PlaceEnemies()
    {
        // dont place enemies right next to the player
        var eligibleTiles = walkableTiles
            .Where(x => x.DijkstraValue > 4)
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
            enemy.SetDeferred(Enemy.PropertyName.Position, (tile.Position * Options.Sizes.TilesetSize) + new Vector2(Options.Sizes.TilesetHalfsize, Options.Sizes.TilesetHalfsize));
            enemy.BodyToDetect = Player as CharacterBody2D;
            // enemy.SetDeferred(Enemy.PropertyName.BodyToDetect, Player as CharacterBody2D);
            this.CallDeferred(Level.MethodName.AddChild, enemy);
            // this.AddChild(enemy);
            enemy.Died += OnEnemyKilled;
        }

        EnemiesLeft = _enemyProperties.MaxEnemies;

        return this;
    }

    private void OnEnemyKilled()
    {
        EnemiesLeft--;
        if (EnemiesLeft == 0)
            EmitSignal(nameof(LevelCompleted));
    }
}