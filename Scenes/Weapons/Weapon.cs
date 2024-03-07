using Godot;
using projectthaumaturgy.Resources.Weapons;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons;

public partial class Weapon : Node2D
{
    [Signal] public delegate void OnAttackEventHandler();
    public bool IsMelee => WeaponStatsComponent.Type == Scripts.Attack.AttackType.Melee;
    public bool IsInAttackAnimation { get; set; }
    
    internal CanvasItem _colorSprite;
    internal CanvasItem _characterSprite;

    public WeaponStatsComponent WeaponStatsComponent { get; private set; }

    public override void _Ready()
    {
        _colorSprite = GetNode<CanvasItem>("Sprites/Color");
        _characterSprite = GetNode<CanvasItem>("../../AnimatedSprites/Color");
        WeaponStatsComponent = GetNode<WeaponStatsComponent>("WeaponStatsComponent");
        
        OnWeaponStatSheetUpdate();
        WeaponStatsComponent.Updated += OnWeaponStatSheetUpdate;
    }
    
    public virtual void Attack()
    {
    }
    
    public virtual void OnWeaponStatSheetUpdate()
    {
        var color = Scripts.Attack.GetElementColor(WeaponStatsComponent.Element);
        _colorSprite.SelfModulate = color;
        _characterSprite.SelfModulate = color;
    }
}