using Godot;

namespace projectthaumaturgy.Resources.Weapons;

public partial class RifleResource : Resource
{
    [Export] public PackedScene Scene { get; private set; }
}