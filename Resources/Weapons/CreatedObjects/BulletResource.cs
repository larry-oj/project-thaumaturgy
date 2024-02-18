using Godot;

namespace projectthaumaturgy.Resources.Weapons.CreatedObjects;

public partial class BulletResource : Resource
{
    [Export] public PackedScene Scene { get; private set; }
}