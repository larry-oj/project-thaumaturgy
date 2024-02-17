using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;

namespace projectthaumaturgy.Scenes.Characters.Player;

public partial class PlayerRunning : State
{
    [Export] private State _idleState;
    [Export] private VelocityComponent _velocityComponent;
    [Export] private InputComponent _inputComponent;
    
    public override void Enter()
    {
        _animationPlayer.Play("running");
    }
    
    public override void Exit()
    {
        _animationPlayer.Stop();
    }

    public override void Process(double delta)
    {
        if (_inputComponent.MovementDirection == Vector2.Zero)
        {
            EmitSignal(nameof(Transitioned), this, _idleState);
        }
    }
    
    public override void PhysicsProcess(double delta)
    {
        _character.Velocity = _inputComponent.MovementDirection * _velocityComponent.MaxSpeed;
        base.PhysicsProcess(delta);
    }
}