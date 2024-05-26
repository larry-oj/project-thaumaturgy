using Godot;

namespace projectthaumaturgy.Scenes.Characters.Knight;

public partial class KnightStunned : StunnedBase
{
	[ExportCategory("Knight")]
	[Export] private KnightAlert _alert;
	[Export] private KnightDead _dead;
	
	protected override void OnTimerTimeout()
	{
		EmitSignal(nameof(Transitioned), this, _alert);
	}
	
	protected override void OnHealthDepleted()
	{
		EmitSignal(nameof(Transitioned), this, _dead);
	}
}