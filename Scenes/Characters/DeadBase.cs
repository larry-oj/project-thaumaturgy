using Godot;
using Godot.Collections;
using projectthaumaturgy.Extensions;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scenes.Levels;
using projectthaumaturgy.Scenes.Pickups;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Characters;

public partial class DeadBase<T> : State where T : Enemy
{
	[ExportCategory("Customization")] 
	[Export] protected Array<int> HealthAmount = new();
	[Export] protected Array<int> ManaAmount = new();
	[Export] protected Array<int> EssenceAmount = new();
	
	[ExportCategory("Mechanical")]
	[Export] protected PackedScene PickupScene;
	[Export] protected CollisionShape2D FeetCollider;
	[Export] protected HitboxComponent HitboxComponent;
	[Export] protected NavigationComponent NavigationComponent;
	[Export] protected DetectorComponent DetectorComponent;
	[Export] protected StatusComponent StatusComponent;

	protected Enemy Self;
	protected Level Level;

	public override void _Ready()
	{
		Level = GetNode<Level>(Options.PathOptions.Level);
		Self = GetNode<Enemy>(Options.PathOptions.CharacterStateToCharacter);
	}

	public override void Enter()
	{
		// called at the end of a frame so that the physics engine runs first
		FeetCollider.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		HitboxComponent.SetDeferred(Area2D.PropertyName.Monitorable, false);
		HitboxComponent.SetDeferred(Area2D.PropertyName.Monitoring, false);
		NavigationComponent.NavigationTimer.Stop();
		DetectorComponent.StopDetection();
		StatusComponent.StopStatus();
		// if (_gunner is Sniper.Sniper sniper)
		// {
		// 	sniper.GetNode<GunnerAlert>("StateMachine/GunnerAlert").Laser.Visible = false;
		// }

		_animationPlayer.Play(Options.AnimationNames.Dead);
		
		var healthAmount = GD.RandRange(HealthAmount[0], HealthAmount[1]);
		var manaAmount = GD.RandRange(ManaAmount[0], ManaAmount[1]);
		var pickupsAmount = healthAmount + manaAmount + GD.RandRange(EssenceAmount[0], EssenceAmount[1]);

		for (var i = 0; i < pickupsAmount; i++)
		{
			var pickup = PickupScene.Instantiate() as Pickup;
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
			pickup.Position = Self.Position.Copy();
			Level.CallDeferred(Node.MethodName.AddChild, pickup);
		}
	}
}