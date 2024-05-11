using Godot;
using projectthaumaturgy.Resources.Weapons;
using projectthaumaturgy.Resources.Weapons.CreatedObjects;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons;

public partial class Weapon : Node2D
{
    [Signal] public delegate void OnAttackEventHandler();
    public bool IsMelee => StatsComponent.Type == Scripts.Attack.AttackType.Melee;
    public bool IsInAttackAnimation { get; set; }
    public bool IsActive { get; set; } = false;

    public SpritesComponent Sprites { get; private set; }
    internal SpritesComponent _characterSprites;

    public WeaponStatsComponent StatsComponent { get; private set; }
    
    [Export] public BulletResource BulletResource { get; set; }
    
    // ABSOLUTELY OBLITERATED THE ENTIRE PROJECT
    // circular dependency is a warcrime
    // [Export] public WeaponResource Resource { get; set; }

    public override void _Ready()
    {
        Sprites = GetNode<SpritesComponent>("SpritesComponent");
        _characterSprites = GetNode(Options.PathOptions.WeaponToCharacter).GetNode<SpritesComponent>("SpritesComponent");
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
        Sprites.SetColor(color);
        
        if (IsActive)
            SetCharacterColor();
    }
    
    public void SetCharacterColor()
        => _characterSprites.SetColor(Sprites.Color.SelfModulate);
}