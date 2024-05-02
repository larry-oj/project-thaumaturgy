using System;
using Godot;

namespace projectthaumaturgy.Scripts;

public partial class WalkerProperties : GodotObject
{
    public float RoomChance = 0.0f;
    public int[] RoomSize = new[] { 0, 0 };
    public float[] TurnChance = new[] { 0.25f, 0.25f, 0.25f, 0.25f };
    public int WalkerMax = 1;
    public float WalkerChance = 0.0f;
}
