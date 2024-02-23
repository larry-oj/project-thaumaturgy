using Godot;

namespace projectthaumaturgy.Scenes.Components;

public interface IDetectorComponent
{
    CharacterBody2D BodyToDetect { get; }
}