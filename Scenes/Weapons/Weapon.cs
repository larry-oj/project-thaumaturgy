using Godot;
using projectthaumaturgy.Resources.Weapons;
using projectthaumaturgy.Resources.Weapons.CreatedObjects;
using projectthaumaturgy.Scenes.Characters;
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

    public WeaponStatsComponent StatsComponent { get; private set; }
    public SpritesComponent CharacterSprites { get; private set; }
    
    private Character _character;
    public Character Character
    {
        get => _character;
        set
        {
            _character = value;
            if (_character != null)
                OnCharacterSetup();
            else 
                OnCharacterClear();
        }
    }
    
    [Export] public BulletResource BulletResource { get; set; }
    
    // ABSOLUTELY OBLITERATED THE ENTIRE PROJECT
    // circular dependency is a warcrime
    // [Export] public WeaponResource Resource { get; set; }

    public override void _Ready()
    {
        Sprites = GetNode<SpritesComponent>("SpritesComponent");
        
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
        
        if (IsActive && Character != null)
            SetCharacterColor();
    }
    
    public void SetCharacterColor()
        => CharacterSprites.SetColor(Sprites.Color.SelfModulate);

    internal virtual void OnCharacterSetup()
    {
        CharacterSprites = _character.GetNode<SpritesComponent>("SpritesComponent");
        if (IsActive)
            SetCharacterColor();
    }

    internal virtual void OnCharacterClear()
    {
        CharacterSprites = null;
    }
}