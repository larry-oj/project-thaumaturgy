using Godot;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons;

public partial class Weapon : Node2D
{
    [Signal] public delegate void OnAttackEventHandler();
    public Attack WeaponAttack;
    
    internal CanvasItem _colorSprite;
    internal CanvasItem _characterSprite;
    
    public override void _Ready()
    {
        _colorSprite = GetNode<CanvasItem>("Sprites/Color");
        _characterSprite = GetNode<CanvasItem>("../../AnimatedSprites/Color");
    }
    
    public virtual void Attack()
    {
    }
    
    public virtual void SetAttack(Attack attack)
    {
        WeaponAttack = attack;

        var color = Scripts.Attack.GetElementColor(attack.Element);
        _colorSprite.SelfModulate = color;
        _characterSprite.SelfModulate = color;
    }
}