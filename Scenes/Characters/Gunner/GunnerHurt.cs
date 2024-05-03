using Godot;
using projectthaumaturgy.Scenes.Components.StateMachine;

namespace projectthaumaturgy.Scenes.Characters.Gunner;

public partial class GunnerHurt : State
{
	[Export] private GunnerAlert _gunnerAlert;
	[Export] private GunnerDead _gunnerDead;
	
	public override void Enter()
	{
		_animationPlayer.Play("hurting");
		_animationPlayer.AnimationFinished += OnHealthDepleted;
	}
	
	public override void Exit()
	{
		_animationPlayer.AnimationFinished -= OnHealthDepleted;
	}
	
	private void OnHealthDepleted(StringName _)
	{
		EmitSignal(nameof(Transitioned), this, _gunnerAlert);
	}
	
	private void OnHealthDepleted()
	{
		EmitSignal(nameof(Transitioned), this, _gunnerDead);
	}
}