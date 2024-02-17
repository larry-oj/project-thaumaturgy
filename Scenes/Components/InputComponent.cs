using Godot;
using projectthaumaturgy.Scenes.Weapons;

namespace projectthaumaturgy.Scenes.Components;

public partial class InputComponent : Node2D
{
	[Export] private CanvasGroup _animatedSprites;
	[Export] private Node2D _weapon;
	
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
		RotateWeaponToMouse();
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
	
	private void RotateWeaponToMouse()
	{
		var mousePos = GetGlobalMousePosition();
		var selfPos = GlobalPosition;
		var angle = mousePos - selfPos;

		var scale = _weapon.Scale;
		scale.Y = angle.X < 0 ? -1 : 1;
		_weapon.Scale = scale;
		_weapon.Rotation = angle.Angle();
	}
}