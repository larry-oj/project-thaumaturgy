using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace projectthaumaturgy.Scripts;

public partial class WalkableTile : Node
{
    public Vector2I Position { get; set; }
    public int DijkstraValue { get; set; }

    public WalkableTile(Vector2I position)
    {
        Position = position;
        DijkstraValue = Mathf.Abs(position.X) + Mathf.Abs(position.Y);
    }
}