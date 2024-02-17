using Godot;
using projectthaumaturgy.Scenes.Characters;

namespace projectthaumaturgy.Scenes.Components.StateMachine;

public partial class State : Node
{
    [Signal]
    public delegate void TransitionedEventHandler(State state, State newState);
    
    protected Node2D _parent;
    protected AnimationPlayer _animationPlayer;

    public void Init(Node2D parent, AnimationPlayer animationPlayer)
    {
        _parent = parent;
        _animationPlayer = animationPlayer;
    }

    public virtual void Enter() {}
    public virtual void Exit() {}
    public virtual void Process(double delta) {}
    public virtual void PhysicsProcess(double delta) {}
}