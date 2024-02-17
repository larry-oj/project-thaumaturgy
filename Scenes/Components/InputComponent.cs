using Godot;

namespace projectthaumaturgy.Scenes.Components;

public partial class InputComponent : Node2D
{
	[Export] private CanvasGroup _animatedSprites;
	
	public virtual Vector2 MovementDirection
	{
		get
		{
			var dir = Vector2.Zero;
			dir.X = Input.GetAxis("player_left", "player_right");
			dir.Y = Input.GetAxis("player_up", "player_down");
			return dir.Normalized();
		}
	}

	public virtual bool IsAttacking => Input.IsActionPressed("player_attack");

	public override void _Process(double delta)
	{
		RotateSpriteToMouse();
	}

	private void RotateSpriteToMouse()
	{
		var mousePos = GetGlobalMousePosition();
		var selfPos = GlobalPosition;
		var angle = mousePos - selfPos;

		var scale = _animatedSprites.Scale;
		scale.X = angle.X < 0 ? -1 : 1;
		_animatedSprites.Scale = scale;
	}
}