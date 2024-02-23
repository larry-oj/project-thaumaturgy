using Godot;
using projectthaumaturgy.Scenes.Components;

namespace projectthaumaturgy.Scenes.Characters.Gunner;

public partial class Gunner : Enemy
{
    public override void _Ready()
    {
        _stateMachine.Init(this, _animationPlayer);
    }
	
    public override void _Process(double delta)
    {
        _stateMachine.Process(delta);
    }
	
    public override void _PhysicsProcess(double delta)
    {
        _stateMachine.PhysicsProcess(delta);
    }
}