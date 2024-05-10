using Godot;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

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
		_animationPlayer.Play(Options.AnimationNames.Reset);
		Timer.Start();
	}
	
	public override void Exit()
	{
		Timer.Stop();
	}
	
	private void OnDamageReceived(GodotObject _)
	{
		_animationPlayer.Play(Options.AnimationNames.Hurt);
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