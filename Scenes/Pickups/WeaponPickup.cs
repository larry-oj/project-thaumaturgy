using Godot;
using projectthaumaturgy.Extensions;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Components;

namespace projectthaumaturgy.Scenes.Pickups;

public partial class WeaponPickup : CanvasGroup
{
	public WeaponResource Weapon { get; set; }
	
	private SpritesComponent _spritesComponent;
	private Node2D _ui;
	
	public override void _Ready()
	{
		_spritesComponent = GetNode<SpritesComponent>("SpritesComponent");
		_ui = GetNode<Node2D>("%UI");
		
		_spritesComponent.Outline.As<Sprite2D>().Texture = Weapon.IconOutline;
		_spritesComponent.Color.As<Sprite2D>().Texture = Weapon.IconColor;

		_spritesComponent.RotationDegrees = GD.Randf();
	}
	
	private void OnAreaEntered(Node area)
	{
		if (area.Owner is Player)
		{
			_ui.Visible = true;
		}
	}
	
	private void OnAreaExited(Node area)
	{
		if (area.Owner is Player)
		{
			_ui.Visible = false;
		}
	}

	public override void _ExitTree()
	{
		_ui.Visible = false;
	}
}