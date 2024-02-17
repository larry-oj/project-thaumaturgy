using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;

namespace projectthaumaturgy.Scenes.Weapons.Pistol;

public partial class Pistol : Weapon
{
    [Export] private StateMachine _stateMachine;
    [Export] private AnimationPlayer _animationPlayer;
    [Export] public InputComponent inputComponent;

    public override void _Ready()
    {
        _stateMachine.Init(this, _animationPlayer);
    }
	
    public override void _Process(double delta)
    {
        _stateMachine.Process(delta);
    }
}