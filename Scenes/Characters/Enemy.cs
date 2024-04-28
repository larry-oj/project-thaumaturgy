using Godot;

namespace projectthaumaturgy.Scenes.Characters;

public partial class Enemy : Character
{
    [Export] public CharacterBody2D BodyToDetect { get; internal set; }
}