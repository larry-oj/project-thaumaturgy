using Godot;
using projectthaumaturgy.Extensions;
using projectthaumaturgy.Scenes.Characters.Gunner;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scenes.Levels;
using projectthaumaturgy.Scenes.Pickups;
using projectthaumaturgy.Scripts;
using System;

public partial class GunnerDead : State
{
	[Export] private PackedScene _pickupScene;
	[Export] private CollisionShape2D _feetCollider;
	[Export] private HitboxComponent _hitboxComponent;
	[Export] private NavigationComponent _navigationComponent;
	[Export] private DetectorComponent _detectorComponent;

	private Gunner _gunner;

	private Level _level;

	public override void _Ready()
	{
		_level = GetNode<Level>(Options.PathOptions.Level);
		_gunner = GetNode<Gunner>(Options.PathOptions.CharacterStateToCharacter);
	}

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

		var pickupsAmount = GD.RandRange(5, 8);
		var healthAmount = GD.RandRange(1, pickupsAmount);

		for (var i = 0; i < pickupsAmount; i++)
		{
			var pickup = _pickupScene.Instantiate() as Pickup;
			if (healthAmount > 0)
			{
				pickup.Type = Pickup.PickupType.Health;
				healthAmount--;
			}
			else
			{
				pickup.Type = Pickup.PickupType.Mana;
			}
			pickup.Position = _gunner.Position.Copy();
			_level.CallDeferred(Level.MethodName.AddChild, pickup);
		}
	}
}
