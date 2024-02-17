using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;

namespace projectthaumaturgy.Scenes.Weapons.Pistol;

public partial class PistolIdle : State
{
    [Export] private PistolShoot _pistolShoot;
    private InputComponent _inputComponent;

    public override void _Ready()
    {
        _inputComponent = GetNode<Pistol>("../..").inputComponent;
    }
    
    public override void Process(double delta)
    {
        if (_inputComponent.IsAttacking)
        {
            EmitSignal(nameof(Transitioned), this, _pistolShoot);
        }
    }
}