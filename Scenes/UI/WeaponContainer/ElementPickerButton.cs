using Godot;
using projectthaumaturgy.Scripts;
using System;

public partial class ElementPickerButton : Button
{
	[Signal] public delegate void ElementPickedEventHandler(Attack.AttackElement element);
	[Export] public Attack.AttackElement Element { get;  set; }

    public override void _Ready()
    {
        Pressed += Picked;
    }

    private void Picked()
    {
		EmitSignal(nameof(ElementPicked), (int)Element);
    }
}
