using Godot;

namespace projectthaumaturgy.Scenes.Components;

public partial class VelocityComponent : Node2D
{
    [Export] public float MaxSpeed { get; set; }
}