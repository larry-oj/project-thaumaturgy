using Godot;
using Godot.Collections;
using projectthaumaturgy.Scenes.Characters;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.CreatedObjects;

public partial class Bullet : Projectile
{
	[Export] private Area2D _ghastArea;
	
	private Array<HitboxComponent> _nearbyEnemies = new();

	private float _magnetize = 0f;
	
	public override void _Ready()
	{
		base._Ready();
		AreaEntered += OnHitboxAreaEntered;
		BodyEntered += OnHitboxAreaEntered;
	}

	public override void _Process(double delta)
	{
		var direction = Transform.X;
		if (_nearbyEnemies.Count > 0)
		{
			_magnetize += Options.Balance.InfusionGhastGrowth * (float)delta;
			var closest = Vector2.Zero;
			foreach (var hb in _nearbyEnemies)
			{
				// measure distance from self to hb
				var distance = GlobalPosition.DistanceTo(hb.GlobalPosition);
				if (closest == Vector2.Zero || distance < GlobalPosition.DistanceTo(closest))
				{
					closest = hb.GlobalPosition;
				}
			}
			
			var tmp = (closest - GlobalPosition).Normalized();
			var a = Options.Balance.InfusionGhastMultiplier + _magnetize;
			var b = 1f - a;
			GlobalRotation = (Transform.X * b + tmp * a).Normalized().Angle();
		}
		
		Position += _velocityComponent.MaxSpeed * direction * (float)delta;
		if (_rayCast2D.IsColliding())
		{
			OnHitboxAreaEntered(_rayCast2D.GetCollider());
		}
	}

	private void OnHitboxAreaEntered(GodotObject body)
	{
		if (body == Attack.Owner) return;

		if (body is HitboxComponent hitbox)
		{
			if (!hitbox.Monitorable) return;
			if (hitbox.Owner == Attack.Owner) return;
			if (Attack.Owner is Enemy && hitbox.Owner is Enemy) return;
			
			hitbox.Damage(Attack);
		}
		
		QueueFree();
	}

	public override void SetAttack(Attack attack)
	{
		base.SetAttack(attack);

		_spritesComponent.SetColor(Attack.GetElementColor(attack.Element));
		
		if (attack.Infusion != Attack.AttackInfusion.Ghastly) return;
		_ghastArea.AreaEntered += OnGhastEntered;
		_ghastArea.AreaExited += OnGhastExited;
	}
	
	private void OnGhastEntered(Area2D area)
	{
		if (area.Owner is Enemy && area is HitboxComponent hitbox)
		{
			_nearbyEnemies.Add(hitbox);
		}
	}
	
	private void OnGhastExited(Area2D area)
	{
		if (area.Owner is Enemy && area is HitboxComponent hitbox)
		{
			_nearbyEnemies.Remove(hitbox);
		}
		if (_nearbyEnemies.Count == 0) _magnetize = 0f;
	}
}