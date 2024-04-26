using System.Linq;
using Godot;
using Godot.Collections;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Levels;

public partial class World : Node2D
{
    [Export] private TileMap _tileMap;
    [Export] private Camera2D _camera;

    private PackedScene _playerScene = ResourceLoader.Load("res://Scenes/Characters/Player/player.tscn") as PackedScene;
    #nullable enable
    public Player? Player { get; set; }
    #nullable disable
    [Signal] public delegate void PlayerFoundEventHandler();

    public Level Level { get; set; }
    
    public override void _Ready()
    {
        /*
        GD.Randomize();
        Level = new Level()
            .SetSize(350)
            .SetPlayerScene(_playerScene)
            .SetTileMap(_tileMap)
            .SetCamera(_camera);

        Level.GenerateBase()
            .PlacePlayer(this, out var player);

        Player = player;
        EmitSignal(nameof(PlayerFound));
        */
    }
    
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept"))
        {
            Level.QueueFree();
            GetTree().ReloadCurrentScene();
        }
    }
    
    public void Reload()
    {
        GetTree().ReloadCurrentScene();
    }
}