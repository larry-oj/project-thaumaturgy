using Godot;
using projectthaumaturgy.Scripts;
using System;

public partial class ManaComponent : Node2D
{
	[Export] public float Max { get; set; }
	[Export] public float GainMultiplier { get; set; } = 1.0f;
	[Export] public float LossMultiplier { get; set; } = 1.0f;
	[Export] public bool IsInfinite { get; set; }

	public float Mana { get; private set; }
	[Signal] public delegate void ManaChangedEventHandler(ManaChange manaChange);

	public override void _Ready()
	{
		Mana = Max;
	}

	public bool TryChangeMana(float amount)
	{
		if (IsInfinite) return true;

		var result = Mana + amount;
		if (result < 0) return false;

		var manaChange = new ManaChange();
		manaChange.BeforeMana = Mana;
		Mana = Mathf.Clamp(result, 0, Max);
		manaChange.AfterMana = Mana;

		EmitSignal(nameof(ManaChanged), manaChange);

		return true;
	}
}
