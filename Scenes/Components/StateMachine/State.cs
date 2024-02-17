using Godot;
using projectthaumaturgy.Scenes.Characters;

namespace projectthaumaturgy.Scenes.Components.StateMachine;

public partial class State : Node
{
    [Signal]
    public delegate void TransitionedEventHandler(State state, State newState);
    
    protected CharacterBody2D _character;
    protected AnimationPlayer _animationPlayer;

    public void Init(CharacterBody2D character, AnimationPlayer animationPlayer)
    {
        _character = character;
        _animationPlayer = animationPlayer;
    }

    public virtual void Enter() {}
    public virtual void Exit() {}
    public virtual void Process(double delta) {}
    public virtual void PhysicsProcess(double delta) {}
}