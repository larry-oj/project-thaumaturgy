using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;
using System;

public partial class GunnerDead : State
{
	[Export] private CollisionShape2D _feetCollider;
	[Export] private HitboxComponent _hitboxComponent;
	[Export] private NavigationComponent _navigationComponent;
	[Export] private DetectorComponent _detectorComponent;

	public override void Enter()
	{
		// this is some fucking voodoo shit and it sucks, but without this it doesnt work
		_feetCollider.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		// _hitboxComponent.Monitoring = false;
		_hitboxComponent.SetDeferred(HitboxComponent.PropertyName.Monitorable, false);
		// _hitboxComponent.Monitorable = false;
		_hitboxComponent.SetDeferred(HitboxComponent.PropertyName.Monitoring, false);
		_navigationComponent.NavigationTimer.Stop();
		_detectorComponent.StopDetection();

		_animationPlayer.Play("dying");
	}
}
