using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.Pistol;

public partial class PistolIdle : State
{
    [Export] private PistolShoot _pistolShoot;
    private Pistol _pistol;

    public override void _Ready()
    {
        _pistol = GetNode<Pistol>(Options.PathOptions.WeaponStateToWeapon);
    }

    public override void Enter()
    {
        _pistol.OnAttack += OnAttacked;
    }

    public override void Exit()
    {
        _pistol.OnAttack -= OnAttacked;
    }

    private void OnAttacked()
    {
        EmitSignal(nameof(Transitioned), this, _pistolShoot);
    }
}