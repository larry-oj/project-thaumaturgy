using System.Globalization;
using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.UI;

public partial class PlayerCurrencyCounter : Label
{
	private CurrencyComponent _currencyComponent;
	public CurrencyComponent CurrencyComponent
	{
		private get => _currencyComponent;
		set
		{
			_currencyComponent = value;
			Text = _currencyComponent.Value.ToString(CultureInfo.InvariantCulture);
			_currencyComponent.CurrencyChanged += OnCurrencyChanged;
		}
	}
	
	private void OnCurrencyChanged(CurrencyChange change)
	{
		Text = change.After.ToString(CultureInfo.InvariantCulture);
	}
}