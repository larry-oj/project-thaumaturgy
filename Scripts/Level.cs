using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Levels;

namespace projectthaumaturgy.Scripts;

public partial class Level : Node
{
    public int size;
    public Array<WalkableTile> walkableTiles;
    public Array<Vector2I> wallTiles;
    private PackedScene _playerScene;
    private TileMap _tileMap;
    private Camera2D _camera;

    public Level()
    {
        walkableTiles = new Array<WalkableTile>();
        wallTiles = new Array<Vector2I>();
    }

    public Level SetSize(int size)
    {
        this.size = size;
        return this;
    }

    public Level SetPlayerScene(PackedScene _playerScene)
    {
        this._playerScene = _playerScene;
        return this;
    }

    public Level SetTileMap(TileMap tileMap)
    {
        _tileMap = tileMap;
        return this;
    }

    public Level SetCamera(Camera2D camera)
    {
        _camera = camera;
        return this;
    }

    public Level GenerateBase()
    {
        new WalkerOrchestrator(this, Vector2I.Zero)
            .AddRooms(3, 3, 0.5f)
            .AddTurnChance(0.33f, 0.34f, 0.33f, 0f)
            .AddMult(3, 0.15f)
            .Walk()
            .QueueFree();

        _tileMap.SetCellsTerrainConnect(0, new Array<Vector2I>(walkableTiles.Select(x => x.Position)), 0, 0, false);
        _tileMap.SetCellsTerrainConnect(0, wallTiles, 0, 1, false);

        return this;
    }

    public Level PlacePlayer(World world, out Player player)
    {
        player = _playerScene.Instantiate() as Player;
        player!.Position = new Vector2(Options.Sizes.TilesetHalfsize, Options.Sizes.TilesetHalfsize);
        player.UniqueNameInOwner = true;
        world.AddChild(player);

        // camera follow
        _camera.GetParent().RemoveChild(_camera);
        player.AddChild(_camera);

        return this;
    }
}