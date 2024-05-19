using Godot;
using Godot.Collections;
using projectthaumaturgy.Scenes.Pickups;

namespace projectthaumaturgy.Scripts;

public partial class InteractableProperties : GodotObject
{
    public Dictionary<PackedScene, float> Interactables { get; set; }
    public int MaxInteractables { get; set; }
    public Dictionary<WeaponResource, float> Weapons { get; set; }
}