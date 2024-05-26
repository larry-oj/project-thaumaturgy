using Godot;
using projectthaumaturgy.Scenes.Characters.Gunner;
using projectthaumaturgy.Scenes.Components.StateMachine;

namespace projectthaumaturgy.Scenes.Characters.Sniper;

public partial class SniperStunned  : StunnedBase<Sniper>
{
	[ExportCategory("Gunner")]
	[Export] private SniperAlert _alert;
	[Export] private SniperDead _dead;
	
	protected override void OnTimerTimeout()
	{
		EmitSignal(State.SignalName.Transitioned, this, _alert);
	}
	
	protected override void OnHealthDepleted()
	{
		EmitSignal(State.SignalName.Transitioned, this, _dead);
	}
}