using Godot;

namespace projectthaumaturgy.Scenes.Components;

public partial class NavigationComponent : NavigationAgent2D
{
	[Export] private float _timerInterval;
	public Timer NavigationTimer { get; private set; }
	
	public override void _Ready()
	{
		NavigationTimer = GetNode<Timer>("NavigationTimer");
		NavigationTimer.WaitTime = _timerInterval;
	}
}