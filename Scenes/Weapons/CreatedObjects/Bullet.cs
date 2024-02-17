using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Weapons.CreatedObjects;

public partial class Bullet : Area2D
{
	public Attack _attack;
	[Export] private VelocityComponent _velocityComponent;
	[Export] private RayCast2D _rayCast2D;
	
	public override void _Process(double delta)
	{
		Position += _velocityComponent.MaxSpeed * Transform.X * (float)delta;
		if (_rayCast2D.IsColliding())
		{
			OnHitboxAreaEntered((Node2D)_rayCast2D.GetCollider());
		}
	}

	private void OnHitboxAreaEntered(Node2D body)
	{
		QueueFree();
	}
}