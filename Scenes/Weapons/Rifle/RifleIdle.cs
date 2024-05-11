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
    
    public override void _Ready()
    {
        _rifle = GetNode<Rifle>(Options.PathOptions.WeaponStateToWeapon);
    }

    public override void Enter()
    {
        _rifle.OnAttack += OnAttacked;
    }

    public override void Exit()
    {
        _rifle.OnAttack -= OnAttacked;
    }

    private void OnAttacked()
    {
        if (_rifle.Character is not Player player) return;
        var manaComponent = player.GetNode<ManaComponent>("ManaComponent");
        
        if (manaComponent != null)
        {
            var success = manaComponent.TryChangeMana(-_rifle.StatsComponent.ManaCost);
            if (!success) return;
        }

        EmitSignal(nameof(Transitioned), this, _rifleShoot);
    }
}