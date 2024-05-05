using Godot;
using projectthaumaturgy.Scenes.Components.StateMachine;

namespace projectthaumaturgy.Scenes.Characters.Gunner;

public partial class GunnerStunned : State
{
	[Export] private GunnerAlert _gunnerAlert;
	[Export] private GunnerDead _gunnerDead;
	public Timer Timer { get; private set; }
	
	public override void _Ready()
	{
		Timer = GetNode<Timer>("Timer");
		Timer.Timeout += OnTimerTimeout;
	}
	
	public override void Enter()
	{
		Timer.Start();
	}
	
	public override void Exit()
	{
		Timer.Stop();
	}
	
	private void OnTimerTimeout()
	{
		EmitSignal(nameof(Transitioned), this, _gunnerAlert);
	}
	
	private void OnHealthDepleted()
	{
		EmitSignal(nameof(Transitioned), this, _gunnerDead);
	}
}