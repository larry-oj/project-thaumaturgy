using Godot;

namespace projectthaumaturgy.Scenes.Components;

public partial class Character : CharacterBody2D
{
    [Export] internal StateMachine.StateMachine _stateMachine;
    [Export] internal AnimationPlayer _animationPlayer;
}