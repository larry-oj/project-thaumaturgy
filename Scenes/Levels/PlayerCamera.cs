using Godot;

namespace projectthaumaturgy.Scenes.Levels;

public partial class PlayerCamera : Camera2D
{
	[Export] public int targetWidth = 1;
	
	public override void _Ready()
	{
		UpdateZoom();
	}
	
	public void UpdateZoom()
	{
		var zoom = GetViewportRect().Size.X / targetWidth;
		Zoom = new Vector2(zoom, zoom);
	}
}