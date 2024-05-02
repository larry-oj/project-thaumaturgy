using Godot;
using projectthaumaturgy.Scenes.Characters;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.Rifle;

public partial class RifleIdle : State
{
    [Export] private RifleShoot _rifleShoot;
    private Rifle _rifle;
    private ManaComponent _manaComponent;
    
    public override void _Ready()
    {
        _rifle = GetNode<Rifle>(Options.PathOptions.WeaponStateToWeapon);
        _rifle.OnAttack += OnAttacked;
        var character = GetNode<Character>(Options.PathOptions.WeaponStateToCharacter);
        if (character is not Player player) return;
        _manaComponent = player.GetNode<ManaComponent>("ManaComponent");
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    private void OnAttacked()
    {
        if (_manaComponent != null)
        {
            var success = _manaComponent.TryChangeMana(-_rifle.StatsComponent.ManaCost);
            if (!success) return;
        }

        EmitSignal(nameof(Transitioned), this, _rifleShoot);
    }
}