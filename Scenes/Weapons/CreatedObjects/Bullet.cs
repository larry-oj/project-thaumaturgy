using Godot;
using projectthaumaturgy.Scenes.Characters;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.CreatedObjects;

public partial class Bullet : Projectile
{
	public override void _Ready()
	{
		base._Ready();
		AreaEntered += OnHitboxAreaEntered;
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
		if (body == Attack.Owner) return;

		if (body is HitboxComponent hitbox)
		{
			if (hitbox.Owner == Attack.Owner) return;
			if (Attack.Owner is Enemy && hitbox.Owner is Enemy) return;
			
			hitbox.Damage(Attack);
		}
		
		QueueFree();
	}

	public override void SetAttack(Attack attack)
	{
		base.SetAttack(attack);

		GetNode<CanvasItem>("Sprites/Color").SelfModulate = Attack.GetElementColor(attack.Element);
	}
}