using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Levels;

namespace projectthaumaturgy.Scripts;

public partial class Level : Node
{
    /// <summary>
    /// Size of the level in walkable tiles.
    /// </summary>
    public int Size;
    /// <summary>
    /// Tiles that are walkable by the player.
    /// </summary>
    public Array<WalkableTile> walkableTiles;
    /// <summary>
    /// Wall tiles (surrounding the walkable tiles)
    /// </summary>
    public Array<Vector2I> wallTiles;
    public PackedScene PlayerScene { get; private set;}

    [Export] public TileMap TileMap { get; private set; }
    [Export] public Camera2D PlayerCamera { get; private set; }

    private WalkerProperties _walkerProperties;

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

    public Level SetPlayerScene(PackedScene _playerScene)
    {
        this.PlayerScene = _playerScene;
        return this;
    }

    public Level SetWalkerProperties(WalkerProperties walkerProperties)
    {
        _walkerProperties?.Free(); // !!!
        _walkerProperties = walkerProperties;
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

    public Level PlacePlayer(out Player player)
    {
        player = PlayerScene.Instantiate() as Player;
        player!.Position = new Vector2(Options.Sizes.TilesetHalfsize, Options.Sizes.TilesetHalfsize);
        player.UniqueNameInOwner = true;
        this.AddChild(player);

        // camera follow
        PlayerCamera.GetParent().RemoveChild(PlayerCamera);
        player.AddChild(PlayerCamera);

        return this;
    }
}