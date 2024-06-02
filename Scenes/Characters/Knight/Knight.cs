using projectthaumaturgy.Scenes.Weapons;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Characters.Knight;

public partial class Knight : Enemy
{
	public override void _Ready()
	{
		_stateMachine.Init(this, _animationPlayer);
		CurrentWeapon = GetNode("Pivot").GetChild<Weapon>(0);
		CurrentWeapon.Character = this;
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
		EmitSignal(nameof(Died));
	}
}
