using Godot;
using projectthaumaturgy.Scripts;
using System;

public partial class EffectPickerButton : Button
{
    [Signal] public delegate void EffectPickedEventHandler(Attack.AttackInfusion infusion);
    [Export] public Attack.AttackInfusion Infusion { get;  set; }

    public override void _Ready()
    {
        Pressed += Picked;
    }

    private void Picked()
    {
        EmitSignal(SignalName.EffectPicked, (int)Infusion);
    }
}