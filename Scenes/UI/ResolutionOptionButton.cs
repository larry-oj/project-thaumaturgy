using Godot;
using Godot.Collections;
using projectthaumaturgy.Extensions;

namespace projectthaumaturgy.Scenes.UI;

public partial class ResolutionOptionButton : OptionButton
{
	private Dictionary<long, Vector2I> _res = new()
	{
		{0, new(1280, 720)},
		{1, new(1920, 1080)}
	};
	
	public override void _Ready()
	{
		foreach (var item in _res)
		{
			this.AddItem($"{item.Value.X}x{item.Value.Y}", (int)item.Key);
		}
		this.Selected = 0;
		this.ItemSelected += (index) =>
		{
			GetWindow().Size = _res[index];
		};
	}
}