using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Characters.Player;

public partial class PlayerIdle : State
{
    [Export] private State _runningState;
    [Export] private State _playerHurting;
    [Export] private InputComponent _inputComponent;

    public override void Enter()
    {
        _animationPlayer.Play("idle");
    }
    
    public override void Exit()
    {
        _animationPlayer.Stop();
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
        EmitSignal(nameof(Transitioned), this, _playerHurting);
    }
}