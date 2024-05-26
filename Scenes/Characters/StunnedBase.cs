using Godot;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Characters;

public partial class StunnedBase : State
{
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
	
	protected virtual void OnTimerTimeout()
	{
		// EmitSignal(nameof(Transitioned), this, _gunnerAlert);
	}
	
	protected virtual void OnHealthDepleted()
	{
		// EmitSignal(nameof(Transitioned), this, _gunnerDead);
	}
}