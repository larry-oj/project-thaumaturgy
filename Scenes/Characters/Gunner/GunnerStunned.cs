using Godot;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Characters.Gunner;

public partial class GunnerStunned : StunnedBase
{
	[ExportCategory("Gunner")]
	[Export] private GunnerAlert _gunnerAlert;
	[Export] private GunnerDead _gunnerDead;
	
	protected override void OnTimerTimeout()
	{
		EmitSignal(nameof(Transitioned), this, _gunnerAlert);
	}
	
	protected override void OnHealthDepleted()
	{
		EmitSignal(nameof(Transitioned), this, _gunnerDead);
	}
}