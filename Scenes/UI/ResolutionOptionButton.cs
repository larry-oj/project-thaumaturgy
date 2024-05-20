using Godot;
using System;
using projectthaumaturgy.Scenes.Components.StateMachine;

public partial class ResolutionOptionButton  : OptionButton
{
	public override void _Ready()
	{
		this.AddItem("1280x720", 0);
		this.AddItem("1920x1080", 1);
		this.Selected = 0;
	}
}

