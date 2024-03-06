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

	private void SetHealthSettings()
	{
		MaxValue = HealthComponent.MaxHealth;
		Value = HealthComponent.Health;
		HealthComponent.HealthChanged += OnHealthChanged;
	}

	private void OnHealthChanged(HealthChange change)
	{
		Value = change.AfterHealth;
	}
}