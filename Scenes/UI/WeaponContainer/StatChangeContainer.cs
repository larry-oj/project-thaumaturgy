using Godot;
using System;

public partial class StatChangeContainer : VBoxContainer
{	
	public Label Label { get; private set; }
	public Button IncreaseButton { get; private set; }
	public Label StatLabel { get; private set; }
	public Label CostLabel { get; private set; }

	[Signal] public delegate void StatChangedEventHandler();

	public override void _Ready()
	{
		Label = GetNode<Label>("%Label");
		IncreaseButton = GetNode<Button>("%IncreaseButton");
		StatLabel = GetNode<Label>("%StatLabel");
		CostLabel = GetNode<Label>("%CostLabelValue");

		IncreaseButton.Pressed += () => EmitSignal(nameof(StatChanged));
	}
}
