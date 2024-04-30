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
    private ManaComponent _manaComponent;

    public override void _Ready()
    {
        _pistol = GetNode<Pistol>(Options.PathOptions.WeaponStateToWeapon);
        var character = GetNode<Character>(Options.PathOptions.WeaponStateToCharacter);
        if (character is not Player player) return;
        _manaComponent = player.GetNode<ManaComponent>("ManaComponent");
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
        if (_manaComponent != null)
        {
            var success = _manaComponent.TryChangeMana(-_pistol.StatsComponent.ManaCost);
            if (!success) return;
        }
        EmitSignal(nameof(Transitioned), this, _pistolShoot);
    }
}