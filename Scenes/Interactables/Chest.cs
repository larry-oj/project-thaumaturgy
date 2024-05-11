using Godot;
using projectthaumaturgy.Extensions;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Levels;
using projectthaumaturgy.Scenes.Pickups;
using projectthaumaturgy.Scenes.Weapons;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Interactables;

public partial class Chest : Area2D
{
    private Sprite2D _closedSprite;
    private Sprite2D _openSprite;
    private Level _level;
    
    private bool _isUsed = false;
    
    [Export] public WeaponResource WeaponResource { get; set; }
    [Export] public PackedScene WeaponPickupScene { get; set; }
	
    public override void _Ready()
    {
        _closedSprite = GetNode<Sprite2D>("Closed");
        _openSprite = GetNode<Sprite2D>("Open");
        _level = GetNode<Level>(Options.PathOptions.Level);
    }

    private void OnAreaEntered(Node area)
    {
        if (area.Owner is not Player) return;
        if (_isUsed) return;
        
        _closedSprite.Visible = false;
        _openSprite.Visible = true;

        var weaponPickup = WeaponPickupScene.Instantiate<WeaponPickup>();
        weaponPickup.Position = this.Position.Copy();
        weaponPickup.Weapon = WeaponResource.Scene.Instantiate<Weapon>();
        _level.CallDeferred(Node.MethodName.AddChild, weaponPickup);
        
        _isUsed = true;
    }
}