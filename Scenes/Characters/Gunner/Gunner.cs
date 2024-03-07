using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Weapons;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Characters.Gunner;

public partial class Gunner : Enemy
{
    public override void _Ready()
    {
        _stateMachine.Init(this, _animationPlayer);
        CurrentWeapon = GetNode("Pivot").GetChild<Weapon>(0);

        CurrentWeapon.WeaponStatsComponent.SetElement(Attack.AttackElement.Water);
    }
	
    public override void _Process(double delta)
    {
        _stateMachine.Process(delta);
    }
	
    public override void _PhysicsProcess(double delta)
    {
        _stateMachine.PhysicsProcess(delta);
    }
    
    private void OnHealthDepleted()
    {
        QueueFree();
    }
}