using Godot;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Levels;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Interactables;

public partial class Interactable  : Area2D
{
	internal bool _isUsed = false;
	internal Level _level;

	public override void _Ready()
	{
		_level = GetNode<Level>(Options.PathOptions.Level);
	}
	
	internal virtual void OnAreaEntered(Node area)
	{
		OnInteracted();
	}

	internal virtual void OnInteracted()
	{
		if (_isUsed) return;
	}
}