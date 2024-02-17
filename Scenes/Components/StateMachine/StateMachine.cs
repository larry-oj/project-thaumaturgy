using Godot;
using Godot.Collections;
using projectthaumaturgy.Scenes.Characters;

namespace projectthaumaturgy.Scenes.Components.StateMachine;

public partial class StateMachine : Node
{
    [Export] 
    private State _initialState;
    private State _currentState;

    public void Init(Node2D parent, AnimationPlayer animationPlayer)
    {
        foreach (var child in GetChildren())
        {
            if (child is not State state) continue;
            
            state.Transitioned += OnChildTransition;
            state.Init(parent, animationPlayer);
        }
        
        _currentState = _initialState;
        _currentState.Enter();
    }

    public void Process(double delta)
    {
        _currentState?.Process(delta);
    }
    
    public void PhysicsProcess(double delta)
    {
        _currentState?.PhysicsProcess(delta);
    }
    
    private void OnChildTransition(State state, State newState)
    {
        if (state != _currentState) return;
        if (newState == null) return;

        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}