using Godot;
using Godot.Collections;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Levels;

public partial class World : Node2D
{
    [Export]
    private TileMap _tileMap;
    private PackedScene _player = ResourceLoader.Load("res://Scenes/Characters/Player/player.tscn") as PackedScene;
    
    #nullable enable
    public Player? Player { get; private set; }
    #nullable disable
    
    public override void _Ready()
    {
        GD.Randomize();
        // GenerateLevel();
        Player = GetNode<Player>("%Player");
    }

    private void GenerateLevel()
    {
        var level = new Level(350);

        var walkerOrch = new WalkerOrchestrator(level, Vector2I.Zero);
        walkerOrch.Walk(0.50f, new [] {3, 3}, new [] {0.33f, 0.34f, 0.33f, 0f}, 3, 0.15f);

        _tileMap.SetCellsTerrainConnect(0, level.walkableTiles, 0, 0, false);
        _tileMap.SetCellsTerrainConnect(0, level.wallTiles, 0, 1, false);
        
        var player = _player.Instantiate() as Player;
        player!.Position = _tileMap.ToGlobal(level.walkableTiles[0]);
        AddChild(player);

        level.QueueFree();
        walkerOrch.QueueFree();
    }
    
    // public override void _Input(InputEvent @event)
    // {
    //     if (@event.IsActionPressed("ui_accept"))
    //     {
    //         GetTree().ReloadCurrentScene();
    //     }
    // }
    
    public void Reload()
    {
        GetTree().ReloadCurrentScene();
    }
}