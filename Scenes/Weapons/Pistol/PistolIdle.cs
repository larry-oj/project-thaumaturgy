using Godot;
using projectthaumaturgy.Scenes.Characters;
using projectthaumaturgy.Scenes.Characters.Player;
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
        if (_pistol.Character is not Player player) return;
        var manaComponent = player.GetNode<ManaComponent>("ManaComponent");
        if (manaComponent != null)
        {
            var success = manaComponent.TryChangeMana(-_pistol.StatsComponent.ManaCost);
            if (!success) return;
        }
        EmitSignal(nameof(Transitioned), this, _pistolShoot);
    }
}