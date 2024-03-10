using Godot;
using projectthaumaturgy.Resources.Weapons;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons;

public partial class Weapon : Node2D
{
    [Signal] public delegate void OnAttackEventHandler();
    public bool IsMelee => StatsComponent.Type == Scripts.Attack.AttackType.Melee;
    public bool IsInAttackAnimation { get; set; }
    
    internal CanvasItem _colorSprite;
    internal CanvasItem _characterSprite;

    public WeaponStatsComponent StatsComponent { get; private set; }
    public CanvasGroup Sprites { get; private set; }
    
    // ABSOLUTELY OBLITERATED THE ENTIRE PROJECT
    // circular dependency is a warcrime
    // [Export] public WeaponResource Resource { get; set; }

    public override void _Ready()
    {
        Sprites = GetNode<CanvasGroup>("Sprites");
        _colorSprite = Sprites.GetNode<CanvasItem>("Color");
        _characterSprite = GetNode(Options.PathOptions.WeaponToCharacter).GetNode<CanvasItem>("AnimatedSprites/Color");
        StatsComponent = GetNode<WeaponStatsComponent>("WeaponStatsComponent");
        
        OnWeaponStatSheetUpdate();
        StatsComponent.Updated += OnWeaponStatSheetUpdate;
    }
    
    public virtual void Attack()
    {
    }
    
    public virtual void OnWeaponStatSheetUpdate()
    {
        var color = Scripts.Attack.GetElementColor(StatsComponent.Element);
        _colorSprite.SelfModulate = color;
        _characterSprite.SelfModulate = color;
    }
}