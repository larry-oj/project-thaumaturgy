using Godot;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Components;

public partial class CurrencyComponent : Node2D
{
	public int Value { get; set; } = 0;
	
	[Export] public bool IsInfinite { get; private set; }
	
	[Signal] public delegate void CurrencyChangedEventHandler(CurrencyChange currencyChange);
	
	public bool TryChangeCurrency(int amount)
	{
		if (IsInfinite) return true;
		
		var result = Value + amount;
		if (result < 0) return false;

		var change = new CurrencyChange();
		change.Before = Value;
		change.After = result;
		Value = result;

		EmitSignal(nameof(CurrencyChanged), change);

		return true;
	}
}