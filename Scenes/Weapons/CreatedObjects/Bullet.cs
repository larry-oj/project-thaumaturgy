using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.CreatedObjects;

public partial class Bullet : Projectile
{
	public override void _Ready()
	{
		base._Ready();
		BodyEntered += OnHitboxAreaEntered;
	}

	public override void _Process(double delta)
	{
		Position += _velocityComponent.MaxSpeed * Transform.X * (float)delta;
		if (_rayCast2D.IsColliding())
		{
			OnHitboxAreaEntered(_rayCast2D.GetCollider());
		}
	}

	private void OnHitboxAreaEntered(GodotObject body)
	{
		if (body is HitboxComponent hitbox)
		{
			hitbox.Damage(Attack);
		}
		
		QueueFree();
	}
}