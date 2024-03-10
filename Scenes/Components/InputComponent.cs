using Godot;
using projectthaumaturgy.Scenes.Weapons;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Components;

public partial class InputComponent : Node2D
{
	[Export] private CanvasGroup _animatedSprites;
	[Export] private Node2D _weaponPivot;
	private Weapon _weapon;
	
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

	public override void _Ready()
	{
		_weapon = GetNode<Node2D>(Options.PathOptions.CharacterComponentToPivot).GetChild<Weapon>(0);
	}
	
	public override void _Process(double delta)
	{
		if (_weapon.IsInAttackAnimation) return;
		
		RotateSpriteToMouse();
		RotateWeaponToMouse();
		
		if (IsAttacking)
			_weapon.Attack();
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
		_weaponPivot.Rotation = angle.Angle();

		if (_weapon.IsMelee) return;
		
		var scale = _weaponPivot.Scale;
		scale.Y = angle.X < 0 ? -1 : 1;
		_weaponPivot.Scale = scale;
	}
}