using Godot;

namespace projectthaumaturgy.Scenes.Levels;

public partial class PlayerCamera : Camera2D
{
	[Export] public int targetWidth = 352;
	
	public override void _Ready()
	{
		UpdateZoom();
		GetViewport().SizeChanged += UpdateZoom;
	}
	
	public void UpdateZoom()
	{
		var zoom = GetViewportRect().Size.X / (float)targetWidth;
		Zoom = new Vector2(zoom, zoom);
	}
}