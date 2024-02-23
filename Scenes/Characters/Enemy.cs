using Godot;

namespace projectthaumaturgy.Scenes.Components;

public partial class Enemy : Character, IDetectorComponent
{
    [Export] public CharacterBody2D BodyToDetect { get; private set; }
}