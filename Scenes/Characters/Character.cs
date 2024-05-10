using Godot;
using Godot.Collections;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scenes.Weapons;

namespace projectthaumaturgy.Scenes.Characters;

public partial class Character : CharacterBody2D
{
    [Export] internal StateMachine _stateMachine;
    [Export] internal AnimationPlayer _animationPlayer;
    
    [Signal] public delegate void DiedEventHandler();

    private Weapon _currentWeapon;
    public Weapon CurrentWeapon
    {
        get => _currentWeapon;
        internal set
        {
            _currentWeapon = value;
            _currentWeapon.IsActive = true;
        }
    } // is Weapons[0] by default
    public Array<Weapon> Weapons { get; internal set; } = new(); // for now max length is 2
}