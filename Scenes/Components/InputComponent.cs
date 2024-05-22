using Godot;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Weapons;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Components;

public partial class InputComponent : Node2D
{
	[Export] private Node2D _sprites;
	[Export] private Node2D _weaponPivot;
	private Player _player;
	
	public Vector2 MovementDirection
	{
		get
		{
			var dir = Vector2.Zero;
			dir.X = Input.GetAxis(Options.Controls.Player.Left, Options.Controls.Player.Right);
			dir.Y = Input.GetAxis(Options.Controls.Player.Up, Options.Controls.Player.Down);
			return dir.Normalized();
		}
	}

	public bool IsAttacking => Input.IsActionPressed(Options.Controls.Player.Attack);
	public bool IsSwappingWeapons => Input.IsActionJustPressed(Options.Controls.Player.SwapActiveWeapon);
	public static bool IsJustAttacked => Input.IsActionJustPressed(Options.Controls.Player.Attack);
	public static bool IsJustInteracted => Input.IsActionJustPressed(Options.Controls.Player.Interact);
	
	public override void _Ready()
	{
		_player = this.GetParent<Player>();
	}
	
	public override void _Process(double delta)
	{
		var curr = _player.CurrentWeapon;
		if (!curr.IsMelee && curr.IsInAttackAnimation) return;
		
		RotateSpriteToMouse();
		RotateWeaponToMouse();
		
		if (IsAttacking)
			_player.CurrentWeapon.Attack(true);
		
		if (IsSwappingWeapons)
			_player.SwapWeapons();
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

		if (_player.CurrentWeapon.IsMelee) return;
		
		var scale = _weaponPivot.Scale;
		scale.Y = angle.X < 0 ? -1 : 1;
		_weaponPivot.Scale = scale;
	}
}