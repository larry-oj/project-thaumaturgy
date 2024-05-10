using Godot;
using projectthaumaturgy.Extensions;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scenes.Levels;
using projectthaumaturgy.Scenes.Pickups;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Characters.Gunner;

public partial class GunnerDead : State
{
	[Export] private PackedScene _pickupScene;
	[Export] private CollisionShape2D _feetCollider;
	[Export] private HitboxComponent _hitboxComponent;
	[Export] private NavigationComponent _navigationComponent;
	[Export] private DetectorComponent _detectorComponent;
	[Export] private StatusComponent _statusComponent;

	private Gunner _gunner;

	private Level _level;

	public override void _Ready()
	{
		_level = GetNode<Level>(Options.PathOptions.Level);
		_gunner = GetNode<Gunner>(Options.PathOptions.CharacterStateToCharacter);
	}

	public override void Enter()
	{
		// called at the end of a frame so that the physics engine runs first
		_feetCollider.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		_hitboxComponent.SetDeferred(Area2D.PropertyName.Monitorable, false);
		_hitboxComponent.SetDeferred(Area2D.PropertyName.Monitoring, false);
		_navigationComponent.NavigationTimer.Stop();
		_detectorComponent.StopDetection();
		_statusComponent.StopStatus();

		_animationPlayer.Play(Options.AnimationNames.Dead);
		
		var healthAmount = GD.RandRange(0, 1);
		var manaAmount = GD.RandRange(1, 2);
		var pickupsAmount = healthAmount + manaAmount + GD.RandRange(2, 3);

		for (var i = 0; i < pickupsAmount; i++)
		{
			var pickup = _pickupScene.Instantiate() as Pickup;
			if (healthAmount > 0)
			{
				pickup!.Type = Pickup.PickupType.Health;
				healthAmount--;
			}
			else if (manaAmount > 0)
			{
				pickup!.Type = Pickup.PickupType.Mana;
				manaAmount--;
			}
			else
			{
				pickup!.Type = Pickup.PickupType.Currency;
			}
			pickup.Position = _gunner.Position.Copy();
			_level.CallDeferred(Node.MethodName.AddChild, pickup);
		}
	}
}