using Godot;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.UI;

public partial class WeaponIconContainer : PanelContainer
{
	[Export] public TextureRect Outline { get; private set; }
	[Export] public TextureRect Color { get; private set; }

	private bool _isActive;
	public bool IsActive
	{
		get => _isActive;
		set
		{
			_isActive = value;
			this.SelfModulate = _isActive ? Options.Colors.UiActivePanel : Options.Colors.UiInactivePanel;
		}
	}
}