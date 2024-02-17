using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace projectthaumaturgy.Scripts;

public partial class Level : Node2D
{
    public int size;
    public Array<Vector2I> walkableTiles;
    public Array<Vector2I> wallTiles;

    public Level(int size)
    {
        this.size = size;
        walkableTiles = new Array<Vector2I>();
        wallTiles = new Array<Vector2I>();
    }
}