using Godot;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scenes.Weapons;

namespace projectthaumaturgy.Scenes.Characters;

public partial class Character : CharacterBody2D
{
    [Export] internal StateMachine _stateMachine;
    [Export] internal AnimationPlayer _animationPlayer;
    
    public Weapon CurrentWeapon { get; internal set; }
}