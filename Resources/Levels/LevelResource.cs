using Godot;
using projectthaumaturgy.Scripts;
using System;

public partial class LevelResource : Resource
{
	[Export] public string Name { get; private set; }
	[Export] public int StageNum { get; private set; }
	[Export] public int MaxSubstagesNum { get; private set; }
	[Export] public int Size { get; private set; }
	[Export] public float RoomChance { get; private set; } = 0.0f;
	[Export] public int[] RoomSize { get; private set; } = new[] { 0, 0 };
	[Export] public float[] TurnChance { get; private set; } = new[] { 0.25f, 0.25f, 0.25f, 0.25f };
	[Export] public int WalkerMax { get; private set; } = 1;
	[Export] public float WalkerChance { get; private set; } = 0.0f;
}