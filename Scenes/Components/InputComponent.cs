using Godot;
using projectthaumaturgy.Scenes.Weapons;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Components;

public partial class InputComponent : Node2D
{
	[Export] private Node2D _sprites;
	[Export] private Node2D _weaponPivot;
	private Weapon _weapon;
	
	public virtual Vector2 MovementDirection
	{
		get
		{
			var dir = Vector2.Zero;
			dir.X = Input.GetAxis(Options.Controls.Player.Left, Options.Controls.Player.Right);
			dir.Y = Input.GetAxis(Options.Controls.Player.Up, Options.Controls.Player.Down);
			return dir.Normalized();
		}
	}

	public virtual bool IsAttacking => Input.IsActionPressed(Options.Controls.Player.Attack);

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

		var scale = _sprites.Scale;
		scale.X = angle.X < 0 ? -1 : 1;
		_sprites.Scale = scale;
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