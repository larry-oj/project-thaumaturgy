using Godot;
using System;

public partial class WeaponResource : Resource
{
	[Export] public PackedScene Scene { get; private set; }
	[Export] public AtlasTexture IconOutline { get; private set; }
	[Export] public AtlasTexture IconColor { get; private set; }
	[Export] public AtlasTexture IconSelect { get; private set; }
}
