using Godot;

namespace projectthaumaturgy.Scenes.Components;

public partial class SpritesComponent : Node2D
{
	[Export] public CanvasItem Outline { get; private set; }
	[Export] public CanvasItem Color { get; private set; }
	
#nullable enable
	private Sprite2D? _outlineBurning;
	private Sprite2D? _outlineFreezing;
	private Sprite2D? _colorBurning;
	private Sprite2D? _colorFreezing;
	private Sprite2D? _stunnedStatus;
#nullable disable
	
	public override void _Ready()
	{
		_outlineBurning = Outline.GetNodeOrNull<Sprite2D>("Burning");
		_outlineFreezing = Outline.GetNodeOrNull<Sprite2D>("Freezing");
		_colorBurning = Color.GetNodeOrNull<Sprite2D>("Burning");
		_colorFreezing = Color.GetNodeOrNull<Sprite2D>("Freezing");
		_stunnedStatus = GetNodeOrNull<Sprite2D>("Stunned");
	}

	public SpritesComponent SetColor(Color color)
	{
		Color.SelfModulate = color;
		return this;
	}
	
	public SpritesComponent SetStunned(bool stunned = true)
	{
		if (_stunnedStatus == null) return this;
		_stunnedStatus.Visible = stunned;
		return this;
	}
	
	public SpritesComponent SetFreezing(bool freezing = true)
	{
		if (_outlineFreezing == null || _colorFreezing == null) return this;
		_outlineFreezing.Visible = freezing;
		_colorFreezing.Visible = freezing;
		return this;
	}
	
	public SpritesComponent SetBurning(bool burning = true)
	{
		if (_outlineBurning == null || _colorBurning == null) return this;
		_outlineBurning.Visible = burning;
		_colorBurning.Visible = burning;
		return this;
	}
	
	public SpritesComponent ResetStatus()
	{
		return this.SetStunned(false)
			.SetFreezing(false)
			.SetBurning(false);
	}
}