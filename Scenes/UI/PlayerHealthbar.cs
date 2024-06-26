using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.UI;

public partial class PlayerHealthbar : ProgressBar
{
	private HealthComponent _healthComponent;
	public HealthComponent HealthComponent
	{
		private get => _healthComponent;
		set
		{
			_healthComponent = value;
			SetHealthSettings();
		}
	}

	public void SetHealthSettings()
	{
		MaxValue = HealthComponent.Max;
		Value = HealthComponent.Health;
		HealthComponent.HealthChanged += OnHealthChanged;
	}

	private void OnHealthChanged(HealthChange change)
	{
		Value = change.After;
	}
}