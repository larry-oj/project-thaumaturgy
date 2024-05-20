using Godot;
using System;
using projectthaumaturgy.Scenes.Components.StateMachine;

public partial class WindowOptionButton  : OptionButton
{
	public override void _Ready()
	{
		this.AddItem("Windowed", 0);
		this.AddItem("Fullscreen", 1);
		this.Selected = 0;
	}
	
}