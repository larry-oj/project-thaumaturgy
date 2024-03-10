using Godot;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.Rifle;

public partial class RifleIdle : State
{
    [Export] private RifleShoot _rifleShoot;
    private Rifle _rifle;
    
    public override void _Ready()
    {
        _rifle = GetNode<Rifle>(Options.PathOptions.WeaponStateToWeapon);
        _rifle.OnAttack += OnAttacked;
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    private void OnAttacked()
    {
        EmitSignal(nameof(Transitioned), this, _rifleShoot);
    }
}