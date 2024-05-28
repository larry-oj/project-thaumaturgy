using Godot;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Pickups;
using projectthaumaturgy.Scenes.Weapons;
using projectthaumaturgy.Scenes.Weapons.CreatedObjects;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Components;

public partial class HitboxComponent : Area2D
{
    [Export] private HealthComponent _healthComponent;
    [Export] private StatusComponent _statusComponent;
    [Export] private ManaComponent _manaComponent;
    [Export] private CurrencyComponent _currencyComponent;
    [Export] private AudioStreamPlayer2D _pickupSound;

    public void Damage(Attack attack)
    {
        var status = attack.StatusEffect?.Copy();
        var infusion = attack.Infusion;
        var attackDamage = attack.Damage;
        _healthComponent.TakeDamage(attack);
        if (status != null && !_healthComponent.IsDead)
        {
            _statusComponent?.TakeStatus(status);
        }
        if (infusion != Attack.AttackInfusion.None)
        {
            _statusComponent?.TakeInfusion(infusion, attackDamage);
        }
    }
    public void Damage(float flat)
    {
        _healthComponent.TakeDamage(flat);
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
            default:
            case Pickup.PickupType.Currency:
                _currencyComponent.TryChangeCurrency(@pickup.Value);
                break;
        }
        if (_pickupSound != null)
            _pickupSound.Playing = true;
    }

    public Weapon TakeWeapon(Weapon weapon)
    {
        if (this.Owner is not Player player) return null;
        Weapon curr = null;
        
        if (player.Weapons.Count == 2)
        {
            curr = player.CurrentWeapon;
            player.ReplaceWeapon(weapon);
        }
        else
        {
            player.TakeWeapon(weapon);
        }
        
        return curr;
    }
}
