using Godot;
using System;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scripts;

public partial class MiniHealthBarComponent : TextureProgressBar
{
	[Export] private HealthComponent _healthComponent;
	
	public override void _Ready()
	{
		MaxValue = _healthComponent.Max;
		Value = _healthComponent.Health;
		_healthComponent.HealthChanged += OnHealthChanged;
	}

	private void OnHealthChanged(HealthChange change)
	{
		Value = change.After;
	}
}
