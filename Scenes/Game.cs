using Godot;
using projectthaumaturgy.Scenes.Levels;

namespace projectthaumaturgy.Scenes;

public partial class Game : Node2D
{
	public UI.UI Ui { get; private set; }
	public World World { get; private set; }
	
	public override void _Ready()
	{
		Ui = GetNode<UI.UI>("%UI");
		World = GetNode<World>("%World");

		Ui.World = World;
		Ui.Player = World.Player;
	}
}