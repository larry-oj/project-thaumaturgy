using Godot;

namespace projectthaumaturgy.Resources.Weapons;

public partial class RifleResource : Resource
{
    [Export] public PackedScene Scene { get; private set; }
    [Export] public AtlasTexture IconOutline { get; private set; }
    [Export] public AtlasTexture IconColor { get; private set; }
}