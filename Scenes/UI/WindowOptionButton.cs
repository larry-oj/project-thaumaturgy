using Godot;
using Godot.Collections;
using projectthaumaturgy.Extensions;

namespace projectthaumaturgy.Scenes.UI;

public partial class WindowOptionButton : OptionButton
{
	private Dictionary<long, DisplayServer.WindowMode> _type = new()
	{
		{0, DisplayServer.WindowMode.Windowed},
		{1, DisplayServer.WindowMode.Fullscreen}
	};
	
	[Export] private ResolutionOptionButton _resolutionOptionButton;
	
	public override void _Ready()
	{
		foreach (var item in _type)
		{
			this.AddItem($"{item.Value.ToString()}", (int)item.Key);
		}
		this.Selected = 0;
		this.ItemSelected += (item) =>
		{
			if (item == 1)
			{
				_resolutionOptionButton.Select(1);
				_resolutionOptionButton.Disabled = true;
				DisplayServer.WindowSetMode(_type[item]);
			}
			else
			{
				DisplayServer.WindowSetMode(_type[item]);
				_resolutionOptionButton.Disabled = false;
				_resolutionOptionButton.Select(0);
				_resolutionOptionButton.EmitSignal(OptionButton.SignalName.ItemSelected, 0);
			}
		};
	}
}