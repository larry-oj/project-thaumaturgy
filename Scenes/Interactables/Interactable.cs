using Godot;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Components;

namespace projectthaumaturgy.Scenes.Interactables;

public partial class Interactable  : Area2D
{
	internal SpritesComponent _spritesComponent;
	internal bool _isOneTimeUse = true;
	internal bool _isUsed = false;
	
	public override void _Ready()
	{
		_spritesComponent = GetNode<SpritesComponent>("SpritesComponent");
	}

	internal virtual void OnAreaEntered(Node area)
	{
		if (area.Owner is Player)
		{
			OnInteracted();
		}
	}

	internal virtual void OnInteracted()
	{
		if (_isOneTimeUse && _isUsed) return;
	}
}