using Godot;
using Godot.Collections;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scenes.Weapons;

namespace projectthaumaturgy.Scenes.Characters;

public partial class Character : CharacterBody2D
{
    [Export] internal StateMachine _stateMachine;
    [Export] internal AnimationPlayer _animationPlayer;
    [Export] internal Node2D _pivot;
    
    [Signal] public delegate void DiedEventHandler();

    private Weapon _currentWeapon;
    public Weapon CurrentWeapon
    {
        get => _currentWeapon;
        internal set
        {
            if (_currentWeapon != null)
            {
                _currentWeapon.IsActive = false;
                _currentWeapon.Character = null;
                _currentWeapon.Visible = false;
            }
            _currentWeapon = value;
            _currentWeapon.IsActive = true;
            _currentWeapon.Character = this;
            _currentWeapon.Visible = true;
            _currentWeapon.SetCharacterColor();
        }
    } // is Weapons[0] by default
    public Array<Weapon> Weapons { get; internal set; } = new(); // for now max length is 2

}