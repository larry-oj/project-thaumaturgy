using Godot;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Pickups;
using projectthaumaturgy.Scenes.Weapons.CreatedObjects;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Components;

public partial class HitboxComponent : Area2D
{
    [Export] private HealthComponent _healthComponent;
    [Export] private ManaComponent _manaComponent;

    public void Damage(Attack attack)
    {
        _healthComponent?.TakeDamage(attack);
    }

    public void TakePickup(Pickup @pickup)
    {
        switch (@pickup.Type)
        {
            case Pickup.PickupType.Health:
                _healthComponent.Heal(@pickup.Value);
                break;
            case Pickup.PickupType.Mana:
                _manaComponent.TryChangeMana(@pickup.Value);
                break;
            case Pickup.PickupType.Elemental:
                // todo
                break;
        }
    }
}