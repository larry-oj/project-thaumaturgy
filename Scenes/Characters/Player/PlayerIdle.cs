using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Characters.Player;

public partial class PlayerIdle : State
{
    [Export] private State _runningState;
    [Export] private InputComponent _inputComponent;

    public override void Enter()
    {
        _animationPlayer.Play(Options.AnimationNames.Idle);
        _animationPlayer.AnimationFinished += OnAnimationFinished;
    }
    
    public override void Exit()
    {
        _animationPlayer.Stop();
        _animationPlayer.AnimationFinished -= OnAnimationFinished;
    }

    public override void Process(double delta)
    {
        if (_inputComponent.MovementDirection != Vector2.Zero)
        {
            EmitSignal(nameof(Transitioned), this, _runningState);
        }
    }

    private void OnDamageTaken(HealthChange change)
    {
        if (change.IsHealing) return;
        _animationPlayer.Play(Options.AnimationNames.Hurt);
    }
    
    private void OnAnimationFinished(StringName animationName)
    {
        if (animationName == Options.AnimationNames.Hurt)
        {
            _animationPlayer.Play(Options.AnimationNames.Idle);
        }
    }
}