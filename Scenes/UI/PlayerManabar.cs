using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scripts;
using System;

namespace projectthaumaturgy.Scenes.UI;

public partial class PlayerManabar : ProgressBar
{
	private ManaComponent _manaComponent;
	public ManaComponent ManaComponent
	{
		private get => _manaComponent;
		set
		{
			_manaComponent = value;
			SetManaSettings();
		}
	}

	private void SetManaSettings()
	{
		MaxValue = ManaComponent.Max;
		Value = ManaComponent.Mana;
		ManaComponent.ManaChanged += OnManaChanged;
	}

	private void OnManaChanged(ManaChange change)
	{
		Value = change.AfterMana;
	}
}
