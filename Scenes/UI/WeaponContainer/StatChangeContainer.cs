using Godot;
using System;

public partial class StatChangeContainer : HBoxContainer
{	
	public Label Label { get; private set; }
	public Button IncreaseButton { get; private set; }
	public Button DecreaseButton { get; private set; }
	public Label StatLabel { get; private set; }

	[Signal] public delegate void StatChangedEventHandler(bool isIncrease);

	public override void _Ready()
	{
		Label = GetNode<Label>("Label");
		IncreaseButton = GetNode<Button>("IncreaseButton");
		DecreaseButton = GetNode<Button>("DecreaseButton");
		StatLabel = GetNode<Label>("StatLabel");

		DecreaseButton.Pressed += () => EmitSignal(nameof(StatChanged), false);
		IncreaseButton.Pressed += () => EmitSignal(nameof(StatChanged), true);
	}
}
